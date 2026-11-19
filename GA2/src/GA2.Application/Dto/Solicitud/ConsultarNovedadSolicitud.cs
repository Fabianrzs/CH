using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ConsultarNovedadSegSolicitud
    {
        public Guid nov_id { get; set; }

        public string nov_descripcion { get; set; }

        public int nov_codigo { get; set; }

        public int nov_orden { get; set; }
    }
}
