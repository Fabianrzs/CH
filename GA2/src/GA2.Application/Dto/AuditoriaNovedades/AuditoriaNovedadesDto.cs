using System;


namespace GA2.Application.Dto
{
    public class AuditoriaNovedadesDto
    {
        public int? TipoDocumento { get; set; }
        public string NDocumento { get; set; }
        public string? DetalleModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public Guid? UsuarioModificacion { get; set; }
        public int IdModulo { get; set; }
        public string Modulo { get; set; }
        public string tipoAccion { get; set; }
        public int IdUnidadEjecutora { get; set; }
    }
}
