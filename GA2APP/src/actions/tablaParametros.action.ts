import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import {
    GET_DATA,
    GET_DATA_SUCCESS,
    GET_DATA_FAILURE,
    UPDATE_DATA,
    UPDATE_DATA_SUCCESS,
    UPDATE_DATA_FAILURE,
    INSERT_DATA,
    INSERT_DATA_SUCCESS,
    INSERT_DATA_FAILURE,
    DELETE_DATA,
    DELETE_DATA_SUCCESS,
    DELETE_DATA_FAILURE,
    ON_ERROR,
} from '../types/tablaParametro.actions.type';
import { DataEdit } from '../types/tablaParametro.model.type';
import ApiCatalog from '../services/api-catalog.service';

const api = ApiCatalog();

export const GetData = (): AppActions => ({
    type: GET_DATA,
});

export const GetDataSuccess = (state: any[]): AppActions => ({
    type: GET_DATA_SUCCESS,
    alldata: state,
});

export const GetDataFailure = (error: any): AppActions => ({
    type: GET_DATA_FAILURE,
    error: error,
});

export const UpdateData = (): AppActions => ({
    type: UPDATE_DATA,
});

export const UpdateDataSuccess = (state: any): AppActions => ({
    type: UPDATE_DATA_SUCCESS,
    data: state,
});

export const UpdateDataFailure = (error: any): AppActions => ({
    type: UPDATE_DATA_FAILURE,
    error: error,
});

export const InsertData = (): AppActions => ({
    type: INSERT_DATA,
});

export const InsertDataSuccess = (state: any): AppActions => ({
    type: INSERT_DATA_SUCCESS,
    data: state,
});

export const InsertDataFailure = (error: any): AppActions => ({
    type: INSERT_DATA_FAILURE,
    error: error,
});

export const DeleteData = (): AppActions => ({
    type: DELETE_DATA,
});

export const DeleteDataSuccess = (state: any): AppActions => ({
    type: DELETE_DATA_SUCCESS,
    data: state,
});

export const DeleteDataFailure = (error: any): AppActions => ({
    type: DELETE_DATA_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});


export const startInsertData =
    (data: any) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(InsertData());
        const response = await api.postAgregarTablasParametros(data);
        if (response?.isSuccess == null) {
            dispatch(InsertDataFailure(response));
        } else {
            dispatch(InsertDataSuccess(response.data));
            toast.success(response.returnMessage);
        }
    };
