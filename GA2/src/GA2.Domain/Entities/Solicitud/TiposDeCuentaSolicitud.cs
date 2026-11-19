using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class TiposDeCuentaSolicitud
    {
       public Guid CSA_ID { get; set; }
        
       public string CSA_DESCRIPCION { get; set; }

       public int CSA_ORDEN { get; set; }
    
       public string CSA_CODIGO { get; set; }
    
       public DateTime CSA_FECHA_CREACION { get; set; }
    
       public DateTime CSA_FECHA_MODIFICACION { get; set; }
       
       public bool CSA_ESTADO { get; set; }
    
    }
}
