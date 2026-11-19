import axios from "axios";
import StorageService from "./storage.service";
import { routebuaapi, routega2api } from '../utils/baserouteapi.route';


const ReintegrosApi = () => {
    
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
        timeout: 900000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getTipoReintegro = () =>
    apiAxios
        .get('/Reintegros/ObtenerTiposReintegro', {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postCrearReintegro = (request: any) => 
    apiAxios
        .post('/Reintegros/CrearteReintegro',request, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postActualizarAfiliadoReintegro = (request: any) => 
    apiAxios
        .post('/Reintegros/UpdateAfiliadoReintegro',request, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const getObtenerListadoReintegros = (idReintegro: string) =>
    apiAxios
        .get(`/Reintegros/GetAfiliadoReintegro?idTipo=${idReintegro}`, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const getObtenerListadoReintegrosPorId = (idReintegro: string) =>
    apiAxios
        .get(`/Reintegros/ConsultarReintegroById?idTipo=${idReintegro}`, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postGuardarAfiliadosReintegro = (request: any) => 
    apiAxios
        .post('/Reintegros/SaveListReintegro',request, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postActualizarSaldoConceptoCuenta = (request: any) => 
    apiAxios
        .post('/cuentas/ActualizarSaldoConceptoCuenta',request, {
            baseURL: routebuaapi,
        })
        .then((res: any) => res.data);

    const postLimpiarSaldoConceptoCuenta = (request: any) => 
    apiAxios
        .post('/cuentas/ClealConceptoCuenta',request, {
            baseURL: routebuaapi,
        })
        .then((res: any) => res.data);
    
    const getConsultarListaReintegros = (request: any) => 
        apiAxios
            .get(
                '/Reintegros/ObtenerListadoReintegros', {
                params: {
                    NumeroRadicado: request?.numeroRadicado,
                    IdUnidadEjecutora: request?.idUnidadEjecutora,
                    IdTipoReintegro: request?.idTipoReintegro
                },
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const postActualizarListaAfiliados = (request: any) => 
    apiAxios
        .post('/Reintegros/UpdateListAfiliados',request, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

        
    const getConsultarBloqueoReintegro = (request: any) =>
    apiAxios
        .get(`/Reintegros/GetDataReintegro?NumeroDocumento=${request.numeroDocumento}&TipoDocumento${request.tipoDocumento}`, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postConsultarUnidadEjecutoraPorRadicado = (radicado: string) => 
    apiAxios
        .post(`/Reintegros/ObtenerCorrespondenciaReintegros?radicado=${radicado}`,{}, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postGenerarPagoReintegro = (request: any) => 
    apiAxios
        .post(`/Reintegros/GenerarPagoReintegro`,request, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postPagosReintegrosSeven = (request: any) => 
    apiAxios
        .post(`/Reintegros/PagosReintegrosSeven`,request, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);

    const postCrearMovimientoCuenta = (request: any) => 
    apiAxios
        .post('CrearLogMovimientoCuenta',request, {
            baseURL: routebuaapi,
        })
        .then((res: any) => res.data);

    const getGenerarOficio = (radicado: string) =>
    apiAxios
        .get(`/Reintegros/ReporteOficio?radicado=${radicado}`, {
            baseURL: routega2api,
            responseType: 'blob',
        })
        .then((res: any) => res.data);

    const postProcesarSaldoConceptoCuenta = (request: any) => 
    apiAxios
        .post('/cuentas/ProcesarSaldoConceptoCuenta',request, {
            baseURL: routebuaapi,
        })
        .then((res: any) => res.data);

    const postPlanillaPago = (request: any) =>
    apiAxios
    .post (
        '/Reintegros/ReportePlanilla',request,{
         baseURL: routega2api,
         responseType: 'blob',
        })
    .then(res => res.data);

    const postInsertarComunicacionesReintegros = (request: any) => 
    apiAxios
        .post('/Reintegros/InsertarComunicaciones',request, {
            baseURL: routega2api,
        })
        .then((res: any) => res.data);


    const getUpdateFechaFinal = (idReintegro: string) =>
    apiAxios
        .get(`/Reintegros/UpdateDateEndReintegro?idReintegro=${idReintegro}`, {
            baseURL: routega2api,
            responseType: 'blob',
        })
        .then((res: any) => res.data);

    return {
        getTipoReintegro,
        postCrearReintegro,
        postActualizarAfiliadoReintegro,
        postLimpiarSaldoConceptoCuenta,
        getObtenerListadoReintegros,
        getObtenerListadoReintegrosPorId,
        postGuardarAfiliadosReintegro,
        postActualizarSaldoConceptoCuenta,
        getConsultarListaReintegros,
        postActualizarListaAfiliados,
        getConsultarBloqueoReintegro,
        postConsultarUnidadEjecutoraPorRadicado,
        postGenerarPagoReintegro,
        postPagosReintegrosSeven,
        postCrearMovimientoCuenta,
        getGenerarOficio,
        postProcesarSaldoConceptoCuenta,
        postPlanillaPago,
        postInsertarComunicacionesReintegros,
        getUpdateFechaFinal
    };
};

export default ReintegrosApi;