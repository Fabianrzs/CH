import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { NotificacionDto } from '../types/notification.model.type';

const ApiNotification = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
    });

    apiAxios.interceptors.request.use(async config => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const ObtenerNotificaciones = () =>
        apiAxios
            .get('notificacion/obtenerNotificaciones', {
                baseURL: routega2api,
            })
            .then(res => res.data)
            .catch(error => error);

    const ActualizarNotificacion = async (Notification: NotificacionDto) => 
           apiAxios
            .patch(`notificacion/actualizarnotificacion`, Notification, {
                baseURL: routega2api 
            })
            .then(res => res.data)
            .catch(error => error)

    const CrearNotificacion = async (Notification: NotificacionDto) => 
        apiAxios
            .post(`notificacion/crearnotificacion`, Notification, {
                baseURL: routega2api    
            })
            .then(res => res.data)
            .catch(error => error)

    const DeleteNotification = async (Notification: NotificacionDto) =>  
        apiAxios
            .patch(`notificacion/DeleteNotificacion`, Notification, {
                baseURL: routega2api              
            })
            .then(res => res.data)
            .catch(error => error)

    return {
        ObtenerNotificaciones,
        ActualizarNotificacion,
        CrearNotificacion,
        DeleteNotification,
    };
};

export default ApiNotification;
