using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class ObtenerTramiteSolicitudDto
    {
        public ObtenerTramiteSolicitudDto() {
            solicitudEncontrada = false;
        }
        public Guid SOL_ASIGNADO { get; set; }
        public Guid solicitudId { get; set; }
        public int SOL_NUMERO_SOLICITUD { get; set; }
        public Guid solicitudTareaId { get; set; }
        public Guid ultimoSolicitudTareaId { get; set; }
        public string? EstadoCredito { get; set; }
        public string? SOL_GRUPO_ASIGNADO { get; set; }

        public bool? Posee_Avac { get; set; }
        public int cuentaId { get; set; }
        public Guid tipoModalidadId { get; set; }
        public Guid tipoSubModalidadId { get; set; }
        public int solicitudEstadoId { get; set; }
        public int clienteId { get; set; }
        public int tipoIdentificacion { get; set; }
        public string direccionCliente { get; set; }
        public string NumeroFactura { get; set; }
        public string SOL_NOMBREASIGNADO { get; set; }
        public string ConsecutivoFactura { get; set; }
        public string telefonoCliente { get; set; }
        public string celularCliente { get; set; }
        public string correoCliente { get; set; }
        public string observacionSolicitud { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public bool estadoSolicitud { get; set; }
        public bool tieneFlujo102 { get; set; }
        public bool tieneFlujo37 { get; set; }
        public int solicitudTareaEstadoId { get; set; }
        public string volverPantalla { get; set; }
        public string descripcionSubModalidad { get; set; }
        public string descripcionModalidad { get; set; }
        public string descripcionProceso { get; set; }
        public decimal valorRetirar { get; set; }
        public List<SaldoDto> SaldosE { get; set; }
        public decimal valorTotal { get; set; } = 0;
        public decimal valorCesantia { get; set; } = 0;
        public decimal valorAhorro { get; set; } = 0;
        public string pestanas { get; set; }
        public string secciones { get; set; }
        public string TimCodigo { get; set; }
        public int Numero_Folio { get; set; }
        
        public DateTime? FechaFirmaEscritura { get; set; }

        public string SOL_RECHAZADA { get; set; }
        public string? Observacion { get; set; }

        #region Campos adicionales
        public bool solicitudEncontrada { get; set; }
        public string nombresCliente { get; set; }
        public string apellidosCliente { get; set; }
        public string identificacionCliente { get; set; }
        public int NumeroSolicitud { get; set; }
        public Guid  IdProcesoSolicitud { get; set; }
        public Guid CreadoPor { get; set; }
        public string NumeroRadicadoSG { get; set; }
        public string NombreTarea { get; set; }
        public string CodProcesoGD { get; set; }
        public string CodSubModGD { get; set; }
        public string? EMBARGO { get; set; } = "NO";
        public int TipoCreditoId { get; set; }
        public string Cdp { get; set; }
        public Guid solicitudCreditoId { get; set; }
        public int estadoCuenta { get; set; }
        public InformacionTurnoDto InformacionTurnoDto { get; set; }
        public List<MatriculaRespuestaDto> solicitudMatriculaDto { get; set; }
        public List<PropietarioRespuestaDto> solicitudPropietarioDto { get; set; }
        public List<TerceroVendedorRespuestaDto> solicitudTerceroVendedorDto { get; set; }
        public List<TerceroBeneficiarioRespuestaDto> solicitudTerceroBeneficiarioDto { get; set; }
        public List<TerceroApoderadoRespuestaDto> solicitudTerceroApoderadoDto { get; set; }
        public List<TerceroEntidadEducativaRespuestaDto> solicitudTerceroEntidadEducativaDto { get; set; }
        public List<SolicitudTerceroBeneficiarioEstudioDto> solicitudTerceroBeneficiarioEstudioDto { get; set; }
        public List<SolicitudBeneficiarioDto> SolicitudBeneficiarioDto { get; set; }

        //public List<SolicitudTerceroBeneficiarioEstudioEntidadEducativaDto> solicitudTerceroBeneficiarioEstudioEntidadEducativaDto { get; set; }
        public SolicitudTerceroEntidadSeguroEducativoDto solicitudTerceroEntidadSeguroEducativoDto { get; set; }
        public SolicitudAdjudicacionDto SolicitudAdjudicacion { get; set; }
        public SolicitudEscrituracionDto SolicitudEscrituracion { get; set; }
        public SolicitudTerceroTenedorAccionesDto solicitudTerceroTenedorAccionesDto { get; set; }
        
        public InformacionConyugeRespuesta InformacionConyugeDto { get; set; }
        #endregion

        #region Conyuge
        public int IdConyuge { get; set; }
        public int IdTipoIdentificacionConyuge { get; set; }
        public string IdentificacionConyuge { get; set; }
        public string PrimerNombreConyuge { get; set; }
        public string SegundoNombreConyuge { get; set; }
        public string PrimerApellidoConyuge { get; set; }
        public string SegundoApellidoConyuge { get; set; }
        public bool ConyugeActivo { get; set; }
        #endregion

        #region acreditacion
        public string? CumpleRequisito { get; set; }
        public string? Causal { get; set; }
        public string? AccionRealizar { get; set; }
        public string? Acuerdo { get; set; }
        #endregion

        public MisCuentasDto? SolicitudMiscuentasDto { get; set; }
    }
}
