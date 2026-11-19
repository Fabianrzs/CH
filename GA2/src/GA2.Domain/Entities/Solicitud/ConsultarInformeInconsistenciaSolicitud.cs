using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConsultarInformeInconsistenciaSolicitud
    {
        public Guid INF_ID { get; set; }
        public string INF_DESCRIPCION { get; set; }
        public int INF_CODIGO { get; set; }
        public int INF_ORDEN { get; set; }
        public int INF_GRUPO { get; set; }
}
}
