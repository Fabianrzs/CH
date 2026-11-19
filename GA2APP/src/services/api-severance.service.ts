import axios from 'axios';
import { routega2api } from '../utils/baserouteapi.route';
import { RequestRequestSeveranceModel } from '../models/requests/request-request-severance.model';
import StorageService from './storage.service';

const SeveranceApi = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}cesantias`,
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getAllRequestSeverance = async (data: RequestRequestSeveranceModel) => 
        apiAxios
            .post('obtenerTramiteCesantias', data)
            .then((res: any) => res.data);

    const getAllInformationBasicServerance = (numeroIdentificacion: number) =>
        apiAxios
            .get(`InformacionBasicaCesantias?numeroIdentificacion=${numeroIdentificacion}`)
            .then((res: any) => res.data);

    return {
        getAllRequestSeverance,
        getAllInformationBasicServerance
    };
};

export default SeveranceApi;
