using System;


namespace GA2.Application.Dto
{
    public class DetalleReintegroDto
    {

        public int IdDetalleReintegro { get; set; }
        public int IdReintegro { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificado { get; set; }
    }
}
