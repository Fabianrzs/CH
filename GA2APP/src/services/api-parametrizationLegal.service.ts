import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import {
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
} from '../types/parametrizationLegal.model.type';

const ApiParametrizationLegal = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async config => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const GetLegalReliefParametrization = () =>
        apiAxios
            .get('parametrizacion/ObtenerLegalAlivio', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetLegalRateParametrization = () =>
        apiAxios
            .get('parametrizacion/ObtenerLegalTasa', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetLegalGmfParametrization = () =>
        apiAxios
            .get('parametrizacion/ObtenerLegalGmf', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const UpdateLegalReliefParametrization = async (legalRelief: LegalAlivioDto) => 
        apiAxios
            .patch(`parametrizacion/CrearLegalAlivio`, legalRelief,
            {
                baseURL: routega2api
            })
            .then(res => res.data);
    
    const UpdateLegalRateParametrization = async (legalRate: LegalTasaDto) => 
        apiAxios
            .patch(`parametrizacion/ActualizarLegalTasa`, legalRate, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const UpdateLegalGmfParametrization = async (legalGmf: LegalGmfDto) => 
        apiAxios
            .patch(`parametrizacion/CrearLegalGmf`, legalGmf, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const InsertLegalRateParametrization = async (legalRate: LegalTasaDto) => 
        apiAxios
            .post(`parametrizacion/CrearLegalTasa`, legalRate, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const DeleteLegalRateParametrization = async (legalRate: LegalTasaDto) => 
        apiAxios
            .patch(`parametrizacion/EliminarLegalTasa`, legalRate, {
                baseURL: routega2api
            })
            .then(res => res.data);

    return {
        GetLegalReliefParametrization,
        GetLegalRateParametrization,
        GetLegalGmfParametrization,
        UpdateLegalReliefParametrization,
        UpdateLegalRateParametrization,
        UpdateLegalGmfParametrization,
        InsertLegalRateParametrization,
        DeleteLegalRateParametrization,
    };
};

export default ApiParametrizationLegal;
