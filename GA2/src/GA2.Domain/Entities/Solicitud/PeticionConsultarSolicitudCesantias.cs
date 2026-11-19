using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class PeticionConsultarSolicitudCesantias
    {
        public string? Identificacion { get; set; }
        public int? EstadoTarea { get; set; } //Pendiente, En tramite, Subsanacion, Aprobada, Rechazada
        public Guid? TipoSolicitud { get; set; }
        public string? NumRadicado { get; set; }
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFin { get; set; }
        public Guid? Modalidad { get; set; }
        public Guid? SubModalidad { get; set; }
        public string NumeroSolictud { get; set; }
        public string UsuarioIngreso { get; set; }
        public string Solicitante { get; set; }
        public string usuario { get; set; }
        // public Guid? idUsuario { get; set; }

    }
}
