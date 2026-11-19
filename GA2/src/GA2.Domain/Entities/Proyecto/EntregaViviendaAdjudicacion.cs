using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class EntregaViviendaAdjudicacion
    {
        public DateTime FechaCreacion{ get; set; }
        public string Entregada { get; set; }
        public string Observacion { get; set; }
        public Guid AdjudicacionProyecto { get; set; }
    }
}
