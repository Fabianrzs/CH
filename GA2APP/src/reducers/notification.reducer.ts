import { NotificationForm } from '../pages/notifications/constants/form.model.type';
import {
    NotificationActionTypes,
    DELETE_NOTIFICATION,
    DELETE_NOTIFICATION_FAILURE,
    DELETE_NOTIFICATION_SUCCESS,
    GET_NOTIFICATION,
    GET_NOTIFICATION_FAILURE,
    GET_NOTIFICATION_SUCCESS,
    INSERT_NOTIFICATION,
    INSERT_NOTIFICATION_FAILURE,
    INSERT_NOTIFICATION_SUCCESS,
    UPDATE_NOTIFICATION,
    UPDATE_NOTIFICATION_FAILURE,
    UPDATE_NOTIFICATION_SUCCESS,
    ON_ERROR,
} from '../types/notification.action.type';
import { NotificacionDto } from '../types/notification.model.type';

export interface NotificationState {
    id: number;
    notification: NotificacionDto;
    notifications: NotificacionDto[];
    error: any;
    loading: boolean;
}

const NotificationStateInitial: NotificationState = {
    id: 0,
    notification: NotificationForm,
    notifications: [],
    error: null,
    loading: false,
};

export const notificationReducer = (
    state: NotificationState = NotificationStateInitial,
    action: NotificationActionTypes,
): NotificationState => {
    switch (action.type) {
        case GET_NOTIFICATION:
            return {
                ...state,
                loading: true,
            };
        case GET_NOTIFICATION_SUCCESS:
            return {
                ...state,
                notifications: action.notifications,
                loading: false,
            };
        case GET_NOTIFICATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_NOTIFICATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_NOTIFICATION_SUCCESS:
            return {
                ...state,
                notification: action.notification,
                loading: false,
            };
        case UPDATE_NOTIFICATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case INSERT_NOTIFICATION:
            return {
                ...state,
                loading: true,
            };
        case INSERT_NOTIFICATION_SUCCESS:
            return {
                ...state,
                notification: action.notification,
                loading: false,
            };
        case INSERT_NOTIFICATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case DELETE_NOTIFICATION:
            return {
                ...state,
                loading: true,
            };
        case DELETE_NOTIFICATION_SUCCESS:
            return {
                ...state,
                notification: action.notification,
                loading: false,
            };
        case DELETE_NOTIFICATION_FAILURE:
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
