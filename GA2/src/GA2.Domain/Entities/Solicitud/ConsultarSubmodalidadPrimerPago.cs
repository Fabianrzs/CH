using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConsultarSubmodalidadPrimerPago
    {
        public Guid SPP_ID { get; set; }
		public string SPP_DESCRIPCION { get; set; }
		public Guid TIM_ID_FK { get; set; } 
		public DateTime SPP_FECHA_REGISTRO { get; set; }
        public string TIM_DESCRIPCION { get; set; }
    }
}
