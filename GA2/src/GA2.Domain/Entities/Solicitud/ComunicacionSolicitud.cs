using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ComunicacionSolicitud
    {
        public Guid CMN_ID { get; set; }
        public Guid SOL_ID { get; set; }
	    public int CSC_ID { get; set; }
        public string CMN_TEXTO_INCLUIR { get; set; }
        public bool CMN_FIRMA_DIGITAL { get; set; }

        public string? CMN_RADICADO { get; set; }

        public string? CMN_RADICADO_RELACIONADO { get; set; }
    }
}
