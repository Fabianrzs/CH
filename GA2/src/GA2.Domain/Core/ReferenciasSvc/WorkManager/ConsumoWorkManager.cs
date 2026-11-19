using GA2.Application.Dto;
using GA2.Domain.Entities.Integraciones;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons.Configuraciones;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GA2.Domain.Core.ReferenciasSvc.WorkManager
{
    public class ConsumoWorkManager : IConsumoWorkManager
    {
        private readonly ConfiguracionesSvc _configuration;
        private readonly EndpointAddress endpointAddress;
        public readonly BasicHttpBinding basicHttpBinding;
        private WorkManagerServicesClient _work;
        private readonly ILogger<ConsumoWorkManager> _logger;
        private readonly IAuditoriaServicesBusinessLogic _auditoria;

        public ConsumoWorkManager(
            IOptions<ConfiguracionesSvc> configurationes,
            ILogger<ConsumoWorkManager> logger, IAuditoriaServicesBusinessLogic auditoria)
        {
            _configuration = configurationes.Value;
            endpointAddress = new EndpointAddress(_configuration.WorkManagerUrlSvc);
            basicHttpBinding = new BasicHttpBinding(endpointAddress.Uri.Scheme.ToLower() == "http" ?
            BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport)
            {
                OpenTimeout = TimeSpan.MaxValue,
                CloseTimeout = TimeSpan.MaxValue,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,

                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue
            };
            _logger = logger;
            _work = this.ObtenerInstanciaCliente();
            _auditoria = auditoria;
        }

        /// <summary>
        /// Obtener la instancia de conexion workmanager
        /// </summary>
        /// <returns>Instancia de servicio workmanager</returns>
        private WorkManagerServicesClient ObtenerInstanciaCliente() => new WorkManagerServicesClient(basicHttpBinding, endpointAddress);


        /// FormInsert **********************************************************************
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Form_InsertResponse> Form_InsertAsync(Form_InsertRequest insertRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());

            var insert = new Form_InsertResponse();

            insert = await _auditoria.EjecutarServicio<Form_InsertRequest, Form_InsertResponse>(
                 _work.Form_InsertAsync, insertRequestDto, ReferenciaSvc.WorkManagent);

            await Task.CompletedTask;
            _logger.LogInformation(_work.State.ToString());
            return insert;
        }
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Form_UpdateResponse> Form_UpdateAsync(Form_UpdateRequest updateRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());

            var update = await _auditoria.EjecutarServicio<Form_UpdateRequest, Form_UpdateResponse>(
                 _work.Form_UpdateAsync, updateRequestDto, ReferenciaSvc.WorkManagent);

            await Task.CompletedTask;

            return update;
        }
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Form_DeleteResponse> Form_DeleteAsync(Form_DeleteRequest deleteRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            await _auditoria.EjecutarServicio<Form_DeleteRequest, Form_DeleteResponse>(
                 _work.Form_DeleteAsync, deleteRequestDto, ReferenciaSvc.WorkManagent);

            await Task.CompletedTask;
            return null;
        }
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Form_GetDataResponse> Form_GetDataAsync(Form_GetDataRequest queryRequestDto)
        {
            //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(queryRequestDto.GetType());
            //System.IO.Stream fs = new System.IO.FileStream("C:\\Users\\desarrollok14\\Desktop\\configCaja\\prueba.txt", System.IO.FileMode.Create);
            //System.Xml.XmlWriter writer =
            //new System.Xml.XmlTextWriter(fs, System.Text.Encoding.Unicode);
            //x.Serialize(writer, queryRequestDto);
            //fs.Close();
            _logger.LogInformation(_work.State.ToString());
            var getdata = await _auditoria.EjecutarServicio<Form_GetDataRequest, Form_GetDataResponse>(
                 _work.Form_GetDataAsync, queryRequestDto, ReferenciaSvc.WorkManagent);
            _logger.LogInformation(_work.State.ToString());
            await Task.CompletedTask;
            //await this._work.CloseAsync();
            _logger.LogInformation(_work.State.ToString());
            return getdata;
        }

        /// AttachFiles *********************************************************************
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Form_AttachFilesResponse> Form_AttachFilesAsync(Form_AttachFilesRequest attachRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            if (_work.State.ToString() == "Closed")
            {
                _work = this.ObtenerInstanciaCliente();
            }
            await _work.OpenAsync();
            _logger.LogInformation(_work.State.ToString());
            var attached = await _auditoria.EjecutarServicio<Form_AttachFilesRequest, Form_AttachFilesResponse>(
                 _work.Form_AttachFilesAsync, attachRequestDto, ReferenciaSvc.WorkManagent);
            await Task.CompletedTask;
            await _work.CloseAsync();
            _logger.LogInformation(_work.State.ToString());


            return attached;
        }
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Form_GetFileCodesResponse> Form_GetFileCodesAsync(Form_GetFileCodes fileRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            ////await this._work.OpenAsync();
            ////_logger.LogInformation(_work.State.ToString());
            //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(fileRequestDto.GetType());
            //System.IO.Stream fs = new System.IO.FileStream("C:\\Users\\desarrollok30\\Desktop\\Form_GetFileCodesRequest.txt", System.IO.FileMode.Create);
            //System.Xml.XmlWriter writer =
            //new System.Xml.XmlTextWriter(fs, System.Text.Encoding.Unicode);
            //x.Serialize(writer, fileRequestDto);
            var file = await _auditoria.EjecutarServicio<Form_GetFileCodes, Form_GetFileCodesResponse>(
                 _work.Form_GetFileCodesAsync, fileRequestDto, ReferenciaSvc.WorkManagent);
            await Task.CompletedTask;
            return file;
        }
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Form_GetBase64StringFileResponse> Form_GetBase64StringFileAsync(Form_GetBase64StringFileRequest stringFileRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            await _auditoria.EjecutarServicio<Form_GetBase64StringFileRequest, Form_GetBase64StringFileResponse>(
                 _work.Form_GetBase64StringFileAsync, stringFileRequestDto, ReferenciaSvc.WorkManagent);
            await Task.CompletedTask;
            return null;
        }

        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Workflow_StartResponse> Workflow_StartAsync(Workflow_StartRequest startWorkflowRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            await _auditoria.EjecutarServicio<Workflow_StartRequest, Workflow_StartResponse>(
                 _work.Workflow_StartAsync, startWorkflowRequestDto, ReferenciaSvc.WorkManagent);
            await Task.CompletedTask;
            return null;
        }

        /// WorkFlowAttachFiles ¨*****************************************************
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Workflow_AttachFilesResponse> Workflow_AttachFilesAsync(Workflow_AttachFilesRequest workflowAttachFilesRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            return await _auditoria.EjecutarServicio<Workflow_AttachFilesRequest, Workflow_AttachFilesResponse>(
                 _work.Workflow_AttachFilesAsync, workflowAttachFilesRequestDto, ReferenciaSvc.WorkManagent);
            //return await Task.CompletedTask;
        }

        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<Workflow_RunTaskResponse> Workflow_RunTaskAsync(Workflow_RunTaskRequest runTaskRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            await _auditoria.EjecutarServicio<Workflow_RunTaskRequest, Workflow_RunTaskResponse>(
                 _work.Workflow_RunTaskAsync, runTaskRequestDto, ReferenciaSvc.WorkManagent);
            await Task.CompletedTask;
            return null;
        }
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>28/06/2021</date>
        public async Task<List_GetItemsResponse> List_GetItemsAsync(List_GetItemsRequest listRequestDto)
        {
            _logger.LogInformation(_work.State.ToString());
            await _auditoria.EjecutarServicio<List_GetItemsRequest, List_GetItemsResponse>(
                 _work.List_GetItemsAsync, listRequestDto, ReferenciaSvc.WorkManagent);
            await Task.CompletedTask;
            return null;
        }
        public async Task<Form_GetParametersResponse> Form_GetParameters(Form_GetParametersRequest getParametersRequest)
        {
            _logger.LogInformation(_work.State.ToString());
            //await this._work.OpenAsync();
            //_logger.LogInformation(_work.State.ToString());
            //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(getParametersRequest.GetType());
            //System.IO.Stream fs = new System.IO.FileStream("C:\\Users\\desarrollok30\\Desktop\\Form_GetParametersRequest.txt", System.IO.FileMode.Create);
            //System.Xml.XmlWriter writer =
            //new System.Xml.XmlTextWriter(fs, System.Text.Encoding.Unicode);
            //x.Serialize(writer, getParametersRequest);
            //Form_GetParametersAsync
            var file = await _auditoria.EjecutarServicio<Form_GetParametersRequest, Form_GetParametersResponse>(
                 _work.Form_GetParametersAsync, getParametersRequest, ReferenciaSvc.WorkManagent);
            await Task.CompletedTask;
            return file;
        }
    }
}
