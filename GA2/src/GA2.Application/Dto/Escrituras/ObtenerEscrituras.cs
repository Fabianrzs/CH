using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Escrituras
{
    public class RequestObtenerEscrituras
    {
        public Guid solicitud_id { get; set; }
        public string cli_identificacion { get; set; }
    }
    public class ObtenerEscrituras
    {
        public int id_escritura { get; set; }
        public Guid soliditud_id { get; set; }
        public string cli_identificacion { get; set; }
        public string escritura { get; set; }
        public DateTime fecha_escritura { get; set; }
        public string notaria { get; set; }
    }
}
