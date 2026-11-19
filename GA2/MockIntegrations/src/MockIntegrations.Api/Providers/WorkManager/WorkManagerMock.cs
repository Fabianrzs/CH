using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Soap;

namespace MockIntegrations.Api.Providers.WorkManager;

/// <summary>
/// E2 — WorkManager / DODO: servicio WCF <c>IWorkManagerServices</c> (basicHttpBinding, DataContractSerializer).
/// Contrato: docs/providers/E02-workmanager.md. Los DTO usan el namespace
/// <c>http://schemas.datacontract.org/2004/07/WorkManager.Entities</c> y orden alfabético estricto.
/// </summary>
public static class WorkManagerMock
{
    public const string Provider = "WorkManager";
    private const string ActionPrefix = "http://tempuri.org/IWorkManagerServices/";
    private const string Source = "ReferenciasSvc/WorkManager/ConsumoWorkManager.cs → ServiciosSOA/WorkManager/WorkManagerService.cs";

    private static readonly ScenarioInfo Fallido = new("fallido", "Success=false con Message; GA2 informa 'El formulario se ha encolado para su procesamiento'.");
    private static readonly ScenarioInfo ResultadoNulo = new("resultado-nulo", "Result con i:nil=\"true\": reproduce el NullReferenceException de WorkManagerService (→ HTTP 409 en GA2).");

    public static SoapService Service { get; } = new()
    {
        Provider = Provider,
        Id = "WorkManager_IWorkManagerServices",
        Contract = "IWorkManagerServices",
        Paths = ["/APIV2_0005/WorkManagerService.svc"],
        Description = "Gestor documental y BPM de Caja Honor (radicación, adjuntos, formularios). "
            + "El Header {User, Token} viaja dentro del DTO (CHKeys:User/Token); el mock acepta cualquier valor.",
        Ga2ConfigKey = "SvcUtils:WorkManagerUrlSvc",
        Operations =
        [
            Op("Form_Insert", Ga2Usage.Business, "Crea el radicado. GA2 guarda BarCode ('<FormCode>-<consecutivo>'; vacío → 'RADICADO NO DISPONIBLE').", Fallido, ResultadoNulo),
            Op("Form_Update", Ga2Usage.Business, "Actualiza un registro del formulario (filtro FilterParameters).", Fallido, ResultadoNulo),
            Op("Form_AttachFiles", Ga2Usage.Business, "Adjunta archivos Base64 a un registro (operación más invocada). Devuelve FileCodes.", Fallido, ResultadoNulo),
            Op("Workflow_AttachFiles", Ga2Usage.Business, "Adjunta archivos al workflow (SOAP directo y manejador de Service Bus).", Fallido, ResultadoNulo),
            Op("Form_GetParameters", Ga2Usage.Business, "Definición del formulario: GA2 arma el enlace DODO con Form.IdFormulario y Form.NombreCampoSticker.",
                Fallido, ResultadoNulo, new ScenarioInfo("sin-formulario", "Form nulo: reproduce el NullReferenceException de ObtenerEnlaceDodo.")),
            Op("Form_GetData", Ga2Usage.PassThrough, "Consulta datos del formulario (endpoint api/credito/getdataforms).", Fallido, ResultadoNulo),
            Op("Form_Delete", Ga2Usage.Business, "Borra registros; sólo desde el manejador de Service Bus WorkManagerDeleteRequest.", Fallido, ResultadoNulo),
            Op("Form_GetFileCodes", Ga2Usage.NotUsed, "Envuelta por GA2 sin consumidores activos.", Fallido, ResultadoNulo),
            Op("Form_GetBase64StringFile", Ga2Usage.NotUsed, "Envuelta por GA2 sin consumidores activos.", Fallido, ResultadoNulo),
            Op("Workflow_Start", Ga2Usage.NotUsed, "Envuelta por GA2 sin consumidores activos.", Fallido, ResultadoNulo),
            Op("Workflow_RunTask", Ga2Usage.NotUsed, "Envuelta por GA2 sin consumidores activos.", Fallido, ResultadoNulo),
            Op("List_GetItems", Ga2Usage.NotUsed, "Envuelta por GA2 sin consumidores activos.", Fallido, ResultadoNulo),
            .. new[]
            {
                "Form_GetFilesDirectories", "ExternalUser_Insert", "ExternalUser_GetByEmailID", "Externaluser_GetByActivationCode",
                "ExternalUser_VerifyAccount", "ExternalUser_ChangePassword", "File_Delete", "Workflow_GetCases", "Workflow_GetTasks",
                "Field_GetValues", "CC_GetService", "CC_ProcessExist", "CC_InserProcess", "CC_UpdateProcess", "CC_ReadConfig",
                "CC_GetPendingProcesses", "Workflow_CreateLink",
            }.Select(NotUsed),
        ],
    };

    public static void Map(WebApplication app) => app.MapSoapService(Service);

    private static SoapOperation Op(string name, Ga2Usage usage, string summary, params ScenarioInfo[] scenarios) => new()
    {
        Name = name,
        SoapAction = ActionPrefix + name,
        Usage = usage,
        Summary = summary,
        Scenarios = scenarios,
        Ga2Source = Source,
    };

    private static SoapOperation NotUsed(string name) => new()
    {
        Name = name,
        SoapAction = ActionPrefix + name,
        Usage = Ga2Usage.NotUsed,
        Summary = "En el contrato WCF, sin cliente en GA2. Responde SOAP Fault.",
        Certainty = ContractCertainty.NotImplemented,
    };
}
