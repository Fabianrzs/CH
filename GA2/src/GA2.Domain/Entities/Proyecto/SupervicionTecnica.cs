using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class SupervicionTecnica
    {
        public Guid? IdSupervicionTecnica { get; set; }
        public Guid IdProyecto { get; set; }
        public string PersonaAtendioVisita { get; set; }
        public string Cargo { get; set; }
        public string SupervisoTecnicoTipoDocumento { get; set; }
        public string NombreSupervisorTecnico { get; set; }
        public DateTime FechaVisita { get; set; }
        public string DesarrolloProyecto { get; set; }
        public string ObservacionGeneral { get; set; }
    }
}
