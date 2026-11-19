using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class InconsistenciaSeguridadSolicitud
    {
      public Guid INC_ID { get; set; }
      public string INC_INCONSISTENCIA { get; set; }
      public bool INC_ESTADO { get; set; }
      public int INC_GRUPO { get; set; }
      public bool INC_SUBSANADO { get; set; }
      public Guid SOL_ID_FK { get; set; }
      public Guid USR_ID_FK { get; set; }
      public DateTime FECHA_CREACION { get; set; }
      public DateTime FECHA_SUBSANACION { get; set; }
      public DateTime FECHA_REGISTRO { get; set; }
      public string RADICADO { get; set; }
      public int ID_CLIENTE { get; set; }
      public string TPO_NOV_ID { get; set; }
      public string CAU_ID { get; set; }
      public string INF_ID { get; set; }  
      public string TPO_NOV_ID_NUEVO { get; set; }
      public string CAU_ID_NUEVO { get; set; }
      public string INF_ID_NUEVO { get; set; }
      public string INC_PUBLICA { get; set; }
      public Guid SLT_ID { get; set; }

    }
}
