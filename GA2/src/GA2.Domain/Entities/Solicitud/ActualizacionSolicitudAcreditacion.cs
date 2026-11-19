using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ActualizacionSolicitudAcreditacion
    {
        public string? CumpleRequisito { get; set; }
        public string? Causal { get; set; }
        public string? AccionRealizar { get; set; }
        public string? Acuerdo { get; set; }
        public Guid SolicitudId { get; set; }
    }
}
