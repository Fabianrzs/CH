using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.ProductoPorCliente
{
    public class InsertPlanPagosProductoCartera
    {
        public long PLN_NUMERO_CREDITO { get; set; }
        public int PLN_NUMERO_CUOTA { get; set; }
        public DateTime PLN_FECHA_PAGO { get; set; }
        public decimal PLN_SALDO_INICIAL { get; set; }
        public decimal PLN_CAPITAL { get; set; }
        public decimal PLN_CAPITAL_POR_PAGAR { get; set; }
        public decimal PLN_INTERESES { get; set; }
        public decimal PLN_INTERES_POR_PAGAR { get; set; }
        public decimal PLN_SEGURO_VIDA { get; set; }
        public decimal PLN_SEGURO_HOGAR { get; set; }
        public decimal PLN_A_PAGAR { get; set; }
        public int PLN_PLAZO { get; set; }
        public decimal PLN_VALOR_DEUDA_REMANENTE { get; set; }
        public string PLN_F14 { get; set; }

    }
}
