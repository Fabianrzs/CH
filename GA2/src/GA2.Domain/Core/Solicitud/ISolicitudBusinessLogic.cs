using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities.Solicitud;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;

namespace GA2.Domain.Core
{

    /// <summary>
    /// Interfaz
    /// <author>Erwin Pantoja España</author>
    /// <date>20/10/2021</date>
    /// </summary>
    ///
    public interface ISolicitudBusinessLogic
    {
        Task<Response<ObtenerTramiteSolicitudDto>> CrearSolicitud(CrearSolicitudDto crearSolicitudDto);
        Task<Response<ObtenerTramiteSolicitudDto>> ConsultarExisteSolicitud(ConsultarSolicitudDto consultareSolicitudDto);
        Task<Response<TerceroApoderadoRespuestaDto>> ConsultarExistePersona(string numeroDocumento);
        Task<Response<TerceroApoderadoRespuestaDto>> ConsultarExistePropProv(string numeroIdentificacion);
        Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarExisteMatricula(string numeroMatricula);
        Task<Response<bool>> CrearSolicitudTarea(CrearSolicitudTareaDto crearSolicitudCompraViviendaDto);
        Task<Response<bool>> ActualizarSolicitudTarea(CrearSolicitudTareaDto crearSolicitudCompraViviendaDto);
        Task<Response<bool>> SubsanarSolicitudTarea(ActualizarSolicitudTareaDto actualizarSolicitudCompraViviendaDto);
        Task<Response<ObtenerTramiteSolicitudDto>> AprobarSolicitudTarea(AprobarSolicitudTareaDto aprobarSolicitudTareaDto);
        Task<Response<ObtenerTramiteSolicitudDto>> AprobarSolicitudTareaDocumento(AprobarSolicitudTareaDto aprobarSolicitudTareaDto);
        Task<Response<bool>> RechazarSolicitudTarea(RechazarSolicitudTareaDto rechazarSolicitudTareaDto);
        Task<Response<bool>> AnularSolicitudTarea(RechazarSolicitudTareaDto rechazarSolicitudTareaDto);
        Task<Response<IEnumerable<DocumentosPorSubModalidadDto>>> ConsultarDocumentosPorSubModalidad(ConsultaDocumentoSubModalidadTarea consultaDocumentoSubModalidadTarea);
        Task<Response<bool>> CargarDocumentosSolicitud(List<CargarDocumentosSolicitudDto> CargarDocumentosSolicitudDto, Guid IdSolicitud);
        Task<Response<IEnumerable<ConsultarArchivoPorSolicitudDto>>> ConsultarArchivoPorSolicitud(Guid IdSolicitud);
        Task<Response<bool>> EliminarArchivoPorSolicitud(EliminarArchivoPorSolicitudDto eliminarArchivoPorSolicitudDto);
        Task<FileResult> DescargarArchivoPorSolicitud(string rutaStorage);
        Task<Response<InconsistenciaSolicitudDto>> InsertarInconsistenciaSolicitud(InconsistenciaSolicitudDto insertarInconsistenciaSolicitud);
        Task<Response<InconsistenciaSolicitudDto>> ActualizarInconsistenciaSolicitud(InconsistenciaSolicitudDto actualizarInconsistenciaSolicitud);
        Task<Response<IEnumerable<InconsistenciaSolicitudDto>>> ConsultarInconsistenciaSolicitud(Guid IdSolicitud);
        Task<Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>>> ConsultarSolicitudCesantias(PeticionConsultarSolicitudCesantiasDto solicitud);
        Task<Response<InconsistenciaSolicitudDto>> EliminarInconsistenciaSolicitud(Guid inconsistenciaId);
        Task<Response<FormulariosSolicitudDto>> ObtenerFomulariosSolicitud(string timCodigo);
        Task<Response<IEnumerable<ComunicacionSolicitudDto>>> ObtenerComunicacionesSolicitud(Guid IdSolicitud);
        Task<Response<ComunicacionSolicitudDto>> InsertarComunicacionSolicitud(ComunicacionSolicitudDto request);
        Task<Response<IEnumerable<ComunicacionSolicitudDto>>> ActualizarComunicacionesSolicitud(IEnumerable<ComunicacionSolicitudDto> request);
        Task<Response<bool>> CargarDocumentosSolicitudCredito(List<CargarDocumentosSolicitudDto> CargarDocumentosSolicitudDto, Guid IdSolicitud);
        Task<Response<ObservacionSolicitudDto>> ActualizarObservacionesSolicitud(ObservacionSolicitudDto observaciones);
        Task<Response<IEnumerable<TrazabilidadSolicitudDto>>> ObtenerTrazabilidadSolicitud(Guid IdSolicitud);
        Task<Response<IEnumerable<TareasSolicitudDto>>> ConsultarAsignacionTareas(Guid IdSolicitud);
        Task<Response<IEnumerable<InconsistenciasDto>>> SP_CONSULTAR_INCONSISTENCIAS_SOLICITUD(Guid IdSolicitud);
        Task<Response<TareasSolicitudDto>> AsignacionDeTareas(TareasSolicitudDto IdSolicitud);
        Task<Response<bool>> ReglasDeNegocioVivienda8(Guid IdSolicitud);
        Task<Response<List<EstadosClienteDto>>> ObtenerEstadosCliente();
        Task<Response<List<EstadoCausalClienteDto>>> ObtenerCausalEstadoCliente();
        Task<Response<InformacionTurnoResponse>> ObtenerInformacionTurno();
        Task<Response<FileResponseDto>> ObtenerDocumentosPorRadicado(string numeroRadicado);
        Task<Response<string>> ObtenerEnlaceDodo(string numeroRadicado);
        Task<Response<bool>> CheckDocumentosSolicitud(CheckDocumentosDto checkDocumentos);
        Task<Response<IEnumerable<CheckArchivo>>> ObtenerArchivosCheck(Guid IdSolicitud);
        Task<Response<ResponseDocumentoSolicitudes>> CrearDocumentoSolicitud(RequestDocumentoSolicitudDto requestDocumentoSolicitud);
        Task<Response<string>> CargaDocumentoSolicitud(IFormFile request);
        Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarSolicitud(FiltroSolicitudes filtro);
        Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarSolicitudAtendidas(FiltroSolicitudes filtro);
        Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarTramiteSolicitud(FiltroSolicitudes filtro);
        Task<Response<MatriculaInmobiliariaRespuestaDto>> ConsultarTReporteDiarioSolicitudes(FiltroSolicitudes filtro);
        Task<Response<List<ConsultarNovedad>>> ConsultarNovedadSolicitud();
        Task<Response<IEnumerable<ConsultarTipoNovedad>>> ConsultarTipoNovedadSolicitud(string nov_descripcion);
        Task<Response<IEnumerable<ConsultarCausalInterrupcionSolicitud>>> ConsultarCausalSolicitud(string nov_descripcion);
        Task<Response<IEnumerable<ConsultarPagoSucesivoSolicitud>>> ConsultarPagoSucesivoSolicitud(string nov_descripcion);
        Task<Response<IEnumerable<ConsultarInformeInconsistenciaSolicitud>>> ConsultarInformeInconsistenciaSolicitud(string nov_descripcion);
        Task<Response<InconsistenciaSolicitudDto>> InsertarInconsistenciaSeguridadSolicitud(InconsistenciaSolicitudDto insertarInconsistenciaSolicitud);
        Task<Response<IEnumerable<InconsistenciaSolicitudDto>>> ConsultarInconsistenciaSeguridadSolicitud(Guid IdSolicitud);
        Task<Response<InconsistenciaSeguridadDto>> ActualizarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadDto actualizarInconsistenciaSolicitud);
        Task<Response<InconsistenciaSeguridadDto>> EliminarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud inconsistencia);
        Task<Response<ObtenerFlujosDodo>> ObtenerFlujosDodo(ObtenerFlujosDodoDto obtenerFlujosDodo);
        Task<Response<IEnumerable<CuentasAfectadasSolicitud>>> ObtenerConceptosAfectado(ObtenerConceptosAfectadosClientes cuentasAfectadas);
        Task<Response<List<CuentasAfectadasSolicitud>>> ObtenerConceptosAfectadosSolicitud(string cuentasAfectadas);
        Task<Response<ReporteAfiliadosCesantiasResponse>> ReporteDeCesantias(ReporteAfiliadosCesantiasRequestDto request);
        Task<Response<IEnumerable<CuentasAfectadasSolicitud>>> ObtenerCuentasAfectadas(ObtenerConceptosAfectadosClientes cuentasAfectadas);
        Task<Response<InformacionConyuge>> InformacionConyuge(InformacionConyugeDto informacionConyuge);
        Task<Response<IEnumerable<ConsultarSubmodalidadPrimerPago>>> ConsultarSubmodalidadesDePrimerPago();
        Task<Response<RespuestaConsultarRadicadoSolicitud>> ConsultarRadicadoSolicitud(ConsultarRadicadoSolicitudDto solicitudDto);
        Task<Response<ConsultarRadicadoSolicitudDto>> InsertarDescripcionInconsistenciaSolicitud(ConsultarRadicadoSolicitudDto insertarInconsistenciaSolicitud);
        Task<Response<IEnumerable<UsuarioIngresoSolicitud>>> ConsultarUsuarioSolicitud(RespuestaConsultarUsuarioIngresoSolicitud usuarioIngreso);
        Task<Response<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>>> ConsultarSolicitanteBeneficiarioTerceroSolicitud(SolicitanteBeneficiarioTerceroSolicitudDto beneficiante);
        Task<Response<IEnumerable<IntervinientesActor>>> IntervinientesActor();
        Task<Response<IntervinientesActorSolicitud>> RegistrarIntervinientes(IntervinientesActorSolicitudDto intervinientesActor);
        Task<Response<IntervinientesActorSolicitud>> ConsultarIntervinientesPorId(Guid idIntervinientes);
        Task<Response<List<Tramite>>> TramiteIntervimiente(ConsultaTramitesIntervinientesActorSolicitud interviniente);
        Task<Response<List<IntervinientesActorSolicitud>>>ConsultarIntervinienteActorTipoYNumeroDocumentoAfiliado(int tipoDocumento,string numeroDocumento); 
        Task<Response<IEnumerable<IntervinientesActorSolicitud>>> FiltrarIntervinientes(BuscarIntervinientesDto intervinientes);
        Task<Response<IntervinientesActorSolicitud>> ActualizarIntervinientes(ActualizarIntervinientesActorSolicitudDto intervinientesActor);
        Task<Response<IntervinientesActorSolicitud>> EliminarIntervinienteActor(EliminarIntervinientesActorSolicitudDto intervinientesActor);
        Task<Response<string>> CrearRadicadoCartaIncosistencia(CrearRadicadoCartaIncosistencias request);
        Task<Response<DataReportSolicitud>> DataReporteSolictud(RequestReporteSolicitud request);
    }
}
