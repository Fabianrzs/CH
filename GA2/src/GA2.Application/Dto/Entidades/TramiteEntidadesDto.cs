using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Entidades
{
    public class TramiteEntidadesDto
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public string beneficiario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Programa { get; set; }
        public string Nivel { get; set; }
        public string SubModalidad { get; set; }
        public string Modalidad { get; set; }
        public string Radicado { get; set; }
        public string Cedula { get; set; }
        public string Estado { get; set; }
        public int IdEstado { get; set; }
    }
}
