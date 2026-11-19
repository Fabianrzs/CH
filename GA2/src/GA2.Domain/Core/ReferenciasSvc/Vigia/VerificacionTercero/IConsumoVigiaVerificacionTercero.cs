using GA2.Application.Dto;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero
{
    public interface IConsumoVigiaVerificacionTercero
    {
        Task<Resultado> VertificacionTerceros(VerificacionTerceroRequest verificacionTercero);
        Task<Resultado> VertificacionTerceroSolicitud(VerificacionTerceroRequest verificacionTercero);
    }
}