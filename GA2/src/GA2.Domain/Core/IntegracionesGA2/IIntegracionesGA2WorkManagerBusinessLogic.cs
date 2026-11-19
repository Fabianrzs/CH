using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IIntegracionesGA2WorkManagerBusinessLogic
    {
        Task<Response<Form_DeleteResponse>> FormDeleteWorkManager(Form_DeleteRequest delete);
        Task<Response<Form_GetBase64StringFileResponse>> FormGetBase64StringFileWorkManager(Form_GetBase64StringFileRequest stringFile);
        Task<Response<Form_GetDataResponse>> FormGetDataWorkManager(Form_GetDataRequest query);
        Task<Response<Form_GetFileCodesResponse>> FormGetFileCodesWorkManager(Form_GetFileCodes file);
        Task<Response<Form_InsertResponse>> FormInsertWorkManager(Form_InsertRequest insert);
        Task<Response<Form_UpdateResponse>> FormUpdateWorkManager(Form_UpdateRequest update);
        Task<Response<List_GetItemsResponse>> ListGetItemsWorkManager(List_GetItemsRequest list);
        Task<Response<Workflow_AttachFilesResponse>> WorkflowAttachFilesWorkManager(Workflow_AttachFilesRequest WorkflowAttachFiles);
    }
}