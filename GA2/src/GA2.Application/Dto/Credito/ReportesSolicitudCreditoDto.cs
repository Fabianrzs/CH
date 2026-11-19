using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class ReportesSolicitudCreditoDto
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int? ConsecutivoSolicitud { get; set; }
        public int? TipoTarea { get; set; }
        public string? NumeroDocumento { get; set; }
        public int? EstadoCredito { get; set; }
        public Guid? UsuarioId { get; set; }

    }
}
