using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.Seven
{
    public class RequestAcualizarFacContDto
    {
        public Guid idSolicitud { get; set; }
        public string fac_cont { get; set; }
        public string fac_nume { get; set; }
        public int tipo_pago { get; set; }
    }
}
