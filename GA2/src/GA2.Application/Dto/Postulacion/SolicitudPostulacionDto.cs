using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class SolicitudPostulacionDto
    {
        public Guid IdSolicitud { get; set; }
        public long NumeroDocumeto { get; set; }
        public int TipoDocumentoId { get; set; }
        public int IdTipoSolicitud { get; set; }
        public int IdModalidad { get; set; }
        public int IdSubModalidad { get; set; }
        public int? ManifestacionAutorizacion { get; set; }
        public int? Departamento1 { get; set; }
        public int? Ciuadad1 { get; set; }
        public int? Departameno2 { get; set; }
        public int? Ciudad2 { get; set; }
        public int? Deparmento3 { get; set; }
        public int? Ciudad3 { get; set; }
        public int? FirmaSolicitud { get; set; }
        public long? NumeroDocumentoFirma { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public long? NumeroCelular { get; set; }
        public int NumeroConvocatoria { get; set; }
        public int? EstadoPostulacion { get; set; }
        public int? ActividadPostulacion { get; set; }
        public string Observacion { get; set; }
        public string NombreFirma { get; set; }
        public string ApeliidoFirma { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool? RepresentanteLegal { get; set; }
        public string SpoDistruePension { get; set; }
        public string SpoUnidadEjecutora { get; set; }
        public string SpoCategoria { get; set; }
        public string NombreCompletoAfiliado { get; set; }
        public Guid UserId { get; set; }
        public string ModalidadSolucionVivienda { get; set; }
        public string AfectacionCuenta { get; set; }
        public double? ValorAfectacion { get; set; }
        public string Fuerza { get; set; }
        public int IdConvocatoria { get; set; }
        public DateTime? FechaRetiroFuerza { get; set; } = null;

        public string? FechaAsignacionInvalidez { get; set; } = null;

        public string? FechaRetiro { get; set; } = null;

        public string?  NoRadicado{ get; set; } = null;

        /*
         * SE CREAR CAMPO MISMO AFILIADO
         */
        public Boolean MismoAfiliado { get; set; }

    }
}
