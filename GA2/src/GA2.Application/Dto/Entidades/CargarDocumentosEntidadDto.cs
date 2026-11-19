using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class CargarDocumentosEntidadDto
    {
        public Guid archivoParametrizadoId { get; set; }
        // public List<IFormFile>? ArchivosCargados { get; set; }
        public IFormFile archivoCargado { get; set; }
        public string numeroFolio { get; set; }
    }
}
