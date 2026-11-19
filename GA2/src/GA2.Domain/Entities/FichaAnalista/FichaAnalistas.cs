using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.FichaAnalista
{
    public  class FichaAnalistas
    {
        public int FA_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public string FA_NOMBRE { get; set; }
        public string FA_CARGO { get; set; }
    }
}
