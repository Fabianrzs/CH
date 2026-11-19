import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { BloqueoDto } from '../types/lock.model.type';

const ApiLock = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin',
           
        },
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async config => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const GetLock = () =>
        apiAxios
            .get('bloqueo/ObtenerBloqueo', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const UpdateLock = async (Lock: BloqueoDto) => 
        apiAxios
            .patch(`bloqueo/ActualizarBloqueo`, Lock, {
                baseURL: routega2api  
            })
            .then(res => res.data);

    const InsertLock = async (Lock: BloqueoDto) => 
        apiAxios
            .post(`bloqueo/CrearBloqueo`, Lock, {
                baseURL: routega2api  
            })
            .then(res => res.data);

    const DeleteLock = async (Lock: BloqueoDto) => 
        apiAxios
            .patch(`bloqueo/EliminarBloqueo`, Lock, {
                baseURL: routega2api
            })
            .then(res => res.data);

    return {
        GetLock,
        UpdateLock,
        InsertLock,
        DeleteLock,
    };
};

export default ApiLock;
