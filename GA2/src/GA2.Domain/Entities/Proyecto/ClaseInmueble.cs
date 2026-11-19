using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class ClaseInmueble
    {
        public Guid ProyectoId { get; set; }
        public string ClaseVivienda { get; set; }
        public string NumeroVivienda { get; set; }
        public string DireccionInmueble { get; set; }
    }
}
