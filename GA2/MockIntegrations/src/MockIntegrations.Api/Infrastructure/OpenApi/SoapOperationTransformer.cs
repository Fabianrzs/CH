using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Scenarios;
using MockIntegrations.Api.Infrastructure.Soap;

namespace MockIntegrations.Api.Infrastructure.OpenApi;

/// <summary>
/// Documenta un endpoint SOAP. OpenAPI no puede distinguir operaciones que comparten ruta y verbo
/// y sólo se diferencian por <c>SOAPAction</c>, así que el endpoint se describe una vez, con la
/// lista de acciones (enum del header), un ejemplo de request y de response por operación y el
/// SOAP Fault. Se documenta como lo que es: SOAP 1.1 sobre HTTP, no REST.
/// </summary>
internal sealed class SoapOperationTransformer(SoapService service, string path)
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var fixtures = context.ApplicationServices.GetRequiredService<FixtureStore>();

        operation.OperationId = path == service.Paths[0] ? service.Id : $"{service.Id}_alias";
        operation.Summary = $"SOAP 1.1 · {service.Contract} ({service.Operations.Count} operaciones)";
        operation.Description = BuildDescription(fixtures);

        operation.Parameters ??= [];
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "SOAPAction",
            In = ParameterLocation.Header,
            Required = true,
            Description = "Acción SOAP 1.1. GA2 la envía entre comillas (p. ej. `\"" + service.Operations[0].SoapAction + "\"`); el mock acepta ambas formas. "
                + "Si falta, el mock despacha por el nombre del primer elemento del `Body`.",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Enum = service.Operations.Select(o => (JsonNode)JsonValue.Create(o.SoapAction)!).ToList(),
            },
        });
        operation.Parameters.Add(ScenarioHeader());

        var requestExamples = new Dictionary<string, IOpenApiExample>();
        var responseExamples = new Dictionary<string, IOpenApiExample>();
        foreach (var op in service.Operations)
        {
            if (fixtures.Get(service.Provider, $"{op.Name}.request.xml") is { } request)
            {
                requestExamples[op.Name] = new OpenApiExample
                {
                    Summary = $"{op.Name} — {op.SoapAction}",
                    Description = "Sobre capturado del proxy WCF real de GA2 con valores de relleno.",
                    Value = JsonValue.Create(request),
                };
            }

            // Las operaciones con lógica propia (TransUnion) publican un ejemplo sólo para documentación.
            if ((fixtures.Get(service.Provider, $"{op.Name}.response-example.xml") ?? fixtures.Get(service.Provider, $"{op.Name}.default.xml")) is { } response)
            {
                responseExamples[op.Name] = new OpenApiExample
                {
                    Summary = $"{op.Name} (escenario default)",
                    Value = JsonValue.Create(TemplatePreview.Render(response)),
                };
            }
        }

        operation.RequestBody = new OpenApiRequestBody
        {
            Required = true,
            Description = "Sobre SOAP 1.1 (`Content-Type: text/xml; charset=utf-8`). Elija en *Examples* la operación y fije el header `SOAPAction` correspondiente.",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["text/xml"] = new()
                {
                    Schema = new OpenApiSchema { Type = JsonSchemaType.String, Description = "Sobre SOAP 1.1" },
                    Examples = requestExamples,
                },
            },
        };

        operation.Responses = new OpenApiResponses
        {
            ["200"] = new OpenApiResponse
            {
                Description = "Respuesta SOAP de la operación (`{Operación}Response`). Ejemplos del escenario `default`; los marcadores dinámicos aparecen con valores ilustrativos.",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["text/xml"] = new() { Schema = new OpenApiSchema { Type = JsonSchemaType.String }, Examples = responseExamples },
                },
            },
            ["400"] = new OpenApiResponse
            {
                Description = "El cuerpo no es XML válido.",
                Content = FaultContent("MockIntegrations: el cuerpo no es XML válido (...)", "s:Client"),
            },
            ["500"] = new OpenApiResponse
            {
                Description = "SOAP Fault (escenario `fault`, acción desconocida, escenario inexistente u operación sin uso en GA2). "
                    + "Con el escenario `http-500` el cuerpo es HTML (`text/html`), sin sobre SOAP.",
                Content = FaultContent("Error técnico simulado por MockIntegrations.", "s:Server"),
            },
        };

        return Task.CompletedTask;
    }

    internal static OpenApiParameter ScenarioHeader() => new()
    {
        Name = ScenarioResolver.ScenarioHeader,
        In = ParameterLocation.Header,
        Required = false,
        Description = "Sólo para pruebas manuales: fuerza un escenario (GA2 nunca envía este header). "
            + "Integrados: `default`, `fault`, `http-500`, `slow`. Los de negocio de cada operación se listan en la descripción y en `GET /_mock/scenarios`.",
        Schema = new OpenApiSchema { Type = JsonSchemaType.String },
    };

    private static Dictionary<string, OpenApiMediaType> FaultContent(string message, string code) => new()
    {
        ["text/xml"] = new()
        {
            Schema = new OpenApiSchema { Type = JsonSchemaType.String },
            Example = JsonValue.Create(SoapReply.Fault(message, code).Body),
        },
    };

    private string BuildDescription(FixtureStore fixtures)
    {
        var sb = new StringBuilder();
        sb.AppendLine(service.Description).AppendLine();
        sb.AppendLine($"**Protocolo:** SOAP 1.1 sobre HTTP (`POST`, `text/xml; charset=utf-8`), despacho por `SOAPAction`. **Contrato:** `{service.Contract}`.");
        sb.AppendLine($"**Configuración en GA2:** `{service.Ga2ConfigKey}`.").AppendLine();
        if (service.Paths.Count > 1)
        {
            sb.AppendLine($"**Rutas equivalentes:** {string.Join(", ", service.Paths.Select(p => $"`{p}`"))}.").AppendLine();
        }

        sb.AppendLine("| Operación | SOAPAction | Uso en GA2 | Escenarios | Descripción |");
        sb.AppendLine("|---|---|---|---|---|");
        foreach (var op in service.Operations)
        {
            var scenarios = fixtures.ScenariosFor(service.Provider, op.Name)
                .Concat(op.Scenarios.Select(s => s.Name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(s => $"`{s}`");
            sb.AppendLine($"| `{op.Name}` | `{op.SoapAction}` | {op.Usage} | {string.Join(" ", scenarios)} | {op.Summary.Replace("|", "\\|")} |");
        }

        sb.AppendLine().AppendLine("Todas las operaciones admiten además los escenarios integrados `fault`, `http-500` y `slow`. "
            + "Las marcadas `NotUsed` no las invoca GA2 y responden un SOAP Fault explícito.");
        return sb.ToString();
    }
}
