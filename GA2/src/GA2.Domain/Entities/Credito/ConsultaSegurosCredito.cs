using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ConsultaSegurosCredito
    {

        public string SSC_CONVENIO_VIDA { get; set; }
        public string SEGURO_VIDA_DESC { get; set; }
        public string SSC_CONVENIO_HOGAR { get; set; }
        public string SEGURO_HOGAR_DESC { get; set; }
        public Guid SLS_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public Guid SDP_ID { get; set; }
    }
}
