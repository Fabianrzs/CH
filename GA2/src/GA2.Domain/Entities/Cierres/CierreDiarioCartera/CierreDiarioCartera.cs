using System;

namespace GA2.Domain.Entities
{
    public class CierreDiarioCartera
    {
        public long CRE_NRO_CREDITO { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public string CATEGORIA { get; set; }
        public string GRADO { get; set; }
        public DateTime FECHA_SOLICITUD { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public string ESTADO_CREDITO { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; }
        public decimal CRE_VALOR_DESEMBOLSO { get; set; }
        public decimal CRE_VALOR_VIVIENDA { get; set; }
        public string TIV_VIVIENDA_DESCRIPCION { get; set; }
        public string TVV_DESCRIPCION { get; set; }
        public decimal CRE_VALOR_CANON_INICIAL { get; set; }
        public decimal CRE_VALOR_OPCION_COMPRA { get; set; }
        public decimal CRE_POR_CANON_INICIAL { get; set; }
        public decimal CRE_POR_OPCION_COMPRA { get; set; }
        public decimal CRE_CANON_EXTRAORDINARIO { get; set; }
        public int CRE_PLAZO_TOTAL { get; set; }
        public int CRE_PLAZO_ACTUAL { get; set; }
        public int CRE_NUMERO_CUOTAS_CANCELADAS { get; set; }
        public decimal CRE_TASA_SEGURO_VIDA { get; set; }
        public decimal CRE_TASA_SEGURO_HOGAR { get; set; }
        public decimal CRE_TASA_CREDITO_EA { get; set; }
        public decimal CRE_TASA_CREDITO_PER { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public decimal CRE_SALDO_CAPITAL { get; set; }
        public decimal CRE_SALDO_CAPITAL_MORA { get; set; }
        public int CRE_DIAS_MORA { get; set; }
        public decimal CRE_VALOR_DEUDA_REMANENTE { get; set; }
        public DateTime CRE_FECHA_ESTADO { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO { get; set; }
        public DateTime CRE_FECHA_PROXIMO_PAGO { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_HOGAR { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_SEGURO_HOGAR { get; set; }
        public decimal CRE_ACUMULADO_SEGURO_VIDA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_SEGURO_VIDA { get; set; }
        public decimal CRE_ACUMULADO_INTERES_MORA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_INTERES_MORA { get; set; }
        public decimal CRE_ACUMULADO_INTERES_CORRIENTE { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO_INTERES_CORRIENTE { get; set; }
        public string CRE_CALIFICACION { get; set; }
        public decimal CRE_PROVISION { get; set; }
        public decimal CRE_PROVISION_PORCENTAJE { get; set; }
        public decimal CRE_PROVISION_ADICIONAL { get; set; }
        public decimal CAUSACION_INTERES_CORRIENTE { get; set; }
        public decimal CAUSACION_INTERES_MORA { get; set; }
        public int AREA_NEGOCIO { get; set; }
        public decimal INT_MORA_ANTERIOR { get; set; }
        public decimal TOTAL_MORA { get; set; }
    }
}
