using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Pagos
{
    public class PlanPagoAbonoCuota
    {

        public Guid CPG_ID { get; set; }
        public Guid CRE_ID { get; set; }
        public DateTime PLN_FECHA_PAGO { get; set; }
        public decimal PLN_SEGURO_HOGAR { get; set; }
        public decimal PLN_SEGURO_VIDA { get; set; }
        public decimal PLN_INTERESES { get; set; }
        public decimal CPG_ABONO_INTERES_MORA { get; set; }
        public decimal PLN_CAPITAL { get; set; }
        public decimal PLN_A_PAGAR { get; set; }
        public decimal CPG_ABONO_CAPITAL_REMANENTE { get; set; }
        public decimal CPG_ABONO_INTERES_REMANENTE { get; set; }
        public int MCO_CONT { get; set; }
        public Guid CPG_ID_REVERSION { get; set; }
        public string CONCEPTO { get; set; }
        public int MCO_NUME { get; set; }
        public decimal CRE_PROVISION { get; set; }

    }
    public class GetMoraCredito
    {

       
        public Guid CRE_ID { get; set; }
        public decimal CRE_ACUMULADO_INTERES_MORA { get; set; }
        public decimal CRE_SALDO_CAPITAL_MORA { get; set; }
        public int CRE_DIAS_MORA { get; set; }
        public DateTime FECHA_APLICACION_PAGO { get; set; }
        public int PAP_ID { get; set; }
       

    }
}
