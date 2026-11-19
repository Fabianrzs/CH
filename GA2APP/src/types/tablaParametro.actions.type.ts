import { DataEdit } from './tablaParametro.model.type';


export const GET_DATA = 'GET_DATA';
export const GET_DATA_SUCCESS = 'GET_DATA_SUCCESS';
export const GET_DATA_FAILURE = 'GET_DATA_FAILURE';
export const INSERT_DATA = 'INSERT_DATA';
export const INSERT_DATA_SUCCESS = 'INSERT_DATA_SUCCESS';
export const INSERT_DATA_FAILURE = 'INSERT_DATA_FAILURE';
export const UPDATE_DATA = 'UPDATE_DATA';
export const UPDATE_DATA_SUCCESS = 'UPDATE_DATA_SUCCESS';
export const UPDATE_DATA_FAILURE = 'UPDATE_DATA_FAILURE';
export const DELETE_DATA = 'DELETE_DATA';
export const DELETE_DATA_SUCCESS = 'DELETE_DATA_SUCCESS';
export const DELETE_DATA_FAILURE = 'DELETE_DATA_FAILURE';
export const ON_ERROR = 'ON_ERROR';

export interface GetData {
    type: 'GET_DATA';
}

export interface GetDataSuccess {
    type: 'GET_DATA_SUCCESS';
    alldata: DataEdit[];
}

export interface GetDataFailure {
    type: 'GET_DATA_FAILURE';
    error: any;
}

export interface UpdateData {
    type: 'UPDATE_DATA';
}

export interface UpdateDataSuccess {
    type: 'UPDATE_DATA_SUCCESS';
    data: DataEdit;
}

export interface UpdateDataFailure {
    type: 'UPDATE_DATA_FAILURE';
    error: any;
}

export interface InsertData {
    type: 'INSERT_DATA';
}

export interface InsertDataSuccess {
    type: 'INSERT_DATA_SUCCESS';
    data: DataEdit;
}

export interface InsertDataFailure {
    type: 'INSERT_DATA_FAILURE';
    error: any;
}

export interface DeleteData {
    type: 'DELETE_DATA';
}

export interface DeleteDataSuccess {
    type: 'DELETE_DATA_SUCCESS';
    data: DataEdit;
}

export interface DeleteDataFailure {
    type: 'DELETE_DATA_FAILURE';
    error: any;
}

export interface OnErrorData {
    type: 'ON_ERROR';
    error: any;
}

export type DataActionTypes =
    | GetData
    | GetDataSuccess
    | GetDataFailure
    | UpdateData
    | UpdateDataSuccess
    | UpdateDataFailure
    | InsertData
    | InsertDataSuccess
    | InsertDataFailure
    | DeleteData
    | DeleteDataSuccess
    | DeleteDataFailure
    | OnErrorData;
