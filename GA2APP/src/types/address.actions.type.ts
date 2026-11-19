import {
    DireccionAbecedario,
    DireccionBis,
    DireccionCardinal,
    DireccionTipo,
} from './address.type';

// Especificacion de accion
export const OBTENER_DIRECCIONES_TIPOS = 'OBTENER_DIRECCIONES_TIPOS';
export const OBTENER_DIRECCIONES_CARDINALES = 'OBTENER_DIRECCIONES_CARDINALES';
export const OBTENER_DIRECCIONES_ABECEDARIO = 'OBTENER_DIRECCIONES_ABECEDARIO';
export const OBTENER_DIRECCIONES_CARDINALES_SUCCESS =
    'OBTENER_DIRECCIONES_CARDINALES_SUCCESS';
export const OBTENER_DIRECCIONES_ABECEDARIO_SUCCESS =
    'OBTENER_DIRECCIONES_ABECEDARIO_SUCCESS';
export const OBTENER_DIRECCIONES_TIPOS_SUCCESS =
    'OBTENER_DIRECCIONES_TIPOS_SUCCESS';
export const ON_ERROR = 'ON_ERROR';
export const OBTENER_DIRECCIONES_BIS = 'OBTENER_DIRECCIONES_BIS';
export const OBTENER_DIRECCIONES_BIS_SUCCESS =
    'OBTENER_DIRECCIONES_BIS_SUCCESS';
export interface ObtenerDireccionesTipos {
    type: 'OBTENER_DIRECCIONES_TIPOS';
}

export interface ObtenerDireccionesTiposSuccess {
    type: 'OBTENER_DIRECCIONES_TIPOS_SUCCESS';
    direccionesTipos: DireccionTipo[];
}

export interface ObtenerDireccionesCardinales {
    type: 'OBTENER_DIRECCIONES_CARDINALES';
}

export interface ObtenerDireccionesCardinalesSuccess {
    type: 'OBTENER_DIRECCIONES_CARDINALES_SUCCESS';
    direccionesCardinales: DireccionCardinal[];
}

export interface ObtenerDireccionBis {
    type: 'OBTENER_DIRECCIONES_BIS';
}

export interface ObtenerDireccionBisSuccess {
    type: 'OBTENER_DIRECCIONES_BIS_SUCCESS';
    direccionesBis: DireccionBis[];
}

export interface ObtenerDireccionesAbecedarios {
    type: 'OBTENER_DIRECCIONES_ABECEDARIO';
}

export interface ObtenerDireccionesAbecedariosSuccess {
    type: 'OBTENER_DIRECCIONES_ABECEDARIO_SUCCESS';
    direccionesAbecedarios: DireccionAbecedario[];
}

export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export type AddressActionTypes =
    | ObtenerDireccionesTipos
    | ObtenerDireccionesCardinales
    | ObtenerDireccionesTiposSuccess
    | ObtenerDireccionesAbecedarios
    | ObtenerDireccionesAbecedariosSuccess
    | ObtenerDireccionesCardinalesSuccess
    | ObtenerDireccionBis
    | ObtenerDireccionBisSuccess
    | OnError;
