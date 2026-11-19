using System;

namespace GA2.Application.Dto.Proyecto
{
    public class SupervisorResponseDto
    {
        public int Codigo { get; set; }
        public int TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public Guid ProyectoId { get; set; }
    }
}
