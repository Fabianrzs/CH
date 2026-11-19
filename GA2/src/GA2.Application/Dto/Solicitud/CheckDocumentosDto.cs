using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class CheckDocumentosDto
    {
        public Guid IdSolicitud { get; set; }
        public string[] DocumentosVerificados { get; set; }

    }
}
