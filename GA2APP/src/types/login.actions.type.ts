import { GrupoOpcionesModel } from './grupo-opciones.model.type';
import { Login } from './login.type';

export const LOGIN_USER = 'LOGIN_USERS';
export const LOGIN_USER_SUCCESS = 'LOGIN_USER_SUCCESS';
export const LOGIN_USER_FAILURE = 'LOGIN_USER_FAILURE';

export const SET_TID = '';
export const SET_OPEN = '';

export interface SetLoginUser {
    type: typeof LOGIN_USER;
    login: Login;
}

export interface SetLoginUserSuccess {
    type: typeof LOGIN_USER_SUCCESS;
    claims: string;
    opciones: GrupoOpcionesModel[];
}

export interface SetLoginUserFailure {
    type: typeof LOGIN_USER_FAILURE;
    error: any;
}

export interface setTID {
    type: typeof SET_TID;
    claims: string | number;
}

export interface setOpen {
    type: typeof SET_OPEN;
    claims: boolean;
}

export type LoginActionTypes =
    | SetLoginUser
    | SetLoginUserSuccess
    | SetLoginUserFailure
    | setTID
    | setOpen;
