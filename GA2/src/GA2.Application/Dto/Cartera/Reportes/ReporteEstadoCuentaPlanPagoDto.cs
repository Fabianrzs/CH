using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteEstadoCuentaPlanPagoDto
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Capital { get; set; }
        public decimal CapitalPorPagar { get; set; }
        public decimal Intereses { get; set; }
        public decimal InteresesPorPagar { get; set; }
        public decimal SeguroHogar { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal ValorAPagar { get; set; }

    }
}
