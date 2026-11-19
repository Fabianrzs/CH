using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class IntervinientesActor
    {
        public Guid ITA_ID { get; set; }
        public string ITA_NOMBRE { get; set; }
        public bool ITA_ESTADO { get; set; }
        public string ITA_CODIGO { get; set; }
        public DateTime ITA_FECHA_REGISTRO { get; set; }
}
}
