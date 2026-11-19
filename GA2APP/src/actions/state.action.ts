import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import {
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
import ApiState from '../services/api-state.service';

const api = ApiState();

export const GetState = (): AppActions => ({
    type: GET_STATE,
});

export const GetStateSuccess = (state: EstadoDto[]): AppActions => ({
    type: GET_STATE_SUCCESS,
    allstates: state,
});

export const GetStateFailure = (error: any): AppActions => ({
    type: GET_STATE_FAILURE,
    error: error,
});

export const UpdateState = (): AppActions => ({
    type: UPDATE_STATE,
});

export const UpdateStateSuccess = (state: EstadoDto): AppActions => ({
    type: UPDATE_STATE_SUCCESS,
    states: state,
});

export const UpdateStateFailure = (error: any): AppActions => ({
    type: UPDATE_STATE_FAILURE,
    error: error,
});

export const InsertState = (): AppActions => ({
    type: INSERT_STATE,
});

export const InsertStateSuccess = (state: EstadoDto): AppActions => ({
    type: INSERT_STATE_SUCCESS,
    states: state,
});

export const InsertStateFailure = (error: any): AppActions => ({
    type: INSERT_STATE_FAILURE,
    error: error,
});

export const DeleteState = (): AppActions => ({
    type: DELETE_STATE,
});

export const DeleteStateSuccess = (state: EstadoDto): AppActions => ({
    type: DELETE_STATE_SUCCESS,
    states: state,
});

export const DeleteStateFailure = (error: any): AppActions => ({
    type: DELETE_STATE_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startGetState = () => async (dispatch: Dispatch<AppActions>) => {
    dispatch(GetState());
    const response = await api.GetState();
    if (response?.isSuccess == null) {
        dispatch(GetStateFailure(response));
    } else {
        dispatch(GetStateSuccess(response.data));

    }
};

export const startUpdateState =
    (states: EstadoDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateState());
        const response = await api.UpdateState(states);
        if (response?.isSuccess == null) {
            dispatch(UpdateStateFailure(response));
        } else {
            dispatch(UpdateStateSuccess(response.data));
            toast.success(response.returnMessage);
        }
    };

export const startInsertState =
    (states: EstadoDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(InsertState());
        const response = await api.InsertState(states);
        if (response?.isSuccess == null) {
            dispatch(InsertStateFailure(response));
        } else {
            dispatch(InsertStateSuccess(response.data));
            toast.success(response.returnMessage);
        }
    };

export const startDeleteState =
    (states: EstadoDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(DeleteState());
        const response = await api.DeleteState(states);
        if (response?.isSuccess == null) {
            dispatch(DeleteStateFailure(response));
        } else {
            dispatch(DeleteStateSuccess(response.data));
        }
    };
