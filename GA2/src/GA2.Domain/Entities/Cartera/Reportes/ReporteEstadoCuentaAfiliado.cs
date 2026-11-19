using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReporteEstadoCuentaAfiliado
    {
        public Guid CRE_ID { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int TCR_ID { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public int ESC_ID { get; set; }
        public string ESC_DESCRIPCION { get; set; }
        public int UEJ_ID { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public int TID_ID { get; set; }
        public string TID_DESCRIPCION { get; set; }
        public int CLC_ID { get; set; }
        public string CLC_DESCRIPCION { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public decimal CRE_VALOR_DESEMBOLSO { get; set; }
        public decimal CRE_VALOR_VIVIENDA { get; set; }
        public decimal CRE_VALOR_OPCION_COMPRA { get; set; }
        public decimal CRE_VALOR_CANON_INICIAL { get; set; }
        public decimal CRE_TASA_CREDITO_EA { get; set; }
        public decimal CRE_TASA_CREDITO_NOM { get; set; }
        public decimal CRE_SALDO { get; set; }
        public int CRE_PLAZO_TOTAL { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO { get; set; }
        public DateTime CRE_FECHA_PROXIMO_PAGO { get; set; }
        public string MODALIDAD { get; set; }
        public string MONEDA { get; set; }
        public string FORMATO_PLAZO { get; set; }
        public string FORMATO_PAGO { get; set; }

    }
}
