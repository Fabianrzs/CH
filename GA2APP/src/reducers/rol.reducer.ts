import { ROLDTO } from '../types/rol.type';
import {
    ADD_ROL,
    DELETE_ROL,
    ERROR_ROL,
    GETALL_ROLES,
    GET_BY_ID_ROL,
    RolActionTypes,
    UPDATE_ROL,
} from '../types/roles.actions.type';

export interface RolState {
    roles: ROLDTO[];
    id: string;
    error: any;
    rol: ROLDTO;
    loading: boolean;
}

const RolStateInitial: RolState = {
    error: null,
    roles: [],
    id: '',
    rol: {} as ROLDTO,
    loading: false,
};

export const RolReducer = (
    state: RolState = RolStateInitial,
    action: RolActionTypes,
): RolState => {
    switch (action.type) {
        case ADD_ROL:
            return {
                ...state,
                rol: action.rol,
            };
        case UPDATE_ROL:
            return {
                ...state,
                rol: action.rol,
            };
        case DELETE_ROL:
            return {
                ...state,
                rol: action.rol,
            };
        case GETALL_ROLES:
            return {
                ...state,
                roles: action.roles,
            };
        case GET_BY_ID_ROL:
            return {
                ...state,
                rol: action.rol,
            };
        case ERROR_ROL:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        default:
            return state;
    }
};
