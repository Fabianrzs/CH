using GA2.Application.Dto;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.CargaProductoTercero
{
    public interface IConsumoVigiaCargaProductoTercero
    {
        Task<Resultado> CargaProductoTerceroAsync(CargaProductoTerceroRequest cargaProductoTerceroRequest);
    }
}