import {
         EmbargoBeneficiariosDto, 
         EmbargoConceptoDto,
         EmbargoDto,
         TipoEmbargoDto, 
         TipoProcesoEmbargoDto, 
         TipoRetencionDto } from "../types/embargo.model.type";
import { EmbargosType, 
         ON_ERROR,
         GET_ALL_TIPOS_EMBARGOS_SUCCESS, 
         GET_ALL_TIPOS_PROCESOS_EMBARGOS_SUCCESS,
         GET_ALL_EMBARGOS_SUCCESS,
         POST_INSERT_EMBARGOS_SUCCESS,
         DELETE_EMBARGOS_SUCCESS,
         POST_UPDATE_EMBARGOS_SUCCESS,
         GET_ALL_UPDATE_EMBARGOS_SUCCESS,
         GET_ALL_EMBAGOS_CUENTA_CONCEPTOS_SUCCESS,
         GET_ALL_TIPOS_RETENCION_SUCCESS,
         POST_INSERT_EMBARGOS_CONCEPTO_SUCCESS,
         POST_UPDATE_EMBARGOS_CONCEPTO_SUCCESS,
         DELETE_EMBARGOS_CONCEPTO_SUCCESS,
         GET_ALL_BENEFICIARIOS_PAGO_SUCCESS,
         POST_INSERT_BENEFICIARIO_PAGO_SUCCESS,
         POST_UPDATE_BENEFICIARIO_PAGO_SUCCESS,
         DELETE_BENEFICIARIO_PAGO_SUCCESS,
         POST_ALL_EMBARGOS_BY_DATE_SUCCESS,
         POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS,
         GET_ALL_DESEMBARGOS_CUENTA_SUCCESS,
         GET_ALL_CONCEPTOS_SUCCESS} from "../types/embargos.actions.type";

export interface EmbargosState {
    embargos: EmbargoDto [];
    embargo: EmbargoDto | null;
    tiposEmbargo: TipoEmbargoDto [];
    tiposProcesos: TipoProcesoEmbargoDto [];
    tiposRetencion: TipoRetencionDto [];
    embargosConcepto: EmbargoConceptoDto [];
    embargoConcepto: EmbargoConceptoDto | null;
    embargoBeneficiarios: EmbargoBeneficiariosDto [];
    embargoBeneficiario: EmbargoBeneficiariosDto | null;
    conceptos: any;
    loading: boolean;
    error: any;
}

const EmbargosStateInitial: EmbargosState = {
    embargos: [],
    embargo: null,
    tiposEmbargo: [],
    tiposProcesos: [],
    tiposRetencion: [],
    embargosConcepto: [],
    embargoConcepto: null,
    embargoBeneficiarios: [],
    embargoBeneficiario: null,
    conceptos: {},
    loading: false,
    error: null
}

export const EmbargosReducer = (
    state: EmbargosState = EmbargosStateInitial,
    action: EmbargosType
): EmbargosState => {
    switch (action.type){
        case GET_ALL_TIPOS_EMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                tiposEmbargo: action.tiposEmbargo,
            };
        break;
        case GET_ALL_TIPOS_PROCESOS_EMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                tiposProcesos: action.tiposProcesos,
            };
        break;
        case GET_ALL_TIPOS_RETENCION_SUCCESS:
            return {
                ...state,
                loading: false,
                tiposRetencion: action.tiposRetencion,
            };
        break;
        case GET_ALL_EMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                embargos: action.embargos,
            };
        break;
        case POST_ALL_EMBARGOS_BY_DATE_SUCCESS:
            return {
                ...state,
                loading: false,
                embargos: action.embargos,
            };
        break;
        case GET_ALL_UPDATE_EMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                embargos: action.embargos,
            };
        break;
        case POST_INSERT_EMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                embargo: action.embargo,
            };
        break;
        case POST_UPDATE_EMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                embargo: action.embargo,
            };
        break;
        case DELETE_EMBARGOS_SUCCESS:
            return {
                ...state,
                loading: false,
                embargo: action.embargo,
            };
        break;
        case GET_ALL_EMBAGOS_CUENTA_CONCEPTOS_SUCCESS:
            return {
                ...state,
                loading: false,
                embargosConcepto: action.embargosConcepto,
            };
        break;
        case POST_INSERT_EMBARGOS_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoConcepto: action.embargoConcepto,
            };
        break;
        case POST_UPDATE_EMBARGOS_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoConcepto: action.embargoConcepto,
            };
        break;
        case DELETE_EMBARGOS_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoConcepto: action.embargoConcepto,
            };
        break;
        case GET_ALL_BENEFICIARIOS_PAGO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoBeneficiarios: action.embargoBeneficiarios,
            };
        break;
        case POST_INSERT_BENEFICIARIO_PAGO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoBeneficiario: action.embargoBeneficiario,
            };
        break;
        case POST_UPDATE_BENEFICIARIO_PAGO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoBeneficiario: action.embargoBeneficiario,
            };
        break;
        case DELETE_BENEFICIARIO_PAGO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoBeneficiario: action.embargoBeneficiario,
            };
        break;
        case POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS:
            return {
                ...state,
                loading: false,
                embargoConcepto: action.embargoConcepto,
            };
        break;
        case GET_ALL_DESEMBARGOS_CUENTA_SUCCESS:
            return {
                ...state,
                loading: false,
                embargosConcepto: action.embargosConcepto,
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