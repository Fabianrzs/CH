import { Dispatch } from 'react';
import { AppActions } from '../types/appactions.type';
import {
    GET_NOTIFICATION,
    GET_NOTIFICATION_FAILURE,
    GET_NOTIFICATION_SUCCESS,
    DELETE_NOTIFICATION,
    DELETE_NOTIFICATION_FAILURE,
    DELETE_NOTIFICATION_SUCCESS,
    INSERT_NOTIFICATION,
    INSERT_NOTIFICATION_FAILURE,
    INSERT_NOTIFICATION_SUCCESS,
    UPDATE_NOTIFICATION,
    UPDATE_NOTIFICATION_FAILURE,
    UPDATE_NOTIFICATION_SUCCESS,
    ON_ERROR,
} from '../types/notification.action.type';
import { NotificacionDto } from '../types/notification.model.type';
import ApiNotification from '../services/api-notification.service';
import NotificationService from '../services/notification.service';
import {
    NotificationCardModel,
    NotificationCardModelEnum,
} from '../models/requests/notification-cards-model';

const api = ApiNotification();

export const GetNotification = (): AppActions => ({
    type: GET_NOTIFICATION,
});

export const GetNotificationSuccess = (
    notifications: NotificacionDto[],
): AppActions => ({
    type: GET_NOTIFICATION_SUCCESS,
    notifications: notifications,
});

export const GetNotificationFailure = (error: any): AppActions => ({
    type: GET_NOTIFICATION_FAILURE,
    error: error,
});

export const UpdateNotification = (): AppActions => ({
    type: UPDATE_NOTIFICATION,
});

export const UpdateNotificationSuccess = (
    notification: NotificacionDto,
): AppActions => ({
    type: UPDATE_NOTIFICATION_SUCCESS,
    notification: notification,
});

export const UpdateNotificationFailure = (error: any): AppActions => ({
    type: UPDATE_NOTIFICATION_FAILURE,
    error: error,
});

export const InsertNotification = (): AppActions => ({
    type: INSERT_NOTIFICATION,
});

export const InsertNotificationSuccess = (
    notification: NotificacionDto,
): AppActions => ({
    type: INSERT_NOTIFICATION_SUCCESS,
    notification: notification,
});

export const InsertNotificationFailure = (error: any): AppActions => ({
    type: INSERT_NOTIFICATION_FAILURE,
    error: error,
});

export const DeleteNotification = (): AppActions => ({
    type: DELETE_NOTIFICATION,
});

export const DeleteNotificationSuccess = (
    notification: NotificacionDto,
): AppActions => ({
    type: DELETE_NOTIFICATION_SUCCESS,
    notification: notification,
});

export const DeleteNotificationFailure = (error: any): AppActions => ({
    type: DELETE_NOTIFICATION_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startGetNotification =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetNotification());
        const response = await api.ObtenerNotificaciones();
        if (response?.isSuccess == null) {
            dispatch(GetNotificationFailure(response));
        } else {
            dispatch(GetNotificationSuccess(response.data));
        }
    };

export const startUpdateNotification =
    (notification: NotificacionDto) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateNotification());
        const response = await api.ActualizarNotificacion(notification);
        if (response?.isSuccess == null) {
            dispatch(UpdateNotificationFailure(response.data));
        } else {
            dispatch(UpdateNotificationSuccess(response.data));
            await NotificationService.showNotification({
                message: 'Notificación actualizada con éxito',
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);
        }
    };

export const startInsertNotification =
    (notification: NotificacionDto) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(InsertNotification());
        const response = await api.CrearNotificacion(notification);
        if (response?.isSuccess == null) {
            dispatch(InsertNotificationFailure(response.data));
        } else {
            dispatch(InsertNotificationSuccess(response.data));
            await NotificationService.showNotification({
                message: 'Notificación agregada con éxito',
                type: NotificationCardModelEnum.success,
            } as NotificationCardModel);
        }
    };
