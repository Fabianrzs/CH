namespace GA2.Domain.Entities.VerificacionDerechos.CuentasAVC
{
    public class ObtenerCuentasAVCResult
    {

        public int ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int ECT_ID { get; set; }
        public int PERIODO_ANIO { get; set; }
        public int PERIODO_MES { get; set; }
        public decimal CTA_SALDO { get; set; }


        public decimal VALOR_AVC { get; set; }
        public decimal SALDO_ANUAL_AVC { get; set; }
        public decimal MOVIMIENTOS_MENSUAL_AVC { get; set; }
        public decimal INTERES_AVC { get; set; }
        public decimal INTERESES_NO_CAUSADO_AVC { get; set; }
        public decimal INTERESES_CAUSADO_AVC { get; set; }
        public decimal SALDO_ANTERIOR_AVC { get; set; }
        public decimal SALDO_FINAL_AVC { get; set; }


        public int CLI_ID { get; set; }
        public string IDENTIFICACION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

        public long NUMERO_SOLICITUD { get; set; }
        public string TIPO_SOLICITUD { get; set; }

        public decimal INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC { get; set; }

    }
}
