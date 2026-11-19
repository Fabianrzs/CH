using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Soap;

namespace MockIntegrations.Api.Providers.Fenix;

/// <summary>
/// E5 — Fénix, validación biométrica. ASMX que devuelve un DataSet .NET (xs:schema + diffgram);
/// GA2 lee <c>ValidarBiometriaResult.Nodes[1]</c> y lo deserializa como <c>diffgram</c>.
/// Contrato: docs/providers/E05-fenix.md.
/// </summary>
public static class FenixMock
{
    public const string Provider = "Fenix";

    public static SoapService Service { get; } = new()
    {
        Provider = Provider,
        Id = "Fenix_WebServicesBiometriasSoap",
        Contract = "WebServicesBiometriasSoap",
        // GA2 configura la URL con "?WSDL" y la usa como dirección: la query string se ignora.
        Paths = ["/WsFenixBiometria/WebServicesBiometrias.asmx"],
        Description = "Evidencia biométrica del afiliado (cédula, rostro, huella, firma). La URL de GA2 termina en `?WSDL` y se usa tal cual como endpoint: el mock la acepta.",
        Ga2ConfigKey = "SvcUtils:FenixServiceSvc",
        Operations =
        [
            new SoapOperation
            {
                Name = "ValidarBiometria",
                SoapAction = "http://tempuri.org/ValidarBiometria",
                Usage = Ga2Usage.Business,
                Summary = "identificacion (xs:int) → DataSet Table1 con A001* (datos) y A008* (xs:unsignedByte, 1 = evidencia existe).",
                Ga2Source = "ReferenciasSvc/Fenix/ConsumoFenix.cs → Credito/Fenix/FenixBusinessLogic.cs",
                Scenarios =
                [
                    new("parcial", "A008HUELLA=0 (biometría incompleta)."),
                    new("sin-datos", "diffgram sin NewDataSet: GA2 responde 'No hay información en Fénix' y crea la notificación ASERA_T."),
                    new("dato-invalido", "A008HUELLA='X': la deserialización falla y GA2 responde 'Usuario sin datos en Fenix'."),
                ],
            },
            new SoapOperation
            {
                Name = "xxValidarBiometria",
                SoapAction = "http://tempuri.org/xxValidarBiometria",
                Usage = Ga2Usage.NotUsed,
                Summary = "En el contrato, sin llamadas en GA2. Responde SOAP Fault.",
                Certainty = ContractCertainty.NotImplemented,
            },
        ],
    };

    public static void Map(WebApplication app) => app.MapSoapService(Service);
}
