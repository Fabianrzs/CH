using System;

namespace GA2.Domain.Entities
{
    public class EstadoSolicitudFondo
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
