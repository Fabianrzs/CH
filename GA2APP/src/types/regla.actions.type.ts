import { Regla } from './proceso.type';

export const CREATE_REGLA = 'CREATE_REGLA';
export const UPDATE_REGLA = 'UPDATE_REGLA';
export const DELETE_REGLA = 'DELETE_REGLA';
export const GET_BY_ID_REGLA = 'GET_BY_ID_REGLA';
export const GET_ALL_REGLAS = 'GET_ALL_REGLAS';

export interface CreateReglaActionType {
    type: typeof CREATE_REGLA;
    regla: Regla;
}

export interface UpdateReglaActionType {
    type: typeof UPDATE_REGLA;
    regla: Regla;
}

export interface DeleteReglaActionType {
    type: typeof DELETE_REGLA;
    regla: Regla;
}

export interface GetByIdReglaActionType {
    type: typeof GET_BY_ID_REGLA;
    regla: Regla;
}

export interface GetAllReglasActionType {
    type: typeof GET_ALL_REGLAS;
    regla: Regla[];
}

export type ReglasActionsTypes =
    | CreateReglaActionType
    | UpdateReglaActionType
    | DeleteReglaActionType
    | GetByIdReglaActionType
    | GetAllReglasActionType;
