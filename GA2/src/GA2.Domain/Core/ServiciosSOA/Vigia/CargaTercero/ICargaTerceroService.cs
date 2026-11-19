using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTercero;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Vigia.CargaTercero
{
    public interface ICargaTerceroService
    {
        Task<Response<Resultado>> CargaTercero(CargaTerceroRequest cargaTercero);
    }
}