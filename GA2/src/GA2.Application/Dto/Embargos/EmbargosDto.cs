using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class EmbargosDto
    {
        public Guid Id { get; set; }
        public Guid ConceptoId { get; set; }
        public string RadicadoWorkManager { get; set; }
        public DateTime FechaRadicadoWorkManager { get; set; }
        public string RadicadoJuzgado { get; set; }
        public DateTime FechaRadicadoJuzgado { get; set; }
        public string EmailRespuesta { get; set; }
       // public string DireccionRespuesta { get; set; }
       // public string NombreDemandante { get; set; }
        public string NombreDemandado { get; set; }
        public string ExpedienteBancoAgrario { get; set; }
        public bool Remanente { get; set; }
        public bool Inconsistencia { get; set; }
        public string aclaracion { get; set; }
        public string Observaciones { get; set; }
        public string Respuesta { get; set; }
        public double Valor { get; set; }
        public int ClienteId { get; set; }
        public Guid JuzgadoId { get; set; }
        public Guid ProcesoId { get; set; }
        public Guid TipoEmbargoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public Guid CreadoPor { get; set; }
        public Guid ActualizadoPor { get; set; }
        public int NumeroSolicitud { get; set; }
        public string Juzgado { get; set; }
        public string TipoEmbargo { get; set; }
        public string TipoProceso { get; set; }
        public string PrimerNombreCliente { get; set; }
        public string SegundoNombreCliente { get; set; }
        public string PrimerApellidoCliente { get; set; }
        public string SegundoApellidoCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string NombreCompletoCliente { get; set; }
        public int TipoIdentificacionId { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Direccion { get; set; }
        public string EstadoEmbargo { get; set; }
        public string TipoActor { get; set; }
        public string CREADOPORNOMBRE { get; set; }
        public string CREADOPORAPELLIDO { get; set; }
        public string ESTADO { get; set; }

    }

    public class RadicadoEmbargoDto
    {
        public string RadicadoWorkManager { get; set; }
        public DateTime FechaRadicadoWorkManager { get; set; }
        public string RadicadoJuzgado { get; set; }
        public DateTime FechaRadicadoJuzgado { get; set; }
        public string EmailRespuesta { get; set; }
        public string NombreDemandado { get; set; }
        public int clienteTipoIdentificacion { get; set; }
        public int ClienteId { get; set; }
        public Guid JuzgadoId { get; set; }
        public string IdentificacionCliente { get; set; }
        public Guid CreadoPor { get; set; }
        public string TipoEmbargo { get; set; }
        public string NombreCompletoCliente { get; set; }
        public string MSG { get; set; }
    }
    public class InsetEmbargosDto
    {

        public Guid Id { get; set; }
        public string Direccion { get; set; }
        public string RadicadoWorkManager { get; set; }
        public DateTime FechaRadicadoWorkManager { get; set; }
        public string RadicadoJuzgado { get; set; }
        public DateTime FechaRadicadoJuzgado { get; set; }
        public string EmailRespuesta { get; set; }
        public string DireccionRespuesta { get; set; }
        public string ExpedienteBancoAgrario { get; set; }
        public bool Remanente { get; set; }
        public bool? Inconsistencia { get; set; }
        public string Observaciones { get; set; }
        public string? aclaracion { get; set; }
        public string Respuesta { get; set; }
        public int ClienteTipoIdentificacion { get; set; }
        public string ClienteIdentificacion { get; set; }
        public Guid JuzgadoId { get; set; }
        public Guid ProcesoId { get; set; }
        public string proceso { get; set; }

        public List<BeneficiariosPagoEmbargoDto> ?BeneficiariosPagos { get; set; }

        public List<InsetEmbargoCuentaConceptoDto> ?ConceptosEmbargos { get; set; }
  
  
    }
    public class ActualizarEstadoEmbargoDto
    {
        public Guid Id { get; set; }
        public bool Inconsistencia { get; set; }
        public string aclaracion { get; set; }
    }

    public class EmbargoCuentaConceptoDto {

        public Guid Id { get; set; }
        public double Valor { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool IndicadorCesantias { get; set; }
        public bool Remanente { get; set; }
        public Guid EmbardoId { get; set; }
        public Guid TipoRetencionId { get; set; }
        public Guid TipoEmbargoId { get; set; }
        public string TipoEmbargo { get; set; }
        public int CuentaId { get; set; }
        public int ConceptoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public Guid CreadoPorId { get; set; }
        public Guid ActualizadoPorId { get; set; }
        public bool Estado { get; set; }
        public string RadicadoWorkManager { get; set; }
        public int NumeroCuenta { get; set; }
        public string TipoRetencion { get; set; }
        public Guid IdTipoProceso { get; set; }
        public string TipoProceso { get; set; }
        public string Descripcion { get; set; }

    }

    public class EmbargoCuentaConceptosDto
    {
        public Guid[] Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class InsetEmbargoCuentaConceptoDto
    {

        public Guid Id { get; set; }
        public double Valor { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool IndicadorCesantias { get; set; }
        public Guid TipoRetencionId { get; set; }
        public Guid TipoEmbargoId { get; set; }
        public int CuentaId { get; set; }
        public int ConceptoId { get; set; }
        public bool Estado { get; set; }
        public int NumeroCuenta { get; set; }
        public bool insert { get; set; }

    }



    public class InsertEmbargoGestorDocumentalDto
    {
        public string RadicadoWorkManager { get; set; }
        public DateTime FechaRadicadoWorkManager { get; set; }
        public string RadicadoJuzgado { get; set; }
        public DateTime FechaRadicadoJuzgado { get; set; }
        public int ClienteTipoIdentificacion { get; set; }
        public string ClienteIdentificacion { get; set; }
        public string CodigoInterno { get; set; }
        public string Modulo { get; set; }
    }

    public class ConsultaEmbargosfiltroDto
    {
        public DateTime? FechaInicial { get; set; }

        public DateTime? FechaFinal { get; set; }

        public int? ClienteTipoIdentificacion { get; set; }
        public int? clienteTipoIndentificacion { get; set; }
        public string? ClienteIdentificacion { get; set; }

        public string? RadicadoWorkManager { get; set; }
        public int? Estado  { get; set; }
        public int? Estados  { get; set; }

    }

    public class PagoEmbargoDto
    {
        public Guid IdEmargo { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteTipoIdentificacion { get; set; }
        public string Observacion { get; set; }
        public string NumeroCuenta { get; set; }
        public string ClienteIdentificacion { get; set; }
        public string TipoEmbargo { get; set; }
        public string NombreRazonSocial { get; set; }
        public decimal Valor { get; set; }
        public bool Aprobado { get; set; }
        public Guid Entidad { get; set; }
        public Guid CreadoPor { get; set; }
        public int TipoRetencionId { get; set; }

        public Guid? TPE_ID { get; set; }
        public string? EBA_RADICADO_WORK_MANAGER { get; set; }
        public Guid? EMB_ID_JUZGADO { get; set; }
        public string? EBA_CLI_IDENTIFICACION { get; set; }
        public string? EBA_NOMBRE_DEMANDADO { get; set; }
        public string? TPE_NOMBRE { get; set; }
        public string? CODIGO_INTERNO { get; set; }
        public string? NOMBRE { get; set; }
        public string? EBA_CREADOPOR_NOMBRE { get; set; }
        public string? EBA_APELLIDO_CREADOPOR { get; set; }
        public string? TIPO_RETENCION { get; set; }
        public string? EBA_EXPEDIENTE_BANCO_AGRARIO { get; set; }
    }


}
