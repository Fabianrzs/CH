using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ConsultarCreditoProspeccion
    {
        public int CRE_NRO_CREDITO { get; set; }
        public int TCR_ID { get; set; }
        public int TIPO_IDENTIFICACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
    }
}
