using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class DocumentoCargarSolicitud
    {
        public int TDC_ID { get; set; }
        public string TDC_DESCRIPCION { get; set; }
        public string DCA_OBLIGATORIO { get; set; }
        public string DCA_CARGA { get; set; }
        public string DCA_CARGA_PREAPRBADO { get; set; }
        public string DCA_VISUALIZA { get; set; }
        public string DCA_VISUALIZA_CLIENTE { get; set; }
    }
}
