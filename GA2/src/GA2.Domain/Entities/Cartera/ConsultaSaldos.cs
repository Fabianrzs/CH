using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ConsultaSaldos
    {
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public decimal CRE_SALDO_LEASING { get; set; }
        public decimal CRE_SALDO_HIPOTECARIO { get; set; }
        public decimal CRE_PROVISION { get; set; }
    }
}
