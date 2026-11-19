import axios from 'axios';
import StorageService from './storage.service';
import { routebuaapi } from '../utils/baserouteapi.route';
import { AfiliacionesExtraordinariasDTO, ConsultaAfiliacionesRequestDTO, TIPOAFILIACIONDTO, } from '../types/typeAffiliate.type';

const ApiTypeAffiliate = () => {
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

    const GetTypeAffiliate = (tipoAfiliacion: TIPOAFILIACIONDTO) =>
        apiAxios
            .post('obtenerTiposAfiliacion', tipoAfiliacion, {
                baseURL: routebuaapi,
            })
            .then(res => res.data);

    const GetAffiliateExtraordinary = (afiliacionExtraordinaria: ConsultaAfiliacionesRequestDTO) =>
        apiAxios
            .post('obtenerAfiliacionByIdentificacion', afiliacionExtraordinaria, {
                baseURL: routebuaapi,
            })
            .then(res => res.data);


    const PostNewExtraordinaryAffiliate = (nuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO) =>
        apiAxios
            .post('InsertarAfiliacion', nuevaAfiliacionExtraordinaria, {
                baseURL: routebuaapi,
            })
            .then(res => res.data);

    const PostUpdateExtraordinaryAffiliate = (actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO) =>
        apiAxios
            .post('ActualizarAfiliacion', actualizarAfiliacionExtraordinaria, {
                baseURL: routebuaapi,
            })
            .then(res => res.data);


    const DeleteAffiliate = (eliminarAfiliacion: AfiliacionesExtraordinariasDTO) =>
        apiAxios
            .delete(`/EliminarAfiliacion?request=${eliminarAfiliacion.idAfiliacion}`, {
                baseURL: routebuaapi,
            })
            .then(res => res.data);

    return {
        GetTypeAffiliate,
        GetAffiliateExtraordinary,
        PostNewExtraordinaryAffiliate,
        PostUpdateExtraordinaryAffiliate,
        DeleteAffiliate
    };
};

export default ApiTypeAffiliate;
