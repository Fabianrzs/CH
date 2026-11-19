import { Dispatch } from 'redux';
import LoginUsers from '../services/api-login.service';
import { AppActions } from '../types/appactions.type';
import { ROLDTO } from '../types/rol.type';
import {
    ADD_ROL,
    DELETE_ROL,
    GETALL_ROLES,
    GET_BY_ID_ROL,
    UPDATE_ROL,
} from '../types/roles.actions.type';
import { SetError } from './user.action';

const api = LoginUsers()

export const AddRol = (rol: ROLDTO): AppActions => ({
    type: ADD_ROL,
    rol,
});

export const DeleteRol = (rol: ROLDTO): AppActions => ({
    type: DELETE_ROL,
    rol,
});

export const UpdateRol = (rol: ROLDTO): AppActions => ({
    type: UPDATE_ROL,
    rol,
});

export const GetAllRoles = (roles: ROLDTO[]): AppActions => ({
    type: GETALL_ROLES,
    roles,
});

export const GetByIdRol = (rol: ROLDTO): AppActions => ({
    type: GET_BY_ID_ROL,
    rol,
});

export const startAddRol =
    (rol: ROLDTO, props: string) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.createROLDTORequest(rol)
        if (response?.isSuccess === null) {
            dispatch(SetError(response));
        } else {
            dispatch(AddRol(response.data));
        }
    };

export const startDeleteRol =
    (rol: ROLDTO) => (dispatch: Dispatch<AppActions>) => {
 
        dispatch(DeleteRol(rol));
    };

export const startUpdateRol =
    (rol: ROLDTO) => (dispatch: Dispatch<AppActions>) => {

        dispatch(UpdateRol(rol));
    };

export const startGetAllRoles =
    (props: string) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.getAllROLDTOsRequest()
        if (response?.isSuccess === null) {
            dispatch(SetError(response));
        } else {
            dispatch(GetAllRoles(response.data));
        }
    };

export const startGetByIdRol =
    (rol: ROLDTO) => (dispatch: Dispatch<AppActions>) => {
  
        dispatch(GetByIdRol(rol));
    };
