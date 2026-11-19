import axios from 'axios';
import { routega2api } from '../utils/baserouteapi.route';
import StorageService from './storage.service';
import { CreateSeveranceModel } from '../models/requests/create-severance.model';
import { ConsultarSolicitudModel } from '../models/requests/consultar-solicitud.model';
import { ParametrosBusqueda, SolicitudTareaModel } from '../models/requests/solicitud-tarea.model';
import { AprobarSolicitudTareaDto } from '../models/requests/aprobar-solicitud.model';
import { RechazarSolicitudTareaDto } from '../models/requests/rechazar-solicitud.model';
import { ConsultarDocumentosSubModalidadTareaModel } from '../models/requests/consultar-documentos-submodalidad-tarea.model';
import { DocumentosEliminarModel } from '../models/requests/documento-eliminar.model';

const SolicitudApi = () => {

	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'application/json',
			'X-Frame-Options': 'sameorigin'
		},
		baseURL: `${routega2api}solicitud`,
		timeout: 60000,
	});

	// enviar carga de documentos por form-data
	const apiAxiosCargaDocumentos = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'multipart/form-data',
		},
		baseURL: `${routega2api}solicitud`,
		timeout: 125000,
	});

	// cargar headers token carga de documentos.
	apiAxiosCargaDocumentos.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	apiAxios.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	const posCreateRequestSeverance = (createSeveranceModel: CreateSeveranceModel) =>
		apiAxios
			.post('crearsolicitud', createSeveranceModel)
			.then((res: any) => res.data);


	const GetConsultarSolucitud = (consultarSolicitudModel: ConsultarSolicitudModel) =>
		apiAxios
			.post(`ConsultarExisteSolicitud`, consultarSolicitudModel)
			.then((res: any) => res.data);

	const PostConsultarSolicitud = (consultarSolicitudModel: ConsultarSolicitudModel) =>
		apiAxios
			.post(`solicitud/ConsultarExisteSolicitud`,
				consultarSolicitudModel, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const PostSolicitudTarea = (solicitudTareaModel: SolicitudTareaModel) =>
		apiAxios
			.post(`CrearSolicitudTarea`, solicitudTareaModel)
			.then((res: any) => res.data);

	const PostActualizarSolicitudTarea = (solicitudTareaModel: SolicitudTareaModel) =>
		apiAxios
			.post(`ActualizarSolicitudTarea`, solicitudTareaModel)
			.then((res: any) => res.data);

	const PutSubsanarSolicitudTarea = (solicitudTareaModel: RechazarSolicitudTareaDto) =>
		apiAxios
			.put(`SubsanarSolicitudTarea`, solicitudTareaModel)
			.then((res: any) => res.data);

	const PostAprobarSolicitudTarea = (aprobarSolicitudTareaDto: AprobarSolicitudTareaDto) =>
		apiAxios
			.post(`AprobarSolicitudTarea`, aprobarSolicitudTareaDto)
			.then((res: any) => res.data);

	const PostAprobarSolicitudTareaDocumentos = (aprobarSolicitudTareaDto: AprobarSolicitudTareaDto) =>
		apiAxios
			.post(`AprobarSolicitudTareaDocumento`, aprobarSolicitudTareaDto)
			.then((res: any) => res.data);

	const PostRechazarSolicitudTarea = (rechazarSolicitudTareaDto: RechazarSolicitudTareaDto) =>
		apiAxios
			.post(`RechazarSolicitudTarea`, rechazarSolicitudTareaDto)
			.then((res: any) => res.data);

	const PostAnularSolicitudTarea = (anularSolicitudTareaDto: RechazarSolicitudTareaDto) =>
		apiAxios
			.post(`AnularSolicitudTarea`, anularSolicitudTareaDto)
			.then((res: any) => res.data);

	const GetConsultarDocumentosPorSubModalidad = (consultarDocumentosSubModalidadTarea: ConsultarDocumentosSubModalidadTareaModel) =>
		apiAxios
			.post(`ConsultarDocumentosPorSubModalidad`, consultarDocumentosSubModalidadTarea)
			.then((res: any) => res.data);


	const GetConsultarDocumentosPorSubModalidadDodo = (request: string) =>
		apiAxios
			.get(`ObtenerDocumentosRadicado?request=${request}`)
			.then((res: any) => res.data);


	const postCargueDocumentosSolicitud = (data: FormData, idSolicitud: string | undefined, numeroRadicado: string | undefined) =>
		apiAxiosCargaDocumentos
			.post(`cargarDocumentosSolicitud/${idSolicitud}`, data)
			.then((res: any) => res.data);


	const postCrearRegistroCargaArchivo = (data: FormData, idSolicitud: string | undefined) =>
		apiAxiosCargaDocumentos
			.post(`CrearRegistroCargaArchivo/${idSolicitud}`, data)
			.then((res: any) => res.data);


	const GetConsultarDocumentosPorSolicitud = (idSolicitud: string) =>
		apiAxios
			.get(`ConsultarArchivoPorSolicitud?IdSolicitud=${idSolicitud}`)
			.then((res: any) => res.data);

	const PostEliminarDocumento = (documentoEliminar: DocumentosEliminarModel) =>
		apiAxios
			.post(`EliminarArchivoPorSolicitud`, documentoEliminar)
			.then((res: any) => res.data);

	const PostConsultarExistePersona = (numeroDocumento: any) =>
		apiAxios
			.get(`ConsultarExistePersona?numeroDocumento=${numeroDocumento}`)
			.then((res: any) => res.data);

	const GetConsultarExistePropProv = (numeroDoc: any) =>
		apiAxios
			.get(`ConsultarExistePropProv?numeroIdentificacion=${numeroDoc}`)
			.then((res: any) => res.data);

	const GetDescargarArchivoSolicitud = async (rutaStorage: string) => {
		const datos = await apiAxios
			.get(`DescargarArchivoPorSolicitud?rutaStorage=${rutaStorage}`, {
				responseType: 'blob'
			})
			.then((res: any) => res.data);
		return datos;
	}

	const PostInsertarInconsistenciaSolicitud = (inconsistencia: any) =>
		apiAxios
			.post(`InsertarInconsistenciasSeguridadSolicitud`, inconsistencia)
			.then((res: any) => res.data);

	const PostConsultarSolicitudes = (search: ParametrosBusqueda) =>
		apiAxios
			.post(`ConsultarSolicitudCesantias`, search)
			.then((res: any) => res.data);

	const GetConsultarInconsistencia = (request: string) =>
		apiAxios
			.get(`ConsultarInconsistenciaSeguridadSolicitud?request=${request}`)
			.then((res: any) => res.data);

	const GetConsultarExisteMatricula = (numeroMatricula: string) =>
		apiAxios
			.get(`ConsultarExisteMatricula?numeroMatricula=${numeroMatricula}`)
			.then((res: any) => res.data);

	const GetFormulariosSolicitud = (timCodigo: string) =>
		apiAxios
			.get(`ObtenerFomulariosSolicitud?timCodigo=${timCodigo}`)
			.then((res: any) => res.data);

	const PostActualizarInconsistenciaSolicitud = (inconsistencia: any) =>
		apiAxios
			.patch(`ActualizarInconsistenciaSolicitud`, inconsistencia)
			.then((res: any) => res.data);

	const EliminarInconsistenciaSolicitud = (inconsistencia: any) =>
		apiAxios
			.delete(`EliminarInconsistenciaSolicitud?inconsistenciaId=${inconsistencia}`)
			.then((res: any) => res.data);

	const ObtenerComunicacionesSolicitud = (idSolicitud: any) =>
		apiAxios
			.get(`ObtenerComunicacionesSolicitud?idSolicitud=${idSolicitud}`)
			.then((res: any) => res.data);

	const InsertarComunicacionesSolicitud = (idSolicitud: any) =>
		apiAxios
			.post(`InsertarComunicacionesSolicitud`, idSolicitud)
			.then((res: any) => res.data);

	const ActualizarComunicacionesSolicitud = (idSolicitud: any) =>
		apiAxios
			.post(`ActualizarComunicacionesSolicitud`, idSolicitud)
			.then((res: any) => res.data);

	const postCargarDocumentosSolicitudCredito = (data: FormData, idSolicitud: string | undefined) =>
		apiAxios
			.post(`${'CargarDocumentosSolicitudCredito/'} ${idSolicitud}`, data)
			.then((res: any) => res.data);


	const actualizarObservaciones = (data: any) =>
		apiAxios
			.post(`ActualizarObservacionesSolicitud/`, data)
			.then((res: any) => res.data);

	const obtenerTrazabilidadSolicitud = (data: any) =>
		apiAxios
			.get(`ObtenerTrazabilidadSolicitud?request=${data}`)
			.then((res: any) => res.data);

	const GetBoteneEnlaceDodo = (data: any) =>
		apiAxios
			.get(`ObtenerEnlaceDodo?request=${data}`)
			.then((res: any) => res.data);

	const PostGuardarDocumentosDodoVerificados = (dodoVerificado: any) =>
		apiAxios
			.post(`solicitud/checkDocumentosSolicitud`,
				dodoVerificado, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const GerObtenerDocumentosVerifiacadosDodo = (data: any) =>
		apiAxios
			.get(`ObtenerDocumentosChecks?request=${data}`)
			.then((res: any) => res.data);

	const GetObtenerNovedades = () => apiAxios
		.get('ConsultarNovedadSolicitud')
		.then((res: any) => res.data);


	const PostObtenerTiposNovedadPorId = (data: any) =>
		apiAxios
			.post(`ConsultarTipoNovedadSolicitud?request=${data}`,)
			.then((res: any) => res.data);
	const PostObtenerTiposCausalId = (data: any) =>
		apiAxios
			.post(`ConsultarCausalSolicitud?request=${data}`,)
			.then((res: any) => res.data);


	const PostObtenerTiposPagosSucesivos = (data: any) =>
		apiAxios
			.post(`ConsultarPagosSucesivosSolicitud?request=${data}`,)
			.then((res: any) => res.data);
	const PostObtenerTiposInformeInconsistenciasSolicitud = (data: any) =>
		apiAxios
			.post(`ConsultarInformeInconsistenciasSolicitud?request=${data}`,)
			.then((res: any) => res.data);

	const EliminarNovedad = (ids: any) =>
		apiAxios
			.patch('EliminarInconsistenciasSeguridadSolicitud', ids)
			.then((res: any) => res.data);

	const ActualizarInconsistencias = (ids: any) =>
		apiAxios
			.patch(`ActualizarInconsistenciaSeguridadSolicitud`, ids)
			.then((res: any) => res.data);

	const PostObtenerFlujoDodo = (data: any) =>
		apiAxios
			.post(`ObtenerFlujoDodo`, data)
			.then((res: any) => res.data);

	const PostObtenerConceptosAfectadosCliente = (data: any) =>
		apiAxios
			.post(`ObtenerConceptosAfectadosCliente`, data)
			.then((res: any) => res.data);

	const PostObtenerCuentasAfectadasCliente = (data: any) =>
		apiAxios
			.post(`ObtenerCuentasAfectadasCliente`, data)
			.then((res: any) => res.data);
	const PostInfoConyuge = (data: any) =>
		apiAxios
			.post(`InformacionConyuge`, data)
			.then((res: any) => res.data);

	const PostConsultaRadicadoSolicitud = (data: any) =>
		apiAxios
			.post(`ConsultarRadicadoSolicitud`, data)
			.then((res: any) => res.data);

	const PostInconsistenciaDescripcionSolicitud = (data: any) =>
		apiAxios
			.post(`InsertarInconsistenciaDescripcionSolicitud`, data)
			.then((res: any) => res.data)

	const PostBuscarUsuarioIngreso = (data: any) =>
		apiAxios
			.post(`ConsultarUsuarioIngresoSolicitud`, data)
			.then((res: any) => res.data)

	const PostBuscarSolicitante = (data: any) =>
		apiAxios
			.post(`ConsultarSolicitanteBeneficiarioTerceroSolicitud`, data)
			.then((res: any) => res.data)

	const GetConsultarIntervinientesActores = () => apiAxios
		.get('ConsultarIntervinientesActores')
		.then((res: any) => res.data);

	const PostGuadarTipoActor = (data: any) =>
		apiAxios
			.post(`RegistrarIntervinientesActores`, data)
			.then((res: any) => res.data)


	const PostObtenerTipoActorById = (data: any) =>
		apiAxios
			.post(`ObtenerIntervinientesPorId?request=${data}`,)
			.then((res: any) => res.data)


	const PostBuscarIntervinientesActores = (data: any) =>
		apiAxios
			.post(`BuscarIntervinientesActores`, data)
			.then((res: any) => res.data)

	const PosttActualizarIntervinientes = (data: any) =>
		apiAxios
			.post(`ActualizarIntervinientes`, data)
			.then((res: any) => res.data)

	const PostEliminarIntervinientePorId = (data: any) =>
	    apiAxios
		.post(`EliminarIntervinientePorId`, data)
		.then((res: any) => res.data)

	const PostListaTramitesIntervinientes = (data: any) =>
		apiAxios
			.post(`TramiteIntervimiente`, data)
			.then((res: any) => res.data)

	const PostConsultarIntervinientePorDocumentoAfiliado = (data: any) =>
	apiAxios
		.post(`ConsultarIntervinientePorDocumentoAfiliado`, data)
		.then((res: any) => res.data)

	const ObtenerRespuestaSolicitud = (consultarSolicitudModel: ConsultarSolicitudModel) =>
	apiAxios
		.post(`ObtenerRespuestaSolicitud`,
			consultarSolicitudModel, {
			baseURL: routega2api, responseType: 'blob',
		})
		.then(res => res.data);

		const ObtenerRespuestaSolicitudInconsistente = (consultarSolicitudModel: ConsultarSolicitudModel) =>
		apiAxios
			.post(`ObtenerRespuestaSolicitudInconsistente`,
				consultarSolicitudModel, {
				baseURL: routega2api, responseType: 'blob',
			})
			.then(res => res.data);

	const CrearRadicadoCartaIncosistencia = (data: any) =>
	apiAxios
		.post(`CrearRadicadoCartaIncosistencia`, data)
		.then((res: any) => res.data)

	const PostConsultaReporteSolcitudesGeneral = (data: any) =>
	apiAxios
		.post(`DataReporteSolictud`, data)
		.then((res: any) => res.data)

	const GetObtenerHistorialCambiosIntervinientes = (idInterviniente: string) =>
	apiAxios
		.get(`Entidades/ObtenerHistorialCambiosIntervinientes?idIntervinientes=${idInterviniente}`,{
			baseURL: routega2api
		})
		.then((res: any) => res.data);

	const PostObtenerPdfFup = (consultarSolicitudModel: ConsultarSolicitudModel) =>
	apiAxios
		.post(`ObtenerRespuestaSolicitudDePago`,
			consultarSolicitudModel, {
			baseURL: routega2api, responseType: 'blob',
		})
		.then(res => res.data);

	return {
		posCreateRequestSeverance,
		GetConsultarSolucitud,
		PostSolicitudTarea,
		PostActualizarSolicitudTarea,
		PostAprobarSolicitudTarea,
		PostRechazarSolicitudTarea,
		PostAnularSolicitudTarea,
		GetConsultarDocumentosPorSubModalidad,
		postCargueDocumentosSolicitud,
		postCrearRegistroCargaArchivo,
		GetConsultarDocumentosPorSolicitud,
		PostEliminarDocumento,
		GetDescargarArchivoSolicitud,
		PostConsultarExistePersona,
		GetConsultarExistePropProv,
		PostInsertarInconsistenciaSolicitud,
		PostConsultarSolicitudes,
		GetConsultarInconsistencia,
		PostConsultarSolicitud,
		GetConsultarExisteMatricula,
		PostAprobarSolicitudTareaDocumentos,
		PutSubsanarSolicitudTarea,
		GetFormulariosSolicitud,
		PostActualizarInconsistenciaSolicitud,
		EliminarInconsistenciaSolicitud,
		ObtenerComunicacionesSolicitud,
		InsertarComunicacionesSolicitud,
		ActualizarComunicacionesSolicitud,
		postCargarDocumentosSolicitudCredito,
		actualizarObservaciones,
		obtenerTrazabilidadSolicitud,
		GetConsultarDocumentosPorSubModalidadDodo,
		GetBoteneEnlaceDodo,
		PostGuardarDocumentosDodoVerificados,
		GerObtenerDocumentosVerifiacadosDodo,
		GetObtenerNovedades,
		PostObtenerTiposNovedadPorId,
		PostObtenerTiposCausalId,
		PostObtenerTiposPagosSucesivos,
		PostObtenerTiposInformeInconsistenciasSolicitud,
		EliminarNovedad,
		ActualizarInconsistencias,
		PostObtenerFlujoDodo,
		PostObtenerConceptosAfectadosCliente,
		PostObtenerCuentasAfectadasCliente,
		PostInfoConyuge,
		PostConsultaRadicadoSolicitud,
		PostInconsistenciaDescripcionSolicitud,
		PostBuscarUsuarioIngreso,
		PostBuscarSolicitante,
		GetConsultarIntervinientesActores,
		PostGuadarTipoActor,
		PostObtenerTipoActorById,
		PostBuscarIntervinientesActores,
		PosttActualizarIntervinientes,
		PostEliminarIntervinientePorId,
		PostListaTramitesIntervinientes,
		ObtenerRespuestaSolicitud,
		ObtenerRespuestaSolicitudInconsistente,
		CrearRadicadoCartaIncosistencia,
		PostConsultaReporteSolcitudesGeneral,
		GetObtenerHistorialCambiosIntervinientes,
		PostConsultarIntervinientePorDocumentoAfiliado,
		PostObtenerPdfFup
	};
};

export default SolicitudApi;
