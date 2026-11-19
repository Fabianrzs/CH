using System;

namespace GA2.Application.Dto.Postulacion
{
    public class InconsistenciaFondoSolidaridadDTO
    {
        public Guid IdSolicitud { get; set; }
        public Guid Code { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
