using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaProductoTercero;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Vigia.CargaProductoTercero
{
    public interface ICargaProductoTerceroService
    {
        Task<Response<Resultado>> CargaProductoTercero(CargaProductoTerceroRequest cargaProductoTercero);
    }
}