using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Haberes
{
    public interface IConsumoHaberes
    {
        Task<string> GenerarReporteHaberesV2(GenerarReporteV2Request reporteHaberes);
        Task<string> GenerarReporteHaberes(GenerarReporteRequest reporteHaberes);
        Task<ReporteHaberesResponse> ReporteHaberes(ReporteHaberesRequest reporteHaberes);
    }
}