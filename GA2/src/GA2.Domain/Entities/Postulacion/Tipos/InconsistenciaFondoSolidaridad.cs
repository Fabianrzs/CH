using System;

namespace GA2.Domain.Entities
{
    public class InconsistenciaFondoSolidaridad
    {
        public Guid IdSolicitud { get; set; }
        public Guid Code { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
