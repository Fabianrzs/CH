import axios from "axios";
import StorageService from "./storage.service";
import { routebuaapi, routega2api } from '../utils/baserouteapi.route';
import { NovedadesAfiliadoDto } from "../types/novedad.model.type";


const ApiNovedades = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const postInsertarNovedadAfiliado = (novedad: NovedadesAfiliadoDto) =>
    apiAxios
        .post(`cronograma/InsertarNovedadAfiliado`, novedad, {
            baseURL: routega2api,
        })
        .then(res => res.data);

    const getObtenerNovedadesAfiliado = (
        idType: number,
        idNumber: string,
    )  =>
    apiAxios
        .get(`cronograma/ObtenerNovedadesAfiliado`, {
            params: {
                tipoDocumentoId: idType,
                numeroDocumento: idNumber,
            },
            baseURL:routega2api,
        })
        .then(res =>res.data)
    
    const getObtenerNovedadesUnidadEjecutora = () =>
    apiAxios
        .get(`cronograma/ObtenerNovedadesUnidadEjecutora`, {
            baseURL:routega2api,
        })
        .then(res => res.data);
        
            

    return {
        postInsertarNovedadAfiliado,
        getObtenerNovedadesAfiliado,
        getObtenerNovedadesUnidadEjecutora,
    };
};

export default ApiNovedades;