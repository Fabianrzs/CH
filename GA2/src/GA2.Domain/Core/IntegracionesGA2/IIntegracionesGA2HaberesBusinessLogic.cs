using GA2.Application.Dto.Haberes;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IIntegracionesGA2HaberesBusinessLogic
    {
        Task<Response<string>> GenerarReporteHaberes(HaberesRequestDto request);
    }
}