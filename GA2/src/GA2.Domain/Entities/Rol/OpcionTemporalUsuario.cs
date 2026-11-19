using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class OpcionTemporalUsuario
    {
        public int UOT_ID { get; set; }
        public Guid USR_ID { get; set; }
        public int OPC_ID { get; set; }
        public string OPC_NOMBRE { get; set; }
        public string OPC_DESCRIPCION { get; set; }
        public string OPC_CODIGO { get; set; }
        public DateTime UOT_FECHA_DESDE { get; set; }
        public DateTime UOT_FECHA_HASTA { get; set; }
    }
}
