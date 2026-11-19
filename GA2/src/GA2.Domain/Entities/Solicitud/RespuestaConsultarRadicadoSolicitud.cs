using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class RespuestaConsultarRadicadoSolicitud
    {
        public Guid SOL_ID { get; set; }
        public string SOL_RADICADO_GD { get; set; }
        public int CLI_ID { get; set; }
        public string SOL_CEDULA { get; set; }
        public string INC_DESCRIPCION { get; set; }
    }
}
