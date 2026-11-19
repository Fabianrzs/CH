import { CHECK_AFFILIATE_EXISTS, CHECK_AFFILIATE_EXISTS_FAILURE, CHECK_AFFILIATE_EXISTS_SUCCESS, DELETE_AFFILIATE, DELETE_AFFILIATE_FAILURE, DELETE_AFFILIATE_SUCCESS, GET_AFFILIATE_EXTRAORDINARY_DETAIL, GET_AFFILIATE_EXTRAORDINARY_DETAIL_FAILURE, GET_AFFILIATE_EXTRAORDINARY_DETAIL_SUCCESS, GET_EXTRAORDINARY_AFFILIATE, GET_EXTRAORDINARY_AFFILIATE_FAILURE, GET_EXTRAORDINARY_AFFILIATE_SUCCESS, POST_NEW_EXTRAORDINARY_AFFILIATE, POST_NEW_EXTRAORDINARY_AFFILIATE_FAILURE, POST_NEW_EXTRAORDINARY_AFFILIATE_SUCCESS, POST_UPDATE_EXTRAORDINARY_AFFILIATE, POST_UPDATE_EXTRAORDINARY_AFFILIATE_FAILURE, POST_UPDATE_EXTRAORDINARY_AFFILIATE_SUCCESS, TIPOAFILIACIONDTOActionTypes } from "../types/typeAffiliate.action.type";
import { AfiliacionesExtraordinariasDTO, AfiliacionesExtraordinariasDTOInicial } from "../types/typeAffiliate.type";

export interface AffiliateExtraordinaryState {
    afiliacionesExtrardinarias: AfiliacionesExtraordinariasDTO[];
    insertarNuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO;
    actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO;
    detalleAfiliacion: AfiliacionesExtraordinariasDTO;
    existeAfiliacion: boolean;
    eliminarAfiliacion: AfiliacionesExtraordinariasDTO;
    error: any;
    loading: boolean;
}

const AffiliateExtraordinaryStateInitial: AffiliateExtraordinaryState = {
    afiliacionesExtrardinarias: [],
    insertarNuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTOInicial,
    actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTOInicial,
    detalleAfiliacion: AfiliacionesExtraordinariasDTOInicial,
    eliminarAfiliacion: AfiliacionesExtraordinariasDTOInicial,
    error: null,
    existeAfiliacion: false,
    loading: false,
}

export const AffiliateExtraordinaryReducer = (
    state: AffiliateExtraordinaryState = AffiliateExtraordinaryStateInitial,
    action: TIPOAFILIACIONDTOActionTypes,
): AffiliateExtraordinaryState => {

    switch (action.type) {
        case GET_EXTRAORDINARY_AFFILIATE:
            return {
                ...state,
                loading: true,
            };
        case GET_EXTRAORDINARY_AFFILIATE_FAILURE:
            return {
                ...state,
                afiliacionesExtrardinarias: [],
                loading: false,
            };
        case GET_EXTRAORDINARY_AFFILIATE_SUCCESS:
            return {
                ...state,
                afiliacionesExtrardinarias: action.afiliacionExtraordinaria,
                loading: false,
            };

        case POST_NEW_EXTRAORDINARY_AFFILIATE:
            return {
                ...state,
                loading: true,
            };
        case POST_NEW_EXTRAORDINARY_AFFILIATE_FAILURE:
            return {
                ...state,
                insertarNuevaAfiliacionExtraordinaria: action.insertarNuevaAfiliacionExtraordinaria,
                loading: false,
            };
        case POST_NEW_EXTRAORDINARY_AFFILIATE_SUCCESS:
            return {
                ...state,
                insertarNuevaAfiliacionExtraordinaria: action.insertarNuevaAfiliacionExtraordinaria,
                loading: false,
            };

        case POST_UPDATE_EXTRAORDINARY_AFFILIATE:
            return {
                ...state,
                loading: true,
            };
        case POST_UPDATE_EXTRAORDINARY_AFFILIATE_FAILURE:
            return {
                ...state,
                actualizarAfiliacionExtraordinaria: action.actualizarAfiliacionExtraordinaria,
                loading: false,
            };
        case POST_UPDATE_EXTRAORDINARY_AFFILIATE_SUCCESS:
            return {
                ...state,
                actualizarAfiliacionExtraordinaria: action.actualizarAfiliacionExtraordinaria,
                loading: false,
            };
        case GET_AFFILIATE_EXTRAORDINARY_DETAIL:
            return {
                ...state,
                loading: true,
            };
        case GET_AFFILIATE_EXTRAORDINARY_DETAIL_FAILURE:
            return {
                ...state,
                detalleAfiliacion: {} as AfiliacionesExtraordinariasDTO,
                loading: false,
            };
        case GET_AFFILIATE_EXTRAORDINARY_DETAIL_SUCCESS:
            return {
                ...state,
                detalleAfiliacion: action.detalleAfiliacion,
                loading: false,
            };  
            
        case CHECK_AFFILIATE_EXISTS:
            return {
                ...state,
                loading: true,
            };
        case CHECK_AFFILIATE_EXISTS_FAILURE:
            return {
                ...state,
                existeAfiliacion: false,
                loading: false,
            };
        case CHECK_AFFILIATE_EXISTS_SUCCESS:
            return {
                ...state,
                existeAfiliacion: action.AfiliacionExiste,
                loading: false,
            };
            
        case DELETE_AFFILIATE:
            return {
                ...state,
                loading: true,
            };

        case DELETE_AFFILIATE_SUCCESS: 
            return {
                ...state,
                afiliacionesExtrardinarias: [...state.afiliacionesExtrardinarias.filter(x => x.idAfiliacion !== action.eliminarAfiliacion.idAfiliacion)],
                loading: false,
            }

        case DELETE_AFFILIATE_FAILURE: 
            return {
                ...state,
                eliminarAfiliacion: {} as AfiliacionesExtraordinariasDTO,
                loading: false,
            };

        default:
            return state;
    }
};
