import { ROLDTO } from './rol.type';


export const ADD_ROL = 'ADD_ROL';
export const UPDATE_ROL = 'UPDATE_ROL';
export const DELETE_ROL = 'DELETE_ROL';
export const GETALL_ROLES = 'GET_ALL_ROLES';
export const GET_BY_ID_ROL = 'GET_BY_ID_ROL';
export const ERROR_ROL = 'ERROR_ROL';

export interface AddRol {
    type: 'ADD_ROL';
    rol: ROLDTO;
}

export interface UpdateRol {
    type: 'UPDATE_ROL';
    rol: ROLDTO;
}

export interface DeleteRol {
    type: 'DELETE_ROL';
    rol: ROLDTO;
}

export interface GetAllRoles {
    type: 'GET_ALL_ROLES';
    roles: ROLDTO[];
}

export interface GetByIdRol {
    type: 'GET_BY_ID_ROL';
    rol: ROLDTO;
}

export interface SetErrorRol {
    type: 'ERROR_ROL';
    error: any;
}

export type RolActionTypes =
    | AddRol
    | UpdateRol
    | DeleteRol
    | GetAllRoles
    | GetByIdRol
    | SetErrorRol;
