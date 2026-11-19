import axios from 'axios';
import StorageService from './storage.service';
import { routebuaapi, routega2api } from '../utils/baserouteapi.route';
import { AdministracionParametroModel, CreateSimulationClient } from '../models/requests/create-simulation-client';
import { CuentaDto, InclusionSaldoRequestDto } from '../types/client.model.type';
import { AddCuentas } from '../types/educational.entities.actions.type';

const ApiClients = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routebuaapi}`,
        timeout: 9000000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getClientByIdentificationTypeIdentificationNumberRequest = (
        idType: number,
        idNumber: string,
    ) =>
        apiAxios
            .get('clientes/obtenerclienteportipoidentificationynumero', {
                params: {
                    tipoDocumentoId: idType,
                    numeroDocumento: idNumber,
                },
                baseURL: routebuaapi,
            })
            .then(res => res.data);


    const getAdministracionCuentasAfiliado = async (data: AdministracionParametroModel) => 
        apiAxios
            .post('ObtenerInformacionCliente', data, {
                baseURL: routega2api
            })
            .then((res: any) => res.data);

    const restoreIntegration = async (data: any) => 
        apiAxios
            .post(`clientes/restaurarIntegracionCliente`, data)
            .then((res: any) => res.data);

    const postSimulationCreditClient = async (data: CreateSimulationClient) => 
        apiAxios
            .post('obtenerSolicCreditoProducto', data)
            .then((res: any) => res.data);

    const getMovementsAccountsClient = async (identification: string) =>
        apiAxios
            .get(`clientes/obtenerCuentasMovimientosAfiliado?identificacion=${identification}`)
            .then((res: any) => res.data);

    const getClientById = async (identification: number) =>
        apiAxios
            .get(`clientes?request=${identification}`)
            .then((res: any) => res.data);

    const getPermisoActualizacion = async (perfil: string) =>
            apiAxios
                .get(`ConsultarPermisoActualizacion?request=${perfil}`)
                .then((res: any) => res.data); 

    const getCuentasConceptosPorTipoNumeroIdentificacion = async (tipo: number, identificacion: string) =>
        apiAxios
            .get(`ObtenerCuentasConceptosPorTipoNumeroIdentificacion`, {
                params: {
                    tipoDocumentoId: tipo,
                    numeroDocumento: identificacion,
                },
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInclusionSaldos = async (inclusionSaldos: InclusionSaldoRequestDto) =>
        apiAxios
            .post(`InsertarInclusionSaldos`, inclusionSaldos,
            {
                baseURL: routebuaapi,
            })
            .then(res => res.data);

    const postCrearCuenta = async (cuenta: any) =>
            apiAxios
                .post(`InsertarCuentas`, cuenta,
                {
                    baseURL: routebuaapi,
                })
                .then(res => res.data);

    const postCambiarEstado = async (nuevoEstado: any) =>
            apiAxios
                .post(`cuentas/ActualizarEctCuentas`, nuevoEstado,
                {
                    baseURL: routebuaapi,
                })
                .then(res => res.data);

    const eliminarInclusionSaldosEncabezado = async (inclusionSaldos: number) =>
        apiAxios
            .delete(`EliminarInclusionSaldosEncabezado?request=${inclusionSaldos}`)
            .then(res => res.data);

    const eliminarInclusionSaldos = async (inclusionSaldos: number) =>
        apiAxios
            .delete(`EliminarInclusionSaldos?request=${inclusionSaldos}`)
            .then(res => res.data);


    const GetClientByIdequest = async (id: number) =>
        apiAxios
            .get(`clientes?request=${id}`)
            .then(res => res.data);

    const UpdateClientRequest = async (data: any) =>
        apiAxios
            .patch(`clientes`, data)
            .then(res => res.data);

    const getObtenerCuentasClienteId = async (idCliente: number) =>
        apiAxios
            .get(`ObtenerCuentasClienteId?request=${idCliente}`)
            .then((res: any) => res.data);

    const getObtenerInformacionCliente = async (identificacion: string) =>
    apiAxios
        .get(`ReporteObtenerInformacionCliente?identificacion=${identificacion}`)
        .then((res: any) => res.data);

    const getConsultaAfiliadosPorFuerza = async (idFuerza: string) =>
        apiAxios
            .get(`clientes/GenerarReporteAfiliadosPorFuerza?idFuerza=${idFuerza}`)
            .then((res: any) => res.data);

    const getConsultaAfiliados = async (request: any) =>
        apiAxios
            .post(`clientes/GenerarReporteAfiliados`, request)
            .then((res: any) => res.data);

    const getConsultaAfiliadosDatosBasicos = async (request: any) =>
        apiAxios
            .post(`clientes/GenerarReporteDatosBasicosAfiliado`, request)
            .then((res: any) => res.data);

    const getConsultarClientePorIdentificacion = async (identificacion: string) =>
    apiAxios
        .get(`clientes/ConsultarClientePorIdentificacion?request=${identificacion}`)
        .then((res: any) => res.data);

    const postObtenerInformacionClienteCedula = (identificacion: number) =>
        apiAxios
        .post(`clientes/ObtenerInformacionClienteCedula?identificacionCliente=${identificacion}`, {}, {
            baseURL: routebuaapi,
        })
        .then((res: any) => res.data);

    const InsertarAnotacionCliente = (data: any) =>
		apiAxios
			.post(`Clientes/InsertarAnotacionCliente`, data, {
				baseURL: routebuaapi,
			})
			.then((res: any) => res.data);

	const EliminarAnotacionCliente = (data: any) =>
		apiAxios
			.post(`Clientes/EliminarAnotacionCliente`, data, {
				baseURL: routebuaapi,
			})
			.then((res: any) => res.data);

    const InsertarEstadoCausalCliente = (data: any) =>
        apiAxios
            .post(`Clientes/InsertarEstadoCausalCliente`, data, {
                baseURL: routebuaapi,
            })
            .then((res: any) => res.data);

    const EliminarEstadoCausalCliente = (data: any) =>
        apiAxios
            .post(`Clientes/EliminarEstadoCausalCliente`, data, {
                baseURL: routebuaapi,
            })
            .then((res: any) => res.data);

    const LevantarEstadoCausalCliente = (data: any) => {
        apiAxios
            .post(`Clientes/LevantarEstadoCausalCliente`, data, {
                baseURL: routebuaapi,
            })
            .then((res: any) => res.data);
    }
    

    return {
        getClientByIdentificationTypeIdentificationNumberRequest,
        restoreIntegration,
        postSimulationCreditClient,
        getMovementsAccountsClient,
        getClientById,
        getAdministracionCuentasAfiliado,
        getCuentasConceptosPorTipoNumeroIdentificacion,
        postInclusionSaldos,
        eliminarInclusionSaldosEncabezado,
        eliminarInclusionSaldos,
        GetClientByIdequest,
        UpdateClientRequest,
        getObtenerCuentasClienteId,
        postCrearCuenta,
        postCambiarEstado,
        getObtenerInformacionCliente,
        getPermisoActualizacion,
        getConsultaAfiliados,
        getConsultaAfiliadosPorFuerza,
        getConsultarClientePorIdentificacion,
		InsertarAnotacionCliente,
		EliminarAnotacionCliente,
        InsertarEstadoCausalCliente,
        EliminarEstadoCausalCliente,
        LevantarEstadoCausalCliente,
        postObtenerInformacionClienteCedula
    };
};

export default ApiClients;
