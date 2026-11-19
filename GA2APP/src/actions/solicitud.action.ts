import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { CreateSeveranceModel } from "../models/requests/create-severance.model";
import { ResponseSeveranceModel } from "../models/requests/response-severance.model";
import { AppActions } from "../types/appactions.type";

import {
	INITIAL_POST_CREATE_SEVERANCE,
	POST_CREATE_SEVERANCE,
	ON_ERROR,
	INITIAL_GET_CONSULTAR_SOLICITUD,
	POST_CONSULTAR_SOLICITUD,
	CLEAR_DETAIL_SOLICITUD,
	ACTUALIZAR_SOLICITUD,
	INITIAL_POST_SOLICITUD_TAREA,
	POST_SOLICITUD_TAREA,
	POST_CONSULTAR_SOLICITUDES,
	GET_CONSULTAR_FORMULARIOS_SOLICITUD
} from "../types/solicitud.actions.type";
import SolicitudApi from "../services/api-solicitud.service";
import AuthService from '../services/auth.service';
import { ConsultarSolicitudModel } from '../models/requests/consultar-solicitud.model';
import { FormulariosSolicitudDto, ParametrosBusqueda, SolicitudDto, SolicitudTareaModel } from '../models/requests/solicitud-tarea.model';
import NotificationService from '../services/notification.service';

const api = SolicitudApi(); 

export const SetError = (error: any): AppActions => ({
	type: ON_ERROR,
	error: error,
});

export const actualizarSolicitud = (): AppActions => ({
	type: ACTUALIZAR_SOLICITUD,
});

export const clearDetailSolicitud = (): AppActions => ({
	type: CLEAR_DETAIL_SOLICITUD,
});

const InitialPostCreateSeverance = (): AppActions => ({
	type: INITIAL_POST_CREATE_SEVERANCE,
});

export const PostCreateSeverance = (responseSeveranceModel: ResponseSeveranceModel): AppActions => ({
	type: POST_CREATE_SEVERANCE,
	responseSeveranceModel,
});

export const startPostCreateSeverance =
	(createSeveranceModel: CreateSeveranceModel) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const user = AuthService.getUserInfo();
			createSeveranceModel.creadoPor = user.USR_ID;
			dispatch(InitialPostCreateSeverance());
			const response = await api.posCreateRequestSeverance(createSeveranceModel);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(PostCreateSeverance(response.data));
			}
		} catch (e) {
			console.error(e);
			dispatch(SetError('server-error'));
		}
	};

const InitialGetConsultarSolicitud = (): AppActions => ({
	type: INITIAL_GET_CONSULTAR_SOLICITUD,
});

export const GetConsultarSolicitud = (consultarSolicitudModel: ConsultarSolicitudModel): AppActions => ({
	type: POST_CONSULTAR_SOLICITUD,
	consultarSolicitudModel,
});

export const startGetConsultarSolicitud =
	(consultarSolicitudModel: ConsultarSolicitudModel) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(InitialGetConsultarSolicitud());
			const response = await api.GetConsultarSolucitud(consultarSolicitudModel);
			if (response.isSuccess === false) {
				dispatch(SetError(response));
			} else {
				dispatch(GetConsultarSolicitud(response.data));
			}
		} catch (e) {
			console.error(e);
			dispatch(SetError('server-error'));
		}
	};

const InitialPostRegistroTarea = (): AppActions => ({
	type: INITIAL_POST_SOLICITUD_TAREA,
});

export const PostRegistroTarea = (solicitudTareaModel: SolicitudTareaModel): AppActions => ({
	type: POST_SOLICITUD_TAREA,
	solicitudTareaModel,
});

export const startPostSolicitudTarea =
	(solicitudTareaModel: SolicitudTareaModel) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const user =  AuthService.getUserInfo();
			solicitudTareaModel.creadoPor = user.USR_ID;

			 dispatch(InitialPostRegistroTarea());
			const response = await api.PostSolicitudTarea(solicitudTareaModel);
			if (response.isSuccess === false) {
				dispatch(SetError(response));
			} else {
				dispatch(PostRegistroTarea(response.data));
			}
		} catch (e) {
			console.error(e);
			dispatch(SetError('server-error'));
		}
	};

	// export const startGetAllTypeModality =
	// (idTipoSolicitud: string) => async (dispatch: Dispatch<AppActions>) => {
	// 	try {
	// 		const response = await api.getAllTypeModality(idTipoSolicitud);
	// 		if (response?.isSuccess == null) {
	// 			dispatch(SetError(response));
	// 		} else {
	// 			dispatch(GetAllTypeModality(response.data));
	// 		}
	// 	} catch (e) {
			
	// 		dispatch(SetError('server-error'));
	// 	}
	// };

export const PostConsultaSolicitudesSuccess = (solicitudes: SolicitudDto[]): AppActions => ({
	type: POST_CONSULTAR_SOLICITUDES,
	solicitudes,
});


export const startPostConsultarSolicitudes =
 (search: ParametrosBusqueda, mostrarMensaje: string) => async (dispatch: Dispatch<AppActions>) => {

		try {
			dispatch(PostConsultaSolicitudesSuccess([]));
			dispatch(InitialPostRegistroTarea());
			const response = await api.PostConsultarSolicitudes(search);
			if (response?.isSuccess === false) {
				dispatch(SetError(response));
			} else {
				dispatch(PostConsultaSolicitudesSuccess(response.data));
				if(mostrarMensaje === 'SI' && (!response.data || response.data.length === 0)){
					toast(`No se encontraron resultados`, { type: toast.TYPE.ERROR });
				}
			}
		} catch (e) {
			dispatch(SetError('server-error'));
		}
	};
	

export const startClearSolicitudes =
	() => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(clearDetailSolicitud());
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetConsultarFormulariosSolicitudSuccess =
	(formulariosSolicitud: FormulariosSolicitudDto): AppActions => ({
		type: GET_CONSULTAR_FORMULARIOS_SOLICITUD,
		formulariosSolicitud: formulariosSolicitud,
});

export const getFormulariosSolicitud =
	(timCodigo: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.GetFormulariosSolicitud(timCodigo);
			if (!response.isSuccess) {
				dispatch(SetError(response));
			} else {
				dispatch(GetConsultarFormulariosSolicitudSuccess(response.data));
			}
		} catch (e) {
			console.error(e);
			dispatch(SetError('server-error'));
		}
	}

export const documentosAgregados = (documentos: any) => ({
		type: "AGREGAR_DOCUMENTOS",
		payload: documentos,
	});
	
