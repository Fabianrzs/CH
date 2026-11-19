import axios from 'axios';
import StorageService from './storage.service';
import {  routega2api } from '../utils/baserouteapi.route';


const ApiEscritura = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}`,

        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

   
    const postEscrituras = async (data: any) =>         
        apiAxios
            .post('CrearEscritura', data)
            .then((res: any) => res.data);

    const getObtenerPuntoAtencion = () =>
        apiAxios
            .get('PuntoAtencion/ObtenerPuntoAtencion')
            .then(res => res.data);

    const posObtnerEscrituras = async (data: any) =>         
        apiAxios
            .post('ObtenerEscritura', data)
            .then((res: any) => res.data);
   
    return {
      
        postEscrituras,
        getObtenerPuntoAtencion,
        posObtnerEscrituras
    }
       
};

export default ApiEscritura;
