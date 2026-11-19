/* eslint-disable prettier/prettier */
import axios from 'axios';
import StorageService from './storage.service';
import { routebuaapi, routega2api } from '../utils/baserouteapi.route';
import ReporteEstadoCuentaAfiliado from '../pages/productClient/components/reportes/types/ReporteEstadoCuentaAfiliado.type';
import { Response } from '../types/response.type';
import { ReporteEstadoCuentaPlanPago } from '../pages/productClient/components/reportes/types/ReporteEstadoCuentaPlanPago.type';
import ReporteEstadoMovimientos from '../pages/productClient/components/reportes/types/ReporteEstadoMovimientos.type';
import { ReporteMensualSeguros } from '../pages/productClient/components/reportes/types/ReporteMensualSeguros.type';
import { DatosReporteCapitalCorriente } from '../pages/productClient/components/reportes/types/DatosReporteCapitalCorriente';
import { ExcelReport } from '../pages/productClient/components/reportes/types/ExcelReport.type';
import { ReporteOpcionCompra } from '../pages/productClient/components/reportes/types/ReporteOpcionCompra.type';

const ApiReportes = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}Reportes`,
        timeout: 850000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const generarListaReportes = async () => 
        apiAxios
            .post('CargarListadoReportes')
            .then((res: any) => res.data);

    const generarReportes = (request: any) =>
        apiAxios
            .post(
                `ReporteCertificadoHaberes?identificacion=${request.identificacion}`,
                {},
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);

    const generarIngresosRetenciones = (request: any) =>
        apiAxios
            .post(
                `ReporteCertificadoIngresosRetenciones?identificacion=${request.identificacion}`,
                {},
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);

    const generarEstadoDeCuenta = (request: any) =>
        apiAxios
            .post(
                `ReporteEstadoDeCuenta?identificacion=${request.identificacion}`,
                {},
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);

    const generarExtratoAnual = (request: any) =>
        apiAxios
            .post(
                'ReporteExtractoAnual', request,  
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);

            
   

    const generarExtratoMensual = (request: any) =>
        apiAxios
            .post(
                'ReporteExtractoMensual', request,
             
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);

    const generarMovimientoCartera = (request: any) =>
        apiAxios
            .post(
                `ReporteMovimientoDeCartera?identificacion=${request.identificacion}`,
                {},
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);
                
    const generarMReporte414 = (request: any) =>
    
        apiAxios
            .post('ReporteSFCF414', request, {
                
                responseType: 'blob',
            })
            
            .then(res => res.data);

    const generarMReporte416 = (request: any) =>
        apiAxios
            .post('ReporteSFCF416', request, {
                responseType: 'blob',
            })
            .then(res => res.data)
            .catch((res)=> {
            })

    const generarMReportePlano414 = (request: any) =>
        apiAxios
            .post('ReportePlanoSFCF414_V2', request, {
                responseType: 'blob',
            })
            .then(res => res.data)
            .catch((res) => {
            })

            // const generarReportePlano341 = async (request: any) => {
            //     try {
            //         const responsePromise = apiAxios.post('ReportePlanoSFCF341', request, {
            //             responseType: 'blob',
            //         });
            
            //         // Configurar el tiempo límite de 3 minutos (180000 milisegundos)
            //         const timeoutPromise = new Promise((resolve, reject) => {
            //             setTimeout(() => {
            //                 reject(new Error('Tiempo límite excedido'));
            //             }, 180000);
            //         });
            
            //         // Esperar a que se complete la acción o el tiempo límite
            //         const response = await Promise.race([responsePromise, timeoutPromise]);
            
            //         return response;
            //     } catch (error) {
            //         console.error('Error al generar el reporte:', error);
            //         throw error;
            //     }
            // };
            const generarReportePlano341 = (request: any) =>
            apiAxios
                .post('ReportePlanoSFCF341', request, {
                    responseType: 'text',
                })
                .then(res => res.data)
                .catch((res) => {
                })
    
    const generarReportePlano416 = (request: any) =>
        apiAxios
            .post('ReportePlanoSFCF416', request, {
                responseType: 'blob',
            })
            .then(res => res.data)
            .catch((res) => {
            })
    const GenerarExcels = async (request: any) =>
        apiAxios
            .post('ReportesExcel', request, {
                responseType: 'blob',
            })
            .then(res => res.data)
            .catch((res) => {
            })

    const GenerarExcelInvariantCulture = async (request: any) =>
    apiAxios
        .post('ReportesExcelInvariantCulture', request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((_) => { });


    const GenerarReporteExcel = async (request: ExcelReport) =>
    apiAxios
        .post('GenerarExcel', request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((_) => { });

    const reporteAseguradora = async () => 
        apiAxios
            .post('ReporteAseguradora')
            .then((res: any) => res.data);

    const reporteCargaAnalista = async () => 
        apiAxios
            .post('ReporteCargaAnalista')
            .then((res: any) => res.data);

    const reporteAvaluos = async () => 
        apiAxios
            .post('ReporteAvaluos')
            .then((res: any) => res.data);

    const reporteGeneralCartera = async (data: any) => 
        apiAxios
            .get('ReporteGeneralCartera', {
                params: data
            })
            .then((res: any) => res.data);

    const reporteGeneralReestructuracion = async (data: any) => 
        apiAxios
            .get('GenerarReporteGeneralCarteraReestructurado', {
                params: data
            })
            .then((res: any) => res.data);

    const reporteAPNRCAR = async (data: any) => 
    apiAxios
        .get('ReporteAPNRCAR', {
            params: data
        })
        .then((res: any) => res.data);

    const reporteRCredito = async (data: any) => 
    apiAxios
        .get('GenerarExcelRCredito', {
            responseType: 'blob',
            params: data
        })
        .then((res: any) => res.data);

    const reporteMensualSeguros = async (data: any) => 
        apiAxios
            .get<Response<ReporteMensualSeguros[]>>('ReporteMensualSeguros', {
                params: data
            })
            .then((res: any) => res.data);

    const reporteCapitalCorriente = async (data: any) => 
    apiAxios
        .get<Response<DatosReporteCapitalCorriente[]>>('CapitalCorriente', {
            params: data
        })
        .then(res => res.data);

    const reporteOpcionCompra = async (data: any) => 
    apiAxios
        .get<Response<ReporteOpcionCompra[]>>('OpcionCompra', {
            params: data
        })
        .then(res => res.data);

    const reporteEstadoCuentaAfiliado = async (data: any) => 
    apiAxios
        .get<Response<ReporteEstadoCuentaAfiliado[]>>('EstadoCuentaAfiliado', {
            params: data
        })
        .then((res) => res.data);


    const reporteEstadoCuentaPlanPago = async (idCredito: string) => 
    apiAxios
        .get<Response<ReporteEstadoCuentaPlanPago[]>>('EstadoCuentaPlanPago', {
            params: {idCredito}
        })
        .then((res) => res.data);
    
    const reporteEstadoMovimientos = async (data: any) => 
    apiAxios
        .get<Response<ReporteEstadoMovimientos[]>>('EstadoMovimientos', {
            params: data
        })
        .then((res) => res.data);

    const reporteGeneralGarantia = async (data: any) => 
    apiAxios
        .get('ReporteGeneralGarantias', {
            params: data
        })
        .then((res: any) => res.data);

    const reporteGeneralMora = async (data: any) =>
        apiAxios
        .get('/ReporteGeneralMora', {
            params: data
        })
        .then(res => res.data);

    const reportePazSalvo = async (data: any) => 
    apiAxios
        .get(`PazSalvo`, {
            responseType: 'blob',
            params: data
        })
        .then((res: any) => res.data);
    
    const reporteInformacionPlanPago = async (request: any) =>
        apiAxios.post(
            "ReporteInformacionPlanPago",
            request
        ).then(res => res.data);

    const RetefuenteAnioGravable = async (data: any) => 
    apiAxios
        .get(`RetefuenteAnioGravable`, {
            responseType: 'blob',
            params: data
        })
        .then((res: any) => res.data);

    const CertificadoRetefuenteCredito = async (data: any) => 
    apiAxios
        .get(`CertificadoRetefuente`, {
            params: data,
            responseType: 'blob'
        })
        .then((res: any) => res.data);

    const reporteGeneralGarantias = async () => 
        apiAxios
            .post('ReporteGeneralGarantias')
            .then((res: any) => res.data);

    const reporteJefe = async () => 
        apiAxios
            .post('jefe')
            .then((res: any) => res.data);
    
    const reportePreliminarCartera = async () => 
        apiAxios
            .post('ReportePreliminarCartera')
            .then((res: any) => res.data);

    const reporteValorVenta = async () => 
        apiAxios
            .post('ReporteValorVenta')
            .then((res: any) => res.data);

    const reportesAnalistasCredito = async () => 
        apiAxios
            .post('pendiente')
            .then((res: any) => res.data);


    const reporteConsultaRestrictivas = async () => 
        apiAxios
            .post('ReporteConsultasRestrictivas')
            .then((res: any) => res.data);

    const oficioSolicitudCesantias = (request: number) =>
        apiAxios
            .post(
                `TransferenciaCesantias?idAfiliacion=${request}`,
                {},
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);

    const oficioSolicitudCesantiasPONAL = (request: number) =>
        apiAxios
            .post(
                `TransferenciaCesantiasPONAL?idAfiliacion=${request}`,
                {},
                {
                    responseType: 'blob',
                },
            )
            .then(res => res.data);

    const oficioRespuestaNoProcedente = (request: number) =>
                apiAxios
                .post (
                    `RespuestaNoProcedente?idAfiliacion=${request}`,
                    {},
                    {
                        responseType: 'blob',
                    },
                )
                .then(res => res.data);
    
    const oficioRespuestaReintegroProcedente = (idAfiliacion: string, opcionReporte: number) =>
        apiAxios
        .post (
            `ReintegroProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: opcionReporte
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaReincorporacionProcedente = (idAfiliacion: string) =>
        apiAxios
        .post (
            `ReincorporacionProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 11
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);
    
    const oficioRespuestaReconocimientoPensionProcedente = (idAfiliacion: string) =>
        apiAxios
        .post (
            `PensionRetiroProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 9
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaNuevaVinculacionProcedente = (idAfiliacion: string) =>
        apiAxios
        .post (
            `NuevaVinculacionProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 8
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaAfiliacionVoluntariaProcedente = (idAfiliacion: string) =>
        apiAxios
        .post (
            `AfiliacionVoluntariaProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 3
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);
    
    const oficioRespuestaAfiliacionExtemporaneaProcedente = (idAfiliacion: string) =>
        apiAxios
        .post (
            `AfiliacionExtemporaneaProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 1
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaRecuperacionCalidadAfiliado = (idAfiliacion: string) =>
        apiAxios
        .post (
            `DesafiliacionVoluntariaProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 10
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaBeneficiarios = (idAfiliacion: string) =>
        apiAxios
        .post (
            `BeneficiariosProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 7
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaAfiliacionVoluntariaVeteranosCredito = (idAfiliacion: string) =>
        apiAxios
        .post (
            `AfiliacionVoluntariaVeteranosCredito`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 5
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaAfiliacionVoluntariaVeteranosProcedenteAD = (idAfiliacion: string) =>
        apiAxios
        .post (
            `AfiliacionVoluntariaVeteranosProcedenteAD`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 5
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const oficioRespuestaAfiliacionVoluntariaVeteranosProcedenteCB = (idAfiliacion: string) =>
        apiAxios
        .post (
            `AfiliacionVoluntariaVeteranosProcedenteCB`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 5
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);
    
    const oficioRespuestaAfiliacionAdminCesantiasProcedente = (idAfiliacion: string) =>
        apiAxios
        .post (
            `AfiliacionAdminCesantiasProcedente`,
            {
                idAfiliacion: idAfiliacion,
                opcionReporte: 2
            },
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data);

    const reporteEstadoIntegracion = (request: any) =>
    apiAxios
        .post('Reportes/ReporteResultadoIntegracion', request
            , {
                baseURL: routega2api,
            })
        .then(res => res.data);
        
        const PostPlanillaPdf = (request: any) =>
        apiAxios
        .post (
            'ReportePlanillaPagoIndiCesantias',request,{
            
                    responseType: 'blob',
                
            })
        .then(res => res.data);


    const ReporteResumenSolicitud = (request: any) =>
        apiAxios
            .post('Reportes/ReporteResumenFinanciero', request
                , {
                    baseURL: routega2api,
                    responseType: 'blob', 

                })
            .then(res => res.data);

    const postReporteTransUnion = (request: any) =>
        apiAxios
        .post (`ReporteTransUnion?NumeroIdentificacion=${request.NumeroIdentificacion}`, 
            {},{
                    responseType: 'blob', 
            })
        .then(res => res.data);
        
    const getReporteConsolidadoFuerza = () =>
        apiAxios
        .get (`reportes/GenerarReporteTotalesPorConcepto`, 
            {
                baseURL: `${routebuaapi}/`
            })
        .then(res => res.data);

    const reporteAfiliadoDevolucionCesantias = (request: any) =>
        apiAxios
            .post('reporte/GenerarReportePagoDeCesantias', request
                , {
                    baseURL: routega2api,
                })
            .then(res => res.data);
        
        const postReporteInformacionComercial = (request: any) =>
        apiAxios
        .post (
            'ReporteTransUnionPropiedades',request,{
            
                    responseType: 'blob',
                
            })
        .then(res=>res.data);

        const postReporteProspeccionReclasificacion = (request: any) =>
        apiAxios
        .post (
            'ReporteProspeccionReclasificacion',request,{
            
                    responseType: 'blob',
                
            })
        .then(res=>res.data);

        const GenerarReporteExcelSolicitudNomina = async (request: any) =>
        apiAxios
            .post('ReporteExcelSolicitudNomina', request, {
                responseType: 'blob',
            })
            .then(res => res.data)
            .catch((res) => {
        })

        const postGenerarFacturaSevenCredito = async (request: string) =>
        apiAxios
            .post('GenerarFacturaSevenCredito', request, {
                params: {
                    request,
                },
                responseType: 'blob'
            })
            .then((res: any) => res.data);

    return {
        generarListaReportes,
        generarReportes,
        generarIngresosRetenciones,
        generarEstadoDeCuenta,
        generarExtratoAnual,
        generarExtratoMensual,
        generarMovimientoCartera,
        generarMReporte414,
        generarMReporte416,
        reporteCargaAnalista,
        reporteAseguradora,
        GenerarExcels,
        GenerarExcelInvariantCulture,
        GenerarReporteExcel,
        reporteAvaluos,
        reporteGeneralCartera,
        reporteGeneralReestructuracion,
        reporteGeneralMora,
        reporteGeneralGarantia,
        reportePazSalvo,
        reporteInformacionPlanPago,
        RetefuenteAnioGravable,
        CertificadoRetefuenteCredito,
        reporteAPNRCAR,
        reporteRCredito,
        reporteMensualSeguros,
        reporteCapitalCorriente,
        reporteOpcionCompra,
        reporteEstadoCuentaAfiliado,
        reporteEstadoCuentaPlanPago,
        reporteEstadoMovimientos,
        reporteGeneralGarantias,
        reporteJefe,
        reportePreliminarCartera,
        reporteValorVenta,
        reportesAnalistasCredito,
        reporteConsultaRestrictivas,
        oficioSolicitudCesantias,
        oficioSolicitudCesantiasPONAL,
        oficioRespuestaNoProcedente,
        oficioRespuestaReintegroProcedente,
        oficioRespuestaReincorporacionProcedente,
        oficioRespuestaReconocimientoPensionProcedente,
        oficioRespuestaNuevaVinculacionProcedente,
        oficioRespuestaAfiliacionVoluntariaProcedente,
        oficioRespuestaAfiliacionExtemporaneaProcedente,
        oficioRespuestaRecuperacionCalidadAfiliado,
        oficioRespuestaBeneficiarios,
        oficioRespuestaAfiliacionVoluntariaVeteranosCredito,
        oficioRespuestaAfiliacionVoluntariaVeteranosProcedenteCB,
        oficioRespuestaAfiliacionVoluntariaVeteranosProcedenteAD,
        oficioRespuestaAfiliacionAdminCesantiasProcedente,
        generarMReportePlano414,
        generarReportePlano416,
        reporteEstadoIntegracion,
        generarReportePlano341,
        PostPlanillaPdf,
        ReporteResumenSolicitud,
        postReporteTransUnion,
        getReporteConsolidadoFuerza, 
        postReporteInformacionComercial, 
        postReporteProspeccionReclasificacion,
        GenerarReporteExcelSolicitudNomina,
        postGenerarFacturaSevenCredito,
        reporteAfiliadoDevolucionCesantias
    };
};

export default ApiReportes;
