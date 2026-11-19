using System.Security;
using System.Xml.Linq;
using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Scenarios;

namespace MockIntegrations.Api.Infrastructure.Soap;

/// <summary>Un servicio SOAP (un .asmx/.svc) con sus operaciones, despachadas por <c>SOAPAction</c>.</summary>
public sealed class SoapService
{
    public required string Provider { get; init; }

    /// <summary>Identificador estable (operationId en OpenAPI).</summary>
    public required string Id { get; init; }

    /// <summary>Nombre del contrato WSDL/WCF (p. ej. <c>SCNMCONTSoap</c>).</summary>
    public required string Contract { get; init; }

    /// <summary>Rutas servidas (la original del proveedor; GA2 sólo cambia host y puerto).</summary>
    public required IReadOnlyList<string> Paths { get; init; }

    public required string Description { get; init; }

    /// <summary>Clave de GA2 que contiene la URL (<c>SvcUtils:*</c>), o el motivo por el que no es configurable.</summary>
    public required string Ga2ConfigKey { get; init; }

    public required IReadOnlyList<SoapOperation> Operations { get; init; }
}

public sealed class SoapOperation
{
    public required string Name { get; init; }

    public required string SoapAction { get; init; }

    public required Ga2Usage Usage { get; init; }

    public required string Summary { get; init; }

    public string? Ga2Source { get; init; }

    public ContractCertainty Certainty { get; init; } = ContractCertainty.Validated;

    /// <summary>Escenarios de negocio documentados (los fixtures adicionales se descubren solos).</summary>
    public IReadOnlyList<ScenarioInfo> Scenarios { get; init; } = [];

    /// <summary>Lógica propia; si es nula se sirve el fixture <c>{Name}.{escenario}.xml</c>.</summary>
    public Func<SoapContext, Task<SoapReply>>? Handler { get; init; }

    public string? Notes { get; init; }
}

/// <summary>Datos de la llamada SOAP en curso.</summary>
public sealed class SoapContext
{
    public required HttpContext Http { get; init; }

    public required SoapService Service { get; init; }

    public required SoapOperation Operation { get; init; }

    public required XDocument Envelope { get; init; }

    /// <summary>Primer hijo de <c>Body</c> (el elemento de la operación).</summary>
    public required XElement Body { get; init; }

    public required ScenarioDecision Scenario { get; init; }

    public required FixtureStore Fixtures { get; init; }

    public required TemplateRenderer Templates { get; init; }

    /// <summary>Valor del primer descendiente del cuerpo con ese nombre local (sin distinguir mayúsculas).</summary>
    public string? Value(string localName) => SoapEnvelope.FindValue(Body, localName);

    /// <summary>Sirve el fixture <c>{Operación}.{escenario}.xml</c> con los marcadores resueltos.</summary>
    public SoapReply Fixture(string? scenario = null)
    {
        var name = scenario ?? Scenario.Name;
        var template = Fixtures.Get(Service.Provider, $"{Operation.Name}.{name}.xml");
        if (template is null)
        {
            return SoapReply.UnknownScenario(Service.Provider, Operation.Name, name);
        }

        return SoapReply.Xml(Templates.Render(template, Value));
    }
}

/// <summary>Respuesta HTTP de un servicio SOAP.</summary>
public sealed record SoapReply(int StatusCode, string Body, string ContentType = SoapEnvelope.ContentType)
{
    public static SoapReply Xml(string envelope) => new(StatusCodes.Status200OK, envelope);

    /// <summary>SOAP 1.1 Fault, como lo genera WCF/ASMX (HTTP 500).</summary>
    public static SoapReply Fault(string message, string faultCode = "s:Server") =>
        new(StatusCodes.Status500InternalServerError,
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><s:Fault>"
            + $"<faultcode>{faultCode}</faultcode><faultstring xml:lang=\"es-CO\">{SecurityElement.Escape(message)}</faultstring>"
            + "</s:Fault></s:Body></s:Envelope>");

    public static SoapReply Html500() =>
        new(StatusCodes.Status500InternalServerError,
            "<html><head><title>Runtime Error</title></head><body><h1>Server Error in '/' Application.</h1><p>Error simulado por MockIntegrations (escenario http-500).</p></body></html>",
            "text/html; charset=utf-8");

    public static SoapReply UnknownScenario(string provider, string operation, string scenario) =>
        Fault($"MockIntegrations: el escenario '{scenario}' no existe para {provider}/{operation}. Consulte GET /_mock/scenarios.", "s:Client");
}

public static class SoapEnvelope
{
    public const string ContentType = "text/xml; charset=utf-8";
    public static readonly XNamespace Soap11 = "http://schemas.xmlsoap.org/soap/envelope/";
    public static readonly XNamespace Soap12 = "http://www.w3.org/2003/05/soap-envelope";

    /// <summary>Extrae la acción del header <c>SOAPAction</c> (SOAP 1.1) o del parámetro <c>action</c> del Content-Type (SOAP 1.2).</summary>
    public static string? ReadAction(HttpRequest request)
    {
        if (request.Headers.TryGetValue("SOAPAction", out var header) && !string.IsNullOrWhiteSpace(header))
        {
            return header.ToString().Trim().Trim('"');
        }

        var contentType = request.ContentType ?? "";
        var idx = contentType.IndexOf("action=", StringComparison.OrdinalIgnoreCase);
        return idx < 0 ? null : contentType[(idx + 7)..].Split(';')[0].Trim().Trim('"');
    }

    public static XElement? ReadBody(XDocument document) =>
        (document.Root?.Element(Soap11 + "Body") ?? document.Root?.Element(Soap12 + "Body"))?.Elements().FirstOrDefault();

    public static string? FindValue(XElement scope, string localName)
    {
        var element = scope.DescendantsAndSelf()
            .FirstOrDefault(e => string.Equals(e.Name.LocalName, localName, StringComparison.OrdinalIgnoreCase));
        if (element is null)
        {
            return null;
        }

        // TransUnion envía XML escapado dentro de <request>: se devuelve el texto tal cual.
        return element.HasElements ? element.ToString(SaveOptions.DisableFormatting) : element.Value;
    }
}
