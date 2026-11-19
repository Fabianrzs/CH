using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class DocumentosCredito
    {
      
        public int TAC_ID { get; set; }

        public int TDC_ID { get; set; }

        public string DCA_OBLIGATORIO { get; set; }

        public string DCA_CARGA { get; set; }

        public string DCA_VISUALIZA { get; set; }
       
    }
}
