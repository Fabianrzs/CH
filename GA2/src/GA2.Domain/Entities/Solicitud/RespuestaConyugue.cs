using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class RespuestaConyugue
    {
        public int CnyId { get; set; }
        public int CliId { get; set; }
        public int CnyTipo { get; set; }
        public string CnyIdentificacion { get; set; }
        public string CnyPrimerNombre { get; set; }
        public string CnySegundoNombre { get; set; }
        public string CnyPrimerApellido { get; set; }
        public string CnySegundoApellido { get; set; }
        public DateTime CnyFechaCrea { get; set; }
        public bool CnyActivo { get; set; }
    }
}
