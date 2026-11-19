import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';

import {
    LiquidacionDto,
    PlazosDto,
    SeguroDto,
    TasaDto,
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
    PlazoDto,
    PlazoTipoActorDto,
    PlazoUndejDto,
    SegurosDto,
    SeguroMunicipioDto,
    LiquidacionGeneralDto,
    ParametroTransaccionDto,
    IPCDto
} from '../types/parametrization.model.type';

const ApiParametrization = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
    });

    apiAxios.interceptors.request.use(async config => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const GetAllParametrizationPlazosGeneral = () =>
        apiAxios
            .get('cartera/obtenerParametrosGeneralesPlazoCredito', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostUpdateParametrizationPlazosGeneral = (plazosGeneral: PlazoDto) =>
        apiAxios
            .post(`cartera/actualizarParametrosGeneralesPlazoCredito`, plazosGeneral, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postAllParametrizationPlazosExcepTipoActor = (tipoCredito: number) =>
        apiAxios
            .post(`cartera/obtenerExcepcionesPlazo`, { tipoCredito }, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInsertParametrizationPlazosExcepTipoActor = (plazoTipoActor: PlazoTipoActorDto) =>
        apiAxios
            .post(`cartera/crearExcepcionPlazo`, plazoTipoActor, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postUpdateParametrizationPlazosExcepTipoActor = (plazoTipoCredito: PlazoTipoActorDto) =>
        apiAxios
            .post(`cartera/actualizarExcepcionesPlazo`, plazoTipoCredito, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postDeleteParametrizationPlazosExcepTipoActor = (plazoTipoCredito: PlazoTipoActorDto) =>
        apiAxios
            .post(`cartera/eliminarExcepcionPlazo`, plazoTipoCredito, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostAllParametrizationPlazosExcepUndej = (tipoCredito: number) =>
        apiAxios
            .post(`cartera/obtenerExcepcionPlazoPorUnidadEjecutora`, { tipoCredito }, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostInsertParametrizationPlazosExcepUndej = (plazoUniej: PlazoUndejDto) =>
        apiAxios
            .post(`cartera/crearExcepcionPlazoUnidadEjecutora`, plazoUniej, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postUpdateParametrizationPlazosExcepUniej = (plazoUniej: PlazoUndejDto) =>
        apiAxios
            .post(`cartera/actualizarExcepcionPlazoUnidadEjecutora`, plazoUniej, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postDeleteParametrizationPlazosExcepUniej = (plazoUniej: PlazoUndejDto) =>
        apiAxios
            .post(`cartera/eliminarExcepcionPlazo`, plazoUniej, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    // Seguros General
    const GetAllParametrizationSegurosGeneral = () =>
        apiAxios
            .get('cartera/obtenerSeguroGeneral', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostUpdateParametrizationSegurosGeneral = (seguroGeneral: SegurosDto) =>
        apiAxios
            .post(`cartera/actualizarSeguroGeneral`, seguroGeneral, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    // Seguros por Municipio

    const GetAllParametrizationSeguroExcepPorMunicipio = () =>
        apiAxios
            .get('cartera/obtenerExcepSeguroPorMunicipio', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostInsertParametrizationExcepSeguroPorMunicipio = (seguroMunicipio: SeguroMunicipioDto) =>
        apiAxios
            .post(`cartera/crearExcepcionPorMunicipio`, seguroMunicipio, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostUpdateParametrizationExcepSeguroPorMunicipio = (seguroMunicipio: SeguroMunicipioDto) =>
        apiAxios
            .post(`cartera/actualizarExcepSeguroPorMunicipio`, seguroMunicipio, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postDeleteParametrizationExcepSeguroPorMunicipio = (seguroMunicipio: SeguroMunicipioDto) =>
        apiAxios
            .post(`cartera/eliminarExcepcionSeguroPorMunicipio`, seguroMunicipio, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetAllParametrizationSeguroRegion = () =>
        apiAxios
            .get('ObtenerParametrosporSeguroRegion', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PutParametrizationSeguroRegion = (request: any) =>
        apiAxios
            .put(`ActualizarSeguroPorRegion`, request, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    // Liquidacion General

    const GetAllParametrizationLiquidacionGeneral = () =>
        apiAxios
            .get('cartera/ObtenerLiquidacionGeneral', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostUpdateParametrizationLiquidacionGeneral = (liquidacionGeneral: LiquidacionGeneralDto) =>
        apiAxios
            .post(`cartera/ActualizarLiquidacionGeneral`, liquidacionGeneral, {
                baseURL: routega2api,
            })
            .then(res => res.data);
    //

    const GetInsuranceParametrization = () =>
        apiAxios
            .get('parametrizacion/ObtenerSeguro', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetRateParametrization = () =>
        apiAxios
            .get('parametrizacion/ObtenerTasa', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetTermParametrization = () =>
        apiAxios
            .get('parametrizacion/ObtenerPlazo', {
                baseURL: routega2api,
            })
            .then(res => res.data);

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

    const GetSettlementParametrization = () =>
        apiAxios
            .get('parametrizacion/ObtenerLiquidacion', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const UpdateInsuranceParametrization = async (insurance: SeguroDto) =>
        apiAxios
            .patch(`parametrizacion/CrearSeguro`, insurance, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const UpdateRateParametrization = async (rate: TasaDto) =>
        apiAxios
            .patch(`parametrizacion/CrearTasa`, rate, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const UpdateTermParametrization = async (term: PlazosDto) =>
        apiAxios
            .patch(`parametrizacion/CrearPlazo`, term, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const UpdateLegalReliefParametrization = async (legalRelief: LegalAlivioDto) =>
        apiAxios
            .patch(`parametrizacion/CrearLegalAlivio`, legalRelief, {
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

    const UpdateSettlementParametrization = async (settlement: LiquidacionDto) =>
        apiAxios
            .patch(`parametrizacion/CrearLiquidacion`, settlement, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const ActualizarParametroTransaccionRequest = (PARAMETROTRANSACCIONDTO: ParametroTransaccionDto) =>
        apiAxios
            .patch('parametrizacion/ActualizarParametroTransaccion', PARAMETROTRANSACCIONDTO, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerParametroTransaccionRequest = () =>
        apiAxios
            .get('parametrizacion/ObtenerParametroTransaccion', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const CrearParametroTransaccionRequest = (PARAMETROTRANSACCIONDTO: ParametroTransaccionDto) =>
        apiAxios
            .patch('parametrizacion/CrearParametroTransaccion', PARAMETROTRANSACCIONDTO, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const CrearIPCRequest = (IPC: IPCDto) =>
        apiAxios
            .patch('parametrizacion/CrearIPC', IPC, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostSincronizarCuentas = (request: any) =>
    apiAxios
        .post(`parametrizacion/ParametrizacionCuenta`, request, {
            baseURL: routega2api,
        })
        .then(res => res.data);
    const CrearProcesosIPCRequest = (IPC: IPCDto) =>
        apiAxios
            .patch('parametrizacion/ProcesosIPC', IPC, {
                baseURL: routega2api,
            })
            .then(res => res.data);
    const ActualizarIPCRequest = (IPC: IPCDto) =>
        apiAxios
            .patch('parametrizacion/CrearIPC', IPC, {
                baseURL: routega2api,
            })
            .then(res => res.data);
    const ObtenerReporteIPCRequest = (IPC: IPCDto) =>
        apiAxios
            .get(`parametrizacion/ObtenerReporteIPC?Desde_Anio=${IPC.Desde_Anio}&Desde_Mes=${IPC.Desde_Mes}&Hasta_Anio=${IPC.Hasta_Anio}&Hasta_Mes=${IPC.Hasta_Mes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ObtenerTodoReporteIPCRequest = () =>
        apiAxios
            .get('parametrizacion/ObtenerTodoReporteIPC', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const PostParametrizacionValorInmueble = (request: any) =>
    apiAxios
        .post(`Parametrizacion/ValorInmueble`, request, {
            baseURL: routega2api,
        })
        .then(res => res.data);

    return {
        GetInsuranceParametrization,
        GetRateParametrization,
        GetTermParametrization,
        GetLegalReliefParametrization,
        GetLegalRateParametrization,
        GetLegalGmfParametrization,
        GetSettlementParametrization,
        UpdateInsuranceParametrization,
        UpdateRateParametrization,
        UpdateTermParametrization,
        UpdateLegalReliefParametrization,
        UpdateLegalRateParametrization,
        UpdateLegalGmfParametrization,
        UpdateSettlementParametrization,
        // 
        GetAllParametrizationPlazosGeneral,
        PostUpdateParametrizationPlazosGeneral,
        postInsertParametrizationPlazosExcepTipoActor,
        postAllParametrizationPlazosExcepTipoActor,
        postUpdateParametrizationPlazosExcepTipoActor,
        postDeleteParametrizationPlazosExcepTipoActor,
        PostAllParametrizationPlazosExcepUndej,
        PostInsertParametrizationPlazosExcepUndej,
        postUpdateParametrizationPlazosExcepUniej,
        postDeleteParametrizationPlazosExcepUniej,
        GetAllParametrizationSegurosGeneral,
        PostUpdateParametrizationSegurosGeneral,
        GetAllParametrizationSeguroExcepPorMunicipio,
        PostInsertParametrizationExcepSeguroPorMunicipio,
        PostUpdateParametrizationExcepSeguroPorMunicipio,
        postDeleteParametrizationExcepSeguroPorMunicipio,
        GetAllParametrizationSeguroRegion,
        PutParametrizationSeguroRegion,
        GetAllParametrizationLiquidacionGeneral,
        PostUpdateParametrizationLiquidacionGeneral,
        ActualizarParametroTransaccionRequest,
        ObtenerParametroTransaccionRequest,
        CrearParametroTransaccionRequest,
        CrearIPCRequest,
        PostSincronizarCuentas,
        CrearProcesosIPCRequest,
        ActualizarIPCRequest,
        ObtenerReporteIPCRequest,
        ObtenerTodoReporteIPCRequest,
        PostParametrizacionValorInmueble
    };
};

export default ApiParametrization;