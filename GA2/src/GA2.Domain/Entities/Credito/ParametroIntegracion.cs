using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ParametroIntegracion
    {
        public Guid PARAM_ID { get; set; }
        public string PARAMETRO { get; set; }
        public string INTEGRACION { get; set; }
        public string VALOR { get; set; }
    }
}
