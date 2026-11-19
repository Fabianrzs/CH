import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { EstadoDto } from '../types/state.model.type';

const ApiState = () => {
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

    const GetState = () =>
        apiAxios
            .get('estado/ObtenerEstado', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const UpdateState = async (states: EstadoDto) => 
        apiAxios
            .patch(`estado/ActualizarEstado`, states, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const InsertState = async (states: EstadoDto) => 
        apiAxios
            .post(`estado/CrearEstado`, states, {
                baseURL: routega2api 
            })
            .then(res => res.data);

    const DeleteState = async (states: EstadoDto) => 
        apiAxios
            .patch(`estado/EliminarEstado`, states, {
                baseURL: routega2api
            })
            .then(res => res.data);

    return {
        GetState,
        UpdateState,
        InsertState,
        DeleteState,
    };
};

export default ApiState;
