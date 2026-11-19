import {
    LegalReliefForm,
    LegalRateForm,
    LegalGmfForm,
} from '../pages/parametrizationLegal/constants/form.model.type';
import {
    ParametrizationLegalActionTypes,
    GET_LEGAL_RELIEF_PARAMETRIZATION,
    GET_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS,
    GET_LEGAL_RELIEF_PARAMETRIZATION_FAILURE,
    UPDATE_LEGAL_RELIEF_PARAMETRIZATION,
    UPDATE_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS,
    UPDATE_LEGAL_RELIEF_PARAMETRIZATION_FAILURE,
    GET_LEGAL_RATE_PARAMETRIZATION,
    GET_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    GET_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    UPDATE_LEGAL_RATE_PARAMETRIZATION,
    UPDATE_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    UPDATE_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    INSERT_LEGAL_RATE_PARAMETRIZATION,
    INSERT_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    INSERT_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    DELETE_LEGAL_RATE_PARAMETRIZATION,
    DELETE_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    DELETE_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    GET_LEGAL_GMF_PARAMETRIZATION,
    GET_LEGAL_GMF_PARAMETRIZATION_SUCCESS,
    GET_LEGAL_GMF_PARAMETRIZATION_FAILURE,
    UPDATE_LEGAL_GMF_PARAMETRIZATION,
    UPDATE_LEGAL_GMF_PARAMETRIZATION_SUCCESS,
    UPDATE_LEGAL_GMF_PARAMETRIZATION_FAILURE,
    ON_ERROR,
} from '../types/parametrizationLegal.action.type';
import {
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
} from '../types/parametrizationLegal.model.type';

export interface ParametrizacionLegalState {
    id: number;
    legalRelief: LegalAlivioDto;
    legalRates: LegalTasaDto[];
    legalRate: LegalTasaDto;
    legalGmf: LegalGmfDto;
    error: any;
    loading: boolean;
}

const ParametrizacionLegalStateInitial: ParametrizacionLegalState = {
    id: 0,
    legalRelief: LegalReliefForm,
    legalRates: [],
    legalRate: LegalRateForm,
    legalGmf: LegalGmfForm,
    error: null,
    loading: false,
};

export const ParametrizationLegalReducer = (
    state: ParametrizacionLegalState = ParametrizacionLegalStateInitial,
    action: ParametrizationLegalActionTypes,
): ParametrizacionLegalState => {
    switch (action.type) {
        case GET_LEGAL_RELIEF_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case GET_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalRelief: action.LegalRelief,
                loading: false,
            };
        case GET_LEGAL_RELIEF_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_LEGAL_RELIEF_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalRelief: action.LegalRelief,
                loading: false,
            };
        case UPDATE_LEGAL_RELIEF_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case GET_LEGAL_RATE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case GET_LEGAL_RATE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalRates: action.legalRates,
                loading: false,
            };
        case GET_LEGAL_RATE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_LEGAL_RATE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_LEGAL_RATE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalRate: action.LegalRate,
                loading: false,
            };
        case UPDATE_LEGAL_RATE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case INSERT_LEGAL_RATE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case INSERT_LEGAL_RATE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalRate: action.LegalRate,
                loading: false,
            };
        case INSERT_LEGAL_RATE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case DELETE_LEGAL_RATE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case DELETE_LEGAL_RATE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalRate: action.LegalRate,
                loading: false,
            };
        case DELETE_LEGAL_RATE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case GET_LEGAL_GMF_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case GET_LEGAL_GMF_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalGmf: action.LegalGmf,
                loading: false,
            };
        case GET_LEGAL_GMF_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_LEGAL_GMF_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_LEGAL_GMF_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                legalGmf: action.LegalGmf,
                loading: false,
            };
        case UPDATE_LEGAL_GMF_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case ON_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        default:
            return state;
    }
};
