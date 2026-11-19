import { Dispatch } from 'react';
import { AppActions } from '../types/appactions.type';
import {
    GET_LOCK,
    DELETE_LOCK,
    DELETE_LOCK_FAILURE,
    DELETE_LOCK_SUCCESS,
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
import ApiLock from '../services/api-lock.service';

const api = ApiLock();

export const GetLock = (): AppActions => ({
    type: GET_LOCK,
});

export const GetLockSuccess = (locks: BloqueoDto[]): AppActions => ({
    type: GET_LOCK_SUCCESS,
    locks: locks,
});

export const GetLockFailure = (error: any): AppActions => ({
    type: GET_LOCK_FAILURE,
    error: error,
});

export const UpdateLock = (): AppActions => ({
    type: UPDATE_LOCK,
});

export const UpdateLockSuccess = (lock: BloqueoDto): AppActions => ({
    type: UPDATE_LOCK_SUCCESS,
    lock: lock,
});

export const UpdateLockFailure = (error: any): AppActions => ({
    type: UPDATE_LOCK_FAILURE,
    error: error,
});

export const InsertLock = (): AppActions => ({
    type: INSERT_LOCK,
});

export const InsertLockSuccess = (lock: BloqueoDto): AppActions => ({
    type: INSERT_LOCK_SUCCESS,
    lock: lock,
});

export const InsertLockFailure = (error: any): AppActions => ({
    type: INSERT_LOCK_FAILURE,
    error: error,
});

export const DeleteLock = (): AppActions => ({
    type: DELETE_LOCK,
});

export const DeleteLockSuccess = (lock: BloqueoDto): AppActions => ({
    type: DELETE_LOCK_SUCCESS,
    lock: lock,
});

export const DeleteLockFailure = (error: any): AppActions => ({
    type: DELETE_LOCK_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startGetLock = () => async (dispatch: Dispatch<AppActions>) => {
    dispatch(GetLock());
    const response = await api.GetLock();
    if (response?.isSuccess == null) {
        dispatch(GetLockFailure(response));
    } else {
        dispatch(GetLockSuccess(response.data));
    }
};

export const startUpdateLock =
    (lock: BloqueoDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateLock());
        const response = await api.UpdateLock(lock);

        if (response?.isSuccess == null) {
            dispatch(UpdateLockFailure(response.data));
        } else {
            dispatch(UpdateLockSuccess(response.data));
        }
    };

export const startInsertLock =
    (lock: BloqueoDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(InsertLock());
        const response = await api.InsertLock(lock);

        if (response?.isSuccess == null) {
            dispatch(InsertLockFailure(response.data));
        } else {
            dispatch(InsertLockSuccess(response.data));
        }
    };

export const startDeleteLock =
    (lock: BloqueoDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(DeleteLock());
        const response = await api.DeleteLock(lock);

        if (response?.isSuccess == null) {
            dispatch(DeleteLockFailure(response.data));
        } else {
            dispatch(DeleteLockSuccess(response.data));
        }
    };
