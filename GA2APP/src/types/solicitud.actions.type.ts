import { ConsultarSolicitudModel } from "../models/requests/consultar-solicitud.model";
import { ResponseSeveranceModel } from "../models/requests/response-severance.model";
import { FormulariosSolicitudDto, SolicitudDto, SolicitudTareaModel } from "../models/requests/solicitud-tarea.model";

export const ON_ERROR = 'ON_ERROR';

export const CLEAR_DETAIL_SOLICITUD = 'CLEAR_DETAIL_SOLICITUD';

export const ACTUALIZAR_SOLICITUD = 'ACTUALIZAR_SOLICITUD';

export const INITIAL_POST_CREATE_SEVERANCE = 'INITIAL_POST_CREATE_SEVERANCE';
export const POST_CREATE_SEVERANCE = 'POST_CREATE_SEVERANCE';

export const INITIAL_GET_CONSULTAR_SOLICITUD = 'INITIAL_GET_CONSULTAR_SOLICITUD';
export const POST_CONSULTAR_SOLICITUD = 'POST_CONSULTAR_SOLICITUD';

export const INITIAL_POST_SOLICITUD_TAREA = 'INITIAL_POST_SOLICITUD_TAREA';
export const POST_SOLICITUD_TAREA = 'POST_SOLICITUD_TAREA';

export const POST_CONSULTAR_SOLICITUDES = 'POST_CONSULTAR_SOLICITUDES';

export const GET_CONSULTAR_FORMULARIOS_SOLICITUD = 'GET_CONSULTAR_FORMULARIOS_SOLICITUD';
export interface OnError {
	type: 'ON_ERROR';
	error: any;
}

export interface actualizarSolicitud {
	type: 'ACTUALIZAR_SOLICITUD';
}

export interface clearDetailSolicitud {
	type: 'CLEAR_DETAIL_SOLICITUD';
}

export interface InitialPostCreateSeverance {
	type: 'INITIAL_POST_CREATE_SEVERANCE';
}

export interface PostCreateSeverance {
	type: 'POST_CREATE_SEVERANCE';
	responseSeveranceModel: ResponseSeveranceModel;
}

export interface InitialGetConsultarSolicitud {
	type: 'INITIAL_GET_CONSULTAR_SOLICITUD';
}

export interface GetConsultarSolicitud {
	type: 'POST_CONSULTAR_SOLICITUD';
	consultarSolicitudModel: ConsultarSolicitudModel;
}

export interface InitialPostRegistroTarea {
	type: 'INITIAL_POST_SOLICITUD_TAREA';
}

export interface PostRegistroTarea {
	type: 'POST_SOLICITUD_TAREA';
	solicitudTareaModel: SolicitudTareaModel;
}

export interface PostConsultarSolicitudesSuccess {
	type: 'POST_CONSULTAR_SOLICITUDES';
	solicitudes: SolicitudDto[];
}

export interface GetConsultarFormulariosSolicitudSuccess {
	type: 'GET_CONSULTAR_FORMULARIOS_SOLICITUD';
	formulariosSolicitud: FormulariosSolicitudDto;
}

export type SolicitudActionsType =
	| InitialPostCreateSeverance
	| PostCreateSeverance
	| InitialGetConsultarSolicitud
	| GetConsultarSolicitud
	| clearDetailSolicitud
	| actualizarSolicitud
	| InitialPostRegistroTarea
	| PostRegistroTarea
	| PostConsultarSolicitudesSuccess
	| GetConsultarFormulariosSolicitudSuccess
	| OnError;
