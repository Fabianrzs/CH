using System;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReportePagosParametersDto
    {
        public TipoFecha TipoFecha { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int? TipoCredito { get; set; }
        public int? UnidadEjecutora { get; set; }
    } 

    public enum TipoFecha
    {
        FechaAplicacion = 1,
        FechaPago = 2
    }
}