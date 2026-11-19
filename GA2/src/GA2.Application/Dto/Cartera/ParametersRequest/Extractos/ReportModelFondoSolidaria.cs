using System;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Extractos
{
    public class  ReportModelFondoSolidaria
    {
        public int? Modelo { get; set; } = null;    
        public string? Fuerza { get; set; } = null; 
        public string? UnidadEjecutora { get; set; } = null;    
        public DateTime? FechaPostulacionFinal { get; set; } = null;
        public DateTime? FechaPostulacionInicial { get; set; } = null;
        public DateTime? FechaInicialPrimerAporte { get; set; } = null;
        public DateTime? FechaInicialPrimerAporteDesde { get; set; } = null;
        public DateTime? FechaInicialPrimerAporteHasta { get; set; } = null;
        public DateTime? FchaUltimoAporteDesde { get; set; } = null;
        public DateTime? FchaUltimoporteHasta { get; set; } = null;
        public bool SinAportarAlaFecha { get; set; }
    }

    public class ReportModelFondoSolidariaResponse
    {
        public string TipoIdentificacionAfiliado { get; set; }
        public string NumeroDocumentoAfiliado { get; set; }
        public string NombreAfiliado { get; set; }
        public string NombreBeneficiario { get; set; }
        public string IdentificacionBeneficiario { get; set; }
        public int PorcentajeParticipacion { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Grado { get; set; }
        public string Modelo { get; set; }
        public int NumeroAporte { get; set; }
        public DateTime FechaUltimoAporte { get; set; }
        public DateTime FechaPrimerAporte { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public decimal Saldo { get; set; }
        public string Fuerza { get; set; }  
    }

    public class ReportStatusFondoSolidariaResponse
    {
        public int Consecutivo { get; set; }
        public string CedulaAfiliado { get; set; }
        public string NombrePostulante { get; set; }
        public string IdentificacionPostulante { get; set; }
        public string Radicado { get; set; }
        public string Fuerza { get; set; }
        public string Categoria { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Ciuedad { get; set; }
        public string EstadoSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Parentesco { get; set; }
        public decimal Participacion { get; set; }
    }
}
