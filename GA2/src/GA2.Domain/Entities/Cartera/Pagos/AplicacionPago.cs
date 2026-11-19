using System;

namespace GA2.Domain.Entities
{
    public class AplicacionPago
    {
        public Guid CPG_ID { get; set; }
        public Guid CRE_ID { get; set; }
        public DateTime CPG_FECHA_PAGO { get; set; }
        public DateTime CPG_FECHA_APLICACION { get; set; }
        public decimal CPG_ABONO_SEGURO_HOGAR { get; set; }
        public decimal CPG_ABONO_SEGURO_VIDA { get; set; }
        public decimal CPG_ABONO_INTERES_CORRIENTE { get; set; }
        public decimal CPG_ABONO_INTERES_MORA { get; set; }
        public decimal CPG_ABONO_CAPITAL { get; set; }
        public decimal CPG_ABONO_CAPITAL_MORA { get; set; }
        public decimal CPG_VALOR_PAGO { get; set; }
        public int DCT_ID { get; set; }
        public int TPP_ID { get; set; }
        public Guid CPG_CREADO_POR { get; set; }
        public DateTime CPG_FECHA_CREACION { get; set; }
        public Guid CPG_MODIFICADO_POR { get; set; }
        public DateTime CPG_FECHA_MODIFICACION { get; set; }
        public string TIPO_PAGO { get; set; }
        public string TPP_CODIGO { get; set; }
        public decimal CPG_ABONO_CAPITAL_REMANENTE { get; set; }
        public decimal CPG_ABONO_INTERES_REMANENTE { get; set; }
        public decimal TOTAL_DEUDA_REMANENTE { get; set; }
        public decimal CANON_EXTRAORDINARIO { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public int TIPO_IDENTIFICACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int TCR_ID { get; set; }
        public string MODALIDAD_PAGO { get; set; }
        public decimal ACUMULADO_REMANENTE { get; set; }
        public Guid CPG_ID_REVERSION { get; set; }
        public bool PAGO_REVERSIBLE { get; set; }
        public string CC_ID_COMPROBANTE{ get; set; }
        public int SV_ID_COMPROBANTE{ get; set; }
        public int CC_ESTADO_ID { get; set; }
        public string TipoRecurso { get; set; } 
    }   
}
