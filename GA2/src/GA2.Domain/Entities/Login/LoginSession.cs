using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class LoginSession
    {
        public int LSS_ID { get; set; }
        public Guid USR_ID { get; set; }
        public DateTime LSS_FECHA_INICIO_LOGIN { get; set; }
        public DateTime LSS_FECHA_ULTIMA_ACTUALIZACION { get; set; }
        public bool LSS_ACTIVA { get; set; }
    }
}
