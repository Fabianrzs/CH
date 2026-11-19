using System;

namespace GA2.Domain.Entities.VerificacionDerechos.CuentasAVAV
{
    public class ObtenerCuentasAVAV
    {
        public long ID { get; set; }
        public int CTA_ID { get; set; }
        public int CTA_NUMERO { get; set; }
        public int CLI_ID { get; set; }
        public int TCT_ID { get; set; }
        public int ECT_ID { get; set; }
        public DateTime CTA_FECHA_CREACION { get; set; }
        public decimal CTA_SALDO { get; set; }

        public DateTime? AVAV { get; set; }
        public decimal VALOR_AVAV { get; set; }
        public decimal SALDO_ANUAL_AVAV { get; set; }
        public decimal INTERESES_AVAV { get; set; }
        public decimal MOVIMIENTOS_MENSUAL_AVAV { get; set; }


        public string IDENTIFICACION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }

    }
    public class ObtenerCuentaAVAVResponse
    {
        public string Identificacion { get; set; }
        public int IdCliente { get; set; }
        public int CuentaId { get; set; }
        public int CuentaNumero { get; set; }
        public int ConceptoId { get; set; }
    }
}
