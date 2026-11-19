using System.Collections.Concurrent;
using System.Globalization;
using System.Security;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MockIntegrations.Api.Infrastructure.Fixtures;

/// <summary>Contadores deterministas (radicados, consecutivos contables, ApplicationId...).</summary>
public sealed class SequenceStore
{
    private readonly ConcurrentDictionary<string, long> _counters = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Devuelve <paramref name="start"/> la primera vez y luego incrementa de uno en uno.</summary>
    public long Next(string name, long start = 1) =>
        _counters.AddOrUpdate(name, start, (_, current) => current + 1);

    public void Reset() => _counters.Clear();
}

public enum TemplateEscaping
{
    /// <summary>Los valores insertados se escapan para XML.</summary>
    Xml,

    /// <summary>Los valores insertados se escapan como contenido de cadena JSON.</summary>
    Json,

    None,
}

/// <summary>
/// Sustituye marcadores en los fixtures:
/// <list type="bullet">
/// <item><c>{{req:Campo}}</c> / <c>{{req:Campo|valor por defecto}}</c>: valor del request (nombre local XML, propiedad JSON, ruta o query).</item>
/// <item><c>{{seq:nombre}}</c>, <c>{{seq:nombre:inicio}}</c>, <c>{{seq:nombre:inicio:formato}}</c>: contador (p. ej. <c>{{seq:wm.barcode:123:D6}}</c> → <c>000123</c>).</item>
/// <item><c>{{now:formato}}</c>: fecha/hora local en formato .NET (por defecto <c>s</c>).</item>
/// <item><c>{{guid}}</c>: GUID nuevo.</item>
/// </list>
/// </summary>
public sealed partial class TemplateRenderer(SequenceStore sequences)
{
    [GeneratedRegex(@"\{\{(?<kind>req|seq|now|guid)(?::(?<arg>[^}]*))?\}\}")]
    private static partial Regex TokenRegex();

    public string Render(string template, Func<string, string?> lookup, TemplateEscaping escaping = TemplateEscaping.Xml) =>
        TokenRegex().Replace(template, match =>
        {
            var arg = match.Groups["arg"].Success ? match.Groups["arg"].Value : "";
            var raw = match.Groups["kind"].Value switch
            {
                "req" => ResolveRequest(arg, lookup),
                "seq" => ResolveSequence(arg),
                "now" => DateTime.Now.ToString(string.IsNullOrEmpty(arg) ? "s" : arg, CultureInfo.InvariantCulture),
                "guid" => Guid.NewGuid().ToString(),
                _ => match.Value,
            };

            return escaping switch
            {
                TemplateEscaping.Xml => SecurityElement.Escape(raw),
                TemplateEscaping.Json => JsonEncodedText.Encode(raw).ToString(),
                _ => raw,
            };
        });

    private static string ResolveRequest(string arg, Func<string, string?> lookup)
    {
        var parts = arg.Split('|', 2);
        var value = lookup(parts[0]);
        return string.IsNullOrEmpty(value) ? (parts.Length > 1 ? parts[1] : "") : value;
    }

    private string ResolveSequence(string arg)
    {
        var parts = arg.Split(':');
        var start = parts.Length > 1 && long.TryParse(parts[1], out var s) ? s : 1;
        var value = sequences.Next(parts[0], start);
        return parts.Length > 2 ? value.ToString(parts[2], CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture);
    }
}
