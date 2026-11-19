using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConsultarNovedad
    {
        public Guid NOV_ID { get; set; }

        public string NOV_DESCRIPCION { get; set; }

        public int NOV_CODIGO { get; set; }

        public int NOV_ORDEN { get; set; }

        public int NOV_GRUPO { get; set; }

    }
}
