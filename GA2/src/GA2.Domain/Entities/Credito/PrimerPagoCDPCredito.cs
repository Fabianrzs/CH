using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class PrimerPagoCDPCredito
    {
        public Guid PPC_ID { get; set; }
        public string PPC_CDP { get; set; }
        public Guid SLS_ID { get; set; }
        public bool PPC_ESTADO { get; set; }
        public DateTime PPC_FECHA_REGISTRO {get; set;}
}
}
