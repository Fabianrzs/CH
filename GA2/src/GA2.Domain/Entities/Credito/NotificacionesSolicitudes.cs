using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class NotificacionesSolicitudes
    {
       public Guid NOT_ID { get; set; }
       public string NOT_CDP { get; set; } 
       public Guid SLS_ID  { get; set; }
       public DateTime NOT_FECHA { get; set; }

       public int NOT_CONSECUTIVO { get; set; }
       public string NOT_CONCECUTIVO_CDP { get; set; }
    }
}
