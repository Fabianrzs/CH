import { Anadido } from './proceso.type';

export const CREATE_ANADIDO = 'CREATE_ANADIDO';
export const UPDATE_ANADIDO = 'UPDATE_ANADIDO';
export const DELETE_ANADIDO = 'DELETE_ANADIDO';
export const GET_BY_ID_ANADIDO = 'GET_BY_ID_ANADIDO';
export const GET_ALL_ANADIDOS = 'GET_ALL_ANADIDOS';

export interface CrearAnadidoType {
    type: typeof CREATE_ANADIDO;
    anadido: Anadido;
}

export interface ActualizarAnadidoType {
    type: typeof UPDATE_ANADIDO;
    anadido: Anadido;
}

export interface EliminarAnadidoType {
    type: typeof DELETE_ANADIDO;
    anadido: Anadido;
}

export interface ObtenerPorIdAnadidoType {
    type: typeof GET_BY_ID_ANADIDO;
    anadido: Anadido;
}

export interface ObtenerAnadidosType {
    type: typeof GET_ALL_ANADIDOS;
    anadido: Anadido;
}

export type AnadidosActionsTypes =
    | CrearAnadidoType
    | ActualizarAnadidoType
    | EliminarAnadidoType
    | ObtenerPorIdAnadidoType
    | ObtenerAnadidosType;
