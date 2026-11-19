using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Application.Main.ProviderRequest.Reintegro
{
    public interface IReintegroProviders
    {
        Task<Response<string>> ActualizarValorSaldoPorcentaje(int idAfiliado, decimal valor);
        Task<Response<string>> ActualizarValorSaldo(int idAfiliado, decimal valor);
        Task<Response<string>> CleanValorSaldo(int idAfiliado, decimal valor);
        Task<bool> ListadoSolicitudByAfiliado(int idAfiliado);
    }
}
