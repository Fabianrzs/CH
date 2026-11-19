using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Journal;
using MockIntegrations.Api.Infrastructure.OpenApi;
using MockIntegrations.Api.Infrastructure.Scenarios;
using MockIntegrations.Api.Providers;

namespace MockIntegrations.Api.Infrastructure.Soap;

public static class SoapEndpoints
{
    /// <summary>
    /// Publica un servicio SOAP: <c>POST {ruta}</c> despacha por <c>SOAPAction</c> y <c>GET {ruta}</c>
    /// devuelve una página de ayuda (no forma parte del contrato). Registra las operaciones en el catálogo.
    /// </summary>
    public static void MapSoapService(this WebApplication app, SoapService service)
    {
        var catalog = app.Services.GetRequiredService<MockCatalog>();
        foreach (var op in service.Operations)
        {
            catalog.Add(new MockOperation
            {
                Provider = service.Provider,
                Name = op.Name,
                Protocol = MockProtocol.Soap11,
                Method = "POST",
                Route = service.Paths[0],
                SoapAction = op.SoapAction,
                Usage = op.Usage,
                Summary = op.Summary,
                Scenarios = op.Scenarios,
                Ga2ConfigKey = service.Ga2ConfigKey,
                Ga2Source = op.Ga2Source,
                Certainty = op.Certainty,
                Notes = op.Notes,
            });
        }

        for (var i = 0; i < service.Paths.Count; i++)
        {
            var path = service.Paths[i];
            // Firma de Delegate (no RequestDelegate) para que el endpoint aparezca en OpenAPI.
            app.MapPost(path, (HttpContext http, CancellationToken _) => DispatchAsync(http, service))
                .WithName(i == 0 ? service.Id : $"{service.Id}_{i}")
                .WithTags(service.Provider)
                .WithGroupName(service.Provider.ToLowerInvariant())
                .AddOpenApiOperationTransformer(new SoapOperationTransformer(service, path).TransformAsync);

            app.MapGet(path, (HttpContext http) => HelpPage(http, service))
                .ExcludeFromDescription();
        }
    }

    private static async Task DispatchAsync(HttpContext http, SoapService service)
    {
        var fixtures = http.RequestServices.GetRequiredService<FixtureStore>();
        var templates = http.RequestServices.GetRequiredService<TemplateRenderer>();
        var resolver = http.RequestServices.GetRequiredService<ScenarioResolver>();
        var delays = http.RequestServices.GetRequiredService<MockDelays>();

        string raw;
        using (var reader = new StreamReader(http.Request.Body, Encoding.UTF8))
        {
            raw = await reader.ReadToEndAsync(http.RequestAborted);
        }

        XDocument document;
        try
        {
            document = XDocument.Parse(raw);
        }
        catch (XmlException ex)
        {
            await WriteAsync(http, SoapReply.Fault($"MockIntegrations: el cuerpo no es XML válido ({ex.Message}).", "s:Client") with { StatusCode = 400 });
            return;
        }

        var body = SoapEnvelope.ReadBody(document);
        var action = SoapEnvelope.ReadAction(http.Request);
        var operation = service.Operations.FirstOrDefault(o => string.Equals(o.SoapAction, action, StringComparison.OrdinalIgnoreCase))
            ?? (action is null && body is not null
                ? service.Operations.FirstOrDefault(o => string.Equals(o.Name, body.Name.LocalName, StringComparison.OrdinalIgnoreCase))
                : null);

        if (body is null || operation is null)
        {
            // Mismo comportamiento que WCF ante una acción desconocida (ActionNotSupported).
            MockCallInfo.Set(http, service.Provider, action ?? "(sin SOAPAction)", "action-not-supported");
            await WriteAsync(http, SoapReply.Fault(
                $"The message with Action '{action}' cannot be processed at the receiver. MockIntegrations: {service.Contract} no tiene esa operación.",
                "s:Client"));
            return;
        }

        var decision = resolver.Resolve(http, service.Provider, operation.Name, name => SoapEnvelope.FindValue(body, name));
        MockCallInfo.Set(http, service.Provider, operation.Name, decision.Name, decision.Source.ToString());

        await delays.ApplyAsync(service.Provider, decision.Name, http.RequestAborted);
        if (decision.Name == BuiltInScenarios.Slow)
        {
            decision = decision with { Name = BuiltInScenarios.Default };
        }

        var context = new SoapContext
        {
            Http = http,
            Service = service,
            Operation = operation,
            Envelope = document,
            Body = body,
            Scenario = decision,
            Fixtures = fixtures,
            Templates = templates,
        };

        var reply = decision.Name switch
        {
            BuiltInScenarios.Fault => SoapReply.Fault($"Error técnico simulado por MockIntegrations en {service.Provider}/{operation.Name}."),
            BuiltInScenarios.Http500 => SoapReply.Html500(),
            _ when operation.Handler is not null => await operation.Handler(context),
            _ when operation.Usage == Ga2Usage.NotUsed && fixtures.Get(service.Provider, $"{operation.Name}.default.xml") is null =>
                SoapReply.Fault($"MockIntegrations: {operation.Name} existe en el contrato {service.Contract}, pero GA2 no la invoca y no tiene un contrato de respuesta validado. Ver docs/pendientes.md."),
            _ => context.Fixture(),
        };

        await WriteAsync(http, reply);
    }

    private static async Task WriteAsync(HttpContext http, SoapReply reply)
    {
        http.Response.StatusCode = reply.StatusCode;
        http.Response.ContentType = reply.ContentType;
        await http.Response.WriteAsync(reply.Body, Encoding.UTF8, http.RequestAborted);
    }

    private static IResult HelpPage(HttpContext http, SoapService service)
    {
        // Swagger UI identifica cada documento por su título.
        var swaggerName = ProviderRegistry.All.FirstOrDefault(p => p.Key == service.Provider)?.Title ?? service.Provider;
        var rows = string.Join("", service.Operations.Select(o =>
            $"<tr><td>{WebUtility.HtmlEncode(o.Name)}</td><td><code>{WebUtility.HtmlEncode(o.SoapAction)}</code></td><td>{o.Usage}</td><td>{WebUtility.HtmlEncode(o.Summary)}</td></tr>"));
        var html = $$"""
            <!doctype html><html lang="es"><head><meta charset="utf-8"><title>{{service.Contract}} · MockIntegrations</title>
            <style>body{font-family:system-ui,sans-serif;margin:2rem;max-width:72rem}td,th{border-bottom:1px solid #ddd;padding:.4rem;text-align:left;vertical-align:top}table{border-collapse:collapse}code{font-size:.85em}</style></head>
            <body><h1>{{service.Provider}} — {{service.Contract}} (mock)</h1>
            <p>{{WebUtility.HtmlEncode(service.Description)}}</p>
            <p>Servicio SOAP 1.1: <code>POST {{WebUtility.HtmlEncode(http.Request.Path)}}</code> con <code>Content-Type: text/xml; charset=utf-8</code> y el header <code>SOAPAction</code>.
            Clave en GA2: <code>{{WebUtility.HtmlEncode(service.Ga2ConfigKey)}}</code>. El mock no publica WSDL: GA2 usa proxies precompilados (<code>Reference.cs</code>).</p>
            <p>Documentación OpenAPI: <a href="/swagger/index.html?urls.primaryName={{Uri.EscapeDataString(swaggerName)}}">/swagger</a></p>
            <table><tr><th>Operación</th><th>SOAPAction</th><th>Uso en GA2</th><th>Descripción</th></tr>{{rows}}</table></body></html>
            """;
        return Results.Content(html, "text/html; charset=utf-8");
    }
}
