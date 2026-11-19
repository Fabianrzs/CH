using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Application.Dto.Cartera.Comprobantes;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.Seven;
using GA2.Application.Dto.Credito.Simulador;
using GA2.Application.Dto.Credito.Simulador.DocumentosAct;
using GA2.Application.Dto.Credito.TU;
using GA2.Application.Dto.Prospeccion;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Cartera.Cierres;
using GA2.Domain.Entities.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Domain.Entities.Cartera.Cierres.CierresCausacionesRealizadas;
using GA2.Domain.Entities.Catalogs;
using GA2.Domain.Entities.Credito;
using GA2.Domain.Entities.Credito.Simulador;
using GA2.Domain.Entities.Credito.Simulador.DocumentosAct;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito;
using GA2.Domain.Entities.Credito.Simulador.SolicitudCredito.Documento;
using GA2.Domain.Entities.Credito.TransUnion;
using GA2.Domain.Entities.Prospeccion;
using GA2.Domain.Entities.Reportes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICreditoRepository
    {
        Task<IEnumerable<RespuestaSolicitudObtenerTramite>> ObtenerTramiteSolicitud(PeticionSolicitudObtenerTramite solicitud);
        Task<IEnumerable<RespuestaFlujoDocumentos>> ObtenerDocumentosFlujo(PeticionFlujoDocumentos solicitud);
        Task<IEnumerable<DocumentosActRespuesta>> ObtenerDocumentosActSolicitud(DocumentosActSolicitud request);
        Task<IEnumerable<DocumentosActRespuesta>> ObtenerDocumentosActSimulacion(DocumentosActSimulacion request);
        Task<IEnumerable<RespuestaObservacionTramite>> ObtenerObservacionTramite(PeticionObtenerObservacionTramite request);
        Task<RespuestaEstadoTramiteSolicitudDesembolso> ObtenerEstadoTramiteSolicitudDesembolso(string numeroDocumento);
        Task<RespuestaValorDesembolsoCredito> ObtenerValorDesembolsoCredito(string numeroDocumento);
        Task<RespuestaActividadTramite> ActualizaActividadTramite(ActividadTramiteSolicitud actualizacion);
        Task<RespuestaActividadTramite> EnviaSubsancionActividadTramite(ActividadTramiteSolicitud actualizacion);
        Task<RespuestaActividadTramite> ApruebaActividadTramite(ActividadTramiteSolicitud actualizacion);
        Task<RespuestaActividadTramite> RechazarActividadTramite(ActividadTramiteSolicitud actualizacion);
        Task<RespuestaActividadTramite> DesistirActividadTramite(ActividadTramiteSolicitud desistimiento);
        Task<RespuestaActividadTramite> DevolverActividadTramite(DevolverActividadTramite devolucion);
        Task<RespuestaCreditoProducto> ObtenerSolicCreditoProducto(PeticionSolicitudCreditoProducto peticion);
        Task<RespuestaCreditoBasica> ObtenerSolicCreditoBasica(PeticionCreditoBasica peticion);
        Task<RespuestaCreditoFinanciero> ObtenerSolicCreditoFinancieros(PeticionCreditoFinanciero peticion);
        Task<SolicCreditoInfTecnica> CrearSolicCreditoInfTecnica(SolicCreditoInfTecnica solicCreditoInfTecnica);
        Task<SolicCreditoInfTecInm> CrearSolicCreditoInfTecInm(SolicCreditoInfTecInm solicCreditoInfTecInm);
        Task<SolicCreditoInfJuridica> CrearSolicCreditoInfJuridica(SolicCreditoInfJuridica solicCreditoInfJuridica);
        Task<SolicCreditoInfJurInm> CrearSolicCreditoInfJurInm(SolicCreditoInfJurInm solicCreditoInfJurInm);
        Task<SolicCreditoInfTecnica> ObtenerSolicCreditoInfTecnica(RequestSolicCreditoInfTecnica requestSolicCreditoInfTecnica);
        Task<IEnumerable<SolicCreditoInfTecInm>> ObtenerSolicCreditoInfTecInm(RequestSolicCreditoInfTecInm requestSolicCreditoInfTecInm);
        Task<SolicCreditoInfJuridica> ObtenerSolicCreditoInfJuridica(RequestSolicCreditoInfJuridica requestSolicCreditoInfJuridica);
        Task<IEnumerable<SolicCreditoInfJurInm>> ObtenerSolicCreditoInfJurInm(RequestSolicCreditoInfJurInm requestSolicCreditoInfJurInm);
        Task<ResponseDocumentoSolicCredito> CrearDocumentoSolicCredito(ResponseDocumentoSolicCredito documentoSolicCredito);
        Task<ResponseDocumentoSimulacion> CrearDocumentoSimulacion(ResponseDocumentoSimulacion documentoSolicCredito);
        Task<UpdateSlsTu> ActualizarSlsReporteTU(UpdateSlsTu requestUpdateSlsTuDto);
        Task<SimulacionCliente> ConsultarSimulacionCliente(string numeroDocumento);
        Task<IEnumerable<PreaprobadoCliente>> SimulacionCliente(string numeroDocumento);
        Task<DatosSolicitudComite> ObtenerDatosSolicComite(Guid guid);
        Task<SolicCreditoRecomendacion> ActualizaSolicCreditoRecomendacion(SolicCreditoRecomendacion request);
        Task<ConsultaSolicitudCredito> ConsultarSolicitudCredito(ConsultaSolicitudCredito request);
        Task<ConsultaSolicitudCredito> ActualizaSolicCreditoSeguro(DatosActualizaSolicCreditoSeguro request);
        Task<IEnumerable<PlanillaSimulacionCredito>> CrearSimulacionCliente(CreacionSimulacionClienteDto request);
        Task<IEnumerable<DetalleSimulacionCliente>> ObtenerDetalleSimulacionCliente(Guid simulacionId);
        Task<ResponseVerificarCierreActSeg> VerificarCierreActSeg(RequestVerificarCierreActSeg request);
        Task<IntegranteComite> CrearIntegranteComiteCredito(IntegranteComite Integrante);
        Task<IntegranteComite> ActualizarIntegranteComiteCredito(IntegranteComite Integrante);
        Task<bool> EliminarIntegranteComiteCredito(long Id_Integrante);
        Task<User> ConsultaUsuarioActaComite(User user);
        Task<ValoresProyectados> GuardarValoresProyectados(ValoresProyectados ValoresProyectados);
        Task<ValoresProyectados> ObtenerValoresProyectados(Guid SLS_ID);


        #region Consecutivo

        Task<Consecutivo> ActualizarConsecutivo(Consecutivo consecutivo);
        Task<Consecutivo> CrearConsecutivo(Consecutivo consecutivo);
        Task<Consecutivo> ObtenerConsecutivoPorId(string consecutivoid);
        Task<Consecutivo> EliminarConsecutivoPorid(string consecutivoid);
        Task<IEnumerable<Consecutivo>> ObtenerConsecutivo();
        #endregion

        #region Aseguradoras
        Task<Aseguradoras> CrearAseguradoras(Aseguradoras aseguradoras);
        Task<Aseguradoras> ActualizarAseguradoras(Aseguradoras aseguradoras);
        Task<Aseguradoras> ObtenerAseguradorasPorId(string aseguradorasid);
        Task<Aseguradoras> EliminarAseguradorasPorid(string aseguradorasid);
        Task<ConsultaSolicitudCredito> ActualizarRadicadoSolicitud(Guid sOC_ID, string barCode);
        Task<IEnumerable<Aseguradoras>> ObtenerAseguradoras();
        #endregion

        #region AlertaAutomaticas
        Task<AlertaAutomaticas> CrearAlertaAutomaticas(AlertaAutomaticas alertaautomaticas);
        Task<AlertaAutomaticas> ActualizarAlertaAutomaticas(AlertaAutomaticas alertaautomaticas);
        Task<AlertaAutomaticas> ObtenerAlertaAutomaticasPorId(string alertaautomaticasid);
        Task<AlertaAutomaticas> EliminarAlertaAutomaticasPorid(string alertaautomaticasid);
        Task<IEnumerable<AlertaAutomaticas>> ObtenerAlertaAutomaticas();

        #endregion

        #region ColorGrilla
        Task<ColorGrilla> EliminarColorGrillaPorid(string colorgrillaid);
        Task<ColorGrilla> ObtenerColorGrillaPorId(string colorgrillaid);
        Task<ColorGrilla> ActualizarColorGrilla(ColorGrilla colorgrilla);
        Task<IEnumerable<ResponseDocumentoSolicCredito>> ObtenerDocumentosSolicCredito(Guid sOC_ID);
        Task<ColorGrilla> CrearColorGrilla(ColorGrilla colorgrilla);
        Task<IEnumerable<ColorGrilla>> ObtenerColorGrilla();

        #endregion

        #region EstadoActividad
        Task<EstadoActividad> CrearEstadoActividad(EstadoActividad estadoActividad);
        Task<EstadoActividad> ActualizarEstadoActividad(EstadoActividad estadoActividad);
        Task<EstadoActividad> ObtenerEstadoActividadPorId(string estadoActividadid);
        Task<EstadoActividad> EliminarEstadoActividadPorid(string estadoActividadid);
        Task<IEnumerable<EstadoActividad>> ObtenerEstadoActividad();
        #endregion

        #region Estado Solicitud
        Task<EstadoSolicitud> CrearEstadoSolicitud(EstadoSolicitud estadosolicitud);
        Task<EstadoSolicitud> ActualizarEstadoSolicitud(EstadoSolicitud estadosolicitud);
        Task<EstadoSolicitud> ObtenerEstadoSolicitudPorId(string estadosolicitudid);
        Task<EstadoSolicitud> EliminarEstadoSolicitudPorid(string estadosolicitudid);
        Task<IEnumerable<EstadoSolicitud>> ObtenerEstadoSolicitud();

        #endregion

        #region Tipo Actividad

        Task<TipoActividad> CrearTipoActividad(TipoActividad tipoActividad);
        Task<TipoActividad> ActualizarTipoActividad(TipoActividad tipoActividad);
        Task<IEnumerable<TipoActividad>> ObtenerTipoActividadPorId(string tipoActividadid); 
        Task<TipoActividad> EliminarTipoActividadPorid(string tipoActividadid);
        Task<IEnumerable<TipoActividad>> ObtenerTipoActividad(int AFL_ID);

        #endregion

        #region Tipo documento
        Task<TipoDocumento> CrearTipoDocumento(TipoDocumento tipodocumento);
        Task<TipoDocumento> ActualizarTipoDocumento(TipoDocumento tipodocumento);
        Task<TipoDocumento> ObtenerTipoDocumentoPorId(string tipodocumentoid);
        Task<TipoDocumento> EliminarTipoDocumentoPorid(string tipodocumentoid);
        Task<IEnumerable<TipoDocumento>> ObtenerTipoDocumento();
        Task<IEnumerable<NMNTA_TIPO_ARCHIVO>> ObtenerTiposArchivos();

        #endregion

        #region Flujo
        Task<Flujo> CrearFlujo(Flujo flujo);
        Task<Flujo> ActualizarFlujo(Flujo flujo);
        Task<Flujo> ObtenerFlujoPorId(string flujoid);
        Task<Flujo> EliminarFlujoPorid(string flujoid);
        Task<IEnumerable<Flujo>> ObtenerFlujo();

        #endregion

        #region flujo Tipo Credito
        Task<FlujoTipoCredito> CrearFlujoTipoCredito(FlujoTipoCredito flujotipoCredito);
        Task<FlujoTipoCredito> ActualizarFlujoTipoCredito(FlujoTipoCredito flujotipoCredito);
        Task<FlujoTipoCredito> ObtenerFlujoTipoCreditoPorId(string flujotipoCreditoid);
        Task<FlujoTipoCredito> EliminarFlujoTipoCreditoPorid(string flujotipoCreditoid);
        Task<IEnumerable<FlujoTipoCredito>> ObtenerFlujoTipoCredito();

        #endregion

        #region Actividad flujo

        Task<ActividadFlujo> CrearActividadFlujo(ActividadFlujo actividadflujo);
        Task<ActividadFlujo> ObtenerActividadFlujoPorId(string actividadflujoid);
        Task<ActividadFlujo> EliminarActividadFlujoPorid(string actividadflujoid);
        Task<IEnumerable<ActividadFlujo>> ObtenerActividadFlujo();

        #endregion

        #region Usuario Actividad
        Task<UsuarioActividad> ObtenerUsuarioActividadPorId(string usuarioactividadid);
        Task<UsuarioActividad> EliminarUsuarioActividadPorid(string usuarioactividadid);
        Task<IEnumerable<UsuarioActividad>> ObtenerUsuarioActividad();
        Task<UsuarioActividad> CrearUsuarioActividad(UsuarioActividad usuarioActividad);
        #endregion

        #region Documento Actividad
        Task<DocumentoActividad> CrearDocumentoActividad(DocumentoActividad documentoactividad);
        Task<DocumentoActividad> ObtenerDocumentoActividadPorId(string documentoactividadid);
        Task<DocumentoActividad> EliminarDocumentoActividadPorid(string documentoactividadid);
        Task<IEnumerable<DocumentoActividad>> ObtenerDocumentoActividad();


        #endregion

        Task<IEnumerable<DatosComiteCredito>> ObtenerComiteCredito(DatosComiteCredito datosComiteCredito);
        Task<DatosComiteCredito> AprobarComiteCredito(DatosComiteCredito datosComiteCredito);
        Task<DatosComiteCredito> CrearComiteCredito(DatosComiteCredito datosComiteCredito);
        Task<IEnumerable<IntegranteComite>> ObtenerIntegranteComite();
        Task<ComiteIntegrante> CrearComiteCreditoIntegrante(ComiteIntegrante integrante);
        Task<IEnumerable<TemaComite>> ObtenerTemasComite();
        Task<IEnumerable<IntegrantePorComite>> ObtenerComiteCreditoIntegrante(IntegrantePorComite integranteComite);
        Task<TemaComiteCredito> CrearComiteCreditoTemas(TemaComiteCredito temaComiteCredito);
        Task<IEnumerable<SolicitudComite>> ObtenerComiteCreditoSolicitud(SolicitudComite solicitudComite);
        Task<SolicitudComiteCreacion> CrearComiteCreditoSolicitud(SolicitudComiteCreacion solicitudComite);
        Task<IEnumerable<TemaComiteCredito>> ObtenerComiteCreditoTemas(RequestTemaComiteCredito solicitudComite);
        Task<HistorialCredito> ObtenerHistorialComercialBuro(string request);
        Task<TasaSolicitudCredito> ActualizaSolicCreditoTasas(TasaSolicitudCredito TasaSolicitudCredito);
        Task<DatosComiteCredito> ActualizarRadicadoComiteCredito(int comiteNumero, string barCode);
        Task<IEnumerable<FuenteRecursos>> ObtenerFuentesRecursos();
        Task<IEnumerable<SolicitudDesembolso>> ObtenerDesembolsosSolicitud(SolicitudDesembolso solicitud);
        Task<SolicitudCreditosDesembolsos> CrearSolicCreditoDesembolsos(SolicitudCreditosDesembolsos solicitud);
        Task<DesembolsoCreditoSolicitud> AplicarDesembolsoSolicitud(DesembolsoCreditoSolicitud desembolso);
        Task<IEnumerable<ObtenerCausalDesistimiento>> ObtenerCausalDesistimiento();
        Task<CreditoFinanciero> CrearSolicCreditoFinancieros(CreditoFinanciero solicitud);
        Task<DatosReparto> ObtenerDatosReparto(Guid request);
        Task<DesistimientoDesembolso> DesistirDesembolsoSolicitud(DesistimientoDesembolso solicitud);
        Task<IEnumerable<ValidacionDocumentos>> ValidarDocumentosFlujo(ValidacionDocumentos solicitud);
        Task<ActualizacionSolicitudCreditoDesemFirma> ActualizaSolicCreditoDesembolso(ActualizacionSolicitudCreditoDesemFirma solicitud);
        Task<ActualizacionSolicitudCreditoDesemFirma> ActualizaSolicCreditoFirmas(ActualizacionSolicitudCreditoDesemFirma solicitud);
        Task<SimulacionDatosPersonales> ObtenerSimulacionPersonales(string request);
        Task<Cuenta> ObtenerCuentaById(string clienteIdentificacion);
        Task<DatosFinancierosPersonales> ObtenerDatosFinancierosPersonales(string request);
        Task<SimulacionDatosPersonales> ActualizarPersonalesVivienda(SimulacionDatosPersonales peticion);
        Task<ActualizacionSolicitudCreditoDesemFirma> ObtenerSolicitudCredito(Guid request, int indicador);
        Task<IEnumerable<SolicitudCreditosDesembolsos>> ObtenerFuentesRecursosSolicitud(Guid IdSolicitud);
        Task<IEnumerable<SolicitudCreditosDesembolsos>> EliminarFuentesRecursosSolicitud(SolicitudCreditosDesembolsos solicitudCreditosDesembolsos);
        Task<IEnumerable<EqivalenciaSimulador>> ObtenerEquivalenciasSimuladorCredito();
        Task<SolicitudSimulacion> CrearSolicitudSimulacionCredito(SolicitudSimulacion solicitudSimulacion);
        Task<SimulacionDetalle> CrearDetalleSimulacion(SimulacionDetalle simulacion);
        Task<SMCSimulacionCliente> CrearSimulacionClienteSMC(SMCSimulacionCliente request);
        Task<SolicitudSimulacion> CrearPreAprobado(SolicitudSimulacion request);
        Task<SolicitudSimulacion> ConsultarPreAprobado(string numeroDocumento);
        Task<SMCSimulacionCliente> ObtenerDatosSolicitudClienteByPreAprobado(SolicitudSimulacion preAprobado);
        Task<SolicitudSimulacion> ActualizarRadicadoPreAprobado(SolicitudSimulacion preAprobado);
        Task<SolicitudSimulacion> ObtenerSolicitudSimulacionSLS(Guid simulacionId);

        Task<SolicitudSimulacion> ObtenerSolicitudInformacionSimulacion(Guid simulacionId); 
        Task<SolicitudSimulacion> ObtenerSolicitudSimulacionCreditoNumeroDocumento(string numeroDocumento);
        Task<SolicitudSimulacion> ObtenerSolicitudCreditoNumeroDocumento(string numeroDocumento);
        Task<SolicitudCredito> CrearSolicitudCredito(SolicitudCredito solicitudCredito);
        Task<SocSolicitudInfobasica> CrearSolicCreditoBasicos(SocSolicitudInfobasica infoBasica);
        Task<IEnumerable<TasaHogarCiudad>> ObtenerTasasHogarCiudad();
        Task<IEnumerable<TasaHogarCiudad>> ObtenerTasasHogarUbicacion(int departamentoId);
        Task<InformacionConyugue> ObtenerSolicCreditoConyugue(PeticionCreditoBasica IdSolicitud);
        Task<InformacionConyugue> CrearSolicCreditoConyugue(InformacionConyugue info);
        Task<IEnumerable<CierreDiarioCartera>> CierreDiarioCartera(RequestCierreDiarioCartera requestCierreDiario);
        Task<SolicitudCredito> ObtenerSolicitudCreditoTipoNumeroIdentificacion(int tipoDocumento, string numeroDocumento);
        Task<bool> RegistroLogTUSimulador(string breakPoint, string request, string response); 
        Task<SolicitudCredito> ObtenerInformacionSolicitudCreditoCartera(int tipoDocumento, string numeroDocumento);
        Task<int> ObtenerSeguroPorUbicacion(int dpcId, int dpdId);
        Task<SocSolicitudInfobasica> ActualizaSolicCreditoBasica(SocSolicitudInfobasica infoBasica);
        Task<SolicCreditoRefererncias> CrearSolicCreditoRefererncias(SolicCreditoRefererncias data);
        Task<SolicCreditoFinancieros> ActualizaSolicCreditoFinancieros(SolicCreditoFinancieros data);
        Task<IEnumerable<SolicCreditoRefererncias>> ObtenerSolicCreditoReferencias(Guid ID_SOC);
        Task<IEnumerable<DocumentoCargarSolicitud>> ObtenerDocumentosRequeridosSolicCredito();
        Task<SolicitudCredito> ActualizaSolicCreditoAutorizacionDatos(SolicitudCredito solic);
        Task<SolicCreditoRefererncias> ObtenerSolicCreditoReferenciasPorTipo(Guid ID_SOC, string tipoReferencia); 
        Task<EstadoSolicCredito> ObtenerEstadoSolicCredito(string documento);
        Task<ResponseDocumentoSolicCredito> ObtenerDocumentosSubidosSolicCredito(RequestDocumentosSubidosProspeccion request);
        Task LimpiarSimulacionCliente(Guid simulacionId);
        Task<ConsultaSolicitudCredito> CrearNoSolicitudCredito(ConsultaSolicitudCredito solic);
        Task<RespuestaSolicitudObtenerTramite> ObtenerTramiteSolicitudCli(string identificacion);
        Task<IEnumerable<User>> ObtenerPeritos();
        Task<TramiteSolicitud> ActualizaTramitePerito(RequestTramiteSolicitud request);
        Task<ResponseActividadTramite> SubsanarActividadTramite(RequestActividadTramite request);
        Task<IEnumerable<ParametroIntegracion>> ObtenerParametrosIntegraciones();
        Task<IEnumerable<InsertarNotificacionFuncionarioExternoResp>> InsertarNotificacionFuncionarioExterno(InsertarNotificacionFuncionarioExterno request);
        Task<InformacionConyugue> EliminarInformacionSolicConyuge(Guid solicitudId);
        Task<IEnumerable<Preaprobaciones>> ObtenerPreaprobaciones(string? NumeroDocumento, string? estado, DateTime? fecha, DateTime? fechaf);
        Task<ResponseDatosSimulacionParaEstudio> ObtenerDatosSimulacionParaEstudio(string numeroDocumento);
        Task<IEnumerable<ObtenerParametrosSeguroUbicacion>> ObtenerParametrosSeguroUbicacion();
        Task<IEnumerable<ParametroSeguroRegion>> ObtenerParametrosporSeguroRegion();
        Task<IEnumerable<ParametroSeguroRegionGui>> ActualizarSeguroPorRegion(int? Id, double? Seguro);
        Task<ResultadoIntegracionDto> InsertarResultadoIntegracion(ResultadoIntegracionDto resultado);
        Task<ProyeccionCliente> ObtenerProyeccionCliente(string? Id);
        Task<IEnumerable<ProyeccionCliente>> ObtenerSimCargo(string? Id);
        Task<SmlvAnualDto> ObtenerSalarioMinimoAnual(ObtenerSmlvAnualDto anio);
        Task<MatriculaCreditoDto> CrearMatriculaCredito(MatriculaCreditoDto request);
        Task<IEnumerable<MatriculaCreditoDto>> ObtenerMatriculasCredito();
        Task<MatriculaCreditoDto> EliminarMatriculasCreditoSolicitud(Guid? solicitudId);
        Task<IEnumerable<MatriculaCreditoDto>> ObtenerMatriculasCreditoporSolicitud(Guid? solicitudId);
        Task<IEnumerable<MatriculaCreditoDto>> ActualizarMatriculasCredito(MatriculaCreditoDto request);
        Task<IEnumerable<CausalesRechazo>> ObtenerCausalesRechazo();
        Task<IEnumerable<ActividadTramiteSolicitud>> ObtenerActividadTramite();
        Task<IEnumerable<EstadoTramites>> ObtenerEstadosTramite();
        Task<IEnumerable<TramitesPendientes>> ObtenerTramitesPendientes();
        Task<bool> ActualizaMPRCONTSolicitudCredito(Guid idSolicitud, int mpr_cont, int tipo_cdp);
        Task<bool> ActualizarCodProveedorSevenSolicitudCredito(Guid idSolicitud, int ProveCod, int tipo_pv);
        Task<bool> ActualizaFACCONTSolicitudCredito(RequestAcualizarFacContDto request);
        Task<bool> ActualizarEstadoDesembolso(Guid idSolicitud, decimal saldoDesembolsado);
        Task<IEnumerable<ObtenerOrdenDescuentos>> ObtenerOrdenDescuentoSaldos();
        Task<IEnumerable<ObtenerOrdenDescuentos>> ObtenerOrdenDescuentoSaldosCredito();
        Task<SocSlsSimulacionCredito> ObtenerSlsSocSolicitudCredito(Guid idSolicitud);
        Task<IEnumerable<CausacionDiariaInteresesCorrientes>> CausacionDiariaInteresCorriente(RequestCausacionDiariaInteresesCorrientes request);
        Task<IEnumerable<CausacionDiariaInteresesCorrientes>> RecuperacionCausacionDiariaInteresCorriente(RequestCausacionDiariaInteresesCorrientes request);
        Task<IEnumerable<CausacionDiariaInteresesCorrientes>> RecuperacionCausacionDiariaInteresesMoratorio(RequestCausacionDiariaInteresesCorrientes request);
        Task<IEnumerable<TipoCierreCartera>> ObtenerTiposDeCierreCartera();
        Task<IEnumerable<CierresCausacionesRealizadas>> ObtenerCierresCausacionesRealizadas(RequestCierresCausacionesRealizadas request);
        Task<IEnumerable<ClienteIdentificacion>> ConsultarClientesCredito();
        Task<int> ConsultarVigenciaSimulacion(string Request);
        Task<SocSlsSimulacionCredito> ObtenerSlsSocSolicitudCreditoNroDocumento(string nroDocumento);
        Task<ProyeccionCliente> ObtenerProyeccionCliente(string Id, int year);
        Task<SimulacionDatosPersonales> ActualizarDatosPersonalesSolicitud(SimulacionDatosPersonales peticion);
        Task<CheckArchivo> CheckArchivosCredito(Guid idSolicitud, string nombreArchivo);
        Task<bool> EliminarCheckArchivosCredito(Guid idSolicitud);
        Task<IEnumerable<CheckArchivo>> ObtenerArchivosCheckCredito(Guid idSolicitud);
        Task<IEnumerable<ArchivosCredito>> ConsultarArchivosPorTipoCredito();
        Task<IEnumerable<MediosCreditoVivienda>> ConsultarMediosCreditosVivienda();
        Task<MediosCreditoVivienda> ObtenerMediosCreditosViviendaById(Guid idMedioCreditoVivienda);
        Task<MediosRecepcion> ObtenerMediosRecepcionById(int idMedioRecepcion);
        Task<DatosAdicionalesSimulacion> ActualizarDatosAdicionalesSimulacion(DatosAdicionalesSimulacion datosSimulacion);
        Task<InformacionProspeccion> ConsultarProspeccionPorNumeroDocumento(int tipoDocumento, string numeroDocumento);
        Task<ValoresProspeccion> ConsultarTasasGeneralesPorAfiliado(int tipoDocumento, string numeroDocumento);
        Task<bool> InsertarValoresSimulacionAfiliado(ValoresSimulacionAfiliadoDto valoresSimulacionAfiliado);
        Task<DatosProspeccionEndudamiento> ConsultarProspeccionEndudamiento(int tipoDocumento, string numeroDocumento);
        Task<IEnumerable<LineaCreditoObligacion>> InsertarLineaCreditoObligacion(LineasCreditoObligacionDto lineasCredito);
        Task<IEnumerable<LineaCreditoObligacion>> ConsultarLineaCreditoObligacion(int tipoDocumento, string numeroDocumento);
        Task<bool> LimpiarLineasCreditoObligacion(int tipoDocumento, string numeroDocumento);
        Task<SimulacionPlanCliente> ObtenerDatosSimulacionClienteSPC(DatosSimulacionUsuarioDto datosSimulacion);
        Task<IEnumerable<SimulacionDetallePlanCliente>> ObtenerDetalleSimulacionClienteSPD(DatosSimulacionUsuarioDto datosSimulacion);
        Task<SMCSimulacionCliente> RestaurarSimulacionCliente(SMCSimulacionCliente request);
        Task<SimulacionDetalle> RestaurarDetallePlanCliente(SimulacionDetalle simulacion);
        Task<LineaCreditoObligacion> RecalcularLineaCreditoObligacion(DatosLineaCreditoObligacionDto datosLinea);
        Task<PeritoDetalle> InsertarPeritoDetalle(PeritoDetalleDto perito);
        Task<PeritoDetalle> ConsultarPeritoDetalle(string solicitudCredito);
        Task<IEnumerable<ConsultarDetalleAprobadosNoDesembolsadosDTO>> ObtenerDetallePagoSeven(RequestDetalleSeven solicitud);
        Task<TipoIdentificacion> ObtenerTipoIdentificacionPorId(int tipo);
        Task<DatosEndeudamientoProspeccion> InsertarDatosEndeudamientoProspeccion(DatosEndeudamientoProspeccionDto datosEndeudamiento);
        Task<DatosEndeudamientoProspeccion> ConsultarDatosEndeudamientoProspeccion(string numeroDocumento);
        Task<bool> GuardarValorCDP(InsertarValoresCDPSeven valoresCDPSeven);
        Task<SegurosDatosSolicitudCredito> RegistrarSeguroSolicitudCredito(SegurosDatosSolicitudCreditoDto segurosDatos);
        Task<IEnumerable<ConsultaSegurosCredito>> ConsultarSegurosSolicitudCredito(ConsultaSegurosCreditoDto consultaSegurosCredito);
        Task<IEnumerable<ObtenerValoresCDPSevenDto>> ObtenerValorCDP(RequestValoresCDPSeven request);
        Task<DocumentosActRespuestaDto> ActualizarDocLegalizacion(DocumentosActEstadoRequestDto request);

        Task<SegurosDatosSolicitudCredito> ConsultarSeguroSimulacionCredito(string numeroDocumento);
        Task<bool> GuardarValorFactura(InsertarValoresFacturaSeven valoresFacturaSeven);
        Task<IEnumerable<ObtenerDetalleFacturaSevenDto>> ObtenerValorFactura(RequestValoresFactura request);
        Task<IEnumerable<ObtenerComprobantesDto>> ObtenerComprobantesCredito(RequestObtenerComprobantesCredito request);
        Task<TasaSimulador> ObtenerTasasSimuladorByTipoCredito(string descripcion, int tipoCredito);
        Task<bool> GuardarAprobacionDesembolso(InsertarAprobadosDesembolso aprobadosDesembolso);
        Task<bool> InsertarRecursosCongelados(CongelamientoRecursosProspeccion congelamientoRecursos);
        Task<User> ConsultarUsuarioPorId(Guid? usuarioId);

        Task<bool> LimpiarRecursosCongelados(Guid? simulacionId, string numeroDocumento);
        Task<bool> InsertarNotificacionesSolicitudes(NotificacionesSolicitudes notificacionesSolicitudes);
        Task<NotificacionesSolicitudes> ConsultarNotificacionesSolicitudes(Guid simulacionId);
        Task<IEnumerable<AprobacionCreditoAsesor>> ConsultarAprobacionAsesor(Guid trsID);
        Task<DatosAsegurabilidadSolicitudCredito> ConsultarDatosAsegurabilidadCredito(Guid solicitudCreditoId);
        Task<bool> LimpiarConceptosCongelados(Guid? simulacionId, string numeroDocumento);
        Task<bool> InsertarConceptosCongelados(ConceptosCongeladosProspeccion conceptosCongelados);
        Task<IEnumerable<ConceptosCongeladosProspeccion>> ConsultarRecursosCongelados(ConceptosCongeladosProspeccion conceptosCongelados);
        Task<IEnumerable<DescuentoTipoVinculacion>> ConsultarTipoVinculacionCredito(string descripcionVinculacion, string tipoCredito);
        Task<bool> InsertarPrimerPagoCDP(InsertarDatosCDPPrimerPago conceptosCongelados);
        Task<PrimerPagoCDPCredito> ConsultarPrimerPagoCDP(Guid simulacionID);

        Task<SolicitudDesembolso> ObtenerDesembolsosSolicitudCredito(Guid idSolicitud);
        Task<bool> ActualizarFechaDesembolsoCredito(Guid idSolicitud, DateTime fechaDesembolso); 

        Task<DatosPersonalesCreditoEducativo> ActualizarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales);
        Task<DatosPersonalesCreditoEducativo> InsertarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales);
        Task<DatosPersonalesCreditoEducativo> EliminarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales);
        Task<DatosPersonalesCreditoEducativo> ConsultarDatosPersonalesEducativo(DatosPersonalesCreditoEducativo datosPersonales);
        Task<BeneficiarioEntidadEducativa> InsertarBeneficiarioEntidadEducativa(BeneficiarioEntidadEducativa datosPersonales);
        Task<BeneficiarioEntidadEducativa> ConsultarBeneficiarioPorNumeroTitular(string numeroTitular);
        Task<UbicacionInmuebleCredito> ObtenerUbicacionViviendaCredito(Guid sls_id);
        Task<IEnumerable<ReportesSolicitudCredito>> ReportesSolicitudCreditoGeneral(ReportesSolicitudCreditoDto datos);
        Task<IEnumerable<ReportesSolicitudCredito>> ReportesSolicitudCreditoGeneralUsuuarioExterno(ReportesSolicitudCreditoDto datos); 
        Task<bool> LimpiarDatosPersonalesSimulacion(string request);
        Task<RespuestaVigiaParam> ConsultarResponseVigiaDesactivado();
        Task<bool> InsertarDatosFacturaSevenCredito(FacturaSevenCredito facturaSeven);
        Task<FacturaSevenCredito> ConsultarDatosFactura(Guid SOC_ID);
        Task<RespuestaObservacionTramite> ConsultarActividadPorTramiteID(Guid tramiteID);
        Task<bool> RegistrarBloqueoCuentaCredito(BloqueoRecursosCredito bloqueoRecursos);
        Task<IEnumerable<BloqueoRecursosCredito>> ConsultarBloqueoCuentaCredito(BloqueoRecursosCreditoDto bloqueoRecursos);
        Task<ConsultarCreditoProspeccion> ConsultarNumeroTipoCreditoProspeccion(int tipoDocumento, string numeroDocumento);
        Task<bool> InsertarValidacionOTPTemporal(ValidacionOTPTemporalDto validacion);
        Task<ValidacionOTPTemporal> ConsultarValidacionOTPTemporal(ValidacionOTPTemporalDto validacion);
        Task<bool> EliminarValidacionOTPTemporal(ValidacionOTPTemporalDto validacion);
        Task InsertarDocumentoSimulacion(ResponseDocumentoSimulacion documento);

        Task<CompraCarteraCredito> InsertarValoresCompraCartera(CompraCarteraCredito compraCartera);
        Task<CompraCarteraCredito> ConsultarValoresCompraCartera(CompraCarteraCredito compraCartera);
        Task<TasaSimulador> ConsultarTasaPorTipoCredito(string descripcionTipoSimulacion);
        Task<(int Resultado, string Mensaje)> ValidarContinuidadCausacionDiaria(
            DateTime fechaCausacion, int tcrId);
        Task InsertarCausacionDiariaLeasingHistoricoEnBatchTVP(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre);
        Task ActualizarCausacionDiariaLeasingSaldoActualEnBatchTVP(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre);
        Task InsertarCausacionDiariaHipotecarioHistoricoEnBatchTVP(
           List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre);
        Task ActualizarCausacionDiariaHipotecarioSaldoActualEnBatchTVP(
            List<CausacionDiariaInteresesCorrientesDto> causaciones, DateTime FechaCierre);
        Task<List<CausacionSaldoActual>> ObtenerSaldoActualCreditoAsync(
            long numeroCredito,
            int idTipoCredito);
        Task<List<CausacionHistorico>> ObtenerHistorialCausacionInteresCorrienteAsync(
    long numeroCredito, int idTipoCredito, DateTime fechaInicial, DateTime fechaFinal);

        Task<SolicitudSimulacion> ActualizarRadicadoPreAprobadoFlujo83(Guid idSimulacion, string Radicado83);
        Task<bool> ActualizarTipoSimulador(string numeroDocumento, string simuladorDescripcion);

        Task<bool> InsertValoresSimulacionCredito(ValoresSimulacionCreditoReclasificacion valores);
        Task<ValoresSimulacionCreditoReclasificacion> ConsultaValoresSimulacionCredito(string numeroDocumento);
        Task<DocumentoCargadoDodo> ConsultaDocumentoCargadoDodo(Guid slsId, string descripcionDocumento); 
        Task<bool> InsertarDocumentoCargadoDodo(DocumentoCargadoDodo documento);        
        Task<IEnumerable<InformacionPerito>> ConsultarSolicitudesTramitePerito(string usuarioPerito);
        Task<IEnumerable<AliadosEstrategicos>> ConsultarAliadosEstrategicos();

        Task<AvaluoComercialCredito> InsertarActualizarAvaluoComercialCredito(AvaluoComercialCredito avaluoComercialCredito); 
        Task<AvaluoComercialCredito> ConsultarAvaluoComercialCredito(Guid soc_id);
        Task<ValidacionCreditoProspeccion> ConsultarValidacionCreditoProspeccion(string numeroDocumento);
        Task<ValidacionCreditoProspeccion> InsertarActualizarValidacionCreditoProspeccion(ValidacionCreditoProspeccion validacionCreditoProspeccion);
        Task<IEnumerable<UsuarioCreditoTramite>> ConsultarUsuarioCredito(int indicador);
        Task<IEnumerable<DocumentosActRespuesta>> ObtenerDocumentosPorTAC(DocumentosActSolicitud request);

        Task<ValidacionCreditoProspeccion> InsertarDocumentoActividadCredito(DocumentosCredito request);
        Task<IEnumerable<TipoDocumento>> ObtenerTiposDocumento();
        Task<bool> EliminarDocumentoActividadCredito(DocumentosCredito request);
        Task<DocumentosCargadoDodo> ObtenerDocumentoCargado(DocumentosCargadoDodo request);
        Task<bool> InsertarDocumentoCargado(DocumentosCargadoDodo request);
        Task<bool> InsertarReporteTransUnion(ReporteProspeccionPreaprobado request);
        Task<IEnumerable<ReporteProspeccionPreaprobado>> ObtenerReporteTransUnion(string numeroIdentificacion, DateTime? fechaDesde,DateTime? fechaHasta,int? estado); 
    }
}
