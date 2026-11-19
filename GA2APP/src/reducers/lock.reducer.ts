import { LockForm } from '../pages/locks/constants/form.model.type';
import {
    LockActionTypes,
    DELETE_LOCK,
    DELETE_LOCK_FAILURE,
    DELETE_LOCK_SUCCESS,
    GET_LOCK,
    GET_LOCK_FAILURE,
    GET_LOCK_SUCCESS,
    INSERT_LOCK,
    INSERT_LOCK_FAILURE,
    INSERT_LOCK_SUCCESS,
    UPDATE_LOCK,
    UPDATE_LOCK_FAILURE,
    UPDATE_LOCK_SUCCESS,
    ON_ERROR,
} from '../types/lock.action.type';
import { BloqueoDto } from '../types/lock.model.type';

export interface LockState {
    id: number;
    lock: BloqueoDto;
    locks: BloqueoDto[];
    error: any;
    loading: boolean;
}

const LockStateInitial: LockState = {
    id: 0,
    lock: LockForm,
    locks: [],
    error: null,
    loading: false,
};

export const LockReducer = (
    state: LockState = LockStateInitial,
    action: LockActionTypes,
): LockState => {
    switch (action.type) {
        case GET_LOCK:
            return {
                ...state,
                loading: true,
            };
        case GET_LOCK_SUCCESS:
            return {
                ...state,
                locks: action.locks,
                loading: false,
            };
        case GET_LOCK_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_LOCK:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_LOCK_SUCCESS:
            return {
                ...state,
                lock: action.lock,
                loading: false,
            };
        case UPDATE_LOCK_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case INSERT_LOCK:
            return {
                ...state,
                loading: true,
            };
        case INSERT_LOCK_SUCCESS:
            return {
                ...state,
                lock: action.lock,
                loading: false,
            };
        case INSERT_LOCK_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case DELETE_LOCK:
            return {
                ...state,
                loading: true,
            };
        case DELETE_LOCK_SUCCESS:
            return {
                ...state,
                lock: action.lock,
                loading: false,
            };
        case DELETE_LOCK_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case ON_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        default:
            return state;
    }
};
