using System.Collections.Generic;
using System.Linq;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ResumenReporteContable
    {
        public decimal TotalValorDebito { get; set; }
        public decimal TotalValorCredito { get; set; }
        public int TotalCreditos { get; set; }
        public IEnumerable<ResumenReporteContablePorTipoCredito> TotalPorTipoCredito { get; set; }

        public ResumenReporteContable()
        {
            TotalPorTipoCredito = Enumerable.Empty<ResumenReporteContablePorTipoCredito>();
        }
    }

    public class ResumenReporteContablePorTipoCredito
    {
        public int IdTipoCredito { get; set; }
        public string CodigoTipoCredito { get; set; }
        public string TipoCredito { get; set; }
        public decimal TotalValorDebito { get; set; }
        public decimal TotalValorCredito { get; set; }
        public int TotalCreditos { get; set; }
    }

    public class ResultadoReportContable
    {
        public ResultadoReportContable() { }

        public ResultadoReportContable(IEnumerable<ReporteContableDto> reporte)
        {
            Reporte = reporte;
        }

        public IEnumerable<ReporteContableDto> Reporte { get; set; }
    }
}
