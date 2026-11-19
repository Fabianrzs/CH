using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ExcepcionTasaUEJDto
    {
        public int IdParametro { get; set; }
        public int IdSimulador { get; set; }
        public int UEJ { get; set; }
        public double TasaEA { get; set; }
    }
}
