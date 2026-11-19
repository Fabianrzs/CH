using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class SubOpciones
    {
        public int SOP_ID { get; set; }
        public string SOP_CODIGO { get; set; }
        public string SOP_NOMBRE { get; set; }
        public string SOP_DESCRIPCION { get; set; }
        public int SOP_ESTADO { get; set; }
        public int OPC_SEG_OPCIONES_ID { get; set; }
    }
}
