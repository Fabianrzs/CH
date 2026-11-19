using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class SegurosDatosSolicitudCredito
    {

        public Guid SSC_ID { get; set; }
        public string SSC_CONVENIO_VIDA { get; set; }
        public string SSC_SEGURO_VIDA { get; set; }
        public string SSC_CONVENIO_HOGAR { get; set; }
        public string SSC_SEGURO_HOGAR { get; set; }
        public string SCC_NUMERO_DOCUMENTO { get; set; }
        public Guid SDP_ID { get; set; }
    }
}
