using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class TramiteSolicitudDto
    {

       public Guid TRS_ID { get; set; }
       public int FLU_ID { get; set; }
       public Guid SOC_ID { get; set; }
       public string TRS_OBSERVACION { get; set; }
       public int TRS_ULTIMA_ACTIVIDAD { get; set; }
       public DateTime TRS_FECHA_INICIO { get; set; }
       public DateTime TRS_FECHA_FINALIZACION { get; set; }
       public Guid ID_USUARIO_PERITO { get; set; }
       public Guid TRS_CREADO_POR { get; set; }
       public DateTime TRS_FECHA_CREACION { get; set; }
       public Guid  TRS_MODIFICADO_POR { get; set; }
       public DateTime TRS_FECHA_MODIFICACION { get; set; }
    }

    public class RequestTramiteSolicitudDto {

        public Guid TRS_ID { get; set; }
        public Guid ACT_ID { get; set; }
        public string TRS_OBSERVACION { get; set; }
        public Guid ID_USUARIO_PERITO { get; set; }
        public Guid? TRS_MODIFICADO_POR { get; set; }
        public DateTime? TRS_FECHA_MODIFICACION { get; set; }

    }

}
