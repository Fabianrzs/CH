using System;

namespace GA2.Domain.Entities.Reportes
{
    public class InformacionProyecionExtractoAnual
    {
        public int CUOTA { get; set; }
        public DateTime FECHA_CUOTA { get; set; }
        public decimal CAPITAL_A_PAGAR { get; set; }
        public decimal CAPITAL_REMANENTE { get; set; }
        public decimal INTERES_A_PAGAR { get; set; }
        public decimal INTERES_REMANENTE { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal SEGURO_HOGAR { get; set; }
        public int DIAS_MORA { get; set; }
        public decimal INTERESES_MORA { get; set; }
        public decimal OTROS_GASTOS { get; set; }
    }
}
