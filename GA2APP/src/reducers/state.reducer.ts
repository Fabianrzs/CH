import { StateForm } from '../pages/state/constants/form.model.type';
import {
    StateActionTypes,
    GET_STATE,
    GET_STATE_SUCCESS,
    GET_STATE_FAILURE,
    UPDATE_STATE,
    UPDATE_STATE_SUCCESS,
    UPDATE_STATE_FAILURE,
    INSERT_STATE,
    INSERT_STATE_SUCCESS,
    INSERT_STATE_FAILURE,
    DELETE_STATE,
    DELETE_STATE_SUCCESS,
    DELETE_STATE_FAILURE,
    ON_ERROR,
} from '../types/state.action.type';
import { EstadoDto } from '../types/state.model.type';

export interface StateState {
    id: number;
    allstates: EstadoDto[];
    states: EstadoDto;
    error: any;
    loading: boolean;
}

const StateStateInitial: StateState = {
    id: 0,
    allstates: [],
    states: StateForm,
    error: null,
    loading: false,
};

export const StateReducer = (
    state: StateState = StateStateInitial,
    action: StateActionTypes,
): StateState => {
    switch (action.type) {
        case GET_STATE:
            return {
                ...state,
                loading: true,
            };
        case GET_STATE_SUCCESS:
            return {
                ...state,
                allstates: action.allstates,
                loading: false,
            };
        case GET_STATE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case UPDATE_STATE:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_STATE_SUCCESS:
            return {
                ...state,
                states: action.states,
                loading: false,
            };
        case UPDATE_STATE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case INSERT_STATE:
            return {
                ...state,
                loading: true,
            };
        case INSERT_STATE_SUCCESS:
            return {
                ...state,
                states: action.states,
                loading: false,
            };
        case INSERT_STATE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case DELETE_STATE:
            return {
                ...state,
                loading: true,
            };
        case DELETE_STATE_SUCCESS:
            return {
                ...state,
                states: action.states,
                loading: false,
            };
        case DELETE_STATE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
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
