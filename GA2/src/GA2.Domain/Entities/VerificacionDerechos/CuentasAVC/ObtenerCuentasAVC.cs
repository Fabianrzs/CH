using System;

namespace GA2.Domain.Entities.VerificacionDerechos.CuentasAVC
{
    public class ObtenerCuentasAVC
    {

        public long ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int CLI_ID { get; set; }
        public int TCT_ID { get; set; }
        public int ECT_ID { get; set; }
        public DateTime CTA_FECHA_CREACION { get; set; }
        public decimal CTA_SALDO { get; set; }

        public DateTime? AVC { get; set; }
        public decimal VALOR_AVC { get; set; }
        public decimal SALDO_ANUAL_AVC { get; set; }
        public decimal INTERESES_AVC { get; set; }
        public decimal MOVIMIENTOS_MENSUAL_AVC { get; set; }


        public string IDENTIFICACION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

    }
}
