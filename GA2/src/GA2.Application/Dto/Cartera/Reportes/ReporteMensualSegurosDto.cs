using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteMensualSegurosDto
    {
        public long NumeroRadicacion { get; set; }
        public string TipoCredito { get; set; }
        public decimal ValorDesembolso { get; set; }
        public decimal ValorInmueble { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaTerminacion { get; set; }
        public string CiudadInmueble { get; set; }
        public string DireccionInmueble { get; set; }
        public string MatriculaInmueble { get; set; }
        public string TipoVivienda { get; set; }
        public decimal? ValorAvaluoComercial { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public DateTime? FechaExpedicionDocumento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Grado { get; set; }
        public string Categoria { get; set; }
        public decimal SeguroHogar { get; set; }
        public decimal SeguroVida { get; set; }
    } 
}
