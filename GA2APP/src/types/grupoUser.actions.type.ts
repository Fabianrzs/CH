import { GrupoOpcionesModel } from "./grupo-opciones.model.type";
import { GruposOfUserModel, GrupoUsersModel } from "./grupoUsers.model";


export const GET_GRUPO_USER_SUCCESS = 'GET_GRUPO_USER_SUCCESS';
export const GET_GRUPO_USER_FAILURE = 'GET_STATE_SUCCESS';

export const GET_GRUPO_OPCIONES_SUCCESS = 'GET_GRUPO_OPCIONES_SUCCESS';
export const GET_GRUPO_OPCIONES_FAILURE = 'GET_GRUPO_OPCIONES_FAILURE';
export const ADD_GRUPO_USER='ADD_GRUPO_USER';
export const ADD_GRUPO_OPCION='ADD_GRUPO_OPCION';
export const GET_OPCIONES_FAILURE='GET_OPCIONES_FAILURE'
export const GET_OPCIONES_SUCCESS='GET_OPCIONES_SUCCESS'
export const GET_GRUPOS_OF_USER_FAILURE='GET_GRUPOS_OF_USER_FAILURE'
export const GET_GRUPOS_OF_USER_SUCCESS='GET_GRUPOS_OF_USER_SUCCESS'

export interface GetGrupoUsuario {
    type:  typeof ADD_GRUPO_USER;
    grupoUser: GrupoUsersModel;     
}

export interface GetGrupoUsuarios {
    type:  typeof GET_GRUPO_USER_SUCCESS;
    grupo: GrupoUsersModel[];     
}

export interface GetGrupoOpcion {
    type:  typeof ADD_GRUPO_OPCION;
    opcion: GrupoUsersModel;     
}

export interface GetGrupotStateFailure {
    type: typeof GET_GRUPO_USER_FAILURE;
    error: any;
}

export interface GetGrupoOpciones {
    type:  typeof GET_GRUPO_OPCIONES_SUCCESS;
    grupo: GrupoUsersModel[];
      
}



export interface GetOpciones {
    type:  typeof GET_OPCIONES_SUCCESS;
    ListOpciones: GrupoOpcionesModel[];
      
}

export interface GetOpcionesFailure {
    type: typeof GET_OPCIONES_FAILURE;
    error: any;
}

export interface GetGrupoOpcionesFailure {
    type: typeof GET_GRUPO_OPCIONES_FAILURE;
    error: any;
}

export interface GetGruposDeUsuario {
    type:  typeof GET_GRUPOS_OF_USER_SUCCESS;
    grupos: GruposOfUserModel[];     
}

export interface GetGruposDeUsuarioFailure {
    type: typeof GET_GRUPOS_OF_USER_FAILURE;
    error: any;
}

export type GrupoUsersActionsTypes =  
    |GetGrupoUsuarios
    |GetGrupotStateFailure
    |GetGrupoOpciones
    |GetGrupoOpcionesFailure
    |GetGrupoUsuario
    |GetGrupoOpcion
    |GetOpciones
    |GetOpcionesFailure
    |GetGruposDeUsuario
    |GetGruposDeUsuarioFailure;
