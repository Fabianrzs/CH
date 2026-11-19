using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ObtenerFlujosDodo
    {
        public int tipoDocumentoId { get; set; }
        public string numeroDocumento { get; set; }
        public bool tieneFlujo37 { get; set; }
        public bool tieneFlujo102 { get; set; }
        public bool tieneFlujo109 { get; set; }
        public bool tieneFlujo194 { get; set; }
    }
}
