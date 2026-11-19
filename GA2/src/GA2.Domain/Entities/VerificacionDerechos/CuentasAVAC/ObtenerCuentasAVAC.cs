using System;

namespace GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC
{
    public class ObtenerCuentasAVAC
    {

        public long ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int CLI_ID { get; set; }
        public int TCT_ID { get; set; }
        public int ECT_ID { get; set; }
        public DateTime CTA_FECHA_CREACION { get; set; }
        public decimal CTA_SALDO { get; set; }

        public DateTime? AVAC { get; set; }
        public decimal VALOR_AVAC { get; set; }
        public decimal SALDO_ANUAL_AVAC { get; set; }
        public decimal INTERESES_AVAC { get; set; }
        public decimal MOVIMIENTOS_MENSUAL_AVAC { get; set; }
        

        public string IDENTIFICACION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }


    }
}
