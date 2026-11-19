import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import {
    GET_MESSAGE,
    GET_MESSAGE_FAILURE,
    GET_MESSAGE_SUCCESS,
    ON_ERROR,
} from '../types/message.action.type';
import { MensajeDto } from '../types/message.model.type';
import ApiMessage from '../services/api-message.service';

const api = ApiMessage();

export const GetMessage = (): AppActions => ({
    type: GET_MESSAGE,
});

export const GetMessageSuccess = (message: MensajeDto): AppActions => ({
    type: GET_MESSAGE_SUCCESS,
    message: message,
});

export const GetMessageFailure = (error: any): AppActions => ({
    type: GET_MESSAGE_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startGetMessage =
    (message: MensajeDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetMessage());
        const response = await api.GetMessage(message);

        if (response?.isSuccess == null) {
            dispatch(GetMessageFailure(response.data));
        } else {
            dispatch(GetMessageSuccess(response.data));
            toast(response.returnMessage);
        }
    };
