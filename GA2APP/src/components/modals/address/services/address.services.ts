import axios from 'axios';
import StorageService from '../../../../services/storage.service';
import { routega2api } from '../../../../utils/baserouteapi.route';

const ApiAddress = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
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

    const ObtenerDireccionesTiposRequest = async () => 
    apiAxios
        .get('catalogos/obtenerdireccionestipos', {
            baseURL: routega2api
        })
        .then((res: any) => res.data);

    const ObtenerDireccionesCardinalRequest = async () => 
        apiAxios
            .get('catalogos/obtenerdireccionescardinales', {
                baseURL: routega2api
            })
            .then((res: any) => res.data);

    const ObtenerDireccionesAbecedariosRequest = async () => 
        apiAxios
            .get('catalogos/obtenerdireccionesabecedarios', {
                baseURL: routega2api
            })
            .then((res: any) => res.data);

    const ObtenerDireccionesBisRequest = async () => 
        apiAxios
            .get('catalogos/obtenerdireccionesbis', {
                baseURL: routega2api
            })
            .then((res: any) => res.data);


    return {
        ObtenerDireccionesTiposRequest,
        ObtenerDireccionesCardinalRequest,
        ObtenerDireccionesAbecedariosRequest,
        ObtenerDireccionesBisRequest
    };
};

export default ApiAddress;

