using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Escritura
{
    public class Escritura
    {
        public int ESC_ID { get; set; }
        public Guid SOC_ID_SOLICITUD { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string ESC_ESCRITURA { get; set; }
        public DateTime ESC_FECHA { get; set; }
        public string ESC_NOTARIA { get; set; }
    }
}
