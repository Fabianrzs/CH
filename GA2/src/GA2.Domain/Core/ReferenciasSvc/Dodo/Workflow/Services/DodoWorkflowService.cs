using GA2.Domain.Core.Dodo.Common;
using GA2.Domain.Core.Dodo.Workflow.Models;
using GA2.Domain.Core.ReferenciasSvc.Common;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Integraciones;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Dodo.Workflow.Services
{
    public class DodoWorkflowService : CommonIntegracionCliente, IDodoWorkflowService
    {
        protected override ReferenciaSvc ReferenciaServicio => ReferenciaSvc.Dodo;

        public DodoWorkflowService(
            HttpClient httpClient,
            IEstadoIntegracionProvider integracionProvider,
            IAuditoriaRepository auditoriaRepository,
            ILogger<DodoWorkflowService> logger)
            : base(httpClient, integracionProvider, auditoriaRepository, logger)
        {
        }

        public Task<Response<WorkflowStartResponse>> StartAsync(
            WorkflowStartRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowStartRequest, WorkflowStartResponse>(
                "api/Workflow/Workflow_Start",
                request,
                cancellationToken);

        public Task<Response<WorkflowSimpleResponse>> RunTaskAsync(
            WorkflowRunTaskRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowRunTaskRequest, WorkflowSimpleResponse>(
                "api/Workflow/Workflow_RunTask",
                request,
                cancellationToken);

        public Task<Response<WorkflowSimpleResponse>> ReturnTaskAsync(
            WorkflowReturnTaskRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowReturnTaskRequest, WorkflowSimpleResponse>(
                "api/Workflow/Workflow_ReturnTask",
                request,
                cancellationToken);

        public Task<Response<WorkflowSimpleResponse>> AbortProcessAsync(
            WorkflowAbortProcessRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowAbortProcessRequest, WorkflowSimpleResponse>(
                "api/Workflow/Workflow_AbortProcess",
                request,
                cancellationToken);

        public Task<Response<DodoFileOperationResponse>> AttachFilesAsync(
            WorkflowAttachFilesRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowAttachFilesRequest, DodoFileOperationResponse>(
                "api/Workflow/Workflow_AttachFiles",
                request,
                cancellationToken);

        public Task<Response<WorkflowGetCasesResponse>> GetCasesAsync(
            WorkflowGetCasesRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowGetCasesRequest, WorkflowGetCasesResponse>(
                "api/Workflow/Workflow_GetCases",
                request,
                cancellationToken);

        public Task<Response<WorkflowGetTasksResponse>> GetTasksAsync(
            WorkflowGetTasksRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowGetTasksRequest, WorkflowGetTasksResponse>(
                "api/Workflow/Workflow_GetTasks",
                request,
                cancellationToken);

        public Task<Response<WorkflowGetFileCodesResponse>> GetFileCodesAsync(
            WorkflowGetFileCodesRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowGetFileCodesRequest, WorkflowGetFileCodesResponse>(
                "api/Workflow/Workflow_GetFileCodes",
                request,
                cancellationToken);

        public Task<Response<WorkflowStepsForReturnResponse>> GetStepsForReturnAsync(
            WorkflowStepsForReturnRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<WorkflowStepsForReturnRequest, WorkflowStepsForReturnResponse>(
                "api/Workflow/Workflow_StepsForReturn",
                request,
                cancellationToken);

        public Task<Response<ProcessParametersResponse>> GetProcessParametersAsync(
            ProcessParametersRequest request,
            CancellationToken cancellationToken = default)
            => PostAsync<ProcessParametersRequest, ProcessParametersResponse>(
                "api/Workflow/Process_Parameters",
                request,
                cancellationToken);
    }
}
