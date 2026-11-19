using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class SolicitudPostulacionRequestDto
    {
        public int? EtapasId { get; set; }
        public long? Identificacion { get; set; }
        public int? TipoSolicitudId { get; set; }
        public int? NoConvocatoria { get; set; }
        public string? NoRadicado { get; set; }
    }
}
