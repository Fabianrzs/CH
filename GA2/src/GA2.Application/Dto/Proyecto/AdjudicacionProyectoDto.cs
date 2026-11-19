using System;

namespace GA2.Application.Dto.Proyecto
{
    public class AdjudicacionProyectoDto
    {
        public string EstadoAdjudicacion { get; set; }
        public string RadicacionAceptacion { get; set; }
        public Guid  IdProyecto { get; set; }
        public Guid IdSolictud { get; set; }
        public int TipoInmueble { get; set; }
        public int TipoVivienda { get; set; }
        public int ClaseVivienda { get; set; }
        public double ValorVivienda { get; set; }
    }
}
