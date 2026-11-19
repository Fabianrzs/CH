using System;
using System.Text.Json.Serialization;

namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    
    public class ProcesoGeneracionReporteDto
    {
        public Guid IdProceso { get; set; }
        public string TipoReporte { get; set; }
        public string NombreReporte { get; set; }
        public string Estado { get; set; }
        public int Progreso { get; set; }
        public string DetalleProceso { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        [JsonIgnore]
        public string BlobContenedor { get; set; }

        [JsonIgnore]
        public string BlobRutaAcceso { get; set; }
    }

    public class ActualizacionEstadoProcesoGeneracionReporteDto
    {
        public Guid IdProceso { get; set; }
        public string Estado { get; set; }
        public int? Progreso { get; set; }
        public string DetalleProceso { get; set; }
        public bool ConcatenarDetalle { get; set; } = true;
    }

    public class ConsultaProcesoGeneracionReporteDto
    {
        public Guid? IdProceso { get; set; }
        public string TipoReporte { get; set; }
        public string NombreReporte { get; set; }
        public string Estado { get; set; }
        public Guid? CreadoPor { get; set; }
    }
}
