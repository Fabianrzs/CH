using System;

namespace GA2.Domain.Entities.Cartera.Pagos
{
    public class ProcesoAplicacionPagosMasivoDetalle : ProcesoAplicacionPagosMasivo
    {
        public long APD_ID { get; set; }
        public string TIPO_NOMINA { get; set; }
        public string COD_CONCEPTO { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string NRO_IDENTIFICACION { get; set; }
        public string CODIGO_MILITAR { get; set; }
        public Guid CRE_ID { get; set; }
        public long NUMERO_CREDITO { get; set; }
        public decimal VALOR_SEGURO_HOGAR { get; set; }
        public decimal VALOR_SEGURO_VIDA { get; set; }
        public decimal VALOR_INTERES_CORRIENTE { get; set; }
        public decimal VALOR_CAPITAL { get; set; }
        public decimal VALOR_CUOTA { get; set; }
        public DateTime FECHA_DESDE { get; set; }
        public DateTime FECHA_HASTA { get; set; }
        public Guid? CPG_ID { get; set; }

    }
}
