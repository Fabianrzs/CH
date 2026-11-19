using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class RequestDocumentoSolicitudDto
    {
        [Key]
        public Guid idSolicitud { get; set; }
        public string rutaImagen { get; set; }
        public string nombreDocumento { get; set; }
        public string DescripcionDocumento { get; set; }
        
    }
}
