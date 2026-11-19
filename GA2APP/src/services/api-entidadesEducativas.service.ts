import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import {
	BloqueosEntidadesEducativas,
	CreateAnotacion,
	CuentaEntidadesEducativas,
	EntidadesEducativasAgregar,
	EntidadesEducativasFiltro,
	EntidadesEducativasFiltroRequest,
	Fidecomiso,
	ProgramaEducativo,
	ProgramasEducativos,
	SedeEntidadEducativa,
} from '../types/educational.entities.type';
import { DocumentosEliminarModel } from '../models/requests/documento-eliminar.model';

const ApiEntidadesEducativas = () => {
	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'X-Frame-Options': 'sameorigin'
		},
		baseURL: `${routega2api}`,
	});

	apiAxios.interceptors.request.use(async (config: any) => {
		const token = StorageService.getSessionToken();
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	});

	const getEntidadesEducativasFiltroRequest = (
		entidadEducativa: EntidadesEducativasFiltroRequest,
	) =>
		apiAxios
			.post(
				'Entidades/ObtenerEntidadEducativaPorNitNombre',
				entidadEducativa,
				{
					baseURL: routega2api,
				},
			)
			.then(res => res.data);

	const updateEntidadEducativa = (
		entidadEducativa: EntidadesEducativasFiltro,
	) =>
		apiAxios
			.post('Entidades/ActualizarEntidadEducativa', entidadEducativa, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const GetSedesEntidadEducativa = (idEntidadEducativa: string) =>
		apiAxios
			.get(
				`Entidades/ObtenerSedesPorEntidadEducativa?request=${idEntidadEducativa}`,
				{
					baseURL: routega2api,
				},
			)
			.then(res => res.data);

	const GetProgramasEntidadEducativa = (idEntidadEducativa: string) =>
		apiAxios
			.get(
				`Entidades/ObtenerProgramaEducativoPorEntidad?idEntidad=${idEntidadEducativa.toUpperCase()}`,
				{
					baseURL: routega2api,
				},
			)
			.then(res => res.data);

	const GetBloqueos = (idEntidadEducativa: string) =>
		apiAxios
			.get(
				`Entidades/ObtenerBloqueoEntidadEducativa?idEntidad=${idEntidadEducativa.toUpperCase()}`,
				{
					baseURL: routega2api,
				},
			)
			.then(res => res.data);
	const lockEntidades = (entidadEducativa: BloqueosEntidadesEducativas) =>

		apiAxios
			.post('Entidades/BloqueoEntidadEducativa', entidadEducativa, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const unlockEntidades = (entidadEducativa: BloqueosEntidadesEducativas) =>
		apiAxios
			.post('Entidades/DesbloqueoEntidadEducativa', entidadEducativa, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const GetCuentas = (idEntidadEducativa: string) =>
		apiAxios
			.get(
				`Entidades/ObtenerCuentaBancariaPorEntidad?idEntidad=${idEntidadEducativa}`,
				{
					baseURL: routega2api,
				},
			)

			.then(res => res.data);

	const startGetTramiteEntity = (idEntidadEducativa: string) =>
		apiAxios
			.get(
				`Entidades/GetDataTramiteEntidades?idEntidad=${idEntidadEducativa}`,
				{
					baseURL: routega2api,
				},
			)
			.then(res => res.data);

	const AddCuenta = (cuentaEntidad: CuentaEntidadesEducativas) =>
		apiAxios
			.post('Entidades/CrearCuentaBancariaEntidadEducativa', cuentaEntidad, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const UpdateCuenta = (cuentaEntidad: CuentaEntidadesEducativas) =>
		apiAxios
			.put('Entidades/ActualizarCuentaBancariaEntidadEducativa', cuentaEntidad, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const DeleteCuenta = (idCuentaEntidad: string) =>
		apiAxios
			.delete(`Entidades/EliminarCuentaBancariaEntidadEducativa?idCuentaEntidad=${idCuentaEntidad}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const AddEducationalEntities = (
		AgregarEntidadEducativa: EntidadesEducativasAgregar | null,
	) =>
		apiAxios
			.post('Entidades/CrearEntidadEducativa', AgregarEntidadEducativa, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const DeleteEntidadEducativa = (idEntidad: string) =>
		apiAxios
			.delete(`Entidades/EliminarEntidadEducativa?idEntidad=${idEntidad}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const ReportEntidadEducativa = (
		AgregarEntidadEducativa: EntidadesEducativasFiltro | null,
	) =>
		apiAxios
			.post('Reportes/ReporteEntidades', AgregarEntidadEducativa, {
				responseType: 'blob'
			})
			.then(res => res.data);

	const AddProgram = (program: ProgramasEducativos | null) =>
		apiAxios
			.post('Entidades/CrearProgramaEducativo', program, {
				baseURL: routega2api,
			})

			.then(res => res.data);

	const DeleteProgram = (request: any) =>
		apiAxios
			.post(`Entidades/EliminarProgramaEducativoPorId`, request, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const UpdateProgram = (program: ProgramaEducativo) =>
		apiAxios
			.put('Entidades/ActualizarProgramaEducativo', program, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const AddFidecomiso = (program: Fidecomiso, id: any) =>
		apiAxios
			.post('Entidades/CrearFideicomisoEntidadEducativa', program, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const UpdateFidecomiso = (program: Fidecomiso, id: any) =>
		apiAxios
			.post('Entidades/ActualizarFideicomisoEntidadEducativa', program, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const DeleteFidecomiso = (request: any) =>
		apiAxios
			.post(`Entidades/EliminarFideicomiso`,request , {
				baseURL: routega2api,
			})
			.then(res => res.data);
			
	const AddCampus = (campus: SedeEntidadEducativa | null) =>
		apiAxios
			.post('Entidades/CrearSedeEntidadEducativa', campus, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const UpdateCampus = (campus: SedeEntidadEducativa | null) =>
		apiAxios
			.put('Entidades/ActualizarSedeEntidadEducativa', campus, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const DeleteCampus = (request: any) =>
		apiAxios
			.post(`Entidades/EliminarSedesEntidadEducativaPorId`, request, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const GetEducationalEntityDetail = (idEntidad: string) =>
		apiAxios
			.get(`Entidades/ObtenerEntidadEducativaDetalle?idEntidad=${idEntidad}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);

	const GetFilesByEntity = (idEntidad: string) =>
		apiAxios
			.get(`Entidades/ConsultarArchivosPorEntidad?idEntidad=${idEntidad}`, {
				baseURL: routega2api,
			})
			.then(res => res.data);
	const GetDescargarArchivoPorId = async (rutaStorage: string) => {
		const datos = await apiAxios
			.get(`Entidades/DescargarArchivoPorId?rutaStorage=${rutaStorage}`, {
				responseType: 'blob'
			})
			.then((res: any) => res.data);
		return datos;
	}

	const PostEliminarDocumento = (documentoEliminar: DocumentosEliminarModel) =>
		apiAxios
			.post(`Entidades/EliminarArchivoEntidad`, documentoEliminar)
			.then((res: any) => res.data);

	const PostUploadDocuments = (data: FormData, idEntidad: string) =>
		apiAxios
			.post(`Entidades/CargarDocumentosEntidad/${idEntidad}`, data)
			.then((res: any) => res.data);

	const PostCrearHistorialEntidad = (idEntidadCambio: any) =>
		apiAxios
			.post('Entidades/CrearHistorialEntidad', idEntidadCambio, {
				baseURL: routega2api,
			}).then(res => res.data);

	const PostCrearAnotacion = (data: CreateAnotacion) =>
		apiAxios
			.post('Entidades/CrearAnotacion', data, {
				baseURL: routega2api,
			}).then(res => res.data);

	const PostConsultarAnotacion = (request: any) =>
		apiAxios
			.post(`Entidades/ConsultarAnotaciones?request=${request}`
			).then(res => res.data);

	const GetChangeHistory = (idEntidad: string) =>
	apiAxios
		.get(`Entidades/ObtenerHistorialCambios?idEntidad=${idEntidad}`, {
			baseURL: routega2api,
		})
		.then(res => res.data);

	return {
		getEntidadesEducativasFiltroRequest,
		updateEntidadEducativa,
		GetSedesEntidadEducativa,
		GetProgramasEntidadEducativa,
		GetBloqueos,
		GetCuentas,
		AddCuenta,
		UpdateCuenta,
		DeleteCuenta,
		lockEntidades,
		unlockEntidades,
		AddEducationalEntities,
		AddProgram,
		DeleteProgram,
		UpdateProgram,
		AddFidecomiso,
		UpdateFidecomiso,
		DeleteFidecomiso,
		AddCampus,
		UpdateCampus,
		DeleteCampus,
		GetEducationalEntityDetail,
		DeleteEntidadEducativa,
		GetFilesByEntity,
		GetDescargarArchivoPorId,
		PostUploadDocuments,
		PostEliminarDocumento,
		ReportEntidadEducativa,
		startGetTramiteEntity,
		PostCrearHistorialEntidad,
		PostCrearAnotacion,
		PostConsultarAnotacion,
		GetChangeHistory
	};
};

export default ApiEntidadesEducativas;
