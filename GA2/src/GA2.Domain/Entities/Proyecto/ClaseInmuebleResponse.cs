using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class ClaseInmuebleResponse
    {
        public int Codigo { get; set; }
        public Guid ProyectoId { get; set; }
        public string ClaseVivienda { get; set; }
        public string NumeroVivienda { get; set; }
        public string DireccionInmueble { get; set; }
    }
}
