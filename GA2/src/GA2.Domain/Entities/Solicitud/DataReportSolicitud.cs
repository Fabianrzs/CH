using System;
using System.Collections.Generic;

namespace GA2.Domain.Entities
{
    public class DataReportSolicitud
    {
        public List<RespuestaConsultarSolicitudCesantias> DataSolicitudes { get; set; }
        public List<Embargo> DataEmbargo { get; set; }
        public List<SolicitudPostulacionResponse> DataFondoSolidaridad { get; set; }
        public List<ReporteUnificado> DataReporteUnificado { get; set; }
    }

    public class ReporteUnificado
    {
        public string Radicado { get; set; }
        public string TipoSolicitud { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string IdentificacionAfiliado { get; set; }
        public string Afiliado { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public string Fuente { get; set; } 
        public string Fuerza { get; set; } 
        public string UnidadEjecutora { get; set; } 
        public string Grado { get; set; } 
        public string Categoria { get; set; } 
    }

}
