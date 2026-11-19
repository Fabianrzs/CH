using System;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Sede Entidad Educativa Dto
    /// </summary>
    public class SedeEntidadEducativaDto
    {
        public Guid? IdSedeEntidad { get; set; }
        public Guid? IdEntidad { get; set; }
        public string NombreSede { get; set; }
        public int CiudadId { get; set; }
        public bool Estado { get; set; }
        public string NombreCiudad { get; set; }
        public int CodigoCiudad { get; set; }
        public Guid? CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }

    }

    public class CrearAnotacionDTO
    {
        
        public Guid IdEntidad { get; set; }
        public string Anotacion { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
       

    }

    public class RespuestaAnotacionDTO
    {
        public string Anotacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }

    }

}
