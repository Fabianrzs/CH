using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class TrazabilidadSolicitudDto
    {
        public Guid IdTareaSolicitud { get; set; }
        public Guid IdTarea { get; set; }
        public string DescripcionTarea { get; set; }
        public Guid IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int IdEstadoSolicitud { get; set; }
        public string DescripcionEstadoSolicitud { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFin { get; set; }

    }
}
