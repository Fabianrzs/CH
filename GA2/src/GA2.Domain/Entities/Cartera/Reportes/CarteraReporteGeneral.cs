using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class CarteraReporteGeneral
    {
        public Guid CRE_ID { get; set; }

        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int TCR_ID { get; set; }
        public int UEJ_ID { get; set; }
        public decimal CRE_VALOR_DESEMBOLSO { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public decimal CRE_TASA_CREDITO_EA { get; set; }
        public DateTime CRE_FECHA_ULTIMO_PAGO { get; set; }
        public DateTime CRE_FECHA_PROXIMO_PAGO { get; set; }
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; }
        public int CRE_PLAZO_TOTAL { get; set; }
        public int CRE_NUMERO_CUOTAS_CANCELADAS { get; set; }
        public int CRE_PLAZO_ACTUAL { get; set; }
        public int CRE_CARTERA_CASTIGADA { get; set; }
        public decimal CRE_SALDO_CAPITAL { get; set; }
        public decimal CRE_VALOR_VIVIENDA { get; set; }
        public decimal CRE_VALOR_OPCION_COMPRA { get; set; }
        public decimal CRE_VALOR_DEUDA_REMANENTE { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public string TIV_VIVIENDA_DESCRIPCION { get; set; }
        public int CLC_ID { get; set; }
        public string CRE_CALIFICACION { get; set; }
        public int SOC_SCORE { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public string CLC_DESCRIPCION { get; set; }
        public decimal PLN_INTERESES { get; set; }
        public decimal PLN_SEGUROS { get; set; }
        public decimal PLN_CAPITAL { get; set; }
        public decimal PLN_A_PAGAR { get; set; }
        public decimal MORA_ANTERIOR { get; set; }
        public int CRE_DIAS_MORA { get; set; }
        public string ESC_DESCRIPCION { get; set; }
        public int ESC_ID { get; set; }
        public string DESCRIPCION_CIUDAD { get; set; }
        public decimal CANON_EXTRAORDINARIO_FINAL { get; set; }
        public decimal PROVISION_TOTAL { get; set; }


    }
}
