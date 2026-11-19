using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class BloqueoRecursosCredito
    {
        public Guid BRC_ID { get; set; }
        public int BRC_TIPO_IDENTIFICACION { get; set; }
        public string BRC_NUMERO_IDENTIFICACION { get; set; }
        public int BRC_NUMERO_CUENTA { get; set; }
        public int BRC_ESTADO { get; set; }
    }
}
