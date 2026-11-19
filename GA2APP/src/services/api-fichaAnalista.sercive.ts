import axios from 'axios';
import StorageService from './storage.service';
import {  routega2api } from '../utils/baserouteapi.route';


const ApiFichaAnalista = () => {
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



    const posGuardarFichaAnalista = async (data: any) =>         
        apiAxios
            .post('FichaAnalista/CrearFichaAnalista', data)
            .then((res: any) => res.data);

    const postObtenerFichaAnalistaPorId = async (data: any) =>         
        apiAxios
            .post('FichaAnalista/ObtenerFichaAnalista', data)
            .then((res: any) => res.data);
   
    return {
        postObtenerFichaAnalistaPorId,
        posGuardarFichaAnalista
    }
       
};

export default ApiFichaAnalista;
