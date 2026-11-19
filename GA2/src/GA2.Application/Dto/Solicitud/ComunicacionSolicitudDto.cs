using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ComunicacionSolicitudDto
    {
        public Guid? IdComunicacion { get; set; }
        public Guid? IdSolicitud { get; set; }
        public int IdCausalComunicacion { get; set; }
        public string TextoIncluir { get; set; }
        public bool FirmaDigital { get; set; }
        public string? Radicado { get; set; }
        public string? RadicadoRelacionado { get; set; }
    }
}
