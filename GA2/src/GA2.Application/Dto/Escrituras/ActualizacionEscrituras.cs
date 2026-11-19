using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Escrituras
{
    public class ActualizacionEscrituras
    {
        [JsonIgnore]
        public int id_escritura { get; set; }
        public Guid solicitud_id { get; set; }
        public string cli_identificacion { get; set; }
        public string escritura { get; set; }
        public DateTime fecha_escritura { get; set; }
        public decimal notaria { get; set; }
    }
}
