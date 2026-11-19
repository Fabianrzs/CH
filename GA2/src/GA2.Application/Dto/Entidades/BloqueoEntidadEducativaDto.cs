using System;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Entidad Educativa Dto
    /// </summary>
    public class BloqueoEntidadEducativaDto
    {
        public Guid IdBloqueo { get; set; }
        public Guid IdEntidad { get; set; }
        public int IdTipoOperacion { get; set; }
        public int IdCausalBloqueo { get; set; }
        public DateTime FechaBloqueo { get; set; }
        public Guid FuncionarioBloqueo { get; set; }
        public int TipoOperacionId { get; set; }
        public string NombreOperacion { get; set; }
        public int CausalOperacionId { get; set; }
        public string NombreCausal { get; set; }
        public string UserName { get; set; }
        public string Observaciones { get; set; }
    }
}
