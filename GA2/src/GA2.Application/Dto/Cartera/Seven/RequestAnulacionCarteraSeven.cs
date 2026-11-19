using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Seven
{
    public class RequestAnulacionCarteraSeven
    {
        public long numeroCredito { get; set; }
        public Guid cpgId { get; set; }
        public int Consecutivo_Seven_Id { get; set; }
        public int Cc_Id { get; set; }
        public int Top_codi { get; set; }
        public int Comprobante_Seven_Id { get; set; }
        
    }
}
