import { Dispatch } from 'redux';
import {
    ADD_USER,
    DELETE_USER,
    ERROR_ACTIONS,
    LOAD_USERS,
    LOAD_USERS_INITIAL,
    UPDATE_USER,
} from '../types/user.actions.type';
import { AppActions } from '../types/appactions.type';
import { User, UseRecuperaContrasena} from '../types/user.type';
import history from '../utils/history.router';
import NotificationService from '../services/notification.service';
import { NotificationCardModel, NotificationCardModelEnum } from '../models/requests/notification-cards-model';
import { Recuperarcontrasena } from '../types/login.type';
import LoginUsers from '../services/api-login.service';


const api=LoginUsers();


export const AddUser = (user: User): AppActions => ({
    type: ADD_USER,
    user,
});

export const UpdateUser = (user: User): AppActions => ({
    type: UPDATE_USER,
    user,
});

export const DeleteUser = (id: string): AppActions => ({
    type: DELETE_USER,
    id,
});

export const LoadUsersInitial = (): AppActions =>({
    type: LOAD_USERS_INITIAL
})

export const LoadUsers = (users: User[]): AppActions => ({
    type: LOAD_USERS,
    users,
});

export const SetError = (error: any): AppActions => ({
    type: ERROR_ACTIONS,
    error,
});

export const startAddUser =
    (user: User,token:string) => async (dispatch: Dispatch<AppActions>) => {


        const response = await api.createUserRequest(user);

        if (response?.isSuccess == null || !response.isSuccess) {
            dispatch(SetError(response));

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);
        } else {

            await NotificationService.showNotification({
                message: "Usuario creado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);
            dispatch(AddUser(user));
            history.push('/Usuarios');
        }
   
    };

export const startDeleteUser =
    (token:string, id:User ) => async (dispatch: Dispatch<AppActions>) => {

        const response = await api.DeleteUserRequest(id)
    
        if (response?.isSuccess == null || !response.isSuccess) {
            dispatch(SetError(response));

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);
        } else {

            await NotificationService.showNotification({
                message: "Usuario se elimino corectamnte.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);
            DeleteUser(id.id);
            history.push('/Usuarios');
        }
    
    };

export const startUpdateUser =
    (user: User,token:string) => async (dispatch: Dispatch<AppActions>) => {

        const response = await api.UpdateUserRequest(user)
 
        if (response?.isSuccess == null || !response.isSuccess) {
            dispatch(SetError(response));

            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);
        } else {

            await NotificationService.showNotification({
                message: "Usuario actualizado exitosamente.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);
            dispatch(UpdateUser(user));
            history.push('/Usuarios');
        }
    };

export const startLoadUsers =
    (token:string) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(LoadUsersInitial())
        const response = await api.getAllUsersRequest()
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
            
        } else {
            dispatch(LoadUsers(response.data));       
        }
    };

    export const startRecuperaContrasena =
    (data:Recuperarcontrasena) => async () => {
        const response = await api.RecupertaContrasena(data)
      
            await NotificationService.showNotification({
                message: "Si el usuario existe, hemos enviado un coreo electrónico para restablecer tu contraseña.",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push('/');
    };

    export const startValidaTokenContrasena =
    (token:string) => async () => {

        const response = await api.ValidaTokenContrasena(token);
        if (response?.isSuccess == null || !response?.isSuccess ) {
           
            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

            history.push('/');
            
        }        
    };


    export const startActualizaContrasena =
    (data:UseRecuperaContrasena) => async () => {

        const response = await api.ActualizaPassword(data)

        if (response?.isSuccess == null || !response?.isSuccess ) {  
            await NotificationService.showNotification({
                message: response.returnMessage,
                image: null,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);

            history.push('/');
            
        }else{

            await NotificationService.showNotification({
                message: "Se restableció la contraseña exitosamente",
                image: null,
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);

            history.push('/');
        }
        
    };

  

