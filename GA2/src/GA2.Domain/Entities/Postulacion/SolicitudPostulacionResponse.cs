using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class SolicitudPostulacionResponse
    {
        public Guid SolicitudId { get; set; }
        public Guid PostulacionSolicitudId { get; set; }
        public int TipoDocumentoId { get; set; }
        public int NumeroDocumento { get; set; }
        public string TipoSolicitudId { get; set; }
        public string TipoModalidadId { get; set; }
        public string TipoSubModalidadId { get; set; }
        public int NumeroConvocatoria { get; set; }
        public int ManifestacionAutorizacion { get; set; }
        public string Departamento1 { get; set; }
        public string Ciudad1 { get; set; }
        public string Deparamento2 { get; set; }
        public string Ciudad2 { get; set; }
        public string Deparamento3 { get; set; }
        public string Ciudad3 { get; set; }
        public string CodigoIdentificacionFirma { get; set; }
        public string NombreIdentificacionFirma { get; set; }
        public long NumeroDocumentoFirma { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public long NumeroCelular { get; set; }
        public string TipoSolicitudNombre  { get; set; }
        public string TipoModalidadNombre { get; set; }
        public string TipoSubModalidadNombre { get; set; }
        public string EtapaProceso { get; set; }
        public string Actividad { get; set; }
        public string Observacion { get; set; }
        public string ObservacionDocumento { get; set; }
        public string NombreFirma { get; set; }
        public string ApellidoFirma { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? NombreCompleto { get; set; }
        public bool RepresentanteLegal { get; set; }
        public string DisfrutePension { get; set; }
        public string HechoDescripcion { get; set; }
        public string? Categoria { get; set; }
        public string? UnidadEjecutora { get; set; }
        public string? NombreCompletoAfiliado { get; set; }
        public int NoConvocatoria { get; set; }
        public string UserName { get; set; }
        public string NoRadicado { get; set; }
        public string NumeroResolucion { get; set; }
        public DateTime? FechaApertura { get; set; }
        public string? FechaCierre { get; set; }
        public string ModalidadSolucionVivienda { get; set; }
        public string AfectacionCuenta { get; set; }
        public double ValorAfectacion { get; set; }
        public Guid UserId { get; set; }
        public DateTime? FechaRetiroFuerza { get; set; } = null;
        public DateTime? FechaRetiro{ get; set; } = null;

        public DateTime? FechaAsignacionInvalidez { get; set; } = null;
        public Boolean MismoAfiliado { get; set; }
        public DateTime? FechaUltimoAporte { get; set; } = null;
    }
}
