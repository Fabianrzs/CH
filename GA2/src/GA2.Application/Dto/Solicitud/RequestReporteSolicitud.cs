using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class RequestReporteSolicitud
    {
        public String? Radicado { get; set; }
        public int? TipoSolicitudes { get; set; }
        public Guid? IdTipoSolicitudes { get; set; }
        public String? usuario { get; set; }
        public DateTime? FechaInicio { get; set; } = null;
        public DateTime? FechaFin { get; set; } = null;
        public DateTime? DiasVencimiento { get; set; } = null;
        public Guid UserId { get; set; }
        public List<String> Estados { get; set; }
    }
}
