import {
    AddressActionTypes,
    OBTENER_DIRECCIONES_ABECEDARIO,
    OBTENER_DIRECCIONES_ABECEDARIO_SUCCESS,
    OBTENER_DIRECCIONES_BIS,
    OBTENER_DIRECCIONES_BIS_SUCCESS,
    OBTENER_DIRECCIONES_CARDINALES,
    OBTENER_DIRECCIONES_CARDINALES_SUCCESS,
    OBTENER_DIRECCIONES_TIPOS,
    OBTENER_DIRECCIONES_TIPOS_SUCCESS,
} from '../types/address.actions.type';
import {
    DireccionAbecedario,
    DireccionBis,
    DireccionCardinal,
    DireccionTipo,
} from '../types/address.type';
import { ON_ERROR } from '../types/catalog.actions.type';

export interface AddressState {
    direccionesTipos: DireccionTipo[];
    direccionesCardinales: DireccionCardinal[];
    direccionesAbecedario: DireccionAbecedario[];
    direccionesBis: DireccionBis[];
    loading: boolean;
    error: any;
}

const InitialState: AddressState = {
    direccionesTipos: [],
    direccionesCardinales: [],
    direccionesAbecedario: [],
    direccionesBis: [],
    loading: false,
    error: null,
};

export const AddressReducer = (
    state: AddressState = InitialState,
    action: AddressActionTypes,
): AddressState => {
    switch (action.type) {
        case OBTENER_DIRECCIONES_TIPOS:
            return {
                ...state,
                loading: true,
            };
        case OBTENER_DIRECCIONES_TIPOS_SUCCESS:
            return {
                ...state,
                loading: false,
                direccionesTipos: action.direccionesTipos,
            };
        case OBTENER_DIRECCIONES_ABECEDARIO:
            return {
                ...state,
                loading: true,
            };
        case OBTENER_DIRECCIONES_ABECEDARIO_SUCCESS:
            return {
                ...state,
                loading: false,
                direccionesAbecedario: action.direccionesAbecedarios,
            };
        case OBTENER_DIRECCIONES_CARDINALES:
            return {
                ...state,
                loading: true,
            };
        case OBTENER_DIRECCIONES_CARDINALES_SUCCESS:
            return {
                ...state,
                loading: false,
                direccionesCardinales: action.direccionesCardinales,
            };
        case OBTENER_DIRECCIONES_BIS:
            return {
                ...state,
                loading: true,
            };
        case OBTENER_DIRECCIONES_BIS_SUCCESS:
            return {
                ...state,
                loading: false,
                direccionesBis: action.direccionesBis,
            };
        case ON_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        default:
            return state;
    }
};
