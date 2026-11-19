using System;

namespace GA2.Application.Dto
{
    public class NovedadesEntidadEducativaRequestDto
    {
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid CodigoUsuario { get; set; }
        public Guid IdEntidad { get; set; }
    }
}
