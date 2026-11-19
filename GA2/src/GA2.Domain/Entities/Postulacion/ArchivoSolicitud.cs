using System;

namespace GA2.Domain.Entities
{
    public class ArchivoSolicitud
    {
        public Guid IdArchivo { get; set; }
        public string NombreArchivoCargado { get; set; }
        public string RutaArchivo { get; set; }
        public string Extension { get; set; }
        public string NombreArchivo { get; set; }
        public int Orden { get; set; }
        public string? Observacion { get; set; }
    }
}
