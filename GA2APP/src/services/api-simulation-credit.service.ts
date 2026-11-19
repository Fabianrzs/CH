import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { CreateSimulationClient } from '../models/requests/create-simulation-client';

const SimulacionCreditoApi = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}credito/crearSimulacionCliente`,
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const postSimulationCreditClient = async (data: CreateSimulationClient) => 
        apiAxios
        .post('obtenerSolicCreditoProducto', data)
        .then((res: any) => res.data);

    return {
        postSimulationCreditClient
    };
};

export default SimulacionCreditoApi;
