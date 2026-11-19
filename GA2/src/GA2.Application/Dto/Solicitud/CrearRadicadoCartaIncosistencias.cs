using System;

namespace GA2.Application.Dto
{
    public class CrearRadicadoCartaIncosistencias
    {
        public int? NoFolios { get; set; }
        public string? Asunto { get; set; }
        public string? Radicado { get; set; }
        public int clienteId { get; set; }
        public string Codigo { get; set; }
        public Guid SolicitudId { get; set; }
        public int IdComunicacion { get; set; }
    }
}
