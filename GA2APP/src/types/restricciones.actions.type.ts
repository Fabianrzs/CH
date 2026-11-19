import { Restricciones } from './proceso.type';

export const CREATE_RESTRICCION = 'CREATE_RESTRICCION';
export const UPDATE_RESTRICCION = 'UPDATE_RESTRICCION';
export const DELETE_RESTRICCION = 'DELETE_RESTRICCION';
export const GET_BY_ID_RESTRICCION = 'GET_BY_ID_RESTRICCION';
export const GET_ALL_RESTRICCION = 'GET_ALL_RESTRICCION';

export interface CrearRestriccionesActionType {
    type: typeof CREATE_RESTRICCION;
    restriccion: Restricciones;
}

export interface UpdateRestriccionesActionType {
    type: typeof UPDATE_RESTRICCION;
    restriccion: Restricciones;
}

export interface DeleteRestriccionesActionType {
    type: typeof DELETE_RESTRICCION;
    restriccion: Restricciones;
}

export interface GetByIdRestriccionesActionType {
    type: typeof CREATE_RESTRICCION;
    restriccionId: string;
}

export interface GetAllRestriccionesActionType {
    type: typeof GET_ALL_RESTRICCION;
    restriccions: Restricciones[];
}
