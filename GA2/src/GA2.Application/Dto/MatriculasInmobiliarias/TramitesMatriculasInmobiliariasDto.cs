using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.MatriculasInmobiliarias
{
    public class TramitesMatriculasInmobiliariasDto
    {
        public string tipoDoc { get; set; }
        public string nroDoc { get; set; }
        public string nombreRazonSocial { get; set; }
        public string nroRadicado { get; set; }
        public string tipoSolicitud { get; set; }
        public string modalidad { get; set; }
        public DateTime fecSolicitud { get; set; }
        public decimal valorSolicitud { get; set; }
        public string estado { get; set; }
        public string Radicado { get; set; }
        public int idCliente { get; set; }
    }
}
