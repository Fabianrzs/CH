using System;
namespace GA2.Application.Dto.AuditoriaNovedades
{
    public class AuditoriaNovedadesGetDto
    {
        public string TipoDocumento { get; set; }
        public int NDocumento { get; set; }
        public string DetalleModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Guid UsuarioModificacion { get; set; }
        public int IdModulo { get; set; }
        public string Modulo { get; set; }
        public string TipoAccion { get; set; }
        public string NombreUnidadEjecutora { get; set; }
    }
}
