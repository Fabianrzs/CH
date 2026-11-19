using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class EstadoCausalCliente
    {
        public int CEC_ID { get; set; }
        public string CEC_CAUSAL { get; set; }
        public int ECL_ID { get; set; }
        public bool CEC_ACTIVO { get; set; }
    }
}
