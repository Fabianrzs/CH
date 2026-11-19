using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class LogisticaAdjudicacion
    {
        public string TimpoDesplazamiento { get; set; }
        public string Distancia { get; set; }
        public string Acompañante { get; set; }
        public int TipoIdentificacion { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public  Guid AdjudicacionProyecto { get; set; }    
    }
}
