using System.Globalization;
using System.Text.RegularExpressions;

namespace MockIntegrations.Api.Infrastructure.Fixtures;

/// <summary>
/// Resuelve los marcadores de un fixture con valores ilustrativos y sin efectos secundarios
/// (no avanza contadores). Se usa para los ejemplos de OpenAPI.
/// </summary>
public static partial class TemplatePreview
{
    [GeneratedRegex(@"\{\{(?<kind>req|seq|now|guid)(?::(?<arg>[^}]*))?\}\}")]
    private static partial Regex TokenRegex();

    public static string Render(string template) =>
        TokenRegex().Replace(template, match =>
        {
            var arg = match.Groups["arg"].Success ? match.Groups["arg"].Value : "";
            switch (match.Groups["kind"].Value)
            {
                case "req":
                    var parts = arg.Split('|', 2);
                    return parts.Length > 1 ? parts[1] : parts[0];
                case "seq":
                    var seq = arg.Split(':');
                    var start = seq.Length > 1 && long.TryParse(seq[1], out var s) ? s : 1;
                    return seq.Length > 2 ? start.ToString(seq[2], CultureInfo.InvariantCulture) : start.ToString(CultureInfo.InvariantCulture);
                case "now":
                    return new DateTime(2026, 1, 15, 10, 30, 0).ToString(string.IsNullOrEmpty(arg) ? "s" : arg, CultureInfo.InvariantCulture);
                default:
                    return "3f2c8a1e-5b7d-4c11-9d1e-0a3b2c4d5e6f";
            }
        });
}
