using System;

namespace GA2.Application.Dto
{
    public class ParametrosObtenerCesantiasDto
    {
        public Guid solicitudId { get; set; }
        public Guid subModalidadId { get; set; }
        public int? IdCliente { get; set; }
    }

    public class ParametrosMovContableDto
    {
        public Guid solicitudId { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
    }
    public class ParametrosMovContableReintegroDto
    {
        public Guid IdReintegro { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
    }
}
