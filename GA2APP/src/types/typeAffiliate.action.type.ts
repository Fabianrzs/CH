import { TIPOAFILIACIONDTO } from "./catalog.type";
import { AfiliacionesExtraordinariasDTO } from "./typeAffiliate.type";

export const GET_TYPE_AFFILIATE = 'GET_TYPE_AFFILIATE';
export const GET_TYPE_AFFILIATE_SUCCESS = 'GET_TYPE_AFFILIATE_SUCCESS';
export const GET_TYPE_AFFILIATE_FAILURE = 'GET_TYPE_AFFILIATE_FAILURE';
export const GET_EXTRAORDINARY_AFFILIATE = 'GET_EXTRAORDINARY_AFFILIATE';
export const GET_EXTRAORDINARY_AFFILIATE_SUCCESS = 'GET_EXTRAORDINARY_AFFILIATE_SUCCESS';
export const GET_EXTRAORDINARY_AFFILIATE_FAILURE = 'GET_EXTRAORDINARY_AFFILIATE_FAILURE';
export const POST_NEW_EXTRAORDINARY_AFFILIATE = 'POST_NEW_EXTRAORDINARY_AFFILIATE';
export const POST_NEW_EXTRAORDINARY_AFFILIATE_SUCCESS = 'POST_NEW_EXTRAORDINARY_AFFILIATE_SUCCESS';
export const POST_NEW_EXTRAORDINARY_AFFILIATE_FAILURE = 'POST_NEW_EXTRAORDINARY_AFFILIATE_FAILURE';
export const POST_UPDATE_EXTRAORDINARY_AFFILIATE = 'POST_UPDATE_EXTRAORDINARY_AFFILIATE';
export const POST_UPDATE_EXTRAORDINARY_AFFILIATE_SUCCESS = 'POST_UPDATE_EXTRAORDINARY_AFFILIATE_SUCCESS';
export const POST_UPDATE_EXTRAORDINARY_AFFILIATE_FAILURE = 'POST_UPDATE_EXTRAORDINARY_AFFILIATE_FAILURE';
export const GET_AFFILIATE_EXTRAORDINARY_DETAIL = 'GET_AFFILIATE_EXTRAORDINARY_DETAIL';
export const GET_AFFILIATE_EXTRAORDINARY_DETAIL_SUCCESS = 'GET_AFFILIATE_EXTRAORDINARY_DETAIL_SUCCESS';
export const GET_AFFILIATE_EXTRAORDINARY_DETAIL_FAILURE = 'GET_AFFILIATE_EXTRAORDINARY_DETAIL_FAILURE';
export const CHECK_AFFILIATE_EXISTS = 'CHECK_AFFILIATE_EXISTS';
export const CHECK_AFFILIATE_EXISTS_SUCCESS = 'CHECK_AFFILIATE_EXISTS_SUCCESS';
export const CHECK_AFFILIATE_EXISTS_FAILURE = 'CHECK_AFFILIATE_EXISTS_FAILURE';
export const DELETE_AFFILIATE = 'DELETE_AFFILIATE';
export const DELETE_AFFILIATE_SUCCESS = 'DELETE_AFFILIATE_SUCCESS';
export const DELETE_AFFILIATE_FAILURE = 'DELETE_AFFILIATE_FAILURE';



export interface getTypeAffiliate {
    type: 'GET_TYPE_AFFILIATE';
}

export interface getTypeAffiliateSuccess {
    type: 'GET_TYPE_AFFILIATE_SUCCESS';
    tipoAfiliacion: TIPOAFILIACIONDTO[];
}

export interface getTypeAffiliateFailure {
    type: 'GET_TYPE_AFFILIATE_FAILURE';
    tipoAfiliacion: TIPOAFILIACIONDTO[] | null;
}

export interface GetAffiliateExtraordinary {
    type: 'GET_EXTRAORDINARY_AFFILIATE';
}

export interface GetAffiliateExtraordinarySuccess {
    type: 'GET_EXTRAORDINARY_AFFILIATE_SUCCESS';
    afiliacionExtraordinaria: AfiliacionesExtraordinariasDTO[];
}

export interface GetAffiliateExtraordinaryFailure {
    type: 'GET_EXTRAORDINARY_AFFILIATE_FAILURE';
    afiliacionExtraordinaria: AfiliacionesExtraordinariasDTO[] | null;
}

export interface PostNewExtraordinaryAffiliate {
    type: 'POST_NEW_EXTRAORDINARY_AFFILIATE';
}

export interface PostNewExtraordinaryAffiliateSuccess {
    type: 'POST_NEW_EXTRAORDINARY_AFFILIATE_SUCCESS';
    insertarNuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO;
}

export interface PostNewExtraordinaryAffiliateFailure {
    type: 'POST_NEW_EXTRAORDINARY_AFFILIATE_FAILURE';
    insertarNuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO;
}

export interface PostUpdateExtraordinaryAffiliate {
    type: 'POST_UPDATE_EXTRAORDINARY_AFFILIATE';
}

export interface PostUpdateExtraordinaryAffiliateSuccess {
    type: 'POST_UPDATE_EXTRAORDINARY_AFFILIATE_SUCCESS';
    actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO;
}

export interface PostUpdateExtraordinaryAffiliateFailure {
    type: 'POST_UPDATE_EXTRAORDINARY_AFFILIATE_FAILURE';
    actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO;
}

export interface GetAffiliateExtraordinaryDetail {
    type: 'GET_AFFILIATE_EXTRAORDINARY_DETAIL';
}

export interface GetAffiliateExtraordinaryDetailSuccess {
    type: 'GET_AFFILIATE_EXTRAORDINARY_DETAIL_SUCCESS';
    detalleAfiliacion: AfiliacionesExtraordinariasDTO;
}

export interface GetAffiliateExtraordinaryDetailFailure {
    type: 'GET_AFFILIATE_EXTRAORDINARY_DETAIL_FAILURE';
    detalleAfiliacion: AfiliacionesExtraordinariasDTO | null;
}

export interface CheckAffiliateExists {
    type: 'CHECK_AFFILIATE_EXISTS';
}

export interface CheckAffiliateExistsSuccess {
    type: 'CHECK_AFFILIATE_EXISTS_SUCCESS';
    AfiliacionExiste: boolean;
}

export interface CheckAffiliateExistsFailure {
    type: 'CHECK_AFFILIATE_EXISTS_FAILURE';
    AfiliacionExiste: boolean | null;
}

export interface DeleteAffiliate {
    type: 'DELETE_AFFILIATE';
}

export interface DeleteAffiliateSuccess {
    type: 'DELETE_AFFILIATE_SUCCESS';
    eliminarAfiliacion: AfiliacionesExtraordinariasDTO;
}

export interface DeleteAffiliateFailure {
    type: 'DELETE_AFFILIATE_FAILURE';
    eliminarAfiliacion: AfiliacionesExtraordinariasDTO| null;
}

export type TIPOAFILIACIONDTOActionTypes =
    | getTypeAffiliate
    | getTypeAffiliateSuccess
    | getTypeAffiliateFailure
    | GetAffiliateExtraordinary
    | GetAffiliateExtraordinarySuccess
    | GetAffiliateExtraordinaryFailure
    | PostNewExtraordinaryAffiliate
    | PostNewExtraordinaryAffiliateSuccess
    | PostNewExtraordinaryAffiliateFailure
    | PostUpdateExtraordinaryAffiliate
    | PostUpdateExtraordinaryAffiliateSuccess
    | PostUpdateExtraordinaryAffiliateFailure
    | GetAffiliateExtraordinaryDetail
    | GetAffiliateExtraordinaryDetailSuccess
    | GetAffiliateExtraordinaryDetailFailure
    | CheckAffiliateExists
    | CheckAffiliateExistsSuccess
    | CheckAffiliateExistsFailure
    | DeleteAffiliate
    | DeleteAffiliateSuccess
    | DeleteAffiliateFailure
    



