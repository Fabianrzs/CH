using MockIntegrations.Api.Infrastructure.Catalog;
using MockIntegrations.Api.Infrastructure.Fixtures;
using MockIntegrations.Api.Infrastructure.Rest;
using MockIntegrations.Api.Providers.Dodo.Models;

namespace MockIntegrations.Api.Providers.Dodo;

/// <summary>
/// E9 — Dodo REST (nueva API del gestor documental, <c>dodo-docs-test:10081</c>). Los modelos son copia
/// de los DTO de GA2 (<c>ReferenciasSvc/Dodo/**/Models</c>). En GA2 sólo la usa <c>DodoController</c>
/// (pass-through en <c>api/integrations/dodo/*</c>). Contrato: docs/providers/E09-dodo-rest.md.
/// Rutas servidas bajo <c>/dodo/</c>: en GA2, <c>Integrationes:Dodo:BaseUrl = http://&lt;mock&gt;/dodo/</c>.
/// </summary>
public static class DodoMock
{
    public const string Provider = "Dodo";
    public const string Prefix = "/dodo/";
    private const string ConfigKey = "Integrationes:Dodo:BaseUrl";

    private static readonly ScenarioInfo Fallido = new("fallido", "HTTP 200 con success=false y message: GA2 devuelve IsSuccess=true con esos datos.");
    private static readonly ScenarioInfo NoEncontrado = new("no-encontrado", "HTTP 404 con texto: GA2 devuelve IsSuccess=false con el cuerpo (salvo en GET api/version, donde es éxito con Data=null).");

    public static void Map(WebApplication app)
    {
        var versionSpec = Spec("Version", "Versión de la API Dodo", "GA2 deserializa la respuesta como `string` JSON: el mock responde una cadena entre comillas.",
            "ReferenciasSvc/Dodo/Version/Services/DodoVersionService.cs", Ga2Usage.PassThrough, [NoEncontrado]);
        app.MapMock("GET", Prefix + "api/version", versionSpec, async (HttpContext http) =>
            {
                var decision = await RestMock.BeginAsync(http, versionSpec, RestMock.Lookup(http));
                return RestMock.BuiltIn(decision, versionSpec) ?? decision.Name switch
                {
                    "default" => Results.Json("1.4.2-mock"),
                    "no-encontrado" => Results.NotFound(),
                    _ => RestMock.UnknownScenario(decision, versionSpec),
                };
            })
            .Produces<string>()
            .Produces(StatusCodes.Status404NotFound);

        Post<FieldGetValuesRequest, FieldGetValuesResponse>(app, "api/Field/Field_GetValues", "Field_GetValues", "Valores de un campo de lista del formulario", "Field",
            (req, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, Items = [new() { Id = 1, Value = "11001", Text = "BOGOTA D.C.", ToolTipText = null }] });
        Post<ListGetItemsRequest, ListGetItemsResponse>(app, "api/Lists/List_GetItems", "List_GetItems", "Ítems de una lista parametrizada", "Field",
            (req, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, Data = [new() { IdItem = 1, IdLista = req.ListId, Codigo = "01", Nombre = "ITEM DE PRUEBA", IdItemPadre = req.FatherItemId, Activo = true, Usuario = "mock", Fecha = Today }] });

        Post<FileCopyToEPRequest, FileSimpleResponse>(app, "api/File/File_CopyToEP", "File_CopyToEP", "Copia un archivo al expediente", "File", (_, _) => Ok<FileSimpleResponse>("Archivo copiado"));
        Post<FileCopyToFatherRequest, FileSimpleResponse>(app, "api/File/File_CopyToFather", "File_CopyToFather", "Copia un archivo al registro padre", "File", (_, _) => Ok<FileSimpleResponse>("Archivo copiado"));
        Post<FileDeleteRequest, FileSimpleResponse>(app, "api/File/File_Delete", "File_Delete", "Elimina un archivo", "File", (_, _) => Ok<FileSimpleResponse>("Archivo eliminado"));

        Post<FormGetParametersRequest, FormGetParametersResponse>(app, "api/Forms/Form_GetParameters", "Form_GetParameters", "Definición del formulario", "Forms",
            (req, _) => new()
            {
                Success = true, Message = "Consulta exitosa",
                Form = new() { IdFormulario = 83, Tabla = "F083", Nombre = "SOLICITUDES KRITERION", Descripcion = "Formulario de prueba (mock)", Estado = 1, Codigo = req.FormCode ?? "83", Usuario = "mock", Fecha = Today, Host = "mock", ArchivoRequerido = false, WorkflowUnico = true },
            });
        Post<FormInsertRequest, DodoFileOperationResponse>(app, "api/Forms/Form_Insert", "Form_Insert", "Crea un registro (radicado) en el formulario", "Forms",
            (req, seq) => FileOperation(seq, $"{(string.IsNullOrEmpty(req.FormCode) ? "83" : req.FormCode)}-{seq.Next("dodo.barcode", 123):D6}", req.Files?.Count ?? 0, "Registro creado", newWorkflow: req.CheckWorkflow == true));
        Post<FormUpdateRequest, FormUpdateResponse>(app, "api/Forms/Form_Update", "Form_Update", "Actualiza registros del formulario", "Forms",
            (_, _) => new() { Success = true, Message = "Registro actualizado", Rows = 1 });
        Post<FormDeleteRequest, FormDeleteResponse>(app, "api/Forms/Form_Delete", "Form_Delete", "Elimina registros del formulario", "Forms",
            (_, _) => new() { Success = true, Message = "Registro eliminado", Rows = 1 });
        Post<FormGetDataRequest, FormGetDataResponse>(app, "api/Forms/Form_GetData", "Form_GetData", "Consulta datos del formulario", "Forms",
            (_, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, Data = [new() { Field = "C0089", Value = "1020304050", Key = "NumeroDocumento" }, new() { Field = "C0002", Value = "JUAN CARLOS PEREZ GOMEZ", Key = "NombreCliente" }] });
        Post<FormAttachFilesRequest, DodoFileOperationResponse>(app, "api/Forms/Form_AttachFiles", "Form_AttachFiles", "Adjunta archivos a un registro", "Forms",
            (req, seq) => FileOperation(seq, req.BarCode, req.Files.Count, "Archivos adjuntados", newWorkflow: false));
        Post<FormGetFileCodesRequest, FormGetFileCodesResponse>(app, "api/Forms/Form_GetFileCodes", "Form_GetFileCodes", "Archivos de un registro", "Forms",
            (_, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, FileProperties = [SampleFile("FC-000001")] });
        Post<FormGetFilesDirectoriesRequest, FormGetFilesDirectoriesResponse>(app, "api/Forms/Form_GetFilesDirectories", "Form_GetFilesDirectories", "Directorios documentales del formulario", "Forms",
            (_, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, Data = [new() { IdDirectorio = 1, IdFormulario = 83, Codigo = "00", Nombre = "GENERAL", IdEstado = 1 }] });
        Post<FormGetBase64StringFileRequest, FormGetBase64StringFileResponse>(app, "api/Forms/Form_GetBase64StringFile", "Form_GetBase64StringFile", "Metadatos de un archivo por código", "Forms",
            (req, _) => new() { Success = true, Message = "Consulta exitosa", File = SampleFile(req.FileCode ?? "FC-000001") });

        Post<SendEmailRequest, SendEmailResponse>(app, "api/Notification/SendEmail", "SendEmail", "Envía un correo (Dodo)", "Notification", (_, _) => Ok<SendEmailResponse>("Correo enviado"));
        Post<GetOfficesRequest, GetOfficesResponse>(app, "api/Offices/GetOffices", "GetOffices", "Oficinas", "Offices",
            (_, _) => new() { Success = true, Message = "Consulta exitosa", OfficeList = [new() { OfficeId = 1, Code = "001", Name = "BOGOTA - SEDE PRINCIPAL" }, new() { OfficeId = 2, Code = "002", Name = "MEDELLIN" }] });
        Post<GetTokenRequest, GetTokenResponse>(app, "api/Token/GetToken", "GetToken", "Token de la API Dodo", "Token",
            (req, _) => new() { Success = true, Message = "Token generado", User = req.User, Token = $"mock-dodo-token-{Guid.NewGuid():N}", TokenDate = DateTime.Now.ToString("s") });
        Post<UserInfoRequest, UserInfoResponse>(app, "api/User/User_Info", "User_Info", "Datos de un usuario", "User",
            (req, _) => new() { Success = true, Message = "Consulta exitosa", UserInfo = new() { UserName = req.UserName, FullName = "USUARIO DE PRUEBA KRITERION", Email = "usuario.prueba@example.com" } });

        Post<WorkflowStartRequest, WorkflowStartResponse>(app, "api/Workflow/Workflow_Start", "Workflow_Start", "Inicia un workflow sobre un radicado", "Workflow",
            (req, seq) => new() { Success = true, Message = "Workflow iniciado", WorkflowId = (int)seq.Next("dodo.workflowid", 9981), FileCodes = Codes(seq, req.Files?.Count ?? 0) });
        Post<WorkflowRunTaskRequest, WorkflowSimpleResponse>(app, "api/Workflow/Workflow_RunTask", "Workflow_RunTask", "Ejecuta un paso del workflow", "Workflow", (_, _) => Ok<WorkflowSimpleResponse>("Tarea ejecutada"));
        Post<WorkflowReturnTaskRequest, WorkflowSimpleResponse>(app, "api/Workflow/Workflow_ReturnTask", "Workflow_ReturnTask", "Devuelve el workflow a un paso anterior", "Workflow", (_, _) => Ok<WorkflowSimpleResponse>("Tarea devuelta"));
        Post<WorkflowAbortProcessRequest, WorkflowSimpleResponse>(app, "api/Workflow/Workflow_AbortProcess", "Workflow_AbortProcess", "Aborta el proceso", "Workflow", (_, _) => Ok<WorkflowSimpleResponse>("Proceso abortado"));
        Post<WorkflowAttachFilesRequest, DodoFileOperationResponse>(app, "api/Workflow/Workflow_AttachFiles", "Workflow_AttachFiles", "Adjunta archivos al workflow", "Workflow",
            (req, seq) => FileOperation(seq, null, req.Files.Count, "Archivos adjuntados", newWorkflow: false, workflowId: req.WorkflowId));
        Post<WorkflowGetCasesRequest, WorkflowGetCasesResponse>(app, "api/Workflow/Workflow_GetCases", "Workflow_GetCases", "Casos de workflow", "Workflow",
            (req, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, WorkflowCases = [new() { WorkflowId = 9981, State = "EN CURSO", CreateDate = Today, FinishDate = null, ProcessId = req.ProcessId, RecordId = 4521 }] });
        Post<WorkflowGetTasksRequest, WorkflowGetTasksResponse>(app, "api/Workflow/Workflow_GetTasks", "Workflow_GetTasks", "Tareas de un workflow", "Workflow",
            (_, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, Tasks = [new() { Step = 1, Description = "RADICACION", State = "PENDIENTE", Priority = "NORMAL", OwnerUser = "mock", CreateDate = Today, ExpirationDate = Today.AddDays(5) }] });
        Post<WorkflowGetFileCodesRequest, WorkflowGetFileCodesResponse>(app, "api/Workflow/Workflow_GetFileCodes", "Workflow_GetFileCodes", "Archivos de un workflow", "Workflow",
            (_, _) => new() { Success = true, Message = "Consulta exitosa", Rows = 1, FileProperties = [SampleFile("FC-000001")] });
        Post<WorkflowStepsForReturnRequest, WorkflowStepsForReturnResponse>(app, "api/Workflow/Workflow_StepsForReturn", "Workflow_StepsForReturn", "Pasos a los que se puede devolver", "Workflow",
            (_, _) => new() { Success = true, Message = "Consulta exitosa", StepList = [new() { Paso = 1, Descripcion = "RADICACION", IdProceso = 10 }] });
        Post<ProcessParametersRequest, ProcessParametersResponse>(app, "api/Workflow/Process_Parameters", "Process_Parameters", "Definición del proceso", "Workflow",
            (req, _) => new() { Success = true, Message = "Consulta exitosa", Parameters = new() { IdProceso = req.ProcessId, Proceso = "PROCESO DE PRUEBA", Descripcion = "Proceso (mock)", Estado = 1, Version = "1", Usuario = "mock", Fecha = Today, Host = "mock", IdFormulario = 83 } });
    }

    private static DateTime Today => DateTime.Today;

    private static RestOperationSpec Spec(string name, string summary, string? description, string source, Ga2Usage usage, IReadOnlyList<ScenarioInfo> scenarios) => new()
    {
        Provider = Provider,
        Name = name,
        Usage = usage,
        Summary = summary,
        Description = description,
        Scenarios = scenarios,
        Ga2ConfigKey = ConfigKey,
        Ga2Source = source,
    };

    /// <summary>POST JSON → JSON, con los escenarios comunes de Dodo (default, fallido, no-encontrado).</summary>
    private static void Post<TRequest, TResponse>(WebApplication app, string route, string name, string summary, string area, Func<TRequest, SequenceStore, TResponse> success)
        where TRequest : class
        where TResponse : class, new()
    {
        var forms = area == "Forms";
        var spec = Spec(name, summary,
            $"`POST {route}` (relativa a `{ConfigKey}`). " + (forms
                ? "En las operaciones de **Forms** GA2 inyecta `header {user, token}` en el body (`CHKeys:User/Token`)."
                : "GA2 reenvía el body que recibe `DodoController` (el `header` lo pone el llamador)."),
            $"ReferenciasSvc/Dodo/{area}/Services/Dodo{area}Service.cs", Ga2Usage.PassThrough, [Fallido, NoEncontrado]);

        app.MapMock("POST", Prefix + route, spec, async (HttpContext http, TRequest request, SequenceStore sequences) =>
            {
                var decision = await RestMock.BeginAsync(http, spec, RestMock.Lookup(http, request));
                return RestMock.BuiltIn(decision, spec) ?? decision.Name switch
                {
                    "default" => Results.Ok(success(request, sequences)),
                    "fallido" => Results.Ok(Failed<TResponse>(name)),
                    "no-encontrado" => Results.Text($"Recurso no encontrado (MockIntegrations: {name})", "text/plain", null, StatusCodes.Status404NotFound),
                    _ => RestMock.UnknownScenario(decision, spec),
                };
            })
            .Accepts<TRequest>("application/json")
            .Produces<TResponse>()
            .Produces<string>(StatusCodes.Status404NotFound, "text/plain")
            .Produces<string>(StatusCodes.Status500InternalServerError, "text/plain");
    }

    private static T Ok<T>(string message) where T : class, new() => WithStatus(new T(), true, message);

    private static T Failed<T>(string operation) where T : class, new() =>
        WithStatus(new T(), false, $"Error funcional simulado por MockIntegrations ({operation})");

    // Todas las respuestas Dodo tienen Success y Message (ver DTO de GA2).
    private static T WithStatus<T>(T response, bool success, string message)
    {
        typeof(T).GetProperty("Success")!.SetValue(response, success);
        typeof(T).GetProperty("Message")!.SetValue(response, message);
        return response;
    }

    private static DodoFileOperationResponse FileOperation(SequenceStore seq, string? barCode, int files, string message, bool newWorkflow, int? workflowId = null) => new()
    {
        Success = true,
        Message = message,
        BarCode = barCode,
        RecordId = (int)seq.Next("dodo.recordid", 4521),
        WorkflowId = workflowId ?? (newWorkflow ? (int)seq.Next("dodo.workflowid", 9981) : 0),
        FileCodes = Codes(seq, files),
        FileIdList = Enumerable.Range(1, files).ToList(),
        DestinationPath = Enumerable.Range(1, files).Select(i => $"/mock/expedientes/{barCode ?? "workflow"}/{i}").ToList(),
    };

    private static List<string> Codes(SequenceStore seq, int count) =>
        Enumerable.Range(0, count).Select(_ => $"FC-{seq.Next("dodo.filecode"):D6}").ToList();

    private static DodoFileProperty SampleFile(string code) => new()
    {
        FileId = 1,
        Code = code,
        Description = "Documento de prueba",
        Ext = "pdf",
        Size = 2048,
        DocDate = Today,
        PublishDate = Today,
        DocumentTypeCode = "00",
        Version = "1",
    };
}
