using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTransaccion;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Vigia.CargaTransaccion
{
    public interface ICargaTransaccionService
    {
        Task<Response<Resultado>> CargaTransaccion(CargaTransaccionRequest CargaTransaccion);
    }
}