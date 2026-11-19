using System;

namespace GA2.Application.Dto.Proyecto
{
    public class RequestConstructoraProyectoDto
    {
        public string? ProyectoId { get; set; } = null;
        public int? IdDepartamento { get; set; } = null;
        public int? IdCiudad { get; set; } = null;
        public int? IdEstado { get; set; } = null;
    }
}
