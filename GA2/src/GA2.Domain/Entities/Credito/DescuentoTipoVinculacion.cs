using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class DescuentoTipoVinculacion
    {
      public Guid DTV_ID { get; set; }
      public string DTV_DESCRIPCION { get; set; }
      public double DTV_PORCENTAJE { get; set; }
      public string DTV_TIPO_CREDITO { get; set; }
      public string DTV_CONCEPTO { get; set; }
      public DateTime DTV_FECHA_REGISTRO { get; set; }
      public bool DTV_ESTADO { get; set; }
      public int DTV_ORDEN { get; set; }
    }
}
