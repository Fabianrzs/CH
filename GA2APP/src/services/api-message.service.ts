import axios from 'axios';
import { MensajeDto } from '../types/message.model.type';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';

const ApiMessage = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
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

    const GetMessage = async (Lock: MensajeDto) => 
        apiAxios
            .patch(`mensaje/GetMensaje`, Lock, {
                baseURL: routega2api
            })
            .then(res => res.data);

    return {
        GetMessage,
    };
};

export default ApiMessage;
