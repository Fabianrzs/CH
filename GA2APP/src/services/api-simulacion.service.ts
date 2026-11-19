/* eslint-disable prettier/prettier */
import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';

const ApiSimulacion = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}Simulacion`,
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

   

	const postGuardarReporteSimulacion = async (request: any) =>
		apiAxios
			.post('GuardarReporteSimulacion', request)
			.then((res: any) => res.data);

    return {

        postGuardarReporteSimulacion,   
        
    };
};

export default ApiSimulacion;
