/* eslint-disable prettier/prettier */
import axios from 'axios';
import moment from 'moment';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { procesoAplicacionMasivaPago, procesoListarAplicacionMasivaPago } from '../types/producto.model.type';
import ResumenReporteContable from '../pages/contable/types/resumenReporteContable.type';
import { Response } from '../types/response.type';
import { TotalReporteCierreMensualContable } from '../pages/contable/types/TotalReporteCierreMensualContable';

const ApiproductoClients = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}credito`,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });


    const ObtenerProductoByidentificacionRequest = (request: any) =>
        apiAxios
            .post('credito/obtenerProductoCliente', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const ObtenerProductosByidentificacionRequest = (request: any) =>
        apiAxios
            .post('credito/obtenerProductosCliente', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);



    const generarListaAplicacionDePago = (unidadEjecutora: number, tipoCredito: number, fechaAplicacion: Date) =>
        apiAxios
            .post('cartera/GenerarLista', {
                unidadEjecutora,
                tipoCredito,
                fechaAplicacion: moment(fechaAplicacion).format('YYYY-MM-DD'),
            },
                {
                    baseURL: routega2api,
                })
            .then(res => res.data);


    const aplicacionDePago = (request: any) =>
        apiAxios
            .post('credito/AplicarPago', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);


    // const aplicacionDePago = async (request: any) => {
    //     const apitokenCrsf = await ApiToken();
    //     return apiAxios
    //         .post(`credito/AplicarPago`, request, {
    //             headers: {
    //                 'GA2-XSRF-TOKEN': apitokenCrsf[1] ?? ''
    //             },
    //             baseURL: routega2api,
    //         })
    //         .then(res => res.data)
    //         .catch(res => res);
    // }

    const getAllAplicacionPago = (idCredito: string) =>
        apiAxios
            .get(`cartera/ObtenerAplicacionPago?request=${idCredito}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getCuotasAdelantadas = (idCredito: string) =>
        apiAxios
            .get(`cartera/ObtenerCuotasAdelantadas?idCredito=${idCredito}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postCierreDiarioCartera = (actualizacionDatos: boolean) =>
        apiAxios
            .post(`credito/CierreDiarioCartera?request=${actualizacionDatos}`,
                {}, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postAplicacionMasivaPagos = (aplicacionMasiva: procesoAplicacionMasivaPago, archivo: FormData) =>
        apiAxios
            .post(`cartera/CargarArchivoAplicacionPagoMasivo/${aplicacionMasiva.fechaAplicacion}&${aplicacionMasiva.valor}&${aplicacionMasiva.formatoAplicar}&${aplicacionMasiva.aplicanPagos}`,
                archivo, {
                baseURL: routega2api,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                }

            })
            .then(res => res.data);


    const postCompararArchivoAplicacionLista = (idUnidadEjecutora: number, tipoCredito: number, archivo: FormData) =>
        apiAxios
            .post(`cartera/CompararArchivoAplicacionLista?unidadEjecutora=${idUnidadEjecutora}&tipoCredito=${tipoCredito}`,
                archivo, {
                baseURL: routega2api,
                headers: {
                    'Content-Type': 'multipart/form-data',
                }

            })
            .then(res => res.data);

    const generarReporteExtractoMensual = (numeroCredito: string, tipoCredito: number, anio: number, mes: number) =>
        apiAxios
            .get(`Reportes/ExtractoMensual`, {
                baseURL: routega2api,
                params: {
                    numeroCredito,
                    tipoCredito,
                    anio,
                    mes
                },
                responseType: 'blob',
            })
            .then(res => res.data);

    const generarReporteExtractoMasivo = (data: any) =>
        apiAxios
            .post(`Reportes/ExtractoMasivo`, data, {
                baseURL: routega2api,
                responseType: 'blob',
            })
            .then(res => res.data);

    const generarReportePlanPagosExtractoMasivo = (data: any) =>
        apiAxios
            .post(`Reportes/GenerarPlanesPagoMasivoZipBase64Async`, data, {
                baseURL: routega2api,
                responseType: 'blob',
            })
            .then(res => res.data);

    const generarReportePlanPagosExtractoMasivoAnual = (data: any) =>
        apiAxios
            .post(`Reportes/GenerarPlanesPagoMasivoAnualZipBase64Async`, data, {
                baseURL: routega2api,
                responseType: 'blob',
                timeout: 1800000
            })
            .then(res => res.data);

    const generarReporteExtractoAnual = (numeroCredito: string, tipoCredito: number, anio: number) =>
        apiAxios
            .get(`Reportes/ExtractoAnual`, {
                baseURL: routega2api,
                params: {
                    numeroCredito,
                    tipoCredito,
                    anio,
                },
                responseType: 'blob',
            })
            .then(res => res.data);

    const consultaProcesoGeneracionReporte = (request: any) =>
        apiAxios
            .post(`Reportes/ConsultaProcesoGeneracionReporte`, request, {
                baseURL: routega2api,
            },)
            .then(res => res.data);

    const cancelarProcesoGeneracionReporte = (idProceso: string) =>
        apiAxios
            .post(`Reportes/CancelarProcesoGeneracionReporte`, {}, {
                baseURL: routega2api,
                params: {
                    idProceso: idProceso
                },
            })
            .then(res => res.data);

    const eliminarProcesoGeneracionReporte = (idProceso: string) =>
        apiAxios
            .post(`Reportes/EliminarProcesoGeneracionReporte`, {}, {
                baseURL: routega2api,
                params: {
                    idProceso: idProceso
                },
            })
            .then(res => res.data);

    const generarTokenReporteMasivo = (idProceso: string) =>
        apiAxios
            .get(`Reportes/GenerarTokenReporteMasivo`, {
                baseURL: routega2api,
                params: {
                    idProceso: idProceso
                },
            })
            .then(res => res.data);

    const obtenerUriReporteMasivo = (idProceso: string, key: string) =>
        apiAxios.getUri({
            url: `${routega2api}Reportes/DescargarReporteMasivo/${idProceso}`,
            params: { key }
        });


    const getReporteInformacionContable = (fechaInicial: Date, fechaFinal: Date, cuentaContable?: string) =>
        apiAxios
            .get('Reportes/InformacionContable', {
                baseURL: routega2api,
                params: {
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    cuentaContable
                },
            })
            .then((res: any) => res.data);

    const generarExcelReporteInformacionContable = (fechaInicial: Date, fechaFinal: Date, cuentaContable?: string) =>
        apiAxios
            .get('Reportes/GenerarExcelReporteInformacionContable', {
                baseURL: routega2api,
                params: {
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    cuentaContable
                },
                responseType: 'blob',
            })
            .then((res: any) => res.data);


    const getReporteCausaciones = (paramsReq: {
        fechaInicial: Date,
        fechaFinal: Date,
        tipoCredito?: number,
        registrosPorPagina?: number,
        numeroPagina?: number,
    }) =>
        apiAxios
            .get('Reportes/Causaciones', {
                baseURL: routega2api,
                params: {
                    fechaInicial: moment(paramsReq.fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(paramsReq.fechaFinal).format('YYYY-MM-DD'),
                    tipoCredito: paramsReq.tipoCredito === 0 ? null : paramsReq.tipoCredito,
                    registrosPorPagina: paramsReq.registrosPorPagina,
                    numeroPagina: paramsReq.numeroPagina
                },
            })
            .then((res: any) => res.data);

    const getReporteTotalCausaciones = (fechaInicial: Date, fechaFinal: Date, tipoCredito?: number) =>
        apiAxios
            .get('Reportes/TotalCausaciones', {
                baseURL: routega2api,
                params: {
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito
                },
            })
            .then<Response<ResumenReporteContable>>((res: any) => res.data);

    const getReporteCierreMensualContable = (paramsReq: {
        anio: number,
        mes: number,
        tipoCredito?: number,
        registrosPorPagina?: number,
        numeroPagina?: number,
    }) =>
        apiAxios
            .get('Reportes/CierreMensualContable', {
                baseURL: routega2api,
                params: {
                    anio: paramsReq.anio,
                    mes: paramsReq.mes,
                    tipoCredito: paramsReq.tipoCredito === 0 ? null : paramsReq.tipoCredito,
                    registrosPorPagina: paramsReq.registrosPorPagina,
                    numeroPagina: paramsReq.numeroPagina
                },
            })
            .then((res: any) => res.data);

    const getReporteTotalCierreMensualContable = (anio: number, mes: number, tipoCredito?: number) =>
        apiAxios
            .get('Reportes/TotalCierreMensualContable', {
                baseURL: routega2api,
                params: {
                    anio: anio,
                    mes: mes,
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito
                },
            })
            .then<Response<TotalReporteCierreMensualContable[]>>((res: any) => res.data);

    const generarExcelReporteCierreMensualContable = (anio: number, mes: number, tipoCredito?: number) =>
        apiAxios
            .get('Reportes/GenerarExcelCierreMensualContable', {
                baseURL: routega2api,
                params: {
                    anio: anio,
                    mes: mes,
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito
                },
                responseType: 'blob',
            })
            .then((res: any) => res.data);

    const getReportePagos = (tipoFecha: number, fechaInicial: Date, fechaFinal: Date,
        tipoCredito?: number, unidadEjecutora?: number) =>
        apiAxios
            .get('Reportes/Pagos', {
                baseURL: routega2api,
                params: {
                    tipoFecha,
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito,
                    unidadEjecutora: unidadEjecutora === 0 ? null : unidadEjecutora
                },
            })
            .then((res: any) => res.data);

    const getReporteContablePagos = (fechaInicial: Date, fechaFinal: Date, tipoCredito?: number) =>
        apiAxios
            .get('Reportes/ReporteContablePagos', {
                baseURL: routega2api,
                params: {
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito
                },
            })
            .then((res: any) => res.data);

    const generarExcelReporteCausaciones = (fechaInicial: Date, fechaFinal: Date, tipoCredito?: number) =>
        apiAxios
            .get('Reportes/GenerarExcelCausaciones', {
                baseURL: routega2api,
                params: {
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito
                },
                responseType: 'blob',
            })
            .then((res: any) => res.data);

    const generarExcelReportePagos = (tipoFecha: number, fechaInicial: Date, fechaFinal: Date,
        tipoCredito?: number, unidadEjecutora?: number) =>
        apiAxios
            .get('Reportes/GenerarExcelPagos', {
                baseURL: routega2api,
                params: {
                    tipoFecha,
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito,
                    unidadEjecutora: unidadEjecutora === 0 ? null : unidadEjecutora
                },
                responseType: 'blob',
            })
            .then((res: any) => res.data);

    const generarExcelReporteContablePagos = (fechaInicial: Date, fechaFinal: Date, tipoCredito?: number) =>
        apiAxios
            .get('Reportes/GenerarExcelReporteContablePagos', {
                baseURL: routega2api,
                params: {
                    fechaInicial: moment(fechaInicial).format('YYYY-MM-DD'),
                    fechaFinal: moment(fechaFinal).format('YYYY-MM-DD'),
                    tipoCredito: tipoCredito === 0 ? null : tipoCredito
                },
                responseType: 'blob',
            })
            .then((res: any) => res.data);

    const getEquivalenciaCuentasContables = () =>
        apiAxios
            .get('Reportes/ObtenerEquivalenciaCuentasContables', {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const getTipoNovedades = () =>
        apiAxios
            .get(`Novedades/ObtenerTiposNovedadesCartera`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getTiposCausalesAceleracionCartera = () =>
        apiAxios
            .get(`Novedades/ObtenerTiposCausalesAceleracionCartera`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postCrearNovedadCartera = (request: any) =>
        apiAxios
            .post('Novedades/CrearNovedadCartera', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postAplicarPagoNovedadCartera = (request: any) =>
        apiAxios
            .post('Novedades/AplicarPagoNovedadCartera', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getObtenerNovedadesCartera = (request: any) =>
        apiAxios
            .get(`Novedades/ObtenerNovedadesCartera?request=${request}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postActualizarNovedadCartera = (request: any) =>
        apiAxios
            .post('ActualizarNovedadCartera', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const getEstadosNovedadCartera = () =>
        apiAxios
            .get(`ObtenerEstadosNovedadCartera`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getSaldoObligacion = (request: any) =>
        apiAxios
            .get(`Novedades/ObtenerSaldoObligacion?request=${request}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const getAlternativosAplicacionPagos = () =>
        apiAxios
            .get(`ObtenerAlternativosAplicacionPago`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getValidarAplicacionPago = (request: any) =>
        apiAxios
            .post(`validarAplicacionPago`, request, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getPlanDePagoLeasing = (request: any) =>
        apiAxios
            .post(`obtenerPlanPago`, request, {
                baseURL: routega2api,

            })
            .then(res => res.data);

    const PostObtenerEstadoAbonoCredito = (request: any) =>
        apiAxios
            .post(`obtenerEstadoAbonoCredito`, request,)
            .then((res: any) => res.data);

    const postCrearProductoCredito = (request: any) =>
        apiAxios
            .post('CrearProductoCredito', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCrearProductoCliente = (formData: any) => {
        const formData2 = new FormData();
        return apiAxios.post('cartera/CrearProductoCliente', formData, {
            baseURL: routega2api,
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        })
            .then(res => res.data);
    };

    const postCrearProductoClientePlanPago = (request: any) =>
        apiAxios
            .post('cartera/CrearProductoClientePlanPago', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    // const postCargueNominaRequest = async (formData: FormData) => 
    // apiAxios
    //     .post('cargarchivonomina', formData)
    //     .then(res => res.data)
    //     .catch(error => error as AxiosError);


    const ActualizarProductoRequest = async (data: any) =>
        apiAxios
            .patch('producto', data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const reversarPago = (request: any) =>
        apiAxios
            .post('credito/ReversarPago', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCreaSCnMcontCrearCuotaCartera = async (request: any) =>
        apiAxios
            .post('CrearCuotaCartera', request)
            .then((res: any) => res.data);

    const postAnularCuotaCartera = (request: any) =>
        apiAxios
            .post('/credito/AnularCuotaCartera', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCierreMensualSeven = (request: any) =>
        apiAxios
            .post('/credito/CierreMensualCarteraSeven', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCierreDiarioSevenIC = (request: any) =>
        apiAxios
            .post('/credito/CierreDiarioCausacionDiariaInteresesCorrientesSeven', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCierreDiarioSevenIM = (request: any) =>
        apiAxios
            .post('/credito/CierreDiarioCarteraInteresMoraSeven', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCierreAnualSeven = (request: any) =>
        apiAxios
            .post('/credito/CierreAnualCarteraSeven', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postCrearComprobanteCartera = (request: any) =>
        apiAxios
            .post('/cartera/CrearComprobanteCartera', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postGenerarComprobanteCausaciones = (request: any) =>
        apiAxios
            .post('/cartera/GenerarComprobanteCausaciones', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postObtenerComprobanteCartera = (request: any) =>
        apiAxios
            .post('/cartera/obtenerComprobanteCartera', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postActualizarMovimientoComprobante = (request: any) =>
        apiAxios
            .put('/cartera/ActualizarMovimientoComprobanteCartera', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);

    const postConstruirMovimientosContables = (request: any) =>
        apiAxios
            .post('/cartera/ConstruirMovimientosContables', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);


    const obtenerRestitucionInmuebleCartera = (request: any) =>
        apiAxios
            .get(`/cartera/ObtenerRestitucionInmuebleCartera?request=${request}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postActualizarPlanPagosMasivo = (request: any) =>
        apiAxios
            .post('cartera/ActualizarPlanPagosMasivo', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);


    return {
        ObtenerProductoByidentificacionRequest,
        aplicacionDePago,
        getAllAplicacionPago,
        getCuotasAdelantadas,
        postCierreDiarioCartera,
        postAplicacionMasivaPagos,
        generarListaAplicacionDePago,
        getTipoNovedades,
        getTiposCausalesAceleracionCartera,
        postCrearNovedadCartera,
        postAplicarPagoNovedadCartera,
        getObtenerNovedadesCartera,
        postActualizarNovedadCartera,
        getEstadosNovedadCartera,
        getSaldoObligacion,
        getAlternativosAplicacionPagos,
        getValidarAplicacionPago,
        getPlanDePagoLeasing,
        PostObtenerEstadoAbonoCredito,
        postCrearProductoCredito,
        postCrearProductoCliente,
        ActualizarProductoRequest,
        reversarPago,
        postCompararArchivoAplicacionLista,
        generarReporteExtractoMensual,
        generarReporteExtractoAnual,
        generarReporteExtractoMasivo,
        generarReportePlanPagosExtractoMasivo,
        generarReportePlanPagosExtractoMasivoAnual,
        consultaProcesoGeneracionReporte,
        cancelarProcesoGeneracionReporte,
        eliminarProcesoGeneracionReporte,
        generarTokenReporteMasivo,
        obtenerUriReporteMasivo,
        getReporteInformacionContable,
        getReporteCausaciones,
        getReporteTotalCausaciones,
        generarExcelReporteCausaciones,
        getReportePagos,
        generarExcelReportePagos,
        getReporteContablePagos,
        generarExcelReporteContablePagos,
        getReporteCierreMensualContable,
        getReporteTotalCierreMensualContable,
        generarExcelReporteCierreMensualContable,
        generarExcelReporteInformacionContable,
        getEquivalenciaCuentasContables,
        postCreaSCnMcontCrearCuotaCartera,
        postAnularCuotaCartera,
        postCierreMensualSeven,
        postCierreDiarioSevenIC,
        postCierreDiarioSevenIM,
        postCierreAnualSeven,
        postCrearComprobanteCartera,
        postObtenerComprobanteCartera,
        postActualizarMovimientoComprobante,
        postCrearProductoClientePlanPago,
        postGenerarComprobanteCausaciones,
        postConstruirMovimientosContables,
        obtenerRestitucionInmuebleCartera,
        ObtenerProductosByidentificacionRequest,
        postActualizarPlanPagosMasivo
    };
};

export default ApiproductoClients;