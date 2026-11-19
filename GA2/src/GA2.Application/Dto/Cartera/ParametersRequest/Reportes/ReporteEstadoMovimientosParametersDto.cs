using System;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReporteEstadoMovimientosParametersDto
    {
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public Guid IdCredito { get; set; }
    }
}