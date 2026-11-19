using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Parametrization
{
    public  class ParametroIntegracionDto
    {
        public Guid ParametroId { get; set; }
        public string Parametro { get; set; }
        public string Integracion { get; set; }
        public string Valor { get; set; }
    }
}
