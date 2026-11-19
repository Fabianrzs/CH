using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class SupervisorResponse
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
