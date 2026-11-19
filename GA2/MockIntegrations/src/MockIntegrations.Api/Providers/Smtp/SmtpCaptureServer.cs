using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using MockIntegrations.Api.Configuration;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Providers.Smtp;

/// <summary>Correo capturado por el servidor SMTP del mock.</summary>
public sealed record CapturedMessage
{
    public required long Id { get; init; }

    public required DateTimeOffset ReceivedAt { get; init; }

    public required string MailFrom { get; init; }

    public required IReadOnlyList<string> RcptTo { get; init; }

    /// <summary>Header <c>Subject</c> decodificado (RFC 2047).</summary>
    public string? Subject { get; init; }

    public string? From { get; init; }

    public string? To { get; init; }

    /// <summary>Nombres de los adjuntos declarados en el MIME (p. ej. <c>Comunicación oficial de preaprobado.pdf</c>).</summary>
    public IReadOnlyList<string> Attachments { get; init; } = [];

    /// <summary>Usuario de AUTH, si el cliente se autenticó (la contraseña no se guarda).</summary>
    public string? AuthenticatedUser { get; init; }

    public required int SizeBytes { get; init; }

    /// <summary>Mensaje completo (RFC 5322) tal como llegó en <c>DATA</c>.</summary>
    public required string Raw { get; init; }
}

public sealed class SmtpMailbox(IOptionsMonitor<MockOptions> options)
{
    private readonly ConcurrentQueue<CapturedMessage> _messages = new();
    private long _sequence;

    public long NextId() => Interlocked.Increment(ref _sequence);

    public void Add(CapturedMessage message)
    {
        _messages.Enqueue(message);
        while (_messages.Count > Math.Max(1, options.CurrentValue.Smtp.Capacity) && _messages.TryDequeue(out _))
        {
        }
    }

    public IReadOnlyList<CapturedMessage> All() => _messages.Reverse().ToList();

    public CapturedMessage? Find(long id) => _messages.FirstOrDefault(m => m.Id == id);

    public void Clear() => _messages.Clear();
}

/// <summary>
/// Servidor SMTP mínimo de captura (sin TLS): acepta EHLO/HELO, AUTH LOGIN/PLAIN (cualquier credencial),
/// MAIL, RCPT, DATA, RSET, NOOP y QUIT, que es lo que usa <c>System.Net.Mail.SmtpClient</c> en GA2
/// (<c>EmailNotificacionRepository</c>: <c>SMTP:HabilitarSsl=false</c>, puerto <c>SMTP:Puerto</c>).
/// Los destinatarios de <c>MockIntegrations:Smtp:RejectRecipients</c> se rechazan con 550.
/// </summary>
public sealed partial class SmtpCaptureServer(IOptionsMonitor<MockOptions> options, SmtpMailbox mailbox, ILogger<SmtpCaptureServer> logger) : BackgroundService
{
    private const string Host = "mockintegrations.smtp";

    public int? BoundPort { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var settings = options.CurrentValue.Smtp;
        if (!settings.Enabled)
        {
            return;
        }

        var listener = new TcpListener(IPAddress.Any, settings.Port);
        try
        {
            listener.Start();
        }
        catch (SocketException ex)
        {
            logger.LogError(ex, "No se pudo abrir el puerto SMTP {Port}; el mock SMTP queda deshabilitado.", settings.Port);
            return;
        }

        BoundPort = ((IPEndPoint)listener.LocalEndpoint).Port;
        logger.LogInformation("Servidor SMTP de captura escuchando en el puerto {Port}", BoundPort);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync(stoppingToken);
                _ = Task.Run(() => HandleClientAsync(client, stoppingToken), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
    {
        using var _ = client;
        await using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        await using var writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true) { NewLine = "\r\n", AutoFlush = true };

        string? from = null;
        string? user = null;
        var rcpt = new List<string>();
        await writer.WriteLineAsync($"220 {Host} MockIntegrations ESMTP");
        try
        {
            while (!ct.IsCancellationRequested && await reader.ReadLineAsync(ct) is { } line)
            {
                var verb = line.Split(' ', 2)[0].ToUpperInvariant();
                switch (verb)
                {
                    case "EHLO":
                        await writer.WriteAsync($"250-{Host}\r\n250-SIZE 52428800\r\n250-8BITMIME\r\n250-AUTH LOGIN PLAIN\r\n250 OK\r\n");
                        break;
                    case "HELO":
                        await writer.WriteLineAsync($"250 {Host}");
                        break;
                    case "AUTH":
                        user = await AuthenticateAsync(line, reader, writer, ct);
                        break;
                    case "MAIL":
                        from = Address(line);
                        rcpt.Clear();
                        await writer.WriteLineAsync("250 2.1.0 Sender OK");
                        break;
                    case "RCPT":
                        var to = Address(line);
                        if (options.CurrentValue.Smtp.RejectRecipients.Contains(to, StringComparer.OrdinalIgnoreCase))
                        {
                            await writer.WriteLineAsync("550 5.1.1 Mailbox unavailable (MockIntegrations: destinatario rechazado por configuración)");
                        }
                        else
                        {
                            rcpt.Add(to);
                            await writer.WriteLineAsync("250 2.1.5 Recipient OK");
                        }

                        break;
                    case "DATA":
                        if (rcpt.Count == 0)
                        {
                            await writer.WriteLineAsync("503 5.5.1 RCPT first");
                            break;
                        }

                        await writer.WriteLineAsync("354 Start mail input; end with <CRLF>.<CRLF>");
                        var raw = await ReadDataAsync(reader, ct);
                        var id = mailbox.NextId();
                        mailbox.Add(Parse(id, from ?? "", rcpt.ToList(), user, raw));
                        await writer.WriteLineAsync($"250 2.6.0 Queued as {id}");
                        from = null;
                        rcpt.Clear();
                        break;
                    case "RSET":
                        from = null;
                        rcpt.Clear();
                        await writer.WriteLineAsync("250 OK");
                        break;
                    case "NOOP":
                        await writer.WriteLineAsync("250 OK");
                        break;
                    case "QUIT":
                        await writer.WriteLineAsync("221 Bye");
                        return;
                    default:
                        await writer.WriteLineAsync("502 5.5.2 Command not implemented");
                        break;
                }
            }
        }
        catch (Exception ex) when (ex is IOException or OperationCanceledException or ObjectDisposedException)
        {
            // Cliente desconectado.
        }
    }

    private static async Task<string?> AuthenticateAsync(string line, StreamReader reader, StreamWriter writer, CancellationToken ct)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string? user = null;
        if (parts.Length > 1 && parts[1].Equals("PLAIN", StringComparison.OrdinalIgnoreCase))
        {
            var payload = parts.Length > 2 ? parts[2] : null;
            if (payload is null)
            {
                await writer.WriteLineAsync("334 ");
                payload = await reader.ReadLineAsync(ct);
            }

            user = Decode(payload)?.Split('\0').Skip(1).FirstOrDefault();
        }
        else
        {
            // AUTH LOGIN, con o sin respuesta inicial (System.Net.Mail envía el usuario en la misma línea).
            if (parts.Length > 2)
            {
                user = Decode(parts[2]);
            }
            else
            {
                await writer.WriteLineAsync("334 VXNlcm5hbWU6");
                user = Decode(await reader.ReadLineAsync(ct));
            }

            await writer.WriteLineAsync("334 UGFzc3dvcmQ6");
            await reader.ReadLineAsync(ct);
        }

        await writer.WriteLineAsync("235 2.7.0 Authentication successful");
        return user;
    }

    private static string? Decode(string? base64)
    {
        try
        {
            return base64 is null ? null : Encoding.UTF8.GetString(Convert.FromBase64String(base64.Trim()));
        }
        catch (FormatException)
        {
            return null;
        }
    }

    private static async Task<string> ReadDataAsync(StreamReader reader, CancellationToken ct)
    {
        var sb = new StringBuilder();
        while (await reader.ReadLineAsync(ct) is { } line)
        {
            if (line == ".")
            {
                break;
            }

            sb.Append(line.StartsWith("..", StringComparison.Ordinal) ? line[1..] : line).Append("\r\n");
        }

        return sb.ToString();
    }

    private static string Address(string line)
    {
        var start = line.IndexOf('<');
        var end = line.IndexOf('>', start + 1);
        return start >= 0 && end > start ? line[(start + 1)..end] : line.Split(':', 2).ElementAtOrDefault(1)?.Trim() ?? "";
    }

    private static CapturedMessage Parse(long id, string from, List<string> rcpt, string? user, string raw)
    {
        var headerEnd = raw.IndexOf("\r\n\r\n", StringComparison.Ordinal);
        var headers = headerEnd >= 0 ? raw[..headerEnd] : raw;
        // Desplegar headers multilínea (RFC 5322 §2.2.3).
        headers = Regex.Replace(headers, "\r\n[ \t]+", " ");

        string? Header(string name) =>
            headers.Split("\r\n").FirstOrDefault(h => h.StartsWith(name + ":", StringComparison.OrdinalIgnoreCase))?[(name.Length + 1)..].Trim();

        var attachments = Attachments(Regex.Replace(raw, "\r\n[ \t]+", " "));

        return new CapturedMessage
        {
            Id = id,
            ReceivedAt = DateTimeOffset.Now,
            MailFrom = from,
            RcptTo = rcpt,
            Subject = Header("Subject") is { } subject ? DecodeWords(subject) : null,
            From = Header("From") is { } f ? DecodeWords(f) : null,
            To = Header("To"),
            Attachments = attachments,
            AuthenticatedUser = user,
            SizeBytes = Encoding.UTF8.GetByteCount(raw),
            Raw = raw,
        };
    }

    /// <summary>
    /// Nombre de cada parte MIME con <c>Content-Disposition: attachment</c>: el <c>filename</c> de la disposición o,
    /// si no está (es lo que hace System.Net.Mail), el <c>name</c> del <c>Content-Type</c>.
    /// </summary>
    private static List<string> Attachments(string unfolded)
    {
        var names = new List<string>();
        foreach (Match disposition in DispositionRegex().Matches(unfolded))
        {
            var start = unfolded.LastIndexOf("\r\n\r\n", disposition.Index, StringComparison.Ordinal);
            var end = unfolded.IndexOf("\r\n\r\n", disposition.Index, StringComparison.Ordinal);
            var headers = unfolded[(start < 0 ? 0 : start)..(end < 0 ? unfolded.Length : end)];
            var name = FileNameRegex().Match(headers) is { Success: true } file ? file : NameRegex().Match(headers);
            if (name.Success)
            {
                names.Add(DecodeParameter(name.Groups["name"].Value.Trim('"')));
            }
        }

        return names;
    }

    [GeneratedRegex(@"^Content-Disposition:\s*attachment", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex DispositionRegex();

    [GeneratedRegex(@"filename\*?=(?<name>""[^""]*""|[^;\r\n]+)", RegexOptions.IgnoreCase)]
    private static partial Regex FileNameRegex();

    [GeneratedRegex(@"\bname=(?<name>""[^""]*""|[^;\r\n]+)", RegexOptions.IgnoreCase)]
    private static partial Regex NameRegex();

    [GeneratedRegex(@"=\?(?<charset>[^?]+)\?(?<enc>[BbQq])\?(?<text>[^?]*)\?=")]
    private static partial Regex EncodedWordRegex();

    /// <summary>
    /// Valor de un parámetro MIME: RFC 2231 (<c>utf-8''Comunicaci%C3%B3n.pdf</c>, lo que usan Python y otros
    /// clientes) o RFC 2047 (<c>=?utf-8?B?...?=</c>, lo que usa System.Net.Mail).
    /// </summary>
    internal static string DecodeParameter(string value)
    {
        var rfc2231 = Regex.Match(value, "^(?<charset>[A-Za-z0-9._-]+)'[^']*'(?<text>.*)$");
        if (!rfc2231.Success)
        {
            return DecodeWords(value);
        }

        Encoding encoding;
        try
        {
            encoding = Encoding.GetEncoding(rfc2231.Groups["charset"].Value);
        }
        catch (ArgumentException)
        {
            encoding = Encoding.UTF8;
        }

        var text = rfc2231.Groups["text"].Value;
        var bytes = new List<byte>(text.Length);
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] == '%' && i + 2 < text.Length)
            {
                bytes.Add(Convert.ToByte(text.Substring(i + 1, 2), 16));
                i += 2;
            }
            else
            {
                bytes.Add((byte)text[i]);
            }
        }

        return encoding.GetString(bytes.ToArray());
    }

    /// <summary>Decodifica encoded-words RFC 2047 (<c>=?utf-8?B?...?=</c>, <c>=?utf-8?Q?...?=</c>).</summary>
    internal static string DecodeWords(string value)
    {
        var joined = Regex.Replace(value, @"\?=\s+=\?", "?==?");
        return EncodedWordRegex().Replace(joined, m =>
        {
            Encoding encoding;
            try
            {
                encoding = Encoding.GetEncoding(m.Groups["charset"].Value);
            }
            catch (ArgumentException)
            {
                encoding = Encoding.UTF8;
            }

            var text = m.Groups["text"].Value;
            if (m.Groups["enc"].Value.Equals("B", StringComparison.OrdinalIgnoreCase))
            {
                return encoding.GetString(Convert.FromBase64String(text));
            }

            var bytes = new List<byte>();
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '_')
                {
                    bytes.Add((byte)' ');
                }
                else if (text[i] == '=' && i + 2 < text.Length)
                {
                    bytes.Add(Convert.ToByte(text.Substring(i + 1, 2), 16));
                    i += 2;
                }
                else
                {
                    bytes.Add((byte)text[i]);
                }
            }

            return encoding.GetString(bytes.ToArray());
        });
    }
}
