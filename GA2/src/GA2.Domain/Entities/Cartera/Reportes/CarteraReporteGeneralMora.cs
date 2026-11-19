using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class CarteraReporteGeneralMora
    {
        public Guid CRE_ID { get; set; }
        public int TCR_ID { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public int UEJ_ID { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public decimal CRE_SALDO_CAPITAL { get; set; }
        public DateTime CRE_FECHA_PROXIMO_PAGO { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO { get; set; }
        public decimal CRE_VALOR_ULTIMO_PAGO { get; set; }
    }
}
