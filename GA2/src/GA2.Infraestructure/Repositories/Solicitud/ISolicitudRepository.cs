using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Solicitud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// Interfaz
    /// <author>Erwin Pantoja España</author>
    /// <date>20/10/2021</date>
    /// </summary>
    ///
    public interface ISolicitudRepository
    {
        Task<ObtenerTramiteSolicitud> CrearSolicitud(CrearSolicitud crearSolicitudDto);
        Task<ObtenerTramiteSolicitud> ConsultarExisteSolicitud(ConsultarSolicitud consultareSolicitudDto);
        Task<ObtenerTramiteSolicitud> ConsultarSolicitudPorActividad(ConsultarSolicitudPorActividad consultareSolicitudDto);
        Task<SolicitudMatricula> InsertarSolicitudMatricula(SolicitudMatricula solicitudMatricula);
        Task<SolicitudPropietario> InsertarSolicitudPropietario(SolicitudPropietario solicitudPropietario);
        Task<SolicitudTerceroApoderado> InsertarSolicitudTerceroApoderado(SolicitudTerceroApoderado solicitudTerceroApoderado);
        Task<SolicitudBeneficiario> SolicitudBeneficiario(SolicitudBeneficiario SolicitudBeneficiario);
        Task<SolicitudTerceroBeneficiario> InsertarSolicitudTerceroBeneficiario(SolicitudTerceroBeneficiario solicitudTerceroBeneficiario);
        Task<SolicitudTerceroConstructor> InsertarSolicitudTerceroConstructor(SolicitudTerceroConstructor solicitudTerceroConstructor);
        Task<SolicitudTerceroVendedor> InsertarSolicitudTerceroVendedor(SolicitudTerceroVendedor solicitudTerceroVendedor);
        Task<InsertarArchivo> InsertarArchivoPorSolicitudTarea(InsertarArchivo insertarArchivo);
        Task<InformacionAdjudicacion> InsertarSolicitudInformacionAdjudicacion(InformacionAdjudicacion InformacionAdjudicacion, Guid SOL_ID_FK, Guid CreadoPor);
        Task<InformacionEscrituracion> InsertarSolicitudInformacionEscrituracion(InformacionEscrituracion InformacionEscrituracion, Guid SOL_ID_FK, Guid CreadoPor);

        Task<bool> ActualizarSolicitudTarea(ActualizarSolicitudTarea actualizarSolicitudTarea);
        Task<IEnumerable<RespuestaMatricula>> ConsultarSolicitudMatricula(Guid solicitudId);
        Task<bool> InsertarMatriculaSolicitud(Guid solicitudId, Guid matriculaId);
        Task<IEnumerable<RespuestaPropietario>> ConsultarSolicitudPropietario(Guid solicitudId);
        Task<SolicitudAdjudicacion> ConsultarSolicitudAdjudicacion(Guid solicitudId);
        Task<SolicitudEscrituracion> ConsultarSolicitudEscrituracion(Guid solicitudId);
        Task<IEnumerable<RespuestaTerceroVendedor>> ConsultarTerceroVendedor(Guid solicitudId, int tipoTercero);
        Task<IEnumerable<RespuestaTerceroBeneficiario>> ConsultarTerceroBeneficiario(Guid solicitudId, int tipoTercero);
        Task<IEnumerable<RespuestaTerceroApoderado>> ConsultarTerceroApoderado(Guid solicitudId, int tipoTercero);
        Task<IEnumerable<SolicitudBeneficiario>> ConsultarSolicitudBeneficiario(Guid solicitudId, int tipoTercero);
        Task<IEnumerable<RespuestaTerceroBeneficiarioEstudio>> ConsultarTerceroBeneficiarioEstudio(Guid solicitudId, int tipoTercero);
        Task<IEnumerable<RespuestaTerceroEntidadEducativa>> ConsultarTerceroEntidadEducativa(Guid solicitudId, int tipoTercero);
        Task<RespuestaTerceroApoderado> ConsultarTerceroRazonSocial(string numeroDocumento);
        Task<SolicitudPropietario> ConsultarPropietarioIntegridad(string numeroIdentificacion);
        Task<RespuestaTerceroEntidadSeguroEducativo> ConsultarTerceroEntidadSeguroEducativo(Guid solicitudId, int tipoTercero);
        Task<RespuestaTerceroTenedorAcciones> ConsultarTerceroTenedorAcciones(Guid solicitudId, int tipoTercero);
        Task<SolicitudTerceroAutorizado> InsertarSolicitudTerceroAutorizado(SolicitudTerceroAutorizado solicitudTerceroAutorizado);
        Task<SolicitudPropietarioMatricula> InsertarSolicitudPropietarioMatricula(SolicitudPropietarioMatricula solicitudPropietarioMatricula);
        Task<SolicitudTerceroEntidadEducativa> InsertarSolicitudTerceroEntidadEducativa(SolicitudTerceroEntidadEducativa solicitudTerceroEntidadEducativa);
        Task<SolicitudTerceroEntidadEducativa> ActualizarSolicitudTerceroEntidadEducativa(SolicitudTerceroEntidadEducativa solicitudTerceroEntidadEducativa);
        Task<SolicitudTerceroBeneficiarioEstudio> InsertarSolicitudTerceroBeneficiarioEstudio(SolicitudTerceroBeneficiarioEstudio solicitudTerceroBeneficiarioEstudio);
        // Task<SolicitudTerceroBeneficiarioEstudioEntidadEducativa> InsertarSolicitudTerceroBeneficiarioEstudioEntidadEducativa(SolicitudTerceroBeneficiarioEstudioEntidadEducativa solicitudTerceroBeneficiarioEstudioEntidadEducativa);
        Task<SolicitudTerceroEntidadSeguroEducativo> InsertarSolicitudTerceroEntidadSeguroEducativo(SolicitudTerceroEntidadSeguroEducativo solicitudTerceroEntidadSeguroEducativo);
        Task<SolicitudTerceroTenedorAcciones> InsertarSolicitudTerceroTenedorAcciones(SolicitudTerceroTenedorAcciones solicitudTerceroTenedorAcciones);
        Task<bool> EliminarTerceroSolicitud(Guid solicitudId);
        Task<bool> EliminarMatriculaPropietarioSolicitud(Guid solicitudId);
        Task<bool> EliminarPropietarioSolicitud(Guid solicitudId);
        Task<bool> EliminarMatriculaInmobiliaria(Guid solicitudId);
        Task<ObtenerTramiteSolicitud> AprobarSolicitudTarea(AprobarSolicitudTarea aprobarSolicitudTarea);
        Task<bool> RechazarSolicitudTarea(RechazarSolicitudTarea rechazarSolicitudTarea);
        Task<bool> AnularSolicitudTarea(AnularSolicitudTarea anularSolicitudTarea);
        Task<IEnumerable<RespuestaDocumentosPorSubModalidad>> ConsultarDocumentosPorSubModalidad(Guid idSubModalidad, Guid idSolicitudtarea);
        Task<IEnumerable<ConsultarArchivoPorSolicitud>> ConsultarArchivoPorSolicitud(Guid solicitudId);
        Task<bool> EliminarArchivoPorSolicitud(EliminarArchivoPorSolicitud eliminarArchivoPorSolicitud);
        Task<bool> ActualizarSolicitud(ActualizarSolicitud actualizarSolicitud);
        Task<InconsistenciaSolicitud> InsertarInconsistenciaSolicitud(InconsistenciaSolicitud insertarInconsistenciaSolicitud);
        Task<InconsistenciaSolicitud> ActualizarInconsistenciaSolicitud(InconsistenciaSolicitud actualizarInconsistenciaSolicitud);
        Task<IEnumerable<InconsistenciaSolicitud>> ConsultarInconsistenciaSolicitud(Guid solicitudId);
        Task<IEnumerable<RespuestaConsultarSolicitudCesantias>> ConsultarSolicitudCesantias(PeticionConsultarSolicitudCesantias solicitud, int PageSize, int PageNumber);
        Task<SolicitudMatriculaInmobiliaria> ConsultarMatriculaInmobiliaria(string numeroMatricula);
        Task<bool> EliminarActualizarDatoSolicitud(CrearSolicitudTareaDto solicitud);
        Task<bool> ActualizaNumeroRadicadoSolicitud(ObtenerTramiteSolicitudDto solicitudTareaDto);
        Task<bool> ActualizaMPRCONTSolicitud(ParametrosObtenerCesantiasDto solicitud, int mpr_cont);
        Task<bool> ActualizaFACTSolicitud(Guid solicitud, string factCont, string factNume);
        Task<bool> ActualizarCodProvSevenTerceroBeneficiario(ParametrosObtenerCesantiasDto solicitud, int TipoTerceroBeneficiarioId, int codProveeSeven);
        Task<bool> ActualizarObservacionSolicitud(CrearSolicitudTareaDto solicitud);
        Task<bool> ActualizarSolicitudMatricula(SolicitudMatricula solicitud);
        Task<Guid> ConsultarSiguienteTareaSolicitud(AprobarSolicitudTarea aprobarSolicitudTarea);
        Task<ObtenerSolicitud> ConsultaSolicitud(Guid solId);
        Task<InconsistenciaSolicitud> EliminarInconsistenciaSolicitud(Guid inconsistenciaId);
        Task<IEnumerable<ObtenerFormularioSolicitud>> ObtenerFomulariosSolicitud(string timCodigo);
        Task<IEnumerable<ComunicacionSolicitud>> ObtenerComunicacionesSolicitud(Guid IdSolicitud);
        Task<ComunicacionSolicitud> InsertarComunicacionSolicitud(ComunicacionSolicitud request);
        Task<ComunicacionSolicitud> ActualizarComunicacionesSolicitud(ComunicacionSolicitud request);
        Task<ObservacionSolicitud> ActualizarObservacionesSolicitud(ObservacionSolicitud request);
        Task<IEnumerable<TrazabilidadSolicitud>> ObtenerTrazabilidadSolicitud(Guid IdSolicitud);
        Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionTareas(Guid IdSolicitud);
        Task<IEnumerable<InconsistenciasDto>> SP_CONSULTAR_INCONSISTENCIAS_SOLICITUD(Guid IdSolicitud);
        Task<TareaSolicitud> AsignacionDeTareas(TareaSolicitud tarea);
        Task<IEnumerable<EstadosCliente>> ObtenerEstadosCliente();
        Task<IEnumerable<EstadoCausalCliente>> ObtenerCausalEstadoCliente();
        Task<bool> GuardarConyugue(RespuestaConyugue conyugue);
        Task<RespuestaConyugue> ConsultarConyugue(int cliId);
        Task<Procesos> ObtenerProcesoPorSubmodalidad(Guid sudModalidadId);
        Task<Procesos> ObtenerProcesoPorSolicitudTarea(Guid solicitudTareaId);
        Task<IEnumerable<Sucursal>> ObtenerSucursales(string sucursales = null);
        Task<IEnumerable<Turno>> ObtenerTurnos(string turno = null);
        Task<IEnumerable<Recepcion>> ObtenerRecepcion(string recepcion = null);
        Task<InformacionTurno> ObterInformacionTurnoSolicitud(Guid IdSolicitud);
        Task<SolicitudInfo> ConsultarModadidaYSubmodalidadXSolicitud(Guid IdSolicitud);
        Task<InformacionTurno> CrearInformacionTurnoSolicitud(InformacionTurno informacion);
        Task<IEnumerable<Fuerzas>> ObtenerFuerzaPorId(int frzId);

        Task<CheckArchivo> CheckArchivosSolicitud(Guid idSolicitud, string nombreArchivo);
        Task<bool> EliminarCheckArchivos(Guid idSolicitud);
        Task<IEnumerable<CheckArchivo>> ObtenerArchivosCheck(Guid idSolicitud);
        Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondo(Guid IdSolicitud);
        Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondoArchivo(Guid IdSolicitud);
        Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondoEntrevista(Guid IdSolicitud);
        Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionVerificationFondo(Guid IdSolicitud);
        Task<IEnumerable<TareaSolicitud>> ConsultarAsignacionFondoCalificacion(Guid IdSolicitud);
        Task<ConsultarNumeroFolioSolicitud> ConsultarInformacionSolicitud(int numeroSolicitud);
        Task<ConsultarNumeroFolioSolicitud> ConsultarSolicitudTareaPorId(Guid SLT_ID);
        Task<IEnumerable<ConsultarNovedad>> ConsultarNovedadSolicitud();
        Task<IEnumerable<ConsultarTipoNovedad>> ConsultarTipoNovedadSolicitud(string nov_descripcion);
        Task<IEnumerable<ConsultarCausalInterrupcionSolicitud>> ConsultarCausalInterrupcionSolicitud(string nov_descripcion);
        Task<IEnumerable<ConsultarPagoSucesivoSolicitud>> ConsultarPagoSucesivoSolicitud(string nov_descripcion);
        Task<IEnumerable<ConsultarInformeInconsistenciaSolicitud>> ConsultarInformeInconsistenciaSolicitud(string nov_descripcion);
        Task<InconsistenciaSeguridadSolicitud> InsertarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud insertarInconsistenciaSolicitud);
        Task<IEnumerable<InconsistenciaSeguridadSolicitud>> ConsultarInconsistenciasSeguridadSolicitud(Guid solicitudId);
        Task<InconsistenciaSeguridadSolicitud> ActualizarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud actualizarInconsistenciaSolicitud);
        Task<InconsistenciaSeguridadSolicitud> EliminarInconsistenciaSeguridadSolicitud(InconsistenciaSeguridadSolicitud actualizarInconsistenciaSolicitud);
        Task<SolicitudMatriculaInmobiliaria> ConsultarMatriculaInmobiliariaPorNumeroMatricula(string numeroMatricula, int ofc_id);
        Task<SolicitudMatriculaInmobiliaria> ParamBloqueoMatricula(Guid matriculaId);
        Task<IEnumerable<ConceptosAfectadosSolicitud>> ConsultarTipoConcepto(string tipoCuenta);
        Task<CuentasAfectadasSolicitud> InsertarConceptoAfectadoCliente(CuentasAfectadasSolicitud cuentasAfectadasSolicitud);
        Task<IEnumerable<CuentasAfectadasSolicitud>> ObtenerConceptosAfectados(ObtenerConceptosAfectadosClientes cuentasAfectadas);
        Task<List<CuentasAfectadasSolicitud>> ObtenerConceptosAfectadosSolicitud(string cuentasAfectadas);
        Task<List<ReporteAfiliadosCesantiasItem>> GenerarReportePagoDeCesantias(ReporteAfiliadosCesantiasRequestDto request);
        Task<IEnumerable<CuentasAfectadasSolicitud>> ObtenerCuentasAfectadas(ObtenerConceptosAfectadosClientes cuentasAfectadas);
        Task<IEnumerable<TiposDeCuentaSolicitud>> ConsultarTipoCuentas();
        Task<IEnumerable<ObtenerInformacionSolicitudCliente>> ObtenerInformacionSolicitudCliente(string cedula);
        Task<SolicitudMatriculaInmobiliaria> DesbloqueoMatriculaSolicitud(Guid MatriculaId);
        Task<IEnumerable<ConsultarSubmodalidadPrimerPago>> ConsultarSubmodalidadPrimerPago();
        Task<RespuestaConsultarRadicadoSolicitud> ConsultarRadicadoSolicitud(Guid SolicitudId);
        Task<RespuestaConsultarRadicadoSolicitud> InsertarDescripcionInconsistenciaSolicitud(RespuestaConsultarRadicadoSolicitud consultarRadicadoSolicitudDto);
        Task<SubmodalidadSolicitud> ObtenerSubModalidadPrimerPago(Guid submodalidadId);
        Task<bool> ActualizarAsignacionSubsidioSolicitud(string numeroMatricula);
        Task<IEnumerable<UsuarioIngresoSolicitud>> ConsultarUsuarioIngresoSolicitud(RespuestaConsultarUsuarioIngresoSolicitud usuarioIngreso);
        Task<IEnumerable<SolicitanteBeneficiarioTerceroSolicitud>> ConsultarSolicitanteApoderadoSolicitud(SolicitanteBeneficiarioTerceroSolicitudDto solicitante);
        Task<IEnumerable<IntervinientesActor>> ObtenerIntervinienteActor();
        Task<IntervinientesActorSolicitud> RegistrarIntervinientesActor(IntervinientesActorSolicitudDto intervinientesActor);
        Task<IntervinientesActorSolicitud> ObtenerIntervinientePorId(Guid idInterviniente);
        Task<List<IntervinientesActorSolicitud>> ConsultarIntervinienteActorTipoYNumeroDocumentoAfiliado(int tipoDocumento, string numeroDocumento);
        Task<List<Tramite>> TramitesInterviniente(ConsultaTramitesIntervinientesActorSolicitud interviniente);
        Task<IEnumerable<IntervinientesActorSolicitud>> BuscarIntervinientesActores(BuscarIntervinientesDto interviniente);
        Task<IntervinientesActorSolicitud> ActualizarIntervinienteActor(ActualizarIntervinientesActorSolicitudDto intervinientes);
        Task<bool> ObtenerIntervinienteActorPorNumeroDocumento(string numeroDocumento);

        Task<IEnumerable<SolicitudTerceroApoderado>> ConsultarTercerosPorSolicitud(Guid solicitudTareaId);

        Task<EstadosCatalogosValorSolicitudes> ConsultarEstadosPorDescripcion(string nombreEstado);
        Task<SolicitudPropietario> ConsultarPropietariosPorSolicitud(Guid solicitudTareaId, string numeroDocumento);
        Task<SolicitudPropietario> ActualizarPropietarioMatricula(SolicitudPropietario propietario);
        Task<IntervinientesActorSolicitud> EliminarIntervinienteActor(EliminarIntervinientesActorSolicitudDto intervinientes);

        Task<ObtenerTramiteSolicitud> AprobarSolicitudTareaAcreditacion(AprobarSolicitudTarea aprobarSolicitudTarea);
        Task<int> ActualizarSolicitudAcreditacion(ActualizacionSolicitudAcreditacion solicitud);

        Task<List<SolicitudAcreditacionWorker>> ObtenerSolicitudAcreditacion();
        Task<bool> ActualizarEstadoInconsistente(Guid solicitudId, string Radicado);
        Task<List<ObtenerTramiteSolicitud>> ObtenerSolicitudEnTramiteDePagoAprobadas();
        Task<bool> GuardarMisCuentas(SolicitudCuenta solicitudCuenta);
        Task<SolicitudCuenta> ConsultarMisCuentas(Guid solicitudId);
        Task<bool> ActualizarTerceroBeneficiario(Guid solId, Guid TER_ID, int tipoDoc, string numDoc, string razonSocial, int tipoCuenta, Guid entidad, string numeroCuenta, decimal valor);
    }

}
