using System;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Programa Educativa Dto
    /// </summary>
    public class ProgramaEducativoDto
    {
        public Guid Id { get; set; }
        public string Programa { get; set; }
        public int Estado { get; set; }
        public Guid IdEntidad { get; set; }
        public Guid nivelEducativoId { get; set; }
        public int idTipoPrograma { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificacionPor { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
