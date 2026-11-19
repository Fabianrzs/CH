using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IVigiaBusinessLogic
    {
        Task<Response<ReferenciasSvc.Vigia.CargaProductoTercero.Resultado>> CargaProductoTerceroAsync(CargaProductoTerceroRequest request);
        Task<Response<ReferenciasSvc.Vigia.CargaTercero.Resultado>> CargarTerceroAsync(CargaTerceroRequest request);
        Task<Response<ReferenciasSvc.Vigia.CargaTransaccion.Resultado>> CargarTransanccionAsync(CargaTransaccionRequest request);
        Task<Response<ResultadoVerificacionTercero>> VertificacionTerceroAsync(VerificacionTerceroRequest request);
        Task<Response<ServiceVigiaVerificacionTercero.Resultado>> ServiceVertificacionTerceroAsync(VerificacionTerceroRequest request);
    }
}