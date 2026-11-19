import {
    ADD_USER,
    DELETE_USER,
    ERROR_ACTIONS,
    LOAD_USERS,
    LOAD_USERS_INITIAL,
    UPDATE_USER,
    UserActionTypes,
} from '../types/user.actions.type';
import { User } from '../types/user.type';

export interface UserState {
    users: User[];
    user: User;
    id: string;
    error: any;
    loading: boolean;
}

const InitialState: UserState = {
    users: [],
    user: {} as User,
    id: '',
    error: null,
    loading: false
};

export const UserReducer = (
    state: UserState = InitialState,
    action: UserActionTypes,
): UserState => {
    switch (action.type) {
        case LOAD_USERS:
            return {
                ...state,
                users: action.users,
                loading: false
            };
        case LOAD_USERS_INITIAL:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_USER:
            return {
                ...state,
                user: action.user,
            };
        case ADD_USER:
            return {
                ...state,
                user: action.user,
            };
        case DELETE_USER:
            return {
                ...state,
                id: action.id,
            };
        case ERROR_ACTIONS:
            return {
                ...state,
                error: action.error,
                loading: false
            };
        default:
            return state;
    }
};
