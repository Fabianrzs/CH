using GA2.Domain.Core.ReferenciasSvc.Fenix;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Fenix
{
    public interface IFenixService
    {
        Task<ValidarBiometriaResponse> ValidarBiometriaAsync(ValidarBiometriaRequest identification);
    }
}