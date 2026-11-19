using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Entidades
{
    public class RequestDocumentoEntidadesDto
    {
        [Key]
        public Guid idEntidades { get; set; }
        public string rutaImagen { get; set; }
        public Guid idArchivo { get; set; }
        public string numeroRadicado { get; set; }

    }
}
