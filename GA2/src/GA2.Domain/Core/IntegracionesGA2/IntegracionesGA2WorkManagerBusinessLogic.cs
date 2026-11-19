using GA2.Application.Main;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class IntegracionesGA2WorkManagerBusinessLogic : IIntegracionesGA2WorkManagerBusinessLogic
    {
        private readonly IIntegracionesGA2WorkManager _integraciones;
        private readonly CHKeysOptions _optionsCH;

        public IntegracionesGA2WorkManagerBusinessLogic(IIntegracionesGA2WorkManager integraciones, IOptions<CHKeysOptions> optionsCH)
        {
            _integraciones = integraciones ?? throw new ArgumentNullException(nameof(integraciones));
            _optionsCH = optionsCH.Value ?? throw new ArgumentNullException(nameof(optionsCH));
        }

        public async Task<Response<Form_InsertResponse>> FormInsertWorkManager(Form_InsertRequest insert)
        {
            var url = _optionsCH.UrlCajaHonor + "api/forminsert";
            var uri = new Uri(url);
            var response = new Response<Form_InsertResponse>();
            var stringResultado = JsonConvert.SerializeObject(insert);
            var result = await _integraciones.FormInsertAsyncWorkManager<Response<Form_InsertResponse>>(uri,
                                                                                                        insert,
                                                                                                        response.GetCorrelation(),
                                                                                                        response.GetCorrelation());

            response = JsonConvert.DeserializeObject<Response<Form_InsertResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }

        public async Task<Response<Form_UpdateResponse>> FormUpdateWorkManager(Form_UpdateRequest update)
        {
            var url = _optionsCH.UrlCajaHonor + "api/formUpdate";
            var uri = new Uri(url);
            var response = new Response<Form_UpdateResponse>();
            var stringResultado = JsonConvert.SerializeObject(update);
            var result = await _integraciones.FormUpdateAsyncWorkManager<Response<Form_UpdateResponse>>(uri,
                                                                                                           update,
                                                                                                           response.GetCorrelation(),
                                                                                                           response.GetCorrelation());

            response = JsonConvert.DeserializeObject<Response<Form_UpdateResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }

        public async Task<Response<Form_DeleteResponse>> FormDeleteWorkManager(Form_DeleteRequest delete)
        {
            var url = _optionsCH.UrlCajaHonor + "api/formdelete";
            var uri = new Uri(url);
            var response = new Response<Form_DeleteResponse>();
            var result = await _integraciones.FormDeleteAsyncWorkManager<Response<Form_DeleteResponse>>(uri,
                                                                                 delete,
                                                                                 response.GetCorrelation(),
                                                                                 response.GetCorrelation());

            response = JsonConvert.DeserializeObject<Response<Form_DeleteResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }

        public async Task<Response<Form_GetDataResponse>> FormGetDataWorkManager(Form_GetDataRequest query)
        {
            var url = _optionsCH.UrlCajaHonor + "api/formgetdata";
            var uri = new Uri(url);
            var response = new Response<Form_GetDataResponse>();
            var result = await _integraciones.FormGetDataAsyncWorkManager<object>(uri, query, response.GetCorrelation(), response.GetCorrelation());
            response = JsonConvert.DeserializeObject<Response<Form_GetDataResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }
        public async Task<Response<Form_GetFileCodesResponse>> FormGetFileCodesWorkManager(Form_GetFileCodes file)
        {
            var url = _optionsCH.UrlCajaHonor + "api/formgetfilecodes";
            var uri = new Uri(url);
            var response = new Response<Form_GetFileCodesResponse>();
            var result = await _integraciones.FormGetFileCodesAsyncWorkManager<Response<Form_GetFileCodesResponse>>(uri, file, response.GetCorrelation(), response.GetCorrelation());
            response = JsonConvert.DeserializeObject<Response<Form_GetFileCodesResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }

        public async Task<Response<Form_GetBase64StringFileResponse>> FormGetBase64StringFileWorkManager(Form_GetBase64StringFileRequest stringFile)
        {
            var url = _optionsCH.UrlCajaHonor + "api/formgetbase64stringfile";
            var uri = new Uri(url);
            var response = new Response<Form_GetBase64StringFileResponse>();
            var result = await _integraciones.FormGetBase64StringFileAsyncWorkManager<object>(uri, stringFile, response.GetCorrelation(), response.GetCorrelation());
            response = JsonConvert.DeserializeObject<Response<Form_GetBase64StringFileResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }

        //public async Task<Response<object>> WorkflowStartWorkManager(Uri url, StartWorkflowRequest startWorkFlow, Guid userId)
        //{
        //    var response = new Response<object>();
        //    var result = await _integraciones.WorkflowStartAsyncWorkManager<object>(url, startWorkFlow, response.GetCorrelation(), userId);
        //    response.Data = JsonConvert.SerializeObject(result);
        //    return response;
        //}

        public async Task<Response<Workflow_AttachFilesResponse>> WorkflowAttachFilesWorkManager(Workflow_AttachFilesRequest WorkflowAttachFiles)
        {
            var url = _optionsCH.UrlCajaHonor + "api/workflowattachfiles";
            var uri = new Uri(url);
            var response = new Response<Workflow_AttachFilesResponse>();
            var stringResultado = JsonConvert.SerializeObject(WorkflowAttachFiles);
            var result = await _integraciones.WorkflowAttachFilesAsyncWorkManager<Response<Workflow_AttachFilesResponse>>(uri, WorkflowAttachFiles, response.GetCorrelation(), response.GetCorrelation());
            response = JsonConvert.DeserializeObject<Response<Workflow_AttachFilesResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }

        //public async Task<Response<object>> WorkflowRunTaskWorkManager(Uri url, RunTaskRequest runTask, Guid userId)
        //{
        //    var response = new Response<object>();
        //    var result = await _integraciones.WorkflowRunTaskAsyncWorkManager<object>(url, runTask, response.GetCorrelation(), userId);
        //    response.Data = JsonConvert.SerializeObject(result);
        //    return response;
        //}

        public async Task<Response<List_GetItemsResponse>> ListGetItemsWorkManager(List_GetItemsRequest list)
        {
            var url = _optionsCH.UrlCajaHonor + "api/listgetitems";
            var uri = new Uri(url);

            var response = new Response<List_GetItemsResponse>();
            var result = await _integraciones.ListGetItemsAsyncWorkManager<Response<List_GetItemsResponse>>(uri, list, response.GetCorrelation(), response.GetCorrelation());
            response = JsonConvert.DeserializeObject<Response<List_GetItemsResponse>>(JsonConvert.SerializeObject(result));
            return response;
        }
    }
}
