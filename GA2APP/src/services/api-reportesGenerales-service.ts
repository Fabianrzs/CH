/* eslint-disable prettier/prettier */
import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';

const ApiReportesGenerales = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}Reportes`,
        timeout: 800000,
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
            .get('/ReporteGeneralCartera')
            .then((res: any) => res.data);
    const ConsultarReportesGenerales = async () =>
       apiAxios
            .get('/ConsultaReportesGenerales')
            .then((res:any) => res.data);
    const generarPreliminarCartera = async () =>
        apiAxios
            .get('/ReportePreliminarCartera')
            .then((res:any) => res.data);
    
    const ReportePlanoSFCF341 = async (request: any) =>

        apiAxios

            .post('/ReportePlanoSFCF341', request, {

                responseType: 'blob',

            })

            .then(res => res.data)

            .catch((res) => {

            })

    
   
    
    const ReportePlanoSFCF414 = async (request:any) =>
    apiAxios
        .post('/ReportePlanoSFCF414_V2',request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((res) => {
        })
    const ReportePlanoSFCF416 = async (request:any) =>
    apiAxios
        .post('/ReportePlanoSFCF416',request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((res) => {
        })
    const ReportePlanoSFCF477 = async (request:any) =>
    apiAxios
        .post('/ReportePlanoSFCF477',request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((res) => {
        })
    const ReportePlanoSFCF453 = async (request:any) =>
    apiAxios
        .post('/ReportePlanoSFCF453',request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((res) => {
        })
    const ReportePlanoSFCF343 = async (request:any) =>
    apiAxios
        .post('/ReportePlanoSFCF343',request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((res) => {
        })

    const ReportePlanoSFCF536 = async (request:any) =>
    apiAxios
        .post('/ReportePlanoSFCF536',request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((res) => {
        })
      
    const ReporteSolicitudTransUnion = async (request:any) =>
    apiAxios
        .post('/ReporteSolicitudTransUnion',request, {
            responseType: 'blob',
        })
        .then(res => res.data)
        .catch((res) => {
        })
    const MovimientoDeCartera = (request: any) =>
        apiAxios
        .post(
            `/ReporteMovimientoDeCartera?identificacion=${request.identificacion}`,
            {},
            {
                responseType: 'blob',
            },
        )
        .then(res => res.data)
        .catch(res => res.data);
    
        const generarMoraCartera = async () =>
        apiAxios
        .post('/ReporteGeneralMora')
        .then(res => res.data);

        const generarReporteGarantias = async () =>
        apiAxios
        .post('/ReporteGeneralGarantias')
        .then(res => res.data);

        const generarReporteInforme414 = async (request: any) =>
        apiAxios
        .post('/ReporteFCF414', request)
        .then(res => res.data);

        const generarReporteInforme453y454 = async (request: any) =>
        apiAxios
        .post('/ReporteFCF453Y454', request)
        .then(res => res.data);

        const generarReporteInforme477 = async (request: any) =>
        apiAxios
        .post('/ReporteFCF477', request)
        .then(res => res.data);

    return {
        generarListaReportes,
        ConsultarReportesGenerales,
        generarPreliminarCartera,
        MovimientoDeCartera,
        generarMoraCartera,
        generarReporteGarantias,
        ReportePlanoSFCF341,
        ReportePlanoSFCF414,
        ReportePlanoSFCF416,
        ReportePlanoSFCF477,
        ReportePlanoSFCF453,
        ReportePlanoSFCF343,
        ReportePlanoSFCF536,
        ReporteSolicitudTransUnion,
        generarReporteInforme414,
        generarReporteInforme453y454,
        generarReporteInforme477,
    };
};

export default ApiReportesGenerales;
