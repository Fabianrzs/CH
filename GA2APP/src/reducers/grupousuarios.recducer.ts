import {
    GrupoUsersActionsTypes,
    GET_GRUPO_USER_SUCCESS,
    GET_GRUPO_USER_FAILURE,
    ADD_GRUPO_USER
  
}from '../types/grupoUser.actions.type';
import { GrupoUsersModel}  from '../types/grupoUsers.model';


export interface GrupoUsuariosState {
    allgrupo: GrupoUsersModel[];
    error: any;
    loading: boolean;
    grupo: GrupoUsersModel;
}

const InitialState: GrupoUsuariosState= {  
    allgrupo: [],
    error: null,
    loading: false,
    grupo:{} as GrupoUsersModel,
};


export const GrupoUsuariosReducer = (
    state: GrupoUsuariosState  = InitialState,
    action: GrupoUsersActionsTypes,
): GrupoUsuariosState => {
    switch (action.type) {
        case GET_GRUPO_USER_SUCCESS:
            return {
                ...state,
                allgrupo:action.grupo,
            };

            case ADD_GRUPO_USER:
                return {
                    ...state,
                    grupo:action.grupoUser,
                };

            
        case GET_GRUPO_USER_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

        default:
            return state;
    }
};
