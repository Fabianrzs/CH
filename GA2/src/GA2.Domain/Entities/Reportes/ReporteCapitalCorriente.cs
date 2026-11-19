using System;

namespace GA2.Domain.Entities
{
    public class ReporteCapitalCorriente
    {
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int TCR_ID { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; }
        public DateTime PLN_FECHA_PAGO { get; set; }
        public decimal PLN_CAPITAL { get; set; }
    }
}
