import { Dispatch } from 'react';
import { NotificationCardModel, NotificationCardModelEnum } from '../models/requests/notification-cards-model';
import GrupoUsers from '../services/api-grupoUsuarios.service';
import NotificationService from '../services/notification.service';
import { AppActions } from '../types/appactions.type';
import history from '../utils/history.router';
import { 
    GET_GRUPO_USER_FAILURE,
    GET_GRUPO_USER_SUCCESS,
    GET_GRUPO_OPCIONES_FAILURE,
    GET_GRUPO_OPCIONES_SUCCESS ,
    ADD_GRUPO_USER,
    ADD_GRUPO_OPCION,
    GET_OPCIONES_FAILURE,
    GET_OPCIONES_SUCCESS,
    GET_GRUPOS_OF_USER_SUCCESS,
    GET_GRUPOS_OF_USER_FAILURE
} from '../types/grupoUser.actions.type'
import { GrupoUsersModel,GruposOfUserModel } from '../types/grupoUsers.model';
import { GrupoOpcionesModel } from '../types/grupo-opciones.model.type';

const api=GrupoUsers();

export const GetGrupoUsersFailure = (error: any): AppActions => ({
    type: GET_GRUPO_USER_FAILURE,
    error: error,
});


export const AddGrupo = (grupo: GrupoUsersModel): AppActions => ({
    type: ADD_GRUPO_USER,
    grupoUser:grupo,
});



export const GetGrupoUsersSuccess = (grupou: GrupoUsersModel[]): AppActions => ({
    type: GET_GRUPO_USER_SUCCESS,
    grupo: grupou,
});

export const GetGrupoOpcionesFailure = (error: any): AppActions => ({
    type: GET_GRUPO_OPCIONES_FAILURE,
    error: error,
});


export const GetGrupoOpcionesSuccess = (grupou: GrupoUsersModel[]): AppActions => ({
    type: GET_GRUPO_OPCIONES_SUCCESS ,
    grupo: grupou,
});

export const addGrupoOpcion = (grupou: GrupoUsersModel): AppActions => ({
    type: ADD_GRUPO_OPCION ,
    opcion: grupou,
});


export const GetOpcionesFailure = (error: any): AppActions => ({
    type: GET_OPCIONES_FAILURE,
    error: error,
});


export const GetOpcionesSuccess = (grupou: GrupoOpcionesModel[]): AppActions => ({
    type: GET_OPCIONES_SUCCESS ,
    ListOpciones: grupou,
});

export const GetGruposOfUserSuccess = (grupos: GruposOfUserModel[]): AppActions => ({
    type: GET_GRUPOS_OF_USER_SUCCESS,
    grupos: grupos,
});

export const GetGruposOfUserFailure = (error: any): AppActions => ({
    type: GET_GRUPOS_OF_USER_FAILURE,
    error: error,
});

export const startGetAllGrupoUsuarios = () => async (dispatch: Dispatch<AppActions>) => {

    const response = await api.getAllGrupoUsuarios();
     
    if (response?.isSuccess == null) {
        dispatch(GetGrupoUsersFailure(response.data));
    } else {
        dispatch(GetGrupoUsersSuccess(response.data));
    }

};


export const startGetAllOpciones = () => async (dispatch: Dispatch<AppActions>) => {

    const response = await api.getAllOpciones();
     
    if (response?.isSuccess == null) {
        dispatch(GetOpcionesFailure(response.data));
    } else {
        dispatch(GetOpcionesSuccess(response.data));
    }

};


export const startGetAllGrupoOpciones = () => async (dispatch: Dispatch<AppActions>) => {

    const response = await api.getAllGrupoOpciones();
    if (response?.isSuccess == null) {
        dispatch(GetGrupoOpcionesFailure(response.data));
    } else {
        dispatch(GetGrupoOpcionesSuccess(response.data));
    }
};

export const startAddGrupoUsuarios=
    (grupo: GrupoUsersModel) => async () => {
        const response = await api.InserGrupoUsuario(grupo);
        if (response?.isSuccess == null || !response.isSuccess) {

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

        } else {

            await NotificationService.showNotification({
                message: "Grupo de usuario creado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push("/GrupoUsuarios");

        }  
    };


    export const startAddGrupoOpciones=
    (grupo: GrupoUsersModel) => async () => {
        const response = await api.InserGrupoOpciones(grupo);
        if (response?.isSuccess == null || !response.isSuccess) {

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

        } else {

            await NotificationService.showNotification({
                message: "Grupo de opciones creado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push("/GrupoOpciones");

        }  
    };



    export const startAddOpciones=
    (grupo: GrupoUsersModel) => async () => {
        const response = await api.InserOpciones(grupo);
        if (response?.isSuccess == null || !response.isSuccess) {

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

        } else {

            await NotificationService.showNotification({
                message: "Grupo de opciones creado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push("/Opciones");

        }  
    };


    export const startActualizaGrupoOpciones=
    (grupo: GrupoUsersModel) => async () => {

        const response = await api.ActualizaGrupoOpciones(grupo);
        if (response?.isSuccess == null || !response.isSuccess) {

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

        } else {

            await NotificationService.showNotification({
                message: "Grupo de opciones actualizado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push("/GrupoOpciones");

        }  
    };


    export const startActualizaGrupoUsuario=
    (grupo: GrupoUsersModel) => async () => {

        

        const response = await api.ActualizaGrupoUsuarios(grupo);
        if (response?.isSuccess == null || !response.isSuccess) {

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

        } else {

            await NotificationService.showNotification({
                message: "Grupo de usuario actualizado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push("/GrupoUsuarios");

        }  
    };




    export const startDeleteGrupoUsuario=
    (grupo: GrupoUsersModel) => async () => {

        const response = await api.DeleteGrupoUsuario(grupo);

        if (response?.isSuccess == null || !response.isSuccess) {
            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

        } else {
            await NotificationService.showNotification({
                message: "Grupo de usuario eliminado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push("/GrupoUsuarios");
        }  
    };

    export const startDeleteGrupoOpciones=
    (grupo: GrupoUsersModel) => async () => {

        const response = await api.DeleteGrupoOpciones(grupo);
        if (response?.isSuccess == null || !response.isSuccess) {
            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

        } else {
            await NotificationService.showNotification({
                message: "Grupo de opciones eliminado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push("/GrupoOpciones");
        }  
    };

    export const startGetGruposUsuarioPorId = (idUsuario:string) => async (dispatch: Dispatch<AppActions>) => {

        const response = await api.ObtenerGruposUsuarioPorId(idUsuario);
        if (response?.isSuccess == null) {
            dispatch(GetGruposOfUserFailure(response.data));
        } else {
            dispatch(GetGruposOfUserSuccess(response.data));
        }
    };



