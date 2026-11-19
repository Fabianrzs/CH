using GA2.Domain.Core.Dodo.Field.Models;
using GA2.Domain.Core.Dodo.Field.Services;
using GA2.Domain.Core.Dodo.File.Models;
using GA2.Domain.Core.Dodo.File.Services;
using GA2.Domain.Core.Dodo.Forms.Models;
using GA2.Domain.Core.Dodo.Forms.Services;
using GA2.Domain.Core.Dodo.Lists.Models;
using GA2.Domain.Core.Dodo.Notification.Models;
using GA2.Domain.Core.Dodo.Notification.Services;
using GA2.Domain.Core.Dodo.Offices.Models;
using GA2.Domain.Core.Dodo.Offices.Services;
using GA2.Domain.Core.Dodo.Token.Models;
using GA2.Domain.Core.Dodo.Token.Services;
using GA2.Domain.Core.Dodo.User.Models;
using GA2.Domain.Core.Dodo.User.Services;
using GA2.Domain.Core.Dodo.Version.Services;
using GA2.Domain.Core.Dodo.Workflow.Models;
using GA2.Domain.Core.Dodo.Workflow.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Integraciones
{
    [ApiController]
    [Route("api/integrations/[controller]")]
    [Produces("application/json")]
    public class DodoController : ControllerBase
    {
        private readonly IDodoTokenService _tokenService;
        private readonly IDodoVersionService _versionnService;
        private readonly IDodoUserService _userService;
        private readonly IDodoOfficesService _officesService;
        private readonly IDodoNotificationService _notificationService;
        private readonly IDodoFieldService _fieldService;
        private readonly IDodoFileService _fileService;
        private readonly IDodoFormsService _formsService;
        private readonly IDodoWorkflowService _workflowService;

        public DodoController(
            IDodoTokenService tokenService,
            IDodoVersionService versionService,
            IDodoUserService userService,
            IDodoOfficesService officesService,
            IDodoNotificationService notificationService,
            IDodoFieldService fieldService,
            IDodoFileService fileService,
            IDodoFormsService formsService,
            IDodoWorkflowService workflowService)
        {
            _tokenService = tokenService;
            _versionnService = versionService;
            _userService = userService;
            _officesService = officesService;
            _notificationService = notificationService;
            _fieldService = fieldService;
            _fileService = fileService;
            _formsService = formsService;
            _workflowService = workflowService;
        }

        // ══════════════════════════════════════════════════════════════════════
        // TOKEN
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Genera un token JWT dinámico para autenticación.</summary>
        [HttpPost("token/get")]
        public async Task<IActionResult> GetToken(
            [FromBody] GetTokenRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _tokenService.GetTokenAsync(request, cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // Version
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Genera un token JWT dinámico para autenticación.</summary>
        [HttpPost("version/get")]
        public async Task<IActionResult> GetVersion(
            CancellationToken cancellationToken)
        {
            var result = await _versionnService.GetVersionInfoAsync(cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // USER
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Consulta información de un usuario del sistema.</summary>
        [HttpPost("user/info")]
        public async Task<IActionResult> GetUserInfo(
            [FromBody] UserInfoRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserInfoAsync(request, cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // OFFICES
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Lista todas las sucursales disponibles.</summary>
        [HttpPost("offices/list")]
        public async Task<IActionResult> GetOffices(
            [FromBody] GetOfficesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _officesService.GetOfficesAsync(request, cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // NOTIFICATION
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Envía una notificación por correo electrónico.</summary>
        [HttpPost("notification/send-email")]
        public async Task<IActionResult> SendEmail(
            [FromBody] SendEmailRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _notificationService.SendEmailAsync(request, cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // FIELD / LISTS
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Obtiene los ítems de un campo tipo lista de un formulario.</summary>
        [HttpPost("field/values")]
        public async Task<IActionResult> GetFieldValues(
            [FromBody] FieldGetValuesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _fieldService.GetFieldValuesAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Obtiene los ítems de una lista maestra por ID.</summary>
        [HttpPost("lists/items")]
        public async Task<IActionResult> GetListItems(
            [FromBody] ListGetItemsRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _fieldService.GetListItemsAsync(request, cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // FILE
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Copia un archivo a un directorio de la estructura archivística.</summary>
        [HttpPost("file/copy-to-ep")]
        public async Task<IActionResult> FileCopyToEP(
            [FromBody] FileCopyToEPRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _fileService.CopyToEPAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Copia un archivo del formulario hijo al formulario padre.</summary>
        [HttpPost("file/copy-to-father")]
        public async Task<IActionResult> FileCopyToFather(
            [FromBody] FileCopyToFatherRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _fileService.CopyToFatherAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Elimina un archivo almacenado en la aplicación.</summary>
        [HttpPost("file/delete")]
        public async Task<IActionResult> FileDelete(
            [FromBody] FileDeleteRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _fileService.DeleteFileAsync(request, cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // FORMS
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Obtiene la configuración/parámetros de un formulario.</summary>
        [HttpPost("forms/parameters")]
        public async Task<IActionResult> FormGetParameters(
            [FromBody] FormGetParametersRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.GetParametersAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Inserta un nuevo registro (radicado) en un formulario.</summary>
        [HttpPost("forms/insert")]
        public async Task<IActionResult> FormInsert(
            [FromBody] FormInsertRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.InsertAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Actualiza registros de un formulario según filtro.</summary>
        [HttpPost("forms/update")]
        public async Task<IActionResult> FormUpdate(
            [FromBody] FormUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.UpdateAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Elimina registros de un formulario según filtro.</summary>
        [HttpPost("forms/delete")]
        public async Task<IActionResult> FormDelete(
            [FromBody] FormDeleteRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.DeleteAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Consulta datos/registros de un formulario con filtros y paginación.</summary>
        [HttpPost("forms/data")]
        public async Task<IActionResult> FormGetData(
            [FromBody] FormGetDataRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.GetDataAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Adjunta uno o varios archivos a un radicado existente.</summary>
        [HttpPost("forms/attach-files")]
        public async Task<IActionResult> FormAttachFiles(
            [FromBody] FormAttachFilesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.AttachFilesAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Obtiene los códigos y propiedades de archivos de un radicado.</summary>
        [HttpPost("forms/file-codes")]
        public async Task<IActionResult> FormGetFileCodes(
            [FromBody] FormGetFileCodesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.GetFileCodesAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Consulta los directorios configurados para un formulario.</summary>
        [HttpPost("forms/directories")]
        public async Task<IActionResult> FormGetFilesDirectories(
            [FromBody] FormGetFilesDirectoriesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.GetFilesDirectoriesAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Descarga un archivo en Base64 por su código.</summary>
        [HttpPost("forms/file-base64")]
        public async Task<IActionResult> FormGetBase64StringFile(
            [FromBody] FormGetBase64StringFileRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _formsService.GetBase64StringFileAsync(request, cancellationToken);
            return Ok(result);
        }

        // ══════════════════════════════════════════════════════════════════════
        // WORKFLOW
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Inicia un flujo de trabajo sobre un radicado.</summary>
        [HttpPost("workflow/start")]
        public async Task<IActionResult> WorkflowStart(
            [FromBody] WorkflowStartRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.StartAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Ejecuta/cumple un paso del flujo de trabajo.</summary>
        [HttpPost("workflow/run-task")]
        public async Task<IActionResult> WorkflowRunTask(
            [FromBody] WorkflowRunTaskRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.RunTaskAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Devuelve el flujo de trabajo a un paso anterior.</summary>
        [HttpPost("workflow/return-task")]
        public async Task<IActionResult> WorkflowReturnTask(
            [FromBody] WorkflowReturnTaskRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.ReturnTaskAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Interrumpe/aborta un flujo de trabajo activo.</summary>
        [HttpPost("workflow/abort")]
        public async Task<IActionResult> WorkflowAbortProcess(
            [FromBody] WorkflowAbortProcessRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.AbortProcessAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Adjunta archivos a un flujo de trabajo.</summary>
        [HttpPost("workflow/attach-files")]
        public async Task<IActionResult> WorkflowAttachFiles(
            [FromBody] WorkflowAttachFilesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.AttachFilesAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Consulta instancias de workflow por formulario y proceso.</summary>
        [HttpPost("workflow/cases")]
        public async Task<IActionResult> WorkflowGetCases(
            [FromBody] WorkflowGetCasesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.GetCasesAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Consulta el historial/trazabilidad de tareas de un workflow.</summary>
        [HttpPost("workflow/tasks")]
        public async Task<IActionResult> WorkflowGetTasks(
            [FromBody] WorkflowGetTasksRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.GetTasksAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Obtiene los archivos asociados a un flujo de trabajo.</summary>
        [HttpPost("workflow/file-codes")]
        public async Task<IActionResult> WorkflowGetFileCodes(
            [FromBody] WorkflowGetFileCodesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.GetFileCodesAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Consulta los pasos disponibles para devolución de un workflow.</summary>
        [HttpPost("workflow/steps-for-return")]
        public async Task<IActionResult> WorkflowStepsForReturn(
            [FromBody] WorkflowStepsForReturnRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.GetStepsForReturnAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>Consulta la configuración/parámetros de un proceso de workflow.</summary>
        [HttpPost("workflow/process-parameters")]
        public async Task<IActionResult> WorkflowProcessParameters(
            [FromBody] ProcessParametersRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _workflowService.GetProcessParametersAsync(request, cancellationToken);
            return Ok(result);
        }
    }
}
