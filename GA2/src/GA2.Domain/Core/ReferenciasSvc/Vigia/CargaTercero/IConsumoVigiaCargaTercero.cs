using GA2.Application.Dto;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTercero
{
    public interface IConsumoVigiaCargaTercero
    {
        Task<Resultado> CargaTerceroAsync(CargaTerceroRequest cargaTerceroRequest);
    }
}