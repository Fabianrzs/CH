using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reintegros
{
    public class CorrespondenciaReintegro
    {
        public string identificacion { get; set; }
        public string observaciones { get; set; }
        public string cargo { get; set; }
        public string contacto { get; set; }
        public string correoElectronico { get; set; }
        public string ciudad { get; set; }
        public string radicadoRelacionado { get; set; }
        public string modalidad { get; set; }
        public string unidadEjecutora { get; set; }
        public string radicadoRelacionadoUE { get; set; }
        public string radicado { get; set; }
    }
}
