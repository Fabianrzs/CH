using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Soap;

namespace MockIntegrations.Api.Providers.Vigia;

/// <summary>
/// E4 — Vigía, verificación de terceros en listas LA/FT. Servicio PHP SOAP 1.1 <b>RPC/encoded</b>.
/// Contrato: docs/providers/E04-vigia.md. Los contratos ASMX de Vigía (carga de terceros, productos,
/// transacciones, verificación v3) están registrados en GA2 pero no se usan: no se simulan.
/// </summary>
public static class VigiaMock
{
    public const string Provider = "Vigia";

    public static SoapService Service { get; } = new()
    {
        Provider = Provider,
        Id = "Vigia_ApplicationServicesPortType",
        Contract = "ApplicationServicesPortType",
        Paths = ["/WebSvc/SOAP_VerificaTercero.php"],
        Description = "Verificación de una persona en listas restrictivas (SARLAFT). Estilo RPC/encoded: `F_VERIFICATERCERO(param1, param2, param3)` → `Resultado {Encontrado, Listas, Metodo}`. "
            + "En GA2 la URL es https: use la URL http del mock (el binding elige seguridad por esquema) o exponga HTTPS con un certificado de confianza.",
        Ga2ConfigKey = "SvcUtils:VigiaVerificarTerceroUrlSvc (API GA2)",
        Operations =
        [
            new SoapOperation
            {
                Name = "F_VERIFICATERCERO",
                SoapAction = "https://vigia.cajahonor.gov.co/WebSvc/SOAP_VerificaTercero.php/F_VERIFICATERCERO",
                Usage = Ga2Usage.Business,
                Summary = "param1 = documento, param2 = tipo de verificación, param3 = porcentaje. GA2 devuelve Encontrado/Listas/Metodo al frontend sin interpretarlos.",
                Ga2Source = "Connected Services/ServiceVigiaVerificacionTercero/VerificacionTerceroVigiaBusinessLogic.cs",
                Certainty = ContractCertainty.StructureOnly,
                Scenarios = [new("reportado", "Encontrado=S, Metodo=DOCUMENTO, Listas='OFAC, ONU'.")],
                Notes = "El dominio de Encontrado/Listas/Metodo y de param2 no está confirmado con Vigía (default: Encontrado=N con Listas vacío).",
            },
        ],
    };

    public static void Map(WebApplication app) => app.MapSoapService(Service);
}
