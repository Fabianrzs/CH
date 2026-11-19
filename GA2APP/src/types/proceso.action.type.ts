import { Proceso } from './proceso.type';

export const CREAR_PROCESO = 'CREAR_PROCESO';
export const UPDATE_PROCESO = 'UPDATE_PROCESO';
export const DELETE_PROCESO = 'DELETE_PROCESO';
export const GET_PROCESO_BY_ID = 'GET_PROCESO_BY_ID';
export const GET_ALL_PROCESO = 'GET_ALL_PROCESO';

export interface CrearProcesoActionType {
    type: typeof CREAR_PROCESO;
    Proceso: Proceso;
}

export interface UpdateProcesoActionType {
    type: typeof UPDATE_PROCESO;
    Proceso: Proceso;
}

export interface DeleteProcesoActionType {
    type: typeof DELETE_PROCESO;
    idPrceso: string;
}

export interface GetByIdProcesoActionType {
    type: typeof GET_PROCESO_BY_ID;
    proceso: Proceso;
}

export interface GetAllProcesoActionType {
    type: typeof GET_ALL_PROCESO;
    procesos: Proceso[];
}

export type ProcesoActionsType =
    | CrearProcesoActionType
    | UpdateProcesoActionType
    | DeleteProcesoActionType
    | GetByIdProcesoActionType
    | GetAllProcesoActionType;
