using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ConvocatoriaPostulante
    {
        public Guid SolicitudId { get; set; }
        public int TipoDocumentoId { get; set; }
        public long NumeroDocumento { get; set; }
        public int NumeroConvocatoria { get; set; }
    }
}
