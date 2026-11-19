using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.WorkManager
{
    public interface IWorkManagerService
    {
        Task<Response<Form_AttachFilesResponse>> FormAttachFilesAsyncCH(AttachRequestDto attachFiles);
        Task<Response<Form_DeleteResponse>> FormDeleteAsyncCH(DeleteRequestDto delete);
        Task<Response<Form_GetBase64StringFileResponse>> FormGetBase64StringFileAsyncCH(StringFileRequestDto stringFile);
        Task<Response<Form_GetDataResponse>> FormGetDataAsyncCH(QueryRequestDto getData);
        Task<Response<Form_GetFileCodesResponse>> FormGetFileCodesAsyncCH(FileRequestDto filerequestcode);
        Task<Response<Form_InsertResponse>> FormInsertAsyncCH(InsertRequestDto insert);
        Task<Response<Form_UpdateResponse>> FormUpdateAsyncCH(UpdateRequestDto update);
        Task<Response<List_GetItemsResponse>> ListGetItemsAsyncCH(ListRequestDto listGetItems);
        Task<Response<Workflow_AttachFilesResponse>> WorkFlowAttachFilesAsyncCH(WorkflowAttachFilesRequestDto attachFilesWorkFlow);
        Task<Response<Workflow_RunTaskResponse>> WorkFlowRunAsyncCH(RunTaskRequestDto runTask);
        Task<Response<Workflow_StartResponse>> WorkFlowStartAsyncCH(StartWorkflowRequestDto startWorkFlow);
        Task<Response<Form_GetParametersResponse>> Form_GetParameters(Form_GetParametersRequest getParametersRequest);
    }
}