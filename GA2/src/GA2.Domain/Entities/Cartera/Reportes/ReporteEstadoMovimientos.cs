using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReporteEstadoMovimientos
    {
        public int? NUMERO_CUOTA { get; set; }
        public DateTime CPG_FECHA_APLICACION { get; set; }
        public DateTime CPG_FECHA_PAGO { get; set; }
        public decimal CPG_ABONO_CAPITAL { get; set; }
        public decimal CPG_ABONO_INTERES_CORRIENTE { get; set; }
        public decimal CPG_ABONO_SEGURO_VIDA { get; set; }
        public decimal CPG_ABONO_SEGURO_HOGAR { get; set; }
        public decimal CPG_ABONO_CAPITAL_REMANENTE { get; set; }
        public decimal CPG_ABONO_INTERES_REMANENTE { get; set; }
        public decimal CPG_ABONO_INTERES_MORA { get; set; }
        public decimal CPG_VALOR_PAGO { get; set; }
        public string TPP_DESCRIPCION { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
    }
}
