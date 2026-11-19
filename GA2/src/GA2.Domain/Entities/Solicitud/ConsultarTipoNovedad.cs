using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConsultarTipoNovedad
    {
      public Guid TPO_NOV_ID { get; set; }
      public string TPO_NOV_DESCRIPCION { get; set; }
      public int TPO_NOV_CODIGO { get; set; }
      public int TPO_NOV_ORDEN { get; set; }
      public Guid NOV_ID { get; set; }
      public int NOV_GRUPO { get; set; }
      public int TPO_NOV_GRUPO { get; set; }
    }
}
