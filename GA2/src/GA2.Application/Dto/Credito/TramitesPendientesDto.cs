using System;

namespace GA2.Application.Dto
{

    /// <summary>
    /// Propiedades Dto de TramitesPendientes 
    /// </summary>
    public class TramitesPendientesDto
    {
        public Guid SolicitudId { get; set; }
        public int TipoCredito { get; set; }
        public string DescripcionCredito { get; set; }
        public int NumeroSolicitud { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public double ValorCredito { get; set; }
        public double Plazo { get; set; }
        public double TasaEA { get; set; }
    }
}
