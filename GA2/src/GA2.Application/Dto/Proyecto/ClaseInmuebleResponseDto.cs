using System;

namespace GA2.Application.Dto.Proyecto
{
    public class ClaseInmuebleResponseDto
    {
        public int Codigo { get; set; }
        public Guid ProyectoId { get; set; }
        public string ClaseVivienda { get; set; }
        public string NumeroVivienda { get; set; }
        public string DireccionInmueble { get; set; }
    }
}
