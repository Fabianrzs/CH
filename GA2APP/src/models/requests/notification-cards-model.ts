export interface NotificationCardModel {
    message: string;
    type: NotificationCardModelEnum;
    image: string | null;
}

export enum NotificationCardModelEnum {
    success,
    error,
    info,
    warn,
    dark,
}
