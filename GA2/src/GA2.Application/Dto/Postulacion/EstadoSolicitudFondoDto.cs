using System;

namespace GA2.Application.Dto.Postulacion
{
    public class EstadoSolicitudFondoDto
    {
        public string NoRadicado { get; set; }
        public string Modalidad { get; set; }
        public string Estado { get; set; }
        public string EstadoArchivo { get; set; }
        public string EstadoVerificacion { get; set; }
        public string EstadoEntrevista { get; set; }
        public string EstadoCalificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
