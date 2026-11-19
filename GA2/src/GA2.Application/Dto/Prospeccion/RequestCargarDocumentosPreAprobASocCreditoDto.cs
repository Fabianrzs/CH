using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Prospeccion
{
    public class RequestCargarDocumentosPreAprobASocCreditoDto
    {
        public Guid Soc_id { get; set; }
        public int Afl_id { get; set; }
    }
}
