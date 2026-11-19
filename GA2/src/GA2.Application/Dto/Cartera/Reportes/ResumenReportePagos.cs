using GA2.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ResumenReportePagos
    {
        public decimal TotalValor { get; set; }
        public int TotalCreditos { get; set; }
        public IEnumerable<ResumenReportePorTipoCredito> TotalPorTipoCredito { get; set; }
        public IEnumerable<ResumenReportePorUnidadEjecutora> TotalPorUnidadEjecutora { get; set; }

        public ResumenReportePagos()
        {
            TotalPorTipoCredito = Enumerable.Empty<ResumenReportePorTipoCredito>();
            TotalPorUnidadEjecutora = Enumerable.Empty<ResumenReportePorUnidadEjecutora>();
        }
    }

    public class ResumenReportePorTipoCredito
    {
        public int IdTipoCredito { get; set; }
        public string CodigoTipoCredito { get; set; }
        public string TipoCredito { get; set; }
        public decimal TotalValor { get; set; }
        public int TotalCreditos { get; set; }
    }

    public class ResumenReportePorUnidadEjecutora
    {
        public string UnidadEjecutora { get; set; }
        public IEnumerable<ResumenReportePorTipoCredito> TiposCredito { get; set; }

        public decimal TotalValor => TiposCredito?.Sum(x => x.TotalValor) ?? 0;
        public int TotalCreditos => TiposCredito?.Sum(x => x.TotalCreditos) ?? 0;


        public ResumenReportePorUnidadEjecutora()
        {
            TiposCredito = Enumerable.Empty<ResumenReportePorTipoCredito>();
        }
    }

    public class ResultadoReportePagos
    {
        public ResultadoReportePagos() { }

        public ResultadoReportePagos(IEnumerable<ReportePagoDto> reporte, ResumenReportePagos resumen)
        {
            Reporte = reporte;
            Resumen = resumen;
        }

        public IEnumerable<ReportePagoDto> Reporte { get; set; }
        public ResumenReportePagos Resumen { get; set; }
    }
}
