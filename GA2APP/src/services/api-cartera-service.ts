import axios from "axios";
import { routega2api } from "../utils/baserouteapi.route";
import StorageService from "./storage.service";

const CarteraApi = () => {
	const apiAxios = axios.create({
		headers: {
			'Cache-Control': 'no-cache',
			'Content-Type': 'application/json',
			'accept': 'application/json',
            'X-Frame-Options': 'sameorigin'
		},

		baseURL: `${routega2api}cartera`,
		timeout: 250000,
	});

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getUltimosCierres = () =>
    apiAxios.get('ConsultarUltimosCierres').then((res: any) => res.data);


    const getActiveTU = () =>
    apiAxios.get('obtenerParamGeneralesCreditoYCartera')
    .then((res: any) => res.data);

    const AplicarCuotasAdelantadas = (request: any) =>
    apiAxios
        .post(`cartera/AplicarCuotasAdelantadasByFechaActual?FechaAplicacion=${request}`, {},
            {
                baseURL: routega2api,
            })
        .then(res => res.data);


    const getConsultaSaldos = (data: any) =>
    apiAxios
        .post('ConsultaSaldosYCartera', data)
        .then((res: any) => res.data);


    const aplicarPagoMasivoDesdeListaSV = (request: any ) =>
    apiAxios
        .post('AplicarPagoMasivoMovContable',request)
        .then((res:any) => res.data);

    const AplicarPagoMasivoTotalizado = (request: any) =>
    apiAxios
        .post(`cartera/AplicarPagoMasivoMovContableTotalizado?Proceso=${request}`, {},
            {
                baseURL: routega2api,
            })
        .then(res => res.data);

    const postObtenerComprobantePorCodigoProceso = (request: any) =>
    apiAxios
        .post(`ObtenerComprobantePorCodigoProceso`, request)
        .then(res => res.data);

    const postGenerarComprobanteSevenPorProceso = (request: any) =>
    apiAxios
        .post(`GenerarComprobanteSevenPorProceso`, request)
        .then(res => res.data);

    const postObtenerMovimientosContableCartera = (request: any) =>
    apiAxios
        .post(`ObtenerMovimientosContableCartera`,request) 
        .then(res => res.data);

    const postGenerarExcelComprobanteDetalle = (request: any) =>
    apiAxios
        .get(`cartera/GenerarExcelComprobanteDetalle?request=${request}`, {
            baseURL: routega2api,
            responseType: 'blob',
        })
        .then((res: any) => res.data);

    const postObtenerProductoCreditoCartera = (request: any) =>
    apiAxios
        .post(`ObtenerProductoCreditoCartera?request=${request}`,) 
        .then(res => res.data);

    const PostGenerarBalanceCartera = (request: any) =>
    apiAxios
        .post(`GenerarBalanceCartera`, request,)
        .then((res: any) => res.data);

    const postGenerarAuxiliarContableCartera = (request: any) =>
    apiAxios
        .post(`GenerarAuxiliarContableCartera`, request, {
            responseType: 'blob', 
        })
        .then((res: any) => res.data);

    const postReversarPagosLista = (request: any) =>
    apiAxios
    .post(`cartera/ReversarPagosLista?request=${request}`, {},
    {
        baseURL: routega2api,
    })
    .then((res: any) => res.data);
    
    const PostReversarPagoUnitarioDeLista = (request: any) =>
    apiAxios
        .post(`ReversarPagoUnitarioDeLista`, request,)
        .then((res: any) => res.data);

    const postObtenerSaldosCartera = (request: any) =>
    apiAxios
        .post(`ObtenerInteresCorrienteActualByCredito`, request,)
        .then((res: any) => res.data);
    const postObtenerHistorialCausacionInteresCorriente = (request: any) =>
    apiAxios
        .post(`ObtenerHistorialCausacionInteresCorriente`, request,)
        .then((res: any) => res.data);
    const postEliminarAfiliadoDelComprobante = (request: any) =>
    apiAxios
        .post(`eliminarAfiliadoDelComprobante`, request,)
        .then((res: any) => res.data);

    return {
        getUltimosCierres,
        getActiveTU,
        AplicarCuotasAdelantadas,
        getConsultaSaldos,
        aplicarPagoMasivoDesdeListaSV,  
        AplicarPagoMasivoTotalizado,
        postObtenerComprobantePorCodigoProceso,
        postGenerarComprobanteSevenPorProceso,
        postObtenerMovimientosContableCartera,
        postGenerarExcelComprobanteDetalle,
        postObtenerProductoCreditoCartera,
        PostGenerarBalanceCartera,
        postGenerarAuxiliarContableCartera,
        postReversarPagosLista,
        PostReversarPagoUnitarioDeLista,
        postObtenerSaldosCartera,
        postObtenerHistorialCausacionInteresCorriente,
        postEliminarAfiliadoDelComprobante

    }
};

export default CarteraApi