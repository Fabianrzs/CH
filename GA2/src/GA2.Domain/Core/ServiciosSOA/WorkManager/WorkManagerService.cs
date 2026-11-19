using AutoMapper;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Transversals.Commons;
using GA2.BuildingBlocks.EventBus.Abstractions;
using System.Threading.Tasks;
using GA2.Domain.Core.IntegrationEvents.Events.WorkManager;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GA2.Domain.Core.Servicios.WorkManager
{
    public class WorkManagerService : IWorkManagerService
    {
        /// <summary>
        /// Instancia de consumo WorkManager
        /// </summary>
        private readonly IConsumoWorkManager _consumo;
        /// <summary>
        /// Eventbus service
        /// </summary>
        // private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public WorkManagerService(IConsumoWorkManager consumo,
                                 // IEventBus eventBus,
                                  IMapper mapper)
        {
            _consumo = consumo;
          //  _eventBus = eventBus;
            _mapper = mapper;
        }

        public async Task<Response<Form_InsertResponse>> FormInsertAsyncCH(InsertRequestDto insert)
        {
            var response = new Response<Form_InsertResponse>();
            response.GetCorrelation();

            var insertar = _mapper.Map<Form_InsertRequest>(insert);

            var insertWorkManager = await _consumo.Form_InsertAsync(insertar);
            if (!insertWorkManager.Form_InsertResult.Success)
            {
                var userCreatedIntegrationEvent = new WorkManagerInsertRequestIntegrationEvent
                {
                    InsertRequest = insertar
                };

                //_eventBus.Publish(userCreatedIntegrationEvent);
            }

            response.Data = insertWorkManager;
            response.ReturnMessage = insertWorkManager.Form_InsertResult.Success != false ? ConstantesWorkManager.FormularioInsertado : ConstantesWorkManager.FormularioEncolado;

            return response;
        }

        public async Task<Response<Form_UpdateResponse>> FormUpdateAsyncCH(UpdateRequestDto update)
        {
            var response = new Response<Form_UpdateResponse>();
            response.GetCorrelation();

            var updated = _mapper.Map<Form_UpdateRequest>(update);

            var updateWorkManager = await _consumo.Form_UpdateAsync(updated);
            if (!updateWorkManager.Form_UpdateResult.Success)
            {
                var updateCreatedIntegrationEvent = new WorkManagerUpdateRequestIntegrationEvent
                {
                    UpdateRequest = updated
                };

                //_eventBus.Publish(updateCreatedIntegrationEvent);
            }

            response.Data = updateWorkManager;
            response.ReturnMessage = updateWorkManager.Form_UpdateResult.Success != false ? ConstantesWorkManager.FormularioActualizado : ConstantesWorkManager.FormularioEncolado;

            return response;
        }

        public async Task<Response<Form_DeleteResponse>> FormDeleteAsyncCH(DeleteRequestDto delete)
        {
            var response = new Response<Form_DeleteResponse>();
            response.GetCorrelation();

            var deleted = _mapper.Map<Form_DeleteRequest>(delete);

            var deleteWorkManager = await _consumo.Form_DeleteAsync(deleted);

            if (!deleteWorkManager.Form_DeleteResult.Success)
            {
                var deleteWorkManagerIntegrationEvent = new WorkManagerDeleteRequestIntegrationEvent
                {
                    DeleteRequest = deleted
                };

                //_eventBus.Publish(deleteWorkManagerIntegrationEvent);
            }

            response.Data = deleteWorkManager;
            response.ReturnMessage = deleteWorkManager.Form_DeleteResult.Success != false ? ConstantesWorkManager.FormularioEliminado : ConstantesWorkManager.FormularioEncolado;

            return response;
        }

        public async Task<Response<Form_GetDataResponse>> FormGetDataAsyncCH(QueryRequestDto getData)
        {
            var response = new Response<Form_GetDataResponse>();
            response.GetCorrelation();

            var getDatas = _mapper.Map<Form_GetDataRequest>(getData);

            var getDataWorkManager = await _consumo.Form_GetDataAsync(getDatas);
            response.Data = getDataWorkManager;
            response.ReturnMessage = getDataWorkManager.Form_GetDataResult.Success != false  && getDataWorkManager.Form_GetDataResult.Rows != 0 ? ConstantesWorkManager.FormularioObtenido : ConstantesWorkManager.FormularioNoObtenido;

            return response;
        }

        public async Task<Response<Form_AttachFilesResponse>> FormAttachFilesAsyncCH(AttachRequestDto attachFiles)
        {
            var response = new Response<Form_AttachFilesResponse>();
            response.GetCorrelation();

            var attaches = _mapper.Map<Form_AttachFilesRequest>(attachFiles);

            var getAttachFilesWorkManager = await _consumo.Form_AttachFilesAsync(attaches);

            if (!getAttachFilesWorkManager.Form_AttachFilesResult.Success)
            {
                var attachFilesWorkManagerntragrationEvent = new WorkManagerAttachRequestIntegrationEvent
                {
                    AttachRequest = attaches
                };

                //_eventBus.Publish(attachFilesWorkManagerntragrationEvent);
            }

            response.Data = getAttachFilesWorkManager;
            response.ReturnMessage = $"{(getAttachFilesWorkManager.Form_AttachFilesResult.Success != false ? ConstantesWorkManager.FormularioArchivoAdjuntado : ConstantesWorkManager.FormularioArchivoNoAdjuntado)}, {getAttachFilesWorkManager.Form_AttachFilesResult.Message}";

            return response;
        }

        public async Task<Response<Form_GetFileCodesResponse>> FormGetFileCodesAsyncCH(FileRequestDto filerequestcode)
        {
            var response = new Response<Form_GetFileCodesResponse>();
            response.GetCorrelation();

            var fcodes = _mapper.Map<Form_GetFileCodes>(filerequestcode);

            var getGetFileCodesWorkManager = await _consumo.Form_GetFileCodesAsync(fcodes);

            response.Data = getGetFileCodesWorkManager;
            response.ReturnMessage = getGetFileCodesWorkManager.Form_GetFileCodesResult.Success != false ? ConstantesWorkManager.FormularioCodigoObtenido : ConstantesWorkManager.FormularioCodigoNoObtenido;

            return response;
        }

        public async Task<Response<Form_GetBase64StringFileResponse>> FormGetBase64StringFileAsyncCH(StringFileRequestDto stringFile)
        {
            var response = new Response<Form_GetBase64StringFileResponse>();
            response.GetCorrelation();

            var stringfilerequest = _mapper.Map<Form_GetBase64StringFileRequest>(stringFile);

            var getGetBase64StringFileWorkManager = await _consumo.Form_GetBase64StringFileAsync(stringfilerequest);

            response.Data = getGetBase64StringFileWorkManager;
            response.ReturnMessage = getGetBase64StringFileWorkManager.Form_GetBase64StringFileResult.Success != false ? ConstantesWorkManager.FormularioObtenidoBaseString : ConstantesWorkManager.FormularioNoObtenidoBaseString;

            return response;
        }

        public async Task<Response<Workflow_StartResponse>> WorkFlowStartAsyncCH(StartWorkflowRequestDto startWorkFlow)
        {
            var response = new Response<Workflow_StartResponse>();
            response.GetCorrelation();

            var start = _mapper.Map<Workflow_StartRequest>(startWorkFlow);

            var WorkFlowStartWorkManager = await _consumo.Workflow_StartAsync(start);

            response.Data = WorkFlowStartWorkManager;
            response.ReturnMessage = WorkFlowStartWorkManager.Workflow_StartResult.Success != false ? ConstantesWorkManager.WorkFlowIniciada : ConstantesWorkManager.WorkFlowNoIniciada;

            return response;
        }

        public async Task<Response<Workflow_AttachFilesResponse>> WorkFlowAttachFilesAsyncCH(WorkflowAttachFilesRequestDto attachFilesWorkFlow)
        {
            var response = new Response<Workflow_AttachFilesResponse>();
            response.GetCorrelation();
            var attches = _mapper.Map<Workflow_AttachFilesRequest>(attachFilesWorkFlow);

            var WorkFlowAttachFilesWorkManager = await _consumo.Workflow_AttachFilesAsync(attches);
            if (!WorkFlowAttachFilesWorkManager.Workflow_AttachFilesResult.Success)
            {
                var workflowAttachfilesIntegrationEvent = new WorkManagerWorkflowAttachFilesRequestIntegrationEvent
                {
                    WorkflowAttachFilesRequest = attches
                };

                //_eventBus.Publish(workflowAttachfilesIntegrationEvent);
            }

            response.Data = WorkFlowAttachFilesWorkManager;
            response.ReturnMessage = WorkFlowAttachFilesWorkManager.Workflow_AttachFilesResult.Success != false ? ConstantesWorkManager.WorkFlowAdjuntadoInstancia : ConstantesWorkManager.FormularioEncolado;

            return response;
        }

        public async Task<Response<Workflow_RunTaskResponse>> WorkFlowRunAsyncCH(RunTaskRequestDto runTask)
        {
            var response = new Response<Workflow_RunTaskResponse>();
            response.GetCorrelation();

            var run = _mapper.Map<Workflow_RunTaskRequest>(runTask);

            var WorkFlowRunTaskWorkManager = await _consumo.Workflow_RunTaskAsync(run);

            response.Data = WorkFlowRunTaskWorkManager;
            response.ReturnMessage = WorkFlowRunTaskWorkManager.Workflow_RunTaskResult.Success != false ? ConstantesWorkManager.WorkFlowTareaCumplida : ConstantesWorkManager.WorkFlowTareaNoCumplida;

            return response;
        }

        public async Task<Response<List_GetItemsResponse>> ListGetItemsAsyncCH(ListRequestDto listGetItems)
        {
            var response = new Response<List_GetItemsResponse>();
            response.GetCorrelation();

            var list = _mapper.Map<List_GetItemsRequest>(listGetItems);

            var listGetItemsWorkManager = await _consumo.List_GetItemsAsync(list);

            response.Data = listGetItemsWorkManager;
            response.ReturnMessage = listGetItemsWorkManager != null ? ConstantesWorkManager.ListItemsObtenidos : ConstantesWorkManager.ListItemsNoObtenidos;

            return response;
        }

        public async Task<Response<Form_GetParametersResponse>> Form_GetParameters(Form_GetParametersRequest getParametersRequest)
        {
            var response = new Response<Form_GetParametersResponse>();
            response.GetCorrelation();

            var parameters = await _consumo.Form_GetParameters(getParametersRequest);

            response.Data = parameters;

            response.ReturnMessage = parameters != null ? ConstantesWorkManager.WorkFlowIniciada : ConstantesWorkManager.WorkFlowNoIniciada;

            return response;

        }
    }
}
