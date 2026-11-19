using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class BuscarIntervinientesDto
    {
        public int TipoDocumento { get; set; }
        public int NumSolicitud { get; set; }
        public string Radicado { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public Guid? ActorId { get; set; }
    }
}
