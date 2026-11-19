using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.WorkManager
{
    public interface IConsumoWorkManager
    {
        Task<Form_AttachFilesResponse> Form_AttachFilesAsync(Form_AttachFilesRequest attachRequestDto);
        Task<Form_DeleteResponse> Form_DeleteAsync(Form_DeleteRequest deleteRequestDto);
        Task<Form_GetBase64StringFileResponse> Form_GetBase64StringFileAsync(Form_GetBase64StringFileRequest stringFileRequestDto);
        Task<Form_GetDataResponse> Form_GetDataAsync(Form_GetDataRequest queryRequestDto);
        Task<Form_GetFileCodesResponse> Form_GetFileCodesAsync(Form_GetFileCodes fileRequestDto);
        Task<Form_InsertResponse> Form_InsertAsync(Form_InsertRequest insertRequestDto);
        Task<Form_UpdateResponse> Form_UpdateAsync(Form_UpdateRequest updateRequestDto);
        Task<List_GetItemsResponse> List_GetItemsAsync(List_GetItemsRequest listRequestDto);
        Task<Workflow_AttachFilesResponse> Workflow_AttachFilesAsync(Workflow_AttachFilesRequest workflowAttachFilesRequestDto);
        Task<Workflow_RunTaskResponse> Workflow_RunTaskAsync(Workflow_RunTaskRequest runTaskRequestDto);
        Task<Workflow_StartResponse> Workflow_StartAsync(Workflow_StartRequest startWorkflowRequestDto);
        Task<Form_GetParametersResponse> Form_GetParameters(Form_GetParametersRequest getParametersRequest);

    }
}