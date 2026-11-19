using System;

namespace GA2.Domain.Entities.Entidades
{
    public class NovedadesEntidadEducativaRequest
    {
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid CodigoUsuario { get; set; }
        public Guid IdEntidad { get; set; }
    }
}
