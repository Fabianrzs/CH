using System.Reflection;
using MockIntegrations.Api.Configuration;
using Microsoft.Extensions.Options;

namespace MockIntegrations.Api.Infrastructure.Fixtures;

/// <summary>
/// Respuestas grabadas por proveedor: <c>Providers/{Proveedor}/Fixtures/{Operación}.{escenario}.{ext}</c>.
/// Los archivos van embebidos en el ensamblado y se pueden reemplazar sin recompilar con
/// <c>MockIntegrations:FixturesOverridePath</c>. El contenido se devuelve byte a byte
/// (importante: Haberes exige el DataSet sin indentar).
/// </summary>
public sealed class FixtureStore
{
    private const string Prefix = "fixtures/";

    /// <summary>Sufijos de fixtures que sólo documentan (ejemplos de OpenAPI), no escenarios servidos.</summary>
    private static readonly string[] DocumentationOnly = ["request", "response-example"];
    private readonly IOptionsMonitor<MockOptions> _options;
    private readonly Dictionary<string, string> _embedded;

    public FixtureStore(IOptionsMonitor<MockOptions> options)
    {
        _options = options;
        var assembly = typeof(FixtureStore).Assembly;
        _embedded = assembly.GetManifestResourceNames()
            .Where(n => n.Replace('\\', '/').StartsWith(Prefix, StringComparison.Ordinal))
            .ToDictionary(n => Normalize(n.Replace('\\', '/')[Prefix.Length..]), n => ReadResource(assembly, n), StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Busca <c>{provider}/Fixtures/{fileName}</c>; primero en la carpeta de override.</summary>
    public string? Get(string provider, string fileName)
    {
        var relative = $"{provider}/Fixtures/{fileName}";
        var overrideRoot = _options.CurrentValue.FixturesOverridePath;
        if (!string.IsNullOrWhiteSpace(overrideRoot))
        {
            var path = Path.Combine(overrideRoot, provider, "Fixtures", fileName);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
        }

        return _embedded.TryGetValue(Normalize(relative), out var content) ? content : null;
    }

    /// <summary>Escenarios con fixture para una operación (<c>{op}.{escenario}.{ext}</c>), sin los ejemplos de documentación.</summary>
    public IReadOnlyList<string> ScenariosFor(string provider, string operation)
    {
        var prefix = Normalize($"{provider}/Fixtures/{operation}.");
        return _embedded.Keys
            .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(k => k[prefix.Length..])
            .Select(rest => rest[..rest.LastIndexOf('.')])
            .Where(s => !s.Contains('.') && !DocumentationOnly.Contains(s, StringComparer.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(s => s == "default" ? "" : s, StringComparer.Ordinal)
            .ToList();
    }

    public IEnumerable<string> AllKeys => _embedded.Keys;

    private static string Normalize(string path) => path.Replace('\\', '/');

    private static string ReadResource(Assembly assembly, string name)
    {
        using var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
