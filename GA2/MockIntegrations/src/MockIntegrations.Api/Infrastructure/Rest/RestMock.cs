using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Journal;
using MockIntegrations.Api.Infrastructure.OpenApi;
using MockIntegrations.Api.Infrastructure.Scenarios;

namespace MockIntegrations.Api.Infrastructure.Rest;

/// <summary>Metadatos de una operación REST simulada.</summary>
public sealed record RestOperationSpec
{
    public required string Provider { get; init; }

    public required string Name { get; init; }

    public required Ga2Usage Usage { get; init; }

    public required string Summary { get; init; }

    public string? Description { get; init; }

    public IReadOnlyList<ScenarioInfo> Scenarios { get; init; } = [];

    public string? Ga2ConfigKey { get; init; }

    public string? Ga2Source { get; init; }

    public ContractCertainty Certainty { get; init; } = ContractCertainty.Validated;
}

public static class RestMock
{
    /// <summary>
    /// Registra un endpoint REST simulado: catálogo, nombre, etiqueta y documento OpenAPI del proveedor,
    /// descripción con los escenarios y el header opcional <c>X-Mock-Scenario</c>.
    /// </summary>
    public static RouteHandlerBuilder MapMock(this WebApplication app, string method, string route, RestOperationSpec spec, Delegate handler)
    {
        app.Services.GetRequiredService<MockCatalog>().Add(new MockOperation
        {
            Provider = spec.Provider,
            Name = spec.Name,
            Protocol = MockProtocol.Rest,
            Method = method,
            Route = route,
            Usage = spec.Usage,
            Summary = spec.Summary,
            Scenarios = spec.Scenarios,
            Ga2ConfigKey = spec.Ga2ConfigKey,
            Ga2Source = spec.Ga2Source,
            Certainty = spec.Certainty,
        });

        return app.MapMethods(route, [method], handler)
            .WithName($"{spec.Provider}_{spec.Name}")
            .WithTags(spec.Provider)
            .WithGroupName(spec.Provider.ToLowerInvariant())
            .WithSummary(spec.Summary)
            .WithDescription(Describe(spec))
            .AddOpenApiOperationTransformer((operation, _, _) =>
            {
                operation.Parameters ??= [];
                operation.Parameters.Add(SoapOperationTransformer.ScenarioHeader());
                return Task.CompletedTask;
            });
    }

    /// <summary>Resuelve el escenario, lo registra en la llamada y aplica la latencia configurada.</summary>
    public static async Task<ScenarioDecision> BeginAsync(HttpContext http, RestOperationSpec spec, Func<string, string?> lookup)
    {
        var decision = http.RequestServices.GetRequiredService<ScenarioResolver>().Resolve(http, spec.Provider, spec.Name, lookup);
        MockCallInfo.Set(http, spec.Provider, spec.Name, decision.Name, decision.Source.ToString());
        await http.RequestServices.GetRequiredService<MockDelays>().ApplyAsync(spec.Provider, decision.Name, http.RequestAborted);
        return decision.Name == BuiltInScenarios.Slow ? decision with { Name = BuiltInScenarios.Default } : decision;
    }

    /// <summary>Respuesta de los escenarios integrados de error técnico, o <c>null</c> si no aplica.</summary>
    public static IResult? BuiltIn(ScenarioDecision decision, RestOperationSpec spec) => decision.Name switch
    {
        BuiltInScenarios.Fault => Results.Text($"Error técnico simulado por MockIntegrations en {spec.Provider}/{spec.Name}.", "text/plain", Encoding.UTF8, 500),
        BuiltInScenarios.Http500 => Results.Content("<html><body><h1>500 Internal Server Error</h1><p>MockIntegrations (http-500)</p></body></html>", "text/html", Encoding.UTF8, 500),
        _ => null,
    };

    public static IResult UnknownScenario(ScenarioDecision decision, RestOperationSpec spec) =>
        Results.Text($"MockIntegrations: el escenario '{decision.Name}' no existe para {spec.Provider}/{spec.Name}. Consulte GET /_mock/scenarios.", "text/plain", Encoding.UTF8, 400);

    /// <summary>Busca un valor por nombre en la ruta, la query y (sin distinguir mayúsculas, en cualquier nivel) en el body.</summary>
    public static Func<string, string?> Lookup(HttpContext http, object? body = null)
    {
        JsonNode? json = null;
        if (body is not null)
        {
            var options = http.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
            json = JsonSerializer.SerializeToNode(body, body.GetType(), options);
        }

        return key =>
        {
            if (http.Request.RouteValues.FirstOrDefault(kv => string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)).Value is { } route)
            {
                return route.ToString();
            }

            if (http.Request.Query.FirstOrDefault(kv => string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase)).Value is { Count: > 0 } query)
            {
                return query.ToString();
            }

            return json is null ? null : FindJson(json, key);
        };
    }

    private static string? FindJson(JsonNode node, string key)
    {
        switch (node)
        {
            case JsonObject obj:
                foreach (var (name, value) in obj)
                {
                    if (string.Equals(name, key, StringComparison.OrdinalIgnoreCase) && value is JsonValue leaf)
                    {
                        return leaf.ToString();
                    }
                }

                foreach (var (_, value) in obj)
                {
                    if (value is not null && FindJson(value, key) is { } found)
                    {
                        return found;
                    }
                }

                return null;
            case JsonArray array:
                return array.OfType<JsonNode>().Select(n => FindJson(n, key)).FirstOrDefault(v => v is not null);
            default:
                return null;
        }
    }

    private static string Describe(RestOperationSpec spec)
    {
        var sb = new StringBuilder();
        if (spec.Description is not null)
        {
            sb.AppendLine(spec.Description).AppendLine();
        }

        sb.AppendLine($"**Uso en GA2:** {spec.Usage}. **Certeza del contrato:** {spec.Certainty}.");
        if (spec.Ga2ConfigKey is not null)
        {
            sb.AppendLine($"**Configuración en GA2:** `{spec.Ga2ConfigKey}`.");
        }

        if (spec.Ga2Source is not null)
        {
            sb.AppendLine($"**Cliente en GA2:** `{spec.Ga2Source}`.");
        }

        sb.AppendLine().AppendLine("**Escenarios** (header `X-Mock-Scenario`, `PUT /_mock/scenarios/...` o reglas de configuración):").AppendLine();
        sb.AppendLine("- `default`: respuesta de éxito.");
        foreach (var scenario in spec.Scenarios)
        {
            sb.AppendLine($"- `{scenario.Name}`: {scenario.Description}");
        }

        sb.AppendLine("- `fault` / `http-500`: HTTP 500 (texto / HTML). `slow`: éxito con retardo.");
        return sb.ToString();
    }
}
