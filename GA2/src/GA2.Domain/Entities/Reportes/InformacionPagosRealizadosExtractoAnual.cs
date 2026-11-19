using System;

namespace GA2.Domain.Entities.Reportes
{
    public class InformacionPagosRealizadosExtractoAnual
    {
        public decimal CAPITAL { get; set; }
        public decimal CAPITAL_REMANENTE { get; set; }
        public DateTime FECHA_VENCIMIENTO { get; set; }
        public decimal INTERES_CORRIENTE { get; set; }
        public decimal INTERES_REMANENTE { get; set; }
        public decimal INTERESES_MORA { get; set; }
        public decimal SALDO_CUOTA { get; set; }
        public decimal SEGURO_HOGAR { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal VALOR_PAGADO { get; set; }
    }
}
