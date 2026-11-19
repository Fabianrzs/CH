using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class CheckDocumentosCreditoDto
    {
        public Guid SolicitudCreditoId { get; set; }
        public string[] DocumentosVerificados { get; set; }
    }
}
