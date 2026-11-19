using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GA2.Application.Dto
{
    public class RequestDocumentoSolicCreditoDto
    {
        [Key]
        public Guid idSolicitudCredito { get; set; }
        public int idTipoDocumento { get; set; }
        public string codigoBarras { get; set; }
        public string rutaImagen { get; set; }
        public Guid creadoPor { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string numero_documento { get; set; }
        public string Radicado { get; set; }
}

    public class CargarDocumentosSolicitudCreditoDto
    {
        public List<RequestDocumentoSolicCreditoDto> Documentos { get; set; } = new();
        public string Radicado { get; set; }
    }
}
