using System;

namespace GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC
{
    public class ObtenerCuentasAVACResult
    {

        public int ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int ECT_ID { get; set; }
        public int PERIODO_ANIO { get; set; }
        public int PERIODO_MES { get; set; }
        public decimal CTA_SALDO { get; set; }


        public decimal VALOR_AVAC { get; set; }
        public decimal SALDO_ANUAL_AVAC { get; set; }
        public decimal MOVIMIENTOS_MENSUAL_AVAC { get; set; }
        public decimal INTERES_AVAC { get; set; }
        public decimal INTERESES_NO_CAUSADO_AVAC { get; set; }
        public decimal INTERESES_CAUSADO_AVAC { get; set; }
        public decimal SALDO_ANTERIOR_AVAC { get; set; }
        public decimal SALDO_FINAL_AVAC { get; set; }


        public int CLI_ID { get; set; }
        public string IDENTIFICACION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

        public long NUMERO_SOLICITUD { get; set; }
        public string TIPO_SOLICITUD { get; set; }

        public decimal INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC { get; set; }

    }
}
