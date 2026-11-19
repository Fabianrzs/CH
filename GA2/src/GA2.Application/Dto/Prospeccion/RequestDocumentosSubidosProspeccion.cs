using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class RequestDocumentosSubidosProspeccion
    {
        public int TipoDocumento { get; set; }
        public Guid IdSolicitud { get; set; }
    }
}
