using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ResponseDocumentoSolicitudDto
    {
        [Key]
        public Guid idDocumentoSolicitud { get; set; }
        public Guid idSolicitud { get; set; }
        public int idTipoDocumento { get; set; }
        public string codigoBarras { get; set; }
        public DateTime fechaDocumento { get; set; }
        public int numeroFolios { get; set; }
        public long tamaño { get; set; }
        public string rutaImagen { get; set; }
        public string estado { get; set; }
        public Guid creadoPor { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
