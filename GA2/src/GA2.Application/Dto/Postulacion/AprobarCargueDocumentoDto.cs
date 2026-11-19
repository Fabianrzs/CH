using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Postulacion
{
    public class AprobarCargueDocumentoDto
    {
        public Guid IdSolicitud { get; set; }
        public int Etapa { get; set; }
    }
}
