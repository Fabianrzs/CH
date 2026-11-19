import { ObtenerDesembargoDto, DesembargoDto, TipoProcesoDesembargoDto } from "../types/desembargo.model.type";
import { 
    DesembargosType,
    ON_ERROR,
    GET_ALL_TIPOS_DESEMBARGOS_SUCCESS,
    GET_ALL_TIPOS_PROCESOS_DESEMBARGOS_SUCCESS,
    GET_ALL_DESEMBARGOS_CUENTA_SUCCESS,
    GET_ALL_DESEMBARGOS_SUCCESS,
    POST_ALL_DESEMBARGOS_BY_DATE_SUCCESS,
    GET_ALL_UPDATE_DESEMBARGOS_SUCCESS,
    POST_INSERT_DESEMBARGOS_SUCCESS,
    POST_UPDATE_DESEMBARGOS_SUCCESS,
    DELETE_DESEMBARGOS_SUCCESS,
    GET_ALL_DESEMBARGOS_CUENTA_CONCEPTOS_SUCCESS,
    POST_INSERT_DESEMBARGOS_CONCEPTO_SUCCESS,
    POST_UPDATE_DESEMBARGOS_CONCEPTO_SUCCESS,
    DELETE_DESEMBARGOS_CONCEPTO_SUCCESS, 
    POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS,
    GET_ALL_CONCEPTOS_SUCCESS
} from "../types/desembargos.actions.type";

export interface DesembargosState {
    desembargos: DesembargoDto [];
    desembargo: DesembargoDto | null;
    tiposDesembargo: DesembargoDto [];
    tiposProcesos: TipoProcesoDesembargoDto [];
    desembargosConcepto: ObtenerDesembargoDto [];
    desembargoConcepto: ObtenerDesembargoDto | null;
    conceptos: any;
    loading: boolean;
    error: any;
}

const DesembargosStateInitial: DesembargosState = {
    desembargos: [],
    desembargo: null,
    tiposDesembargo: [],
    tiposProcesos: [],
    desembargosConcepto: [],
    desembargoConcepto: null,
    conceptos: {},
    loading: false,
    error: null
}

export const DesembargosReducer = (
    state: DesembargosState = DesembargosStateInitial,
    action: DesembargosType
): DesembargosState => {
    switch (action.type){
        case GET_ALL_TIPOS_DESEMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                tiposDesembargo: action.tiposDesembargo,
            };
        break;
        case GET_ALL_TIPOS_PROCESOS_DESEMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                tiposProcesos: action.tiposProcesos,
            };
        break;
        case GET_ALL_DESEMBARGOS_CUENTA_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargosConcepto: action.desembargosConcepto,
            };
        break;
        case GET_ALL_DESEMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargos: action.desembargos,
            };
        break;
        case POST_ALL_DESEMBARGOS_BY_DATE_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargos: action.desembargos,
            };
        break;
        case GET_ALL_UPDATE_DESEMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargos: action.desembargos,
            };
        break;
        case POST_INSERT_DESEMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargo: action.desembargo,
            };
        break;
        case POST_UPDATE_DESEMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargo: action.desembargo,
            };
        break;
        case DELETE_DESEMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargo: action.desembargo,
            };
        break;
        case GET_ALL_DESEMBARGOS_CUENTA_CONCEPTOS_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargosConcepto: action.desembargosConcepto,
            };
        break;
        case POST_INSERT_DESEMBARGOS_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargoConcepto: action.desembargoConcepto,
            };
        break;
        case POST_UPDATE_DESEMBARGOS_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargoConcepto: action.desembargoConcepto,
            };
        break;
        case DELETE_DESEMBARGOS_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargoConcepto: action.desembargoConcepto,
            };
        break;
        case POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                desembargoConcepto: action.desembargoConcepto,
            };
        break;
        case GET_ALL_CONCEPTOS_SUCCESS:
            return {
                ...state,
                loading: false,
                conceptos: action.conceptos,
            };
        break;
        case ON_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        break;
        default:
            return state;
    }
    return state;
}