import {
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
} from './parametrizationLegal.model.type';

// Especificacion de accion
export const GET_LEGAL_RELIEF_PARAMETRIZATION =
    'GET_LEGAL_RELIEF_PARAMETRIZATION';
export const GET_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS =
    'GET_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS';
export const GET_LEGAL_RELIEF_PARAMETRIZATION_FAILURE =
    'GET_LEGAL_RELIEF_PARAMETRIZATION_FAILURE';
export const UPDATE_LEGAL_RELIEF_PARAMETRIZATION =
    'UPDATE_LEGAL_RELIEF_PARAMETRIZATION';
export const UPDATE_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS =
    'UPDATE_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS';
export const UPDATE_LEGAL_RELIEF_PARAMETRIZATION_FAILURE =
    'UPDATE_LEGAL_RELIEF_PARAMETRIZATION_FAILURE';
export const GET_LEGAL_RATE_PARAMETRIZATION = 'GET_LEGAL_RATE_PARAMETRIZATION';
export const GET_LEGAL_RATE_PARAMETRIZATION_SUCCESS =
    'GET_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
export const GET_LEGAL_RATE_PARAMETRIZATION_FAILURE =
    'GET_LEGAL_RATE_PARAMETRIZATION_FAILURE';
export const UPDATE_LEGAL_RATE_PARAMETRIZATION =
    'UPDATE_LEGAL_RATE_PARAMETRIZATION';
export const UPDATE_LEGAL_RATE_PARAMETRIZATION_SUCCESS =
    'UPDATE_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
export const UPDATE_LEGAL_RATE_PARAMETRIZATION_FAILURE =
    'UPDATE_LEGAL_RATE_PARAMETRIZATION_FAILURE';
export const INSERT_LEGAL_RATE_PARAMETRIZATION =
    'INSERT_LEGAL_RATE_PARAMETRIZATION';
export const INSERT_LEGAL_RATE_PARAMETRIZATION_SUCCESS =
    'INSERT_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
export const INSERT_LEGAL_RATE_PARAMETRIZATION_FAILURE =
    'INSERT_LEGAL_RATE_PARAMETRIZATION_FAILURE';
export const DELETE_LEGAL_RATE_PARAMETRIZATION =
    'DELETE_LEGAL_RATE_PARAMETRIZATION';
export const DELETE_LEGAL_RATE_PARAMETRIZATION_SUCCESS =
    'DELETE_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
export const DELETE_LEGAL_RATE_PARAMETRIZATION_FAILURE =
    'DELETE_LEGAL_RATE_PARAMETRIZATION_FAILURE';
export const GET_LEGAL_GMF_PARAMETRIZATION = 'GET_LEGAL_GMF_PARAMETRIZATION';
export const GET_LEGAL_GMF_PARAMETRIZATION_SUCCESS =
    'GET_LEGAL_GMF_PARAMETRIZATION_SUCCESS';
export const GET_LEGAL_GMF_PARAMETRIZATION_FAILURE =
    'GET_LEGAL_GMF_PARAMETRIZATION_FAILURE';
export const UPDATE_LEGAL_GMF_PARAMETRIZATION =
    'UPDATE_LEGAL_GMF_PARAMETRIZATION';
export const UPDATE_LEGAL_GMF_PARAMETRIZATION_SUCCESS =
    'UPDATE_LEGAL_GMF_PARAMETRIZATION_SUCCESS';
export const UPDATE_LEGAL_GMF_PARAMETRIZATION_FAILURE =
    'UPDATE_LEGAL_GMF_PARAMETRIZATION_FAILURE';
export const ON_ERROR = 'ON_ERROR';

export interface GetLegalReliefParametrization {
    type: 'GET_LEGAL_RELIEF_PARAMETRIZATION';
}

export interface GetLegalReliefParametrizationSuccess {
    type: 'GET_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS';
    LegalRelief: LegalAlivioDto;
}

export interface GetLegalReliefParametrizationFailure {
    type: 'GET_LEGAL_RELIEF_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface UpdateLegalReliefParametrization {
    type: 'UPDATE_LEGAL_RELIEF_PARAMETRIZATION';
}

export interface UpdateLegalReliefParametrizationSuccess {
    type: 'UPDATE_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS';
    LegalRelief: LegalAlivioDto;
}

export interface UpdateLegalReliefParametrizationFailure {
    type: 'UPDATE_LEGAL_RELIEF_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface GetLegalRateParametrization {
    type: 'GET_LEGAL_RATE_PARAMETRIZATION';
}

export interface GetLegalRateParametrizationSuccess {
    type: 'GET_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
    legalRates: LegalTasaDto[];
}

export interface GetLegalRateParametrizationFailure {
    type: 'GET_LEGAL_RATE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface UpdateLegalRateParametrization {
    type: 'UPDATE_LEGAL_RATE_PARAMETRIZATION';
}

export interface UpdateLegalRateParametrizationSuccess {
    type: 'UPDATE_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
    LegalRate: LegalTasaDto;
}

export interface UpdateLegalRateParametrizationFailure {
    type: 'UPDATE_LEGAL_RATE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface InsertLegalRateParametrization {
    type: 'INSERT_LEGAL_RATE_PARAMETRIZATION';
}

export interface InsertLegalRateParametrizationSuccess {
    type: 'INSERT_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
    LegalRate: LegalTasaDto;
}

export interface InsertLegalRateParametrizationFailure {
    type: 'INSERT_LEGAL_RATE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface DeleteLegalRateParametrization {
    type: 'DELETE_LEGAL_RATE_PARAMETRIZATION';
}

export interface DeleteLegalRateParametrizationSuccess {
    type: 'DELETE_LEGAL_RATE_PARAMETRIZATION_SUCCESS';
    LegalRate: LegalTasaDto;
}

export interface DeleteLegalRateParametrizationFailure {
    type: 'DELETE_LEGAL_RATE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface GetLegalGmfParametrization {
    type: 'GET_LEGAL_GMF_PARAMETRIZATION';
}

export interface GetLegalGmfParametrizationSuccess {
    type: 'GET_LEGAL_GMF_PARAMETRIZATION_SUCCESS';
    LegalGmf: LegalGmfDto;
}

export interface GetLegalGmfParametrizationFailure {
    type: 'GET_LEGAL_GMF_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface UpdateLegalGmfParametrization {
    type: 'UPDATE_LEGAL_GMF_PARAMETRIZATION';
}

export interface UpdateLegalGmfParametrizationSuccess {
    type: 'UPDATE_LEGAL_GMF_PARAMETRIZATION_SUCCESS';
    LegalGmf: LegalGmfDto;
}

export interface UpdateLegalGmfParametrizationFailure {
    type: 'UPDATE_LEGAL_GMF_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface OnErrorInsurance {
    type: 'ON_ERROR';
    error: any;
}

export type ParametrizationLegalActionTypes =
    | GetLegalReliefParametrization
    | GetLegalReliefParametrizationSuccess
    | GetLegalReliefParametrizationFailure
    | UpdateLegalReliefParametrization
    | UpdateLegalReliefParametrizationSuccess
    | UpdateLegalReliefParametrizationFailure
    | GetLegalRateParametrization
    | GetLegalRateParametrizationSuccess
    | GetLegalRateParametrizationFailure
    | UpdateLegalRateParametrization
    | UpdateLegalRateParametrizationSuccess
    | UpdateLegalRateParametrizationFailure
    | InsertLegalRateParametrization
    | InsertLegalRateParametrizationSuccess
    | InsertLegalRateParametrizationFailure
    | DeleteLegalRateParametrization
    | DeleteLegalRateParametrizationSuccess
    | DeleteLegalRateParametrizationFailure
    | GetLegalGmfParametrization
    | GetLegalGmfParametrizationSuccess
    | GetLegalGmfParametrizationFailure
    | UpdateLegalGmfParametrization
    | UpdateLegalGmfParametrizationSuccess
    | UpdateLegalGmfParametrizationFailure
    | OnErrorInsurance;
