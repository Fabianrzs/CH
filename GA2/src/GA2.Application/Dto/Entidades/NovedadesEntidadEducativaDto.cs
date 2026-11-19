using System;

namespace GA2.Application.Dto
{
    public class NovedadesEntidadEducativaDto
    {
        public Guid IdNovedad { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string usuario { get; set; }
    }
}
