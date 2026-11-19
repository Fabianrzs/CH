using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Scenarios;
using MockIntegrations.Api.Infrastructure.Soap;

namespace MockIntegrations.Api.Providers.Bua;

/// <summary>
/// E3 — BUA SOAP (Base Única de Afiliados), contrato WCF <c>IBUA_API.ObtenerBUA</c>.
/// GA2 tiene dos proxies (ws.01 y ws.02) con pequeñas diferencias de tipos; un único fixture sirve
/// a ambos (enteros en <c>valor</c> y <c>tasaAporte</c>). Contrato: docs/providers/E03-bua.md.
/// </summary>
public static class BuaMock
{
    public const string Provider = "Bua";

    public static SoapService Service { get; } = new()
    {
        Provider = Provider,
        Id = "Bua_IBUA_API",
        Contract = "IBUA_API",
        Paths =
        [
            "/Kriterion.ws.02/BaseUnicaAfiliados/BUA_API.svc",
            "/Kriterion.ws.01/BaseUnicaAfiliados/BUA_API.svc",
        ],
        Description = "Ficha completa del afiliado (datos personales, residencia, cónyuge, cuentas, aportes, embargos, bloqueos). "
            + "ws.02 se redirige con SvcUtils:BuaUrlServiceApi; ws.01 tiene la URL fija en Reference.cs (requiere DNS/hosts hacia el mock).",
        Ga2ConfigKey = "SvcUtils:BuaUrlServiceApi (ws.02). ws.01: URL fija http://cpvmp-09x/Kriterion.ws.01/... (ConsumoBua.cs ignora SvcUtils:BuaUrlSvc)",
        Operations =
        [
            new SoapOperation
            {
                Name = "ObtenerBUA",
                SoapAction = "http://tempuri.org/IBUA_API/ObtenerBUA",
                Usage = Ga2Usage.Business,
                Summary = "Consulta del afiliado por identificación y tipo_Id. GA2 reintenta 1 → 4 → 5 cuando el resultado es nulo.",
                Ga2Source = "ReferenciasSvc/Bua/ConsumoBua.cs (ws.01); Connected Services/ServiceReference1/BuaService2BusinessLogic.cs (ws.02)",
                Scenarios =
                [
                    new("no-encontrado", "ObtenerBUAResult nulo: GA2 lo trata como afiliado inexistente y prueba el siguiente tipo_Id."),
                    new("encontrado-tipo-4", "Nulo para tipo_Id distinto de 4 y ficha completa para 4: ejercita el reintento 1 → 4 de GA2."),
                ],
                Handler = Handle,
            },
        ],
    };

    public static void Map(WebApplication app) => app.MapSoapService(Service);

    private static Task<SoapReply> Handle(SoapContext context)
    {
        var scenario = context.Scenario.Name switch
        {
            "encontrado-tipo-4" => context.Value("tipo_Id") == "4" ? BuiltInScenarios.Default : "no-encontrado",
            var other => other,
        };
        return Task.FromResult(context.Fixture(scenario));
    }
}
