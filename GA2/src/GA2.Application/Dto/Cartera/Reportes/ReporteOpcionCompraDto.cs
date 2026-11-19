using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteOpcionCompraDto
    {
        public long NumeroCredito { get; set; }
        public string NumeroIdentificacion { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public string UnidadEjecutora { get; set; }
        public decimal CapitalRemanente { get; set; }
        public string NombreAfiliado { get; set; }
        public decimal ValorOpcionCompra { get; set; }
    }
}
