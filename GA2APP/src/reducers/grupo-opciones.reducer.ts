import { GrupoOpcionesModel } from '../types/grupo-opciones.model.type';
import {
    GrupoUsersActionsTypes,
    GET_GRUPO_OPCIONES_FAILURE,
    GET_GRUPO_OPCIONES_SUCCESS,
    ADD_GRUPO_OPCION, 
    GET_OPCIONES_FAILURE,
    GET_OPCIONES_SUCCESS, 
}from '../types/grupoUser.actions.type';
import { GrupoUsersModel}  from '../types/grupoUsers.model';


export interface GrupoOpcionesState {
    allgrupoOpciones: GrupoUsersModel[];
    error: any;
    loading: boolean;
    opcion:GrupoUsersModel;
    allOpciones:GrupoOpcionesModel[];
}

const InitialState: GrupoOpcionesState= {  
    allgrupoOpciones: [],
    allOpciones:[],
    opcion:{} as GrupoUsersModel,
    error: null,
    loading: false,

};


export const GrupoOpcionesReducer = (
    state: GrupoOpcionesState  = InitialState,
    action: GrupoUsersActionsTypes,
): GrupoOpcionesState=> {
    switch (action.type) {
        case GET_GRUPO_OPCIONES_SUCCESS:
            return {
                ...state,
                allgrupoOpciones:action.grupo,
            };
      
        case GET_GRUPO_OPCIONES_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

            case GET_OPCIONES_SUCCESS:
                return {
                    ...state,
                    allOpciones:action.ListOpciones,
                };

             case GET_OPCIONES_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };


            case ADD_GRUPO_OPCION:
            return {
                ...state,
                opcion:action.opcion,
            };
             
            
        default:
            return state;
    }
};
