using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class SolicitudEscrituracionDto
    {
        public Guid SIE_ID { get; set; }
        public Guid SOL_ID_FK { get; set; }
        public string SIE_NOTARIA { get; set; }
        public string SIE_NUMERO_ESCRITURACION { get; set; }
        public string SIE_FECHA_ESCRITURACION { get; set; }
    }
}
    