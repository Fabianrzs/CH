using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ValidacionCreditoProspeccion
    {
        public Guid VCP_ID { get; set; }
        public string VCP_NUMERO_DOCUMENTO { get; set; }
        public bool VCP_CARGAR_DOCUMENTO { get; set; }
        public bool VCP_ESTADO { get; set; }
        public DateTime VCP_FECHA { get; set; }
    }
}
