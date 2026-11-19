import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import {
    ParametrosVerificacionDerechoDto,
    InfoVerificacionDerechoDto,
    ResultVerificacionDerechoDto,
    FiltroVerificacionDerechoDto,
    VerificacionDerechoPorPeriodoDto,
    InterecesNoCausados,
    ParametrosAbonosInteresDto,
    InteresNoCausadoDto

} from '../types/verificacion.model.type';
import {
    IPCDto

} from '../types/parametrization.model.type';

const ApiVerificacionDerecho = () => {
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

    const CrearIPCRequest = (IPC: IPCDto) =>
        apiAxios
            .patch('parametrizacion/CrearIPC', IPC, {
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

    const ObtenerTodoVerificacionDerechoRequest = () =>
        apiAxios
            .get('VerificacionDerechos/ObtenerTodoVerificacionDerechos', {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ObtenerTodoVerificacionDerechoFiltroRequest = (filtro: FiltroVerificacionDerechoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoVerificacionDerechosFiltroByPeriodo?DesdeAnio=${filtro.DesdeAnio}&DesdeMes=${filtro.DesdeMes}&HastaAnio=${filtro.HastaAnio}&HastaMes=${filtro.HastaMes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerTodoVerificacionDerechoByPeriodoRequest = (filtro: VerificacionDerechoPorPeriodoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoVerificacionDerechosByPeriodo?Anio=${filtro.anio}&Mes=${filtro.mes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerTodoDatosVerificacionDerechosByPeriodosRequest = (filtro: VerificacionDerechoPorPeriodoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoDatosVerificacionDerechosByPeriodos?Anio=${filtro.anio}&Mes=${filtro.mes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerTodoVerificacionDerechosInfoByPeriodoFiltroRequest = (filtro: VerificacionDerechoPorPeriodoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoVerificacionDerechosInfoByPeriodo?Anio=${filtro.anio}&Mes=${filtro.mes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ActualizarTodoEctCuentasRequest = (IPC: VerificacionDerechoPorPeriodoDto) =>
        apiAxios
            .patch('VerificacionDerechos/ActualizarTodoEctCuentas', IPC, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ObtenerInteresesNoCausadosRequest = (valores: InteresNoCausadoDto) =>
        apiAxios
            .patch('VerificacionDerechos/ObtenerTodoVerificacionDerechosResultByAnioCuenta', valores, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ObtenerTodoInteresAbonoRequest = () =>
        apiAxios
            .get('VerificacionDerechos/ObtenerTodoInteresAbonos', {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ObtenerConsolidadoFiltroByPeriodoRequest = (filtro: VerificacionDerechoPorPeriodoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerConsolidadoFiltroByPeriodo?Anio=${filtro.anio}&Mes=${filtro.mes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ObtenerTodoInteresesNoCausadosByIdentificacionRequest = (filtro: number) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoInteresesNoCausadosByIdentificacion?IDENTIFICACION=${filtro}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerTodoAbonoInteresFiltroRequest = (IPC: FiltroVerificacionDerechoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoValoresInteresAbonadosFiltroByPeriodo?DesdeAnio=${IPC.DesdeAnio}&DesdeMes=${IPC.DesdeMes}&HastaAnio=${IPC.HastaAnio}&HastaMes=${IPC.HastaMes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerInformacionAbonoInteresUnidadEjecutoraFiltroByPeriodoRequest = (filtro: VerificacionDerechoPorPeriodoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerInformacionAbonoInteresUnidadEjecutoraFiltroByPeriodo?Anio=${filtro.anio}&Mes=${filtro.mes}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ObtenerInteresesNoCausadosPorAnioRequest = (valores: InteresNoCausadoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoInteresesNoCausadosByIdentificacionYAnio?Anio=${valores.anio}&UsuarioId=${valores.usuarioId}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerInteresesNoCausadosPorAnioYConceptoRequest = (valores: InteresNoCausadoDto) =>
        apiAxios
            .get(`VerificacionDerechos/ObtenerTodoInteresesNoCausadosByIdentificacionYAnioConcepto?Anio=${valores.anio}&UsuarioId=${valores.usuarioId}&Concepto=${valores.concepto}&TipoCuentaId=${valores.tipoCuentaId}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const ActualizarParametrosInteresAbonadosMovimientosRequest = (valores: ParametrosAbonosInteresDto) =>
        apiAxios
            .post('VerificacionDerechos/ActualizarAbonoInteresMovimientos', valores, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    return {

        CrearIPCRequest,
        CrearProcesosIPCRequest,
        ActualizarIPCRequest,
        ObtenerReporteIPCRequest,
        ObtenerTodoReporteIPCRequest,
        ObtenerTodoVerificacionDerechoRequest,
        ObtenerTodoVerificacionDerechoFiltroRequest,
        ObtenerTodoVerificacionDerechoByPeriodoRequest,
        ObtenerTodoDatosVerificacionDerechosByPeriodosRequest,
        ObtenerTodoVerificacionDerechosInfoByPeriodoFiltroRequest,
        ActualizarTodoEctCuentasRequest,
        ObtenerInteresesNoCausadosRequest,
        ObtenerTodoInteresAbonoRequest,
        ObtenerConsolidadoFiltroByPeriodoRequest,
        ObtenerInformacionAbonoInteresUnidadEjecutoraFiltroByPeriodoRequest,
        ObtenerTodoAbonoInteresFiltroRequest,
        ObtenerTodoInteresesNoCausadosByIdentificacionRequest,
        ObtenerInteresesNoCausadosPorAnioRequest,
        ObtenerInteresesNoCausadosPorAnioYConceptoRequest,
        ActualizarParametrosInteresAbonadosMovimientosRequest

    };
};

export default ApiVerificacionDerecho;