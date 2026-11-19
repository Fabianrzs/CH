using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConsultarPagoSucesivoSolicitud
    {
        public Guid PAS_ID { get; set; }
        public string PAS_DESCRIPCION { get; set; }
        public int PAS_CODIGO { get; set; }
        public int PAS_ORDEN { get; set; }
        public int PAS_GRUPO { get; set; }
}
}
