using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class TramiteDesafiliacion
    {
        public int TMD_ID { get; set; }
        public int AFL_ID { get; set; }
        public Guid PCS_ID { get; set; }
        public Guid TIM_ID { get; set; }
        public decimal TMD_VALOR { get; set; }
    }
}
