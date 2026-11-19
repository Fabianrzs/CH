using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class ConsultarSolicitudDto
    {
        public Guid tipoSubModalidadId { get; set; }
        public string identificacion { get; set; }
        public int tipoIdentificacion { get; set; }
        public string? tipoCuenta { get; set; } = String.Empty;
        public Guid solicitudTareaId { get; set; }
        public string ?cdp {get;set;}
        public string nroPlanilla { get; set; }
        public string tareaNombre { get; set; }
        public string tipoVinculacion { get; set; }
        public string comprobante { get; set; }
        public string? solicitante { get; set; }
    }

    public class CartaInconsistenciaPdfDto
    {
        // Encabezado
        public string CiudadFecha { get; set; }

        // Afiliado
        public string NombreAfiliado { get; set; }
        public string CorreoAfiliado { get; set; }
        public string DireccionAfiliado { get; set; }
        public string CiudadAfiliado { get; set; }

        // Caja superior derecha
        public string Radicado { get; set; }
        public string Radicado378 { get; set; }
        public string FechaGeneracion { get; set; }

        // Cuerpo
        public string FechaTramite { get; set; }
        public string TextoInconsistencia { get; set; }
        public string NombreSolicitud { get; set; }

        // Pie
        public string NombreAsesor { get; set; }
        public string CargoAsesor { get; set; }
    }

    public class RespuestaSolicitudDePagoPdfDto
    {
        // Encabezado
        public string Radicado { get; set; }
        public string PuntoRadicacion { get; set; }
        public string FechaRadicacion { get; set; }
        public string Folios { get; set; }

        // Información del afiliado
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public string Fuerza { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string CiudadMunicipio { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public string EmailPersonal { get; set; }
        public string EmailInstitucional { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string EstadoCivil { get; set; }
        public string TipoDocumentoConyuge { get; set; }
        public string NumeroIdentificacionConyuge { get; set; }
        public string NombresConyuge { get; set; }
        public string ApellidosConyuge { get; set; }

        // Información de trámites
        public string TipoTramite { get; set; }
        public string Modalidad { get; set; }
        public string ValorSolicitadoPrimerPago { get; set; }
        public string IdentificacionEstudiante { get; set; }
        public string NombreApellidosEstudiante { get; set; }
        public string ProgramaAcademico { get; set; }
        public string FechaPago { get; set; }
        public string NumeroMatriculaInmobiliaria { get; set; }
        public string NumeroEscrituraInmueble { get; set; }
        public string DireccionInmueble { get; set; }
        public string DepartamentoInmueble { get; set; }
        public string CiudadMunicipioInmueble { get; set; }
        public string Notaria { get; set; }
        public string FechaEscrituracion { get; set; }
        public string IdentificacionVendedor { get; set; }
        public string NombreApellidosVendedor { get; set; }
        public string TipoVivienda { get; set; }
        public string ValorSegundoPago { get; set; }

        // Beneficiarios
        public string Beneficiarios { get; set; }

        // Destinatarios de pago
        public string DestinatarioNombre { get; set; }
        public string DestinatarioTipoDocumento { get; set; }
        public string DestinatarioNumeroDocumento { get; set; }
        public string DestinatarioBanco { get; set; }
        public string DestinatarioCheque { get; set; }
        public string DestinatarioTipoCuenta { get; set; }
        public string DestinatarioNumeroCuenta { get; set; }

        // Declaraciones
        public string NotificacionEmail { get; set; }
        public string NotificacionDireccionCorrespondencia { get; set; }
        public string Observaciones { get; set; }
        public string FechaImpresion { get; set; }
    }
}
