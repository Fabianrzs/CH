using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.Simulador.SolicitudCredito.Documento
{
    public class RequestDocumentoSimDto
    {
        [Key]
        public Guid idSimulacion { get; set; }
        public int idTipoDocumento { get; set; }
        public string codigoBarras { get; set; }
        public string rutaImagen { get; set; }
        public Guid creadoPor { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string NumeroDocumento { get; set; }
    }
}
