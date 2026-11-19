using System;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Programa Educativa Dto
    /// </summary>
    public class ListarProgramaEducativoDto
    {
        public Guid IdEntidad { get; set; }
        public string RazonSocialEntidad { get; set; }
        public Guid IdPrograma { get; set; }
        public string Programa { get; set; }
        public Guid NivelEducativoId { get; set; }
        public string NivelEducativo { get; set; }
        public string CatalogoValorId { get; set; }
        public string CatalogoValorDescripcion { get; set; }
        public bool Estado { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
        public Guid IdProgramaEntidad { get; set; }
    }
}
