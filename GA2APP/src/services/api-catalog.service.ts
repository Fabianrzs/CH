import axios from 'axios';
import StorageService from './storage.service';
import { routega2api, routebuaapi } from '../utils/baserouteapi.route';
import { CATEGORIES, FORCEDTO, IDENTIFICATIONTYPEDTO, PORCENTAJEDESCAFILIACIONESDTO } from '../types/catalog.type';

const CatalogApi = () => {
	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'X-Frame-Options': 'sameorigin'
		},
		baseURL: `${routega2api}Catalogos`,
		timeout: 15000,
	});

	const apiAxiosTwo = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
		},
		baseURL: `${routega2api}solicitud`,
		timeout: 15000,
	});


	apiAxios.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	apiAxiosTwo.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	const getActivityStatus = () =>
		apiAxios.get(`obtenerEstadoActividad`).then((res: any) => res.data);

	const getCreditType = () =>
		apiAxios.get(`obtenerTipoCredito`).then((res: any) => res.data);

	const getAllStates = () =>
		apiAxios.get('obtenerdepartamentos').then((res: any) => res.data);

	const getAllCities = () =>
		apiAxios.get('obtenerciudades').then((res: any) => res.data);

	const getCityOfState = (request: string) =>
		apiAxios
			.get('obtenerciduadespordepartamento', {
				params: {
					request,
				},
			})
			.then((res: any) => res.data);

	const getCitiesByDepartment = (idDepartment: number) =>
		apiAxios
			.get(
				`Catalogos/obtenerciduadespordepartamento?request=${idDepartment}`,
				{
					baseURL: routega2api,
				},
			)
			.then((res: any) => res.data);

	const getPhoneTypes = () =>
		apiAxios
			.get('Catalogos/obtenerTiposTelefonos', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getMailTypes = () =>
		apiAxios
			.get('Catalogos/obtenerTiposCorreos', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getCurrencyTypes = () =>
		apiAxios
			.get('Catalogos/obtenerTiposMoneda', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getCategories = () =>
		apiAxios
			.get('Catalogos/obtenercategorias', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getGrados = () =>
		apiAxios
			.get('Catalogos/obtenergrados', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);


	const getCreditLine = (tcr_id: number) =>
		apiAxios
			.get(`Catalogos/ObtenerLineasCredito?IdTipoCredito=${tcr_id}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getPayrollCategoryConcept = async (data: any) =>
		apiAxios
			.post('obtenerConceptoNominaCategoria', data)
			.then((res: any) => res.data);

	const getEmailDomains = () =>
		apiAxios
			.get('Catalogos/ObtenerDominiosCorreo', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAddressTypes = () =>
		apiAxios
			.get('Catalogos/ObtenerTiposDireccion', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getTypeHome = () =>
		apiAxios
			.get('Catalogos/obtenertipodeviviendas', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getEstadoVivienda = () =>
		apiAxios
			.get('Catalogos/obtenerEstadoVivienda', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllEXECUTINGUNITSRequest = () =>
		apiAxios
			.get('Catalogos/obtenerunidadesejecutoras', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllTYPESFILERequest = () =>
		apiAxios
			.get('credito/ObtenerTiposArchivos', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllSTATEFILERequest = () =>
		apiAxios
			.get('ObtenerEstadosArchivo', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAbonoUnico = () =>
		apiAxios
			.get('obtenerabonounico', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getTipoAbono = () =>
		apiAxios
			.get('catalogos/ObtenerTipoAbono', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getTipodeviviendas = () =>
		apiAxios
			.get('Catalogos/obtenertipodeviviendas', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getTipoSubsidio = () =>
		apiAxios
			.get('catalogos/ObtenerTipoSubsidio', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getVivienda = () =>
		apiAxios
			.get('Catalogos/obtenerTiVivienda', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllTypeRequest = (IdTipoActor: number) =>
		apiAxios
			.get(`Catalogos/ObtenerTipoSolicitud?IdTipoActor=${IdTipoActor}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);


	const getAllTypeModality = (idTipoSolicitud: any) =>
		apiAxios
			.get(`Catalogos/ObtenerTipoModalidad?idTipoSolicitud=${idTipoSolicitud}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getEstadoCartera = () =>
		apiAxios
			.get('Catalogos/obtenerEstadoCartera', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);


	const getAllTypeSubModality = (idTipoModalidad: string) =>
		apiAxios
			.get(`Catalogos/ObtenerTipoSubModalidad?IdTipoModalidad=${idTipoModalidad}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllTypeDocuments = () =>
		apiAxios
			.get(`Catalogos/ObtenerTiposIdentificacion`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllCausalesBloqueoCuenta = () =>
		apiAxios
			.get(`ObtenerCausalBloqueoCuentasConceptos`, {
				baseURL: routebuaapi,
			})
			.then((res: any) => res.data);

	const ActualizaBloqueoCuentas = async (data: any) => {
		try {

			return apiAxios
				.post(`InsertarBoqueoCuentaConcecto`, data, {
					baseURL: routebuaapi
				})
				.then(res => res.data);
		} catch {
			return null;
		}
	}

	const CrearNuevaCuenta = async (data: any) => {
		try {

			return apiAxios
				.post(`InsertarNuevasCuentas`, data, {
					baseURL: routebuaapi
				})
				.then(res => res.data);
		} catch {
			return null;
		}
	}

	const getAllTipoCuenta = () =>
		apiAxios
			.get(`Catalogos/ObtenerTipoCuenta`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllEntidadBancaria = () =>
		apiAxios
			.get(`Catalogos/ObtenerEntidadBancaria`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getConsultarInformacionTurno = () =>
		apiAxiosTwo
			.get(`solicitud/ObtenerInformacionTurno`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllEntidadEducativa = () =>
		apiAxios
			.get(`Entidades/ObtenerEntidadEducativa`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllProgramaEducativo = (idEntidad: string) =>
		apiAxios
			.get(`Entidades/ObtenerProgramaEducativo?idEntidad=${idEntidad}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllCatalogoPorId = (catalogoId: number) =>
		apiAxios
			.get(`Catalogos/ObtenerValoresCatalogosPorIdCatalogo?idCatalogo=${catalogoId}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllCatalogoTablasParametros = () =>
		apiAxios
			.get(`Catalogos/ObtenerCategoriasTablasParametros`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getTablasParametrosPorCategoria = async (categoriaID: number) =>
		apiAxios
			.get(`Parametrizacion/ObtenerTablasParametrosPorCategoria?categoriaId=${categoriaID}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postAgregarTablasParametros = async (data: any) =>
		apiAxios
			.post(`Parametrizacion/AgregarTablasParametros`, data, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getEsquemasTablasParametros = async (nombreTabla: string) =>
		apiAxios
			.get(`Catalogos/ObtenerEsquemasTablasParametros?nombreTabla=${nombreTabla}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getDatosTablasParametros = async (nombreTabla: string) =>
		apiAxios
			.get(`Catalogos/ObtenerDatosTablasParametros?nombreTabla=${nombreTabla}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAverageCost = () =>
		apiAxios
			.get(`Catalogos/ObtenerMediosDeEnvio`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getFormat = () =>
		apiAxios
			.get(`Catalogos/ObtenerFormatos`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllEntidaSeguroEducativo = () =>
		apiAxios
			.get(`Catalogos/ObtenerEntidadSeguroEducativo`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postInsertEntidaSeguroEducativo = (entidad: any) =>
	apiAxios
		.post(`Catalogos/InsertarEntidadSeguroEducativo`, entidad,{
			baseURL: routega2api,
		})
		.then((res: any) => res.data);

	const postInsertTypeId = (typeId: IDENTIFICATIONTYPEDTO) =>
		apiAxios
			.post(`catalogos/CrearDocumentoTipo`, typeId, { baseURL: routega2api })
			.then((res: any) => res.data);

	const UpdateTypeId = (typeId: IDENTIFICATIONTYPEDTO) =>
		apiAxios
			.patch(`catalogos/ActualizarDocumentoTipo`, typeId, { baseURL: routega2api })
			.then(res => res.data);

	const getAllGeneros = () =>
		apiAxios
			.get(`Catalogos/ObtenerTipoSexo`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postInsertTypeGenero = (sextype: any) =>
		apiAxios
			.post(`Catalogos/CrearSexo`, sextype, { baseURL: routega2api })
			.then((res: any) => res.data);

	const updateTypeGenero = (sextype: any) =>
		apiAxios
			.patch(`Catalogos/ActualizarSexo`, sextype, { baseURL: routega2api })
			.then(res => res.data);

	const getAllForces = () =>
		apiAxios
			.get(`Catalogos/obtenerfuerzas`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postInsertForce = (force: FORCEDTO) =>
		apiAxios
			.post(`Catalogos/CrearFuerza`, force, { baseURL: routega2api })
			.then((res: any) => res.data);

	const updateForce = (force: FORCEDTO) =>
		apiAxios
			.patch(`Catalogos/ActualizarFuerza`, force, { baseURL: routega2api })
			.then(res => res.data);

	const getTypeAffiliate = () =>
		apiAxios
			.get('Catalogos/obtenerTiposAfiliacion', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getAffiliateState = () =>
		apiAxios
			.get('Catalogos/obtenerestadosafiliacion', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getQualityField = () =>
		apiAxios
			.get('Catalogos/obtenerCampoCalidadAfiliaciones', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getAffiliateTypeProcedure = () =>
		apiAxios
			.get('Catalogos/obtenertiposafiliacionprocedente', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getAllCategories = () =>
		apiAxios
			.get(`Catalogos/obtenercategorias`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postInsertCategory = (category: CATEGORIES) =>
		apiAxios
			.post(`Catalogos/CrearCategoria`, category, { baseURL: routega2api })
			.then((res: any) => res.data);

	const updateCategory = (category: CATEGORIES) =>
		apiAxios
			.patch(`Catalogos/ActualizarCategoria`, category, { baseURL: routega2api })
			.then(res => res.data);

	const getDiscountPercentage = () =>
		apiAxios
			.get(`Catalogos/ObtenerPorcentajesDescuento`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const postInsertDiscountPercentage = (DiscountPercentage: PORCENTAJEDESCAFILIACIONESDTO) =>
		apiAxios
			.post(`Catalogos/InsertarPorcentajesDescuento`, DiscountPercentage, { baseURL: routega2api })
			.then((res: any) => res.data);


	const getAllGroupInconsistencies = () =>
		apiAxios
			.get(`catalogos/ListarGruposInconsistencia`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getInconsistenciesByGroup = (grupo: number) =>
		apiAxios
			.get(`catalogos/ObtenerInconsistencia?GrupoInconsistencia=${grupo}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllToolsInconsistencies = () =>
		apiAxios
			.get(`catalogos/ObtenerHerramientaInconsistencia`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllPointsAttention = () =>
		apiAxios
			.get(`catalogos/ObtenerPuntosAtencionInconsistencia`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllRequestState = () =>
		apiAxios
			.get(`catalogos/obtenerEstadoTareaSolicitud`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getRegistrationOffice = () =>
		apiAxios
			.get('Catalogos/ObtenerOficinasDeRegistro', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getCausalBlockage = () =>
		apiAxios
			.get('Catalogos/obtenerCausalBloqueo', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getRequestTypeRealStateRegistration = () =>
		apiAxios
			.get('Catalogos/ObtenerTipoSolicitudNovedadMatricula', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getAllProgramaEducativoPorEntidad = (idEntidad: string) =>
		apiAxios
			.get(`Entidades/ObtenerProgramaEducativoPorEntidad?idEntidad=${idEntidad}`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAllNivelEducativo = () =>
		apiAxios
			.get(`catalogos/ObtenerNivelEducativo`, {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const GetModalidadAfiliacionVeterano = () =>
		apiAxios
			.get('Catalogos/obtenerModalidadAfiliacion', {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const ObtenerTipoSolicitudDesafiliacion = () =>
		apiAxios
			.get('Catalogos/ObtenerTipoSolicitudDesafiliacion', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const ObtenerFormatoAAplicar = () =>
		apiAxios
			.get('Catalogos/ObtenerFormatoAAplicar', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const ObtenerEstratos = () =>
		apiAxios
			.get('catalogos/ObtenerEstrato', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getAllLIVINGPLACESRequest = () =>
		apiAxios
			.get('catalogos/obtenertipodeviviendas', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getGetAllCivilStatesRequest = () =>
		apiAxios
			.get('catalogos/obtenerEstadosCiviles', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getAllCountriesRequest = () =>
		apiAxios
			.get('catalogos/obtenerpaises', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getAllEconomicActivitiesRequest = () =>
		apiAxios
			.get('catalogos/obtenerActividadesEconomicas', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getAllProfessionsRequest = () =>
		apiAxios
			.get('catalogos/obtenerProfesiones', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const obtenerTiVivienda = () =>
		apiAxios
			.get('catalogos/obtenerTiVivienda', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getAllEstadoViviendaRequest = () =>
		apiAxios
			.get('catalogos/obtenerEstadoVivienda', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getObtenerConceptoNomina = () =>
		apiAxios
			.get('ObtenerConceptoNomina', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);
	const getObtenerConceptoNominaParam = () =>
		apiAxios
			.get('ObtenerConceptoNominaParam', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getCausalesBloqueoCuenta = () =>
		apiAxios
			.get('catalogos/ObtenerCausalesBloqueoCuenta', {
				baseURL: routega2api,
			})
			.then(res => res.data)

	const getObtenerValoresCatalogosPorIdCatalogo = (idCatalogo: number) =>
		apiAxios
			.get(`ObtenerValoresCatalogosPorIdCatalogo?idCatalogo=${idCatalogo}`)
			.then((res: any) => res.data);

	const getValoresCatalogoPorNombre = (nombreCatalogo: string) =>
		apiAxios
			.get(`Catalogos/ObtenerValoresCatalogoPorNombre?nombreCatalogo=${nombreCatalogo}`, {
				baseURL: routega2api,
			})
			.then(res => res.data)
	const obtenerConceptos = () =>
		apiAxios
			.get(`ObtenerConceptos`)
			.then((res: any) => res.data);

	const postDeleteGenero = (id: number) =>
		apiAxios
			.post(`Catalogos/EliminarTipoDeSexo?id=${id}`, {}, { baseURL: routega2api })
			.then((res: any) => res.data);

	const postDeleteTipoDocumento = (id: number) =>
		apiAxios
			.post(`Catalogos/EliminarTiposIdentificacion?id=${id}`, {}, { baseURL: routega2api })
			.then((res: any) => res.data);

	const postDeleteFuerza = (id: number) =>
		apiAxios
			.post(`Catalogos/EliminarFuerza?id=${id}`, {}, { baseURL: routega2api })
			.then((res: any) => res.data);

	const postDeleteCategoria = (id: number) =>
		apiAxios
			.post(`Catalogos/EliminarCategorias?id=${id}`, {}, { baseURL: routega2api })
			.then((res: any) => res.data);


	const ObtenerConceptosNovedades = () =>
		apiAxios
			.get(`ObtenerConceptosNovedades`)
			.then((res: any) => res.data);

	const obtenerTipoActorEmbargo = () =>
		apiAxios
			.get(`ObtenerTipoDeActorEmbargo`)
			.then((res: any) => res.data);

	const getTipoConceptosReporte = () =>
		apiAxios
			.get('catalogos/ObtenerUnidadEjecutoraReporte', {
				baseURL: routega2api,
			})
			.then((res: any) => res.data);

	const getAlternativosAplicacionPagos = () =>
		apiAxios
			.get(`ObtenerAlternativosAplicacionPago`, {
				baseURL: routega2api,
			})
			.then(res => res.data);


	const getEstadosCliente = () =>
		apiAxios
			.get(`solicitud/ObtenerEstadosCliente`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const ObtenerTipoReporteGA2 = () =>
		apiAxios
			.get(`Catalogos/ObtenerTipoReporteGA2`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getBeneficiarioCartera = () =>
		apiAxios
			.get(`Catalogos/ObtenerBeneficiarioCartera`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getConceptosCartera = () =>
		apiAxios
			.get(`Catalogos/ObtenerConceptosCartera`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getTipoComprobante = () =>
		apiAxios
			.get(`Catalogos/ObtenerTipoComprobanteCartera`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const getObtenerIpLocal = (item:any) =>
		apiAxios
			.get(`Catalogos/ObtenerIpLocal?hexadecimal=${item}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const postInsertarConceptoNomina = (Datos: any) =>
		apiAxios
			.post(`Catalogos/InsertarConceptoNomina`, Datos, { baseURL: routega2api })
			.then((res: any) => res.data);
	const putActualizarConceptoNomina = (Datos: any) =>
		apiAxios
			.put(`Catalogos/ActualizarConceptoNomina`, Datos, { baseURL: routega2api })
			.then((res: any) => res.data);
	

	return {
		getAllCausalesBloqueoCuenta,
		getActivityStatus,
		getCreditType,
		getCitiesByDepartment,
		getAllStates,
		getAllCities,
		getCityOfState,
		getPhoneTypes,
		getMailTypes,
		getCurrencyTypes,
		getCategories,
		getPayrollCategoryConcept,
		getEmailDomains,
		getAddressTypes,
		getTypeHome,
		getAbonoUnico,
		getTipoAbono,
		getTipoSubsidio,
		getEstadoVivienda,
		getTipodeviviendas,
		getVivienda,
		getAllTypeRequest,
		getAllTypeModality,
		getAllTypeSubModality,
		getEstadoCartera,
		getAllTypeDocuments,
		getAllTipoCuenta,
		getAllEntidadBancaria,
		getConsultarInformacionTurno,
		getAllCatalogoPorId,
		getAllCatalogoTablasParametros,
		getTablasParametrosPorCategoria,
		postAgregarTablasParametros,
		getAllProgramaEducativo,
		getAllEntidadEducativa,
		getAverageCost,
		getFormat,
		getAllEntidaSeguroEducativo,
		postInsertEntidaSeguroEducativo,
		postInsertTypeId,
		UpdateTypeId,
		getAllGeneros,
		postInsertTypeGenero,
		updateTypeGenero,
		getAllForces,
		postInsertForce,
		updateForce,
		getTypeAffiliate,
		getAffiliateState,
		getQualityField,
		getAffiliateTypeProcedure,
		getAllCategories,
		postInsertCategory,
		updateCategory,
		getDiscountPercentage,
		postInsertDiscountPercentage,
		getRegistrationOffice,
		getCausalBlockage,
		getRequestTypeRealStateRegistration,
		ActualizaBloqueoCuentas,
		getAllGroupInconsistencies,
		getInconsistenciesByGroup,
		getAllToolsInconsistencies,
		getAllPointsAttention,
		getAllRequestState,
		CrearNuevaCuenta,
		getAllProgramaEducativoPorEntidad,
		getAllNivelEducativo,
		GetModalidadAfiliacionVeterano,
		ObtenerTipoSolicitudDesafiliacion,
		ObtenerFormatoAAplicar,
		ObtenerEstratos,
		getGrados,
		getCreditLine,
		getAllEXECUTINGUNITSRequest,
		getAllLIVINGPLACESRequest,
		getGetAllCivilStatesRequest,
		getAllCountriesRequest,
		getAllEconomicActivitiesRequest,
		getAllProfessionsRequest,
		obtenerTiVivienda,
		getAllEstadoViviendaRequest,
		getCausalesBloqueoCuenta,
		getObtenerConceptoNomina,
		getObtenerValoresCatalogosPorIdCatalogo,
		getValoresCatalogoPorNombre,
		obtenerConceptos,
		postDeleteGenero,
		postDeleteTipoDocumento,
		postDeleteFuerza,
		ObtenerConceptosNovedades,
		postDeleteCategoria,
		obtenerTipoActorEmbargo,
		getEsquemasTablasParametros,
		getDatosTablasParametros,
		getTipoConceptosReporte,
		getAlternativosAplicacionPagos,
		getEstadosCliente,
		ObtenerTipoReporteGA2,
		getAllTYPESFILERequest,
		getAllSTATEFILERequest,
		getBeneficiarioCartera,
		getConceptosCartera,
		getTipoComprobante,
		getObtenerIpLocal,
		postInsertarConceptoNomina,
		putActualizarConceptoNomina,
		getObtenerConceptoNominaParam
	};
};

export default CatalogApi;
