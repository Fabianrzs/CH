import { MensajeDto } from './message.model.type';

// Especificacion de accion
export const GET_MESSAGE = 'GET_MESSAGE';
export const GET_MESSAGE_SUCCESS = 'GET_MESSAGE_SUCCESS';
export const GET_MESSAGE_FAILURE = 'GET_MESSAGE_FAILURE';

export const ON_ERROR = 'ON_ERROR';

export interface GetMessage {
    type: 'GET_MESSAGE';
}

export interface GetMessageSuccess {
    type: 'GET_MESSAGE_SUCCESS';
    message: MensajeDto;
}

export interface GetMessageFailure {
    type: 'GET_MESSAGE_FAILURE';
    error: any;
}

export interface OnErrorMessage {
    type: 'ON_ERROR';
    error: any;
}

export type MessageActionTypes =
    | GetMessage
    | GetMessageSuccess
    | GetMessageFailure
    | OnErrorMessage;
