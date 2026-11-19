using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConsultarCausalInterrupcionSolicitud
    {
        public Guid CAU_ID { get; set; }
        public string CAU_DESCRIPCION { get; set; }
        public int CAU_CODIGO { get; set; }
        public int CAU_ORDEN { get; set; }
        public string NOV_DESCRIPCION { get; set; }
        public int CAU_GRUPO { get; set; }
    }
}
