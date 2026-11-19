using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteEstadoMovimientosDto
    {
        public int? NumeroCuota { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal AbonoCapital { get; set; }
        public decimal AbonoInteresCorriente { get; set; }
        public decimal AbonoSeguroVida { get; set; }
        public decimal AbonoSeguroHogar { get; set; }
        public decimal AbonoCapitalRemanente { get; set; }
        public decimal AbonoInteresRemanente { get; set; }
        public decimal AbonoInteresMora { get; set; }
        public decimal ValorPago { get; set; }
        public string TipoPago { get; set; }
        public decimal SaldoCapital { get; set; }

    }
}
