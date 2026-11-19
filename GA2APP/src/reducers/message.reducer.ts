import { MessageForm } from '../pages/home/constants/messageForm.model.type';
import {
    MessageActionTypes,
    GET_MESSAGE,
    GET_MESSAGE_FAILURE,
    GET_MESSAGE_SUCCESS,
    ON_ERROR,
} from '../types/message.action.type';
import { MensajeDto } from '../types/message.model.type';

export interface MessageState {
    id: number;
    message: MensajeDto;
    error: any;
    loading: boolean;
}

const MessageStateInitial: MessageState = {
    id: 0,
    message: MessageForm,
    error: null,
    loading: false,
};

export const MessageReducer = (
    state: MessageState = MessageStateInitial,
    action: MessageActionTypes,
): MessageState => {
    switch (action.type) {
        case GET_MESSAGE:
            return {
                ...state,
                loading: true,
            };
        case GET_MESSAGE_SUCCESS:
            return {
                ...state,
                message: action.message,
                loading: false,
            };
        case GET_MESSAGE_FAILURE:
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
