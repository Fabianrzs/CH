import { GrupoOpcionesModel } from '../types/grupo-opciones.model.type';
import {
    LoginActionTypes,
    LOGIN_USER,
    LOGIN_USER_FAILURE,
    LOGIN_USER_SUCCESS,
} from '../types/login.actions.type';

export interface LoginState {
    claims: string;
    opciones:GrupoOpcionesModel[];
    error: any;
    loading: boolean;
}

const InitialState: LoginState = {
    claims: '',
    opciones:[],
    error: null,
    loading: false,
};

export const LoginReducer = (
    state: LoginState = InitialState,
    action: LoginActionTypes,
): LoginState => {
    switch (action.type) {
        case LOGIN_USER:
            return {
                ...state,
                loading: true,
            };
        case LOGIN_USER_SUCCESS:
            return {
                ...state,
                claims: action.claims,
                opciones:action.opciones,
                loading: false,
            };
        case LOGIN_USER_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        default:
            return state;
    }
};
