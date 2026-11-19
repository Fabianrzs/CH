using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class EstadosCatalogosValorSolicitudes
    {

        public int CVL_ID { get; set; }
		public int CAT_ID { get; set; }
		public int CVL_CODIGO { get; set; }
		public string CVL_DESCRIPCION { get; set; }
		public int CVL_ACTIVO { get; set; }

    }
}
