using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Fenix
{
    public interface IConsumoFenix
    {
        Task<ValidarBiometriaResponse> ValidarBiometriaAsync(ValidarBiometriaRequest identification);
    }
}