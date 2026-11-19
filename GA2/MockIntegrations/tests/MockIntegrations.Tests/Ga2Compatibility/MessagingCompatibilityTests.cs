using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using GA2.Domain.Core.IntegrationEvents.Events.WorkManager;
using MockIntegrations.Api.Providers.ServiceBus;
using MockIntegrations.Api.Providers.Smtp;
using MockIntegrations.Tests.Infrastructure;

namespace MockIntegrations.Tests.Ga2Compatibility;

/// <summary>E11 SMTP (con <c>System.Net.Mail.SmtpClient</c> como GA2) y E12 Service Bus (formato de los eventos que consume GA2).</summary>
[Collection(MockServerCollection.Name)]
public sealed class MessagingCompatibilityTests(MockServer server) : IAsyncLifetime
{
    public async ValueTask InitializeAsync() => await server.ResetAsync();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    [Fact]
    public async Task Smtp_captures_the_mail_EmailNotificacionRepository_sends()
    {
        // Misma configuración que appsettings SMTP de GA2: sin SSL, sin credenciales, HTML y PDF adjunto en memoria.
        using var smtp = new SmtpClient("127.0.0.1", server.SmtpPort) { EnableSsl = false, UseDefaultCredentials = false, Timeout = 30_000 };
        using var message = new MailMessage
        {
            From = new MailAddress("notificaciones@cajahonor.gov.co", "CajaHonor"),
            Subject = "Comunicación oficial de Preaprobado de crédito hipotecario",
            Body = "<html><body><p>Estimado JUAN CARLOS PEREZ GOMEZ</p><img src=\"data:image/png;base64,iVBORw0KGgo=\"/></body></html>",
            IsBodyHtml = true,
        };
        message.To.Add("afiliado@example.com");
        message.Attachments.Add(new Attachment(new MemoryStream("%PDF-1.4\n%%EOF\n"u8.ToArray()), "Comunicación oficial de preaprobado.pdf", MediaTypeNames.Application.Pdf));

        await smtp.SendMailAsync(message);

        var captured = Assert.Single(server.GetService<SmtpMailbox>().All());
        Assert.Equal("notificaciones@cajahonor.gov.co", captured.MailFrom);
        Assert.Equal(["afiliado@example.com"], captured.RcptTo);
        Assert.Equal("Comunicación oficial de Preaprobado de crédito hipotecario", captured.Subject);
        Assert.Contains("Comunicación oficial de preaprobado.pdf", captured.Attachments);

        var listed = await server.Http.GetFromJsonAsync<List<CapturedMessage>>("smtp/messages");
        Assert.Single(listed!);
    }

    [Fact]
    public async Task Smtp_decodes_rfc2231_attachment_names_sent_by_other_clients()
    {
        // Regresión (pruebas manuales 2026-10-19): clientes como Python codifican el nombre con RFC 2231.
        const string mime = "From: notificaciones@cajahonor.gov.co\r\nTo: afiliado@example.com\r\nSubject: Prueba\r\nMIME-Version: 1.0\r\n"
            + "Content-Type: multipart/mixed; boundary=\"b1\"\r\n\r\n--b1\r\nContent-Type: text/plain\r\n\r\nhola\r\n--b1\r\n"
            + "Content-Type: application/pdf\r\nContent-Disposition: attachment;\r\n filename*=utf-8''Comunicaci%C3%B3n%20oficial.pdf\r\n\r\nJVBERi0=\r\n--b1--\r\n";
        using var client = new System.Net.Sockets.TcpClient("127.0.0.1", server.SmtpPort);
        using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.ASCII);
        await using var writer = new StreamWriter(stream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true };
        async Task<string> Send(string line)
        {
            await writer.WriteLineAsync(line);
            var reply = await reader.ReadLineAsync();
            while (reply is { Length: > 3 } && reply[3] == '-')
            {
                reply = await reader.ReadLineAsync();
            }

            return reply!;
        }

        await reader.ReadLineAsync();
        await Send("HELO pruebas");
        await Send("MAIL FROM:<notificaciones@cajahonor.gov.co>");
        await Send("RCPT TO:<afiliado@example.com>");
        Assert.StartsWith("354", await Send("DATA"));
        Assert.StartsWith("250", await Send(mime + "."));
        await Send("QUIT");

        Assert.Equal(["Comunicación oficial.pdf"], Assert.Single(server.GetService<SmtpMailbox>().All()).Attachments);
        Assert.Equal("Comunicación.pdf", SmtpCaptureServer.DecodeParameter("utf-8''Comunicaci%C3%B3n.pdf"));
        Assert.Equal("Comunicación.pdf", SmtpCaptureServer.DecodeParameter("=?utf-8?B?Q29tdW5pY2FjacOzbi5wZGY=?="));
    }

    [Fact]
    public async Task Smtp_rejected_recipient_raises_SmtpFailedRecipientException_like_GA2_expects()
    {
        using var smtp = new SmtpClient("127.0.0.1", server.SmtpPort);
        await Assert.ThrowsAsync<SmtpFailedRecipientException>(() =>
            smtp.SendMailAsync("notificaciones@cajahonor.gov.co", "rechazado@mock.test", "Comunicado Oficial - Rechazo", "<p>x</p>"));
        Assert.Empty(server.GetService<SmtpMailbox>().All());
    }

    [Fact]
    public async Task Smtp_accepts_authentication_when_GA2_configures_credentials()
    {
        using var smtp = new SmtpClient("127.0.0.1", server.SmtpPort) { Credentials = new System.Net.NetworkCredential("notificaciones@cajahonor.gov.co", "clave") };
        await smtp.SendMailAsync("notificaciones@cajahonor.gov.co", "afiliado@example.com", "Comunicado Oficial - aprobación de crédito", "<p>x</p>");

        Assert.Equal("notificaciones@cajahonor.gov.co", Assert.Single(server.GetService<SmtpMailbox>().All()).AuthenticatedUser);
    }

    [Fact]
    public void Service_bus_subjects_match_the_GA2_event_names()
    {
        // GA2 filtra por Subject = nombre del tipo sin el sufijo "IntegrationEvent".
        var ga2Events = typeof(WorkManagerInsertRequestIntegrationEvent).Assembly.GetTypes()
            .Where(t => t.Namespace == typeof(WorkManagerInsertRequestIntegrationEvent).Namespace && t.Name.EndsWith("IntegrationEvent"))
            .Select(t => t.Name[..^"IntegrationEvent".Length])
            .ToHashSet();

        Assert.Equal(ga2Events, ServiceBusMock.Events.Keys.ToHashSet());
    }

    [Theory]
    [InlineData("WorkManagerInsertRequest", typeof(WorkManagerInsertRequestIntegrationEvent))]
    [InlineData("WorkManagerUpdateRequest", typeof(WorkManagerUpdateRequestIntegrationEvent))]
    [InlineData("WorkManagerDeleteRequest", typeof(WorkManagerDeleteRequestIntegrationEvent))]
    [InlineData("WorkManagerWorkflowAttachFilesRequest", typeof(WorkManagerWorkflowAttachFilesRequestIntegrationEvent))]
    [InlineData("WorkManagerAttachRequest", typeof(WorkManagerAttachRequestIntegrationEvent))]
    public void Service_bus_sample_is_deserialized_like_EventBusServiceBus_and_shows_the_known_defect(string subject, Type eventType)
    {
        // EventBusServiceBus: JsonSerializer.Deserialize(body, tipo) con opciones por defecto.
        var body = ServiceBusMock.Sample(subject).ToJsonString();
        var integrationEvent = System.Text.Json.JsonSerializer.Deserialize(body, eventType)!;

        var payload = eventType.GetProperty(ServiceBusMock.Events[subject].PayloadProperty)!.GetValue(integrationEvent);
        Assert.NotNull(payload);

        // Defecto documentado de GA2: System.Text.Json ignora el campo público 'request' del proxy WCF.
        Assert.Null(payload.GetType().GetField("request")!.GetValue(payload));
    }

    [Fact]
    public async Task Service_bus_publish_without_connection_string_is_explicit()
    {
        using var response = await server.Http.PostAsync("_mock/servicebus/publish/WorkManagerInsertRequest", new StringContent("", Encoding.UTF8, "application/json"));
        Assert.Equal(System.Net.HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }
}
