using GA2.Domain.Core.ReferenciasSvc.Haberes;
using GA2.Transversals.Commons;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios.Haberes
{
    public interface IHaberesService
    {
        Task<Response<string>> GenerarReporteHaberesV2(GenerarReporteV2Request reporte);
        Task<Response<string>> GenerarReporteHaberes(GenerarReporteRequest reporte);
        Task<Response<ReporteHaberesResponse>> ReporteHaberes(ReporteHaberesRequest reporteHaberes);
    }
}