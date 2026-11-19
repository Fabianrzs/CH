using System;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReporteContableParametersDto
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public int? TipoCredito { get; set; }

        public int? RegistrosPorPagina { get; set; }
        public int? NumeroPagina { get; set; }
    }
}