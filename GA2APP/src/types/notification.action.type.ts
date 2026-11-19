import { NotificacionDto } from './notification.model.type';

// Especificacion de accion
export const GET_NOTIFICATION = 'GET_NOTIFICATION';
export const GET_NOTIFICATION_SUCCESS = 'GET_NOTIFICATION_SUCCESS';
export const GET_NOTIFICATION_FAILURE = 'GET_NOTIFICATION_FAILURE';
export const UPDATE_NOTIFICATION = 'UPDATE_NOTIFICATION';
export const UPDATE_NOTIFICATION_SUCCESS = 'UPDATE_NOTIFICATION_SUCCESS';
export const UPDATE_NOTIFICATION_FAILURE = 'UPDATE_NOTIFICATION_FAILURE';
export const INSERT_NOTIFICATION = 'INSERT_NOTIFICATION';
export const INSERT_NOTIFICATION_SUCCESS = 'INSERT_NOTIFICATION_SUCCESS';
export const INSERT_NOTIFICATION_FAILURE = 'INSERT_NOTIFICATION_FAILURE';
export const DELETE_NOTIFICATION = 'DELETE_NOTIFICATION';
export const DELETE_NOTIFICATION_SUCCESS = 'DELETE_NOTIFICATION_SUCCESS';
export const DELETE_NOTIFICATION_FAILURE = 'DELETE_NOTIFICATION_FAILURE';

export const ON_ERROR = 'ON_ERROR';

export interface GetNotification {
    type: 'GET_NOTIFICATION';
}

export interface GetNotificationSuccess {
    type: 'GET_NOTIFICATION_SUCCESS';
    notifications: NotificacionDto[];
}

export interface GetNotificationFailure {
    type: 'GET_NOTIFICATION_FAILURE';
    error: any;
}

export interface UpdateNotification {
    type: 'UPDATE_NOTIFICATION';
}

export interface UpdateNotificationSuccess {
    type: 'UPDATE_NOTIFICATION_SUCCESS';
    notification: NotificacionDto;
}

export interface UpdateNotificationFailure {
    type: 'UPDATE_NOTIFICATION_FAILURE';
    error: any;
}

export interface InsertNotification {
    type: 'INSERT_NOTIFICATION';
}

export interface InsertNotificationSuccess {
    type: 'INSERT_NOTIFICATION_SUCCESS';
    notification: NotificacionDto;
}

export interface InsertNotificationFailure {
    type: 'INSERT_NOTIFICATION_FAILURE';
    error: any;
}

export interface DeleteNotification {
    type: 'DELETE_NOTIFICATION';
}

export interface DeleteNotificationSuccess {
    type: 'DELETE_NOTIFICATION_SUCCESS';
    notification: NotificacionDto;
}

export interface DeleteNotificationFailure {
    type: 'DELETE_NOTIFICATION_FAILURE';
    error: any;
}

export interface OnErrornotification {
    type: 'ON_ERROR';
    error: any;
}

export type NotificationActionTypes =
    | GetNotification
    | GetNotificationSuccess
    | GetNotificationFailure
    | UpdateNotification
    | UpdateNotificationSuccess
    | UpdateNotificationFailure
    | InsertNotification
    | InsertNotificationSuccess
    | InsertNotificationFailure
    | DeleteNotification
    | DeleteNotificationSuccess
    | DeleteNotificationFailure
    | OnErrornotification;
