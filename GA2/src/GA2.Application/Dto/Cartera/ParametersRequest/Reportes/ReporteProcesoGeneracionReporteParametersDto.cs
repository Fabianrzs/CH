using System;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReporteProcesoGeneracionReporteParametersDto
    {

        public int? TipoCredito { get; set; }
        public string? TipoReporte { get; set; }
        public Guid? IdProceso { get; set; }
    }
}