using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class CarteraReporteGeneralMoraDto
    {
        public Guid IdCredito { get; set; }
        public int IdTipoCredito { get; set; }
        public string TipoCredito { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public string UnidadEjecutora { get; set; }
        public long NumeroCredito { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public decimal SaldoCapital { get; set; }
        public DateTime FechaProximoPago { get; set; }
        public DateTime FechaUltimoPago { get; set; }
        public decimal ValorCuota { get; set; }
        public decimal ValorUltimoPago { get; set; }
    }
}
