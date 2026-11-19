using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.Dodo.Workflow.Models;
using GA2.Transversals.Commons;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Workflow.Services
{
    public interface IDodoWorkflowService
    {
        /// <summary>Inicia un flujo de trabajo sobre un radicado.</summary>
        Task<Response<WorkflowStartResponse>> StartAsync(
            WorkflowStartRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Ejecuta/cumple un paso del flujo de trabajo.</summary>
        Task<Response<WorkflowSimpleResponse>> RunTaskAsync(
            WorkflowRunTaskRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Devuelve el flujo de trabajo a un paso anterior.</summary>
        Task<Response<WorkflowSimpleResponse>> ReturnTaskAsync(
            WorkflowReturnTaskRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Interrumpe/aborta un flujo de trabajo activo.</summary>
        Task<Response<WorkflowSimpleResponse>> AbortProcessAsync(
            WorkflowAbortProcessRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Adjunta archivos a un flujo de trabajo.</summary>
        Task<Response<DodoFileOperationResponse>> AttachFilesAsync(
            WorkflowAttachFilesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Consulta instancias de workflow por formulario y proceso.</summary>
        Task<Response<WorkflowGetCasesResponse>> GetCasesAsync(
            WorkflowGetCasesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Consulta el historial/trazabilidad de tareas de un workflow.</summary>
        Task<Response<WorkflowGetTasksResponse>> GetTasksAsync(
            WorkflowGetTasksRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Obtiene los archivos asociados a un flujo de trabajo.</summary>
        Task<Response<WorkflowGetFileCodesResponse>> GetFileCodesAsync(
            WorkflowGetFileCodesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Consulta los pasos disponibles para devolución de un workflow.</summary>
        Task<Response<WorkflowStepsForReturnResponse>> GetStepsForReturnAsync(
            WorkflowStepsForReturnRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>Consulta la configuración/parámetros de un proceso de workflow.</summary>
        Task<Response<ProcessParametersResponse>> GetProcessParametersAsync(
            ProcessParametersRequest request,
            CancellationToken cancellationToken = default);
    }
}
