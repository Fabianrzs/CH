using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReporteAnioGravable
    {
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string NOMBRE_TITULAR { get; set; }
        public int TCR_ID { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public string CLC_DESCRIPCION { get; set; }
        public decimal CRE_VALOR_DESEMBOLSO { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
        public decimal CUOTAS_PERIODO { get; set; }
        public decimal INTERESES_PERIODO { get; set; }
        public decimal COSTOS_FINANCIEROS { get; set; }
        public DateTime FECHA_CERTIFICACION { get; set; }
        public DateTime FECHA_EXPEDICION_REPORTE { get; set; }
    }
}
