using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Embargos.Seven
{
    public class PagosEmbargosSevenRespuesta
    {

        public Guid EBA_ID { get; set; }
        public int PEM_CODIGO_PROVEEDOR { get; set; }
        public int PEM_CDP { get; set; }
        public int PEM_NUMERO_FACTURA { get; set; }
        public int PEM_FLUJO { get; set; }
    }
}
