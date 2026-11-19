import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { CierreMensualModel } from '../types/cierre-mensual.model.type';

const ApiCierreMensual = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}`,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });


    const getListaProgramacionCierre = () => 
    apiAxios             
        .get(`ConsultarProgramacionCierreMensualCuentas`,{
            baseURL: routega2api,
        })
        .then(res => res.data); 
    

    const  setCierreMensual= async (data: CierreMensualModel) => apiAxios
            .post(`CierreMensual/ProgramarCierreMensualCuentas`, data)
            .then((res: any) => res.data)

    const getIPC = () =>
    apiAxios
        .get('ObtenerIPC', {
            baseURL: routega2api,
        })
        .then(res => res.data);

  
    return {   
        setCierreMensual, 
        getIPC, 
        getListaProgramacionCierre,  
    };
};

export default ApiCierreMensual;
