using Microsoft.AspNetCore.Http;
using System;

namespace GA2.Application.Dto
{
    public class CargarDocumentosSolicitudDto
    {
        public Guid archivoParametrizadoId { get; set; }
        public IFormFile archivosCargados { get; set; }
        public string numeroFolio { get; set; }
    }
}
