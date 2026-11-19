using System;

namespace GA2.Domain.Entities.Cartera.Cierres.CierreAnual
{
    public class ReporteCierreAnualCartera
    {
        public Guid ID_CREDITO { get; set; }
        public long NUMERO_CREDITO { get; set; }
        public string IDENTIFICACION { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public int ID_TIPO_CREDITO { get; set; }
        public string TIPO_CREDITO { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
        public decimal SALDO_CAPITAL_TRADICIONAL { get; set; }
        public decimal ACUMULADO_SEGURO_HOGAR { get; set; }
        public decimal ACUMULADO_SEGURO_VIDA { get; set; }
        public decimal PROVISION_GENERICA { get; set; }
        public decimal ABONO_SEGURO_HOGAR { get; set; }
        public decimal ABONO_SEGURO_VIDA { get; set; }
        public decimal ABONO_INTERES_CORRIENTE { get; set; }
        public decimal ABONO_INTERES_MORA { get; set; }
        public decimal ABONO_CAPITAL { get; set; }
        public decimal ABONO_CAPITAL_REMANENTE { get; set; }
        public decimal ABONO_INTERES_REMANENTE { get; set; }
        public DateTime FECHA_PRIMER_PAGO { get; set; }
        public DateTime FECHA_PRIMERA_APLICACION { get; set; }
        public DateTime FECHA_ULTIMO_PAGO { get; set; }
        public DateTime FECHA_ULTIMA_APLICACION { get; set; }
        public int ANIO { get; set;  }
        public int DIAS_MORA { get; set; }
        public string CALIFICACION_CARTERA { get; set; }
        public int ID_ESTADO_CREDITO { get; set; }
        public decimal PROVISION { get; set; }
        public decimal PROVISION_PORCENTAJE { get; set; }
        public decimal PROVISION_ADICIONAL { get; set; }
    }
}
