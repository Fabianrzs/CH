using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Prospeccion
{
    public class IdentificacionProspeccion
    {
        public int tipoDocumento { get; set; }
        public string numeroDocumento { get; set; } 
        public Guid USR_ID { get; set; }
        public Guid SLS_ID { get; set; }
    }
}
