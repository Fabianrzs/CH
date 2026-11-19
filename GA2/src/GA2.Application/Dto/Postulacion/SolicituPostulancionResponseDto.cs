using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Postulacion
{
    public class SolicituPostulancionResponseDto
    {
        public Guid IdSolicitud { get; set; }
        public int TipoDocumentoId { get; set; }
        public long NumeroDocumeto { get; set; }
        public int IdTipoSolicitud { get; set; }
        public int IdModalidad { get; set; }
        public int IdSubModalidad { get; set; }
        public int ManifestacionAutorizacion { get; set; }
        public int Departamento1 { get; set; }
        public int Ciuadad1 { get; set; }
        public int Departameno2 { get; set; }
        public int Ciudad2 { get; set; }
        public int Deparmento3 { get; set; }
        public int Ciudad3 { get; set; }
        public int FirmaSolicitud { get; set; }
        public long NumeroDocumentoFirma { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public long NumeroCelular { get; set; }
        public int NumeroConvocatoria { get; set; }
        public IEnumerable<PostulanteResponseDto> postulantes { get; set; }
        public string TipoSolicitudNombre { get; set; }
        public string TipoModalidadNombre { get; set; }
        public string TipoSubModalidadNombre { get; set; }
        public string EstadoProceso { get; set; }
        public string Actividad { get; set; }
        public string Observacion { get; set; }
        public string ObservacionDocumento { get; set; }
        public string NombreFirma { get; set; }
        public string ApellidoFirma { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? NombreCompleto { get; set; }
        public bool RepresentanteLegal { get; set; }
        public string DisfrutePension { get; set; }
        public string? HechoDescripcion { get; set; }
        public string? Categoria { get; set; }
        public string? UnidadEjecutora { get; set; }
        public string? NombreCompletoAfiliado { get; set; }
        public int NoConvocatoria { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string NoRadicado { get; set; }
        public string NumeroResolucion { get; set; }
        public DateTime? FechaApertura { get; set; }
        public string? FechaCierre { get; set; }
        public string NumeroResolucionCierre { get; set; }
        public string ModalidadSolucionVivienda { get; set; }
        public string AfectacionCuenta { get; set; }
        public double ValorAfectacion { get; set; }
        public DateTime? FechaRetiroFuerza { get; set; } = null;
        public DateTime? FechaRetiro { get; set; } = null;
        public DateTime? FechaAsignacionInvalidez { get; set; } = null;
        public Boolean MismoAfiliado { get; set; }
    }
}
