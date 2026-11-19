using System;

namespace GA2.Domain.Entities.Entidades
{
    public class NovedadesEntidadEducativa
    {
        public Guid IdNovedad { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string usuario { get; set; }
    }
}
