using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.BUA
{
    public class CausalEstadoCliente
    {
        public int ECC_ESTADO_CAUSAL_ID { get; set; }
        public int ECC_CAUSAL_ID { get; set; }
        public int ECC_ESTADO_ID { get; set; }
        public int CLI_ID { get; set; }
        public string ECC_RAZON { get; set; }
        public string ECC_OBSERVACIONES { get; set; }
        public DateTime ECC_FECHA_ESTADO_CAUSAL { get; set; }
        public DateTime? ECC_FECHA_RETIRO { get; set; }
        public bool ECC_ESTADO { get; set; }
    }
}
