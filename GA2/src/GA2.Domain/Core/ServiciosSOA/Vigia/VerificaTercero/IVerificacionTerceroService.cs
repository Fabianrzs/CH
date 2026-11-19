using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Vigia.VerificaTercero
{
    public interface IVerificacionTerceroService
    {
        Task<Response<Resultado>> VertificacionTercero(VerificacionTerceroRequest verificacionTercero);
        Task<Response<Resultado>> VertificacionTerceroSolicitud(VerificacionTerceroRequest verificacionTercero);
    }
}