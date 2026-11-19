import {
	POST_CREATE_SEVERANCE,
	SolicitudActionsType,
	INITIAL_POST_CREATE_SEVERANCE,
	CLEAR_DETAIL_SOLICITUD,
	INITIAL_GET_CONSULTAR_SOLICITUD,
	POST_CONSULTAR_SOLICITUD,
	ON_ERROR,
	ACTUALIZAR_SOLICITUD,
	INITIAL_POST_SOLICITUD_TAREA,
	POST_SOLICITUD_TAREA,
	POST_CONSULTAR_SOLICITUDES,
	GET_CONSULTAR_FORMULARIOS_SOLICITUD
} from "../types/solicitud.actions.type";
import { ResponseSeveranceModel } from '../models/requests/response-severance.model';
import { ConsultarSolicitudModel } from "../models/requests/consultar-solicitud.model";
import { FormulariosSolicitudDto, SolicitudDto, SolicitudTareaModel } from "../models/requests/solicitud-tarea.model";

export interface SolicitudState {
	loading: boolean;
	error: any;
	responseSeveranceModel: ResponseSeveranceModel | any;
	consultarSolicitudModel: ConsultarSolicitudModel | any;
	solicitudTareaModel: SolicitudTareaModel | any;
	solicitudes: SolicitudDto[] | null;
	formulariosSolicitud: FormulariosSolicitudDto | any;
}

const DetailInitialState = {
	error: null,
	loading: false,
	responseSeveranceModel: null,
	consultarSolicitudModel: null,
	solicitudTareaModel: null,
	solicitudes: null,
	formulariosSolicitud: null
};

const InitialState: SolicitudState = {
	...DetailInitialState,
};

const SolicitudReducer = (
	state: SolicitudState = InitialState,
	action: SolicitudActionsType,
): SolicitudState => {
	
	switch (action.type) {

		
		case ON_ERROR:
			return {
				...state,
				loading: false,
				error: action.error,
			};
		case ACTUALIZAR_SOLICITUD:
			return {
				...state
			}
		case CLEAR_DETAIL_SOLICITUD:
			return {
				...state,
				...DetailInitialState
			}
		case INITIAL_GET_CONSULTAR_SOLICITUD:
			return DetailInitialState
		case INITIAL_POST_CREATE_SEVERANCE:
		case INITIAL_POST_SOLICITUD_TAREA:
			return {
				...state,
				loading: true,
				error: null,
			};

		case POST_CONSULTAR_SOLICITUD:
			return {
				...state,
				loading: false,
				error: null,
				consultarSolicitudModel: action.consultarSolicitudModel,
			};
		case POST_CREATE_SEVERANCE:
			return {
				...state,
				loading: false,
				responseSeveranceModel: action.responseSeveranceModel,
			}
		case POST_SOLICITUD_TAREA:
			return {
				...state,
				loading: false,
				solicitudTareaModel: action.solicitudTareaModel,
			}
		case POST_CONSULTAR_SOLICITUDES:
			return {
				...state,
				loading: false,
				solicitudes: action.solicitudes
			};
		case GET_CONSULTAR_FORMULARIOS_SOLICITUD:
			return {
				...state,
				loading: false,
				formulariosSolicitud: action.formulariosSolicitud
			};
		default:
			return state;
	}
};

export default SolicitudReducer;
