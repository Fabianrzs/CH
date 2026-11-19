using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.PAXC
{
    public class Paxc
    {
        public int PAXC_ID { get; set; }
        public Guid USR_ID { get; set; }
        public string USR_NOMBRE { get; set; }
        public int DPC_ID { get; set; }
        public string DPC_CIUDAD { get; set; }
    }
}
