using GA2.Application.Dto;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTransaccion
{
    public interface IConsumoVigiaCargaTransaccion
    {
        Task<Resultado> CargaTransaccionAsync(CargaTransaccionRequest cargaTransaccionRequest);
    }
}