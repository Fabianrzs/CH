import { EstadoDto } from './state.model.type';


export const GET_STATE = 'GET_STATE';
export const GET_STATE_SUCCESS = 'GET_STATE_SUCCESS';
export const GET_STATE_FAILURE = 'GET_STATE_FAILURE';
export const INSERT_STATE = 'INSERT_STATE';
export const INSERT_STATE_SUCCESS = 'INSERT_STATE_SUCCESS';
export const INSERT_STATE_FAILURE = 'INSERT_STATE_FAILURE';
export const UPDATE_STATE = 'UPDATE_STATE';
export const UPDATE_STATE_SUCCESS = 'UPDATE_STATE_SUCCESS';
export const UPDATE_STATE_FAILURE = 'UPDATE_STATE_FAILURE';
export const DELETE_STATE = 'DELETE_STATE';
export const DELETE_STATE_SUCCESS = 'DELETE_STATE_SUCCESS';
export const DELETE_STATE_FAILURE = 'DELETE_STATE_FAILURE';
export const ON_ERROR = 'ON_ERROR';

export interface GetState {
    type: 'GET_STATE';
}

export interface GetStateSuccess {
    type: 'GET_STATE_SUCCESS';
    allstates: EstadoDto[];
}

export interface GetStateFailure {
    type: 'GET_STATE_FAILURE';
    error: any;
}

export interface UpdateState {
    type: 'UPDATE_STATE';
}

export interface UpdateStateSuccess {
    type: 'UPDATE_STATE_SUCCESS';
    states: EstadoDto;
}

export interface UpdateStateFailure {
    type: 'UPDATE_STATE_FAILURE';
    error: any;
}

export interface InsertState {
    type: 'INSERT_STATE';
}

export interface InsertStateSuccess {
    type: 'INSERT_STATE_SUCCESS';
    states: EstadoDto;
}

export interface InsertStateFailure {
    type: 'INSERT_STATE_FAILURE';
    error: any;
}

export interface DeleteState {
    type: 'DELETE_STATE';
}

export interface DeleteStateSuccess {
    type: 'DELETE_STATE_SUCCESS';
    states: EstadoDto;
}

export interface DeleteStateFailure {
    type: 'DELETE_STATE_FAILURE';
    error: any;
}

export interface OnErrorState {
    type: 'ON_ERROR';
    error: any;
}

export type StateActionTypes =
    | GetState
    | GetStateSuccess
    | GetStateFailure
    | UpdateState
    | UpdateStateSuccess
    | UpdateStateFailure
    | InsertState
    | InsertStateSuccess
    | InsertStateFailure
    | DeleteState
    | DeleteStateSuccess
    | DeleteStateFailure
    | OnErrorState;
