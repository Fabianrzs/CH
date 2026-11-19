using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class ValidacionCreditoProspeccionDto
    {
        public Guid VcpId { get; set; }
        public string NumeroDocumento { get; set; }
        public bool CargarDocumento { get; set; }
        public bool Estado { get; set; }
        public DateTime Fecha { get; set; }
    }

}
