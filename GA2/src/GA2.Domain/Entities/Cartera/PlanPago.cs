using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class PlanPago
    {

        public DateTime FECHA { get; set; }
        public int CUOTA { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal SEGURO_HOGAR { get; set; }
        public decimal CUOTA_TRADICIONAL { get; set; }
        public decimal CUOTA_UNIDAD_PAGADORA { get; set; }
        public decimal CAPITAL_TRADICIONAL { get; set; }
        public decimal INTERES_TRADICIONAL { get; set; }
        public decimal SALDO_TRADICIONAL { get; set; }
        public decimal CAPITAL_REDUCIDO { get; set; }
        public decimal INTERES_REDUCIDO { get; set; }
        public decimal SALDO_REDUCIDO { get; set; }
        public decimal CAPITAL_POR_PAGAR { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
        public decimal INTERES_POR_PAGAR { get; set; }
        public decimal SALDO_TOTAL { get; set; }
        public decimal INTERES { get; set; }
        public decimal CAPITAL { get; set; }
        public decimal SALDO { get; set; }
        public decimal SALDO_INICIAL { get; set; }
        public decimal DEUDA_REMANENTE { get; set; }
        public string ESTADO_CUOTA { get; set; }
    }

}
