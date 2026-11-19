using GA2.Application.Dto;
using GA2.Application.Dto.Haberes;
using GA2.Transversals.Commons;
using System;
using System.Threading.Tasks;

namespace GA2.Application.Main
{
    public interface IIntegracionesGA2WorkManager
    {
            Task<object> CargaProductoTercero<TOutput>(Uri url, CargaProductoTerceroRequest cargaProductoTerceroRequest, Guid correlationId, Guid userId);
            Task<object> CargaTercero<TOutput>(Uri url, CargaTerceroRequest cargaTerceroRequest, Guid correlationId, Guid userId);
            Task<object> CargaTransaccion<TOutput>(Uri url, CargaTransaccionRequest cargaTransaccionRequest, Guid correlationId, Guid userId);
            Task<object> FormDeleteAsyncWorkManager<TOutput>(Uri url, object delete, Guid correlationId, Guid userId);
            Task<object> FormGetBase64StringFileAsyncWorkManager<TOutput>(Uri url, object stringFile, Guid correlationId, Guid userId);
            Task<object> FormGetDataAsyncWorkManager<TOutput>(Uri url, object query, Guid correlationId, Guid userId);
            Task<object> FormGetFileCodesAsyncWorkManager<TOutput>(Uri url, object file, Guid correlationId, Guid userId);
            Task<object> FormInsertAsyncWorkManager<TOutput>(Uri url, object insert, Guid correlationId, Guid userId);
            Task<object> FormUpdateAsyncWorkManager<TOutput>(Uri url, object update, Guid correlationId, Guid userId);
            Task<Response<string>> GenerarReporteHaberes<TOutput>(Uri url, HaberesRequestDto request, Guid correlationId, Guid userId);
            Task<object> ListGetItemsAsyncWorkManager<TOutput>(Uri url, object list, Guid correlationId, Guid userId);
            Task<object> ObtenetBua<TOutput>(Uri url, ObtenerBua buaDto, Guid correlationId, Guid userId);
            Task<object> ValidarBiometria<TOutput>(Uri url, int request, Guid correlationId, Guid userId);
            Task<Response<ResultadoVerificacionTercero>> VerificacionTercero<TOutput>(Uri url, VerificacionTerceroRequest verificacionTerceroRequest, Guid correlationId, Guid userId);
            Task<object> WorkflowAttachFilesAsyncWorkManager<TOutput>(Uri url, object workFlowAttach, Guid correlationId, Guid userId);
            Task<object> WorkflowRunTaskAsyncWorkManager<TOutput>(Uri url, object runTask, Guid correlationId, Guid userId);
            Task<object> WorkflowStartAsyncWorkManager<TOutput>(Uri url, object startWorkFlow, Guid correlationId, Guid userId);
    }
}
