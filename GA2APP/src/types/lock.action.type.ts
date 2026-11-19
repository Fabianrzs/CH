import { BloqueoDto } from './lock.model.type';

// Especificacion de accion
export const GET_LOCK = 'GET_LOCK';
export const GET_LOCK_SUCCESS = 'GET_LOCK_SUCCESS';
export const GET_LOCK_FAILURE = 'GET_LOCK_FAILURE';
export const UPDATE_LOCK = 'UPDATE_LOCK';
export const UPDATE_LOCK_SUCCESS = 'UPDATE_LOCK_SUCCESS';
export const UPDATE_LOCK_FAILURE = 'UPDATE_LOCK_FAILURE';
export const INSERT_LOCK = 'INSERT_LOCK';
export const INSERT_LOCK_SUCCESS = 'INSERT_LOCK_SUCCESS';
export const INSERT_LOCK_FAILURE = 'INSERT_LOCK_FAILURE';
export const DELETE_LOCK = 'DELETE_LOCK';
export const DELETE_LOCK_SUCCESS = 'DELETE_LOCK_SUCCESS';
export const DELETE_LOCK_FAILURE = 'DELETE_LOCK_FAILURE';

export const ON_ERROR = 'ON_ERROR';

export interface GetLock {
    type: 'GET_LOCK';
}

export interface GetLockSuccess {
    type: 'GET_LOCK_SUCCESS';
    locks: BloqueoDto[];
}

export interface GetLockFailure {
    type: 'GET_LOCK_FAILURE';
    error: any;
}

export interface UpdateLock {
    type: 'UPDATE_LOCK';
}

export interface UpdateLockSuccess {
    type: 'UPDATE_LOCK_SUCCESS';
    lock: BloqueoDto;
}

export interface UpdateLockFailure {
    type: 'UPDATE_LOCK_FAILURE';
    error: any;
}

export interface InsertLock {
    type: 'INSERT_LOCK';
}

export interface InsertLockSuccess {
    type: 'INSERT_LOCK_SUCCESS';
    lock: BloqueoDto;
}

export interface InsertLockFailure {
    type: 'INSERT_LOCK_FAILURE';
    error: any;
}

export interface DeleteLock {
    type: 'DELETE_LOCK';
}

export interface DeleteLockSuccess {
    type: 'DELETE_LOCK_SUCCESS';
    lock: BloqueoDto;
}

export interface DeleteLockFailure {
    type: 'DELETE_LOCK_FAILURE';
    error: any;
}

export interface OnErrorLock {
    type: 'ON_ERROR';
    error: any;
}

export type LockActionTypes =
    | GetLock
    | GetLockSuccess
    | GetLockFailure
    | UpdateLock
    | UpdateLockSuccess
    | UpdateLockFailure
    | InsertLock
    | InsertLockSuccess
    | InsertLockFailure
    | DeleteLock
    | DeleteLockSuccess
    | DeleteLockFailure
    | OnErrorLock;
