using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ObtenerConceptosAfectadosClientes
    {
        public Guid SolicitudId {  get; set; }

        public string Identificacion { get; set; }
    }
}
