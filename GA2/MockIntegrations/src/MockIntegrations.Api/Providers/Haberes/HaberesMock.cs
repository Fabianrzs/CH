using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Soap;

namespace MockIntegrations.Api.Providers.Haberes;

/// <summary>
/// E6 — Haberes, Modelo Canónico de Datos (<c>wsModeloCanonico.asmx</c>). Contrato: docs/providers/E06-haberes.md.
/// El DataSet de <c>ReporteHaberes</c> se sirve minificado: GA2 lee <c>Nodes[1].FirstNode</c> y falla si hay espacios.
/// </summary>
public static class HaberesMock
{
    public const string Provider = "Haberes";
    private const string Source = "ReferenciasSvc/Haberes/ConsumoHaberes.cs → Credito/Haberes/HaberesBusinessLogic.cs";

    private static readonly ScenarioInfo SinReporte = new("sin-reporte", "Status=false sin Report: GA2 deja el Base64 nulo y el guardado en Blob falla aguas abajo.");

    public static SoapService Service { get; } = new()
    {
        Provider = Provider,
        Id = "Haberes_wsModeloCanonicoDatosSoap",
        Contract = "wsModeloCanonicoDatosSoap",
        Paths = ["/MCDIntegrationServices/wsModeloCanonico.asmx"],
        Description = "Reportes y certificados de haberes. Ojo: con GA2 compilado en DEBUG, GenerarReporte va por la pasarela ktrionApi (`api/generarreportehaberes`), no por este servicio.",
        Ga2ConfigKey = "SvcUtils:HaberesUrlSvc",
        Operations =
        [
            Op("GenerarReporte", Ga2Usage.Business, "sReporte, customerIdentification, accountId → ServiceResponse {Message, Report (PDF), Status}. Sólo en Release.", SinReporte),
            Op("GenerarReporteV2", Ga2Usage.Business, "Igual que GenerarReporte con month y year.", SinReporte),
            Op("ReporteHaberes", Ga2Usage.Business, "DataSet REPORTE_HABERES con los datos del certificado (minificado).",
                new ScenarioInfo("vacio", "Sólo xs:schema (Nodes.Count = 1): GA2 no arma el reporte.")),
            .. new[] { "CustomerAccount", "DetalleMovimiento", "DetalleBloqueos", "ExtractoGeneral", "DeclaracionRenta" }
                .Select(name => new SoapOperation
                {
                    Name = name,
                    SoapAction = "http://tempuri.org/" + name,
                    Usage = Ga2Usage.NotUsed,
                    Summary = "En el contrato, sin llamadas en GA2. Responde SOAP Fault.",
                    Certainty = ContractCertainty.NotImplemented,
                }),
        ],
    };

    public static void Map(WebApplication app) => app.MapSoapService(Service);

    private static SoapOperation Op(string name, Ga2Usage usage, string summary, params ScenarioInfo[] scenarios) => new()
    {
        Name = name,
        SoapAction = "http://tempuri.org/" + name,
        Usage = usage,
        Summary = summary,
        Scenarios = scenarios,
        Ga2Source = Source,
    };
}
