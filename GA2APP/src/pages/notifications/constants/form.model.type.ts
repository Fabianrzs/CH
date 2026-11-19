import { NotificacionDto } from '../../../types/notification.model.type';

export const NotificationForm: NotificacionDto = {
    id: '',
    mensaje: '',
    receptor: '',
    tipo: '',
    visto: false,
    emisor: '',
    fechaCreacion: new Date(),
    estado: false,
};
