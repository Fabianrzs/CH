import axios from 'axios';
import StorageService from './storage.service';
import { routega2api, routebuaapi } from '../utils/baserouteapi.route';
import { ClienteDto } from '../types/client.model.type';


const ApiClientesUnidadEjecutora = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routebuaapi}`,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const PostClientByUnidadEjecutoraRequest = async (
        data: any
    ) =>
        apiAxios
            .post( `ObtenetClientesPorUnidadEjecutora?idUnidadEjecutora=${data.idUnidadEjecutora}`)

            .then(res => res.data);

            return {
                PostClientByUnidadEjecutoraRequest
            };
};

export default ApiClientesUnidadEjecutora;
