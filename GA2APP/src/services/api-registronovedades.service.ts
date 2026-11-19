import axios from "axios";
import StorageService from "./storage.service";
import { routega2api } from '../utils/baserouteapi.route';
import { ConsultarAuditoriaNovedadesDto, InsertarAuditoriaNovedadesDto } from "../types/registro-novedades.model.type";

const ApiAuditoriasNovedades = () => {

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

    const getAuditoriaNovedades = (pagina: any, filas: any) =>
    apiAxios
        .get(`AuditoriaNovedades/ConsultarAuditoriaNovedades?idModulo=${1}`,{
            baseURL: routega2api,
        })
        .then(res => res.data);

    const postAuditoriaNovedades = (registroNovedad: InsertarAuditoriaNovedadesDto) =>
    apiAxios
        .post(`cronograma/InsertarAudoriaNovedades`, registroNovedad,{
            baseURL: routega2api,
        })
        .then(res => res.data);

    const getObtenerReporteNovedades = () =>
    apiAxios
    .get(`AuditoriaNovedades/ObtenerReporteNovedades`,{
        baseURL: routega2api,
    })
    .then(res => res.data);
    
        
    return {
        getAuditoriaNovedades,
        postAuditoriaNovedades,
        getObtenerReporteNovedades
    };
};

export default ApiAuditoriasNovedades;