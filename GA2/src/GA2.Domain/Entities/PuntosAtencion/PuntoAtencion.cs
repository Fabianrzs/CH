using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.PuntosAtencion
{
    public class PuntoAtencion
    {
        public int PA_ID { get; set; }
        public string PA_PUNTO { get; set; }

        public int PA_TIPO_AFILIADO { get; set; }
    }
}
