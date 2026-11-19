using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReporteEstadoCuentaPlanPago
    {
        public int PLN_NUMERO_CUOTA { get; set; }
        public DateTime PLN_FECHA_PAGO { get; set; }
        public decimal PLN_CAPITAL { get; set; }
        public decimal PLN_CAPITAL_POR_PAGAR { get; set; }
        public decimal PLN_INTERESES { get; set; }
        public decimal PLN_INTERES_POR_PAGAR { get; set; }
        public decimal PLN_SEGURO_HOGAR { get; set; }
        public decimal PLN_SEGURO_VIDA { get; set; }
        public decimal PLN_A_PAGAR { get; set; }

    }
}
