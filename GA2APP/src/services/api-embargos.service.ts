import axios from "axios";
import StorageService from "./storage.service";
import { routega2api } from '../utils/baserouteapi.route';
import {
    ActualizarEstadoEmbargoDto,
    BusquedaEmbargos,
    EmbargoBeneficiariosDto,
    EmbargoConceptoDto,
    EmbargoDto,
    EmbargoModelDto,
    InsertarPagoEmbargo,
    radicadoEmbargoDto
} from "../types/embargo.model.type";
import { convertToUpdate } from "../pages/embargos/constants/conversionesEmbargo";
import { convertToInsertEmbargoConcepto } from "../pages/embargoConceptos/constants/conversionesEmbargoConcepto";
import { convertToInsertBeneficiary } from "../pages/payBeneficiary/constants/conversionesBeneficiario";

const ApiEmbargo = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getTiposEmbargos = () =>
        apiAxios
            .get(`ObtenerTiposEmbargos`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getTiposProcesosEmbargos = () =>
        apiAxios
            .get(`ObtenerTiposProcesosEmbargos`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getAllEmbargos = () =>
        apiAxios
            .get(`ObtenerEmbargos`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postAllEmbargosByDate = (fechaInicial: Date, fechaFinal: Date) =>
        apiAxios
            .post(`ObtenerEmbargosRangoFecha`, {
                fechaInicial: fechaInicial,
                fechaFinal: fechaFinal
            }, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInsertEmbargo = async (embargo: EmbargoModelDto) =>
        apiAxios
            .post(`InsertarEmbargo`, embargo, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerPagoEmbargo = (PagoEmbargo: any) =>
        apiAxios.get('ObtenerPagoEmbargo', {
            baseURL: routega2api,
            params: {
                request: PagoEmbargo
            }})
            .then(res => res.data);


    const InsertarPagoEmbargo = async (embargo: InsertarPagoEmbargo) =>
        apiAxios
            .post(`InsertarPagoEmbargo`, embargo, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInsertRadicadoEmbargo = async (embargo: radicadoEmbargoDto) =>
        apiAxios
            .post(`RadicadoEmbargo`, embargo, {
                baseURL: routega2api,
            })
            .then(res => res.data);
    const postUpdateEmbargo = (embargo: EmbargoModelDto) =>
        apiAxios
            .post(`ActualizarEmbargo`, embargo, {
                baseURL: routega2api,
            })
            .then(res => res.data);
            const postUpdateEstadoEmbargo=(embargo: ActualizarEstadoEmbargoDto) =>
            apiAxios
                .post(`ActualizarEstadoEmbargo`, embargo, {
                    baseURL: routega2api,
                })
                .then(res => res.data);

    const deleteEmbargo = (embargo: string) =>
        apiAxios
            .delete(`EliminarEmbargo?request=${embargo}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getEmbargosCuentaConcepto = () =>
        apiAxios
            .get(`ObtenerEmbargosCuentaConcepto`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getTiposRetencion = () =>
        apiAxios
            .get(`ObtenerTiposRetenciones`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInsertEmbargoCuentaConcepto = (embargoConcepto: EmbargoConceptoDto, conceptoId: number) =>
        apiAxios
            .post(`InsertarEmbargoCuentaConcepto`, convertToInsertEmbargoConcepto(embargoConcepto, conceptoId), {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postUpdateEmbargoCuentaConcepto = (embargoConcepto: EmbargoConceptoDto) =>
        apiAxios
            .post(`ActualizarEmbargoCuentaConcepto`, embargoConcepto, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const deleteEmbargoConcepto = (concepto: any) =>
        apiAxios
            .post('EliminarEmbargoCuentaConcepto', concepto, { baseURL: routega2api })
            .then(res => res.data);


    const getAllBeneficiariosEmbago = () =>
        apiAxios
            .get(`ObtenerBeneficiariosEmbargo`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInsertEmbargoBeneficiario = (embargoBeneficiario: EmbargoBeneficiariosDto) =>
        apiAxios
            .post(`InsertarBeneficiariosPagoEmbargos`, convertToInsertBeneficiary(embargoBeneficiario), {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postUpdateEmbargoBeneficiario = (embargoBeneficiario: EmbargoBeneficiariosDto) =>
        apiAxios
            .post(`ActualizarBeneficiariosPagoEmbargos`, embargoBeneficiario, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const deleteEmbargoBeneficiario = (embargoBeneficiario: EmbargoBeneficiariosDto) =>
        apiAxios
            .delete(`EliminarBeneficiariosPagoEmbargos?request=${embargoBeneficiario.id}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postInsertDesembargo = (embargoConcepto: any) =>
        apiAxios
            .post(`InsertarDesembargo`, embargoConcepto, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getAllDesembargos = () =>
        apiAxios
            .get(`ObtenerDesembargo`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const getObtenerCuentasPorRadicadoWorkManager = (radicado: string) =>
        apiAxios
            .get(
                `ObtenerCuentasPorRadicadoWordManager?request=${radicado}`,
                {
                    baseURL: routega2api,
                },
            )
            .then(res => res.data);

    const getObtenerConceptoPorCuenta = (idCuenta: number) =>
        apiAxios
            .get(
                `ObtenerConceptoCuentaId?idCuenta=${idCuenta}`,
                {
                    baseURL: routega2api,
                },
            )
            .then(res => res.data);

    const getObtenerConceptosEmbargoById = (idEmbargo: string) =>
        apiAxios
            .get(
                `ObtenerConceptosEmbargoById?request=${idEmbargo}`,
                {
                    baseURL: routega2api,
                },
            )
            .then(res => res.data);

    const postObtenerRangoFiltros = (filtros: BusquedaEmbargos) =>
        apiAxios
            .post(`ObtenerEmbargosRangoFiltros`, filtros, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const postObtenerRespuesta = (solicitudId: string) =>
        apiAxios
            .post(`ObtenerRespuestaEmbargo?request=${solicitudId}`, {}, {
                baseURL: routega2api,
                responseType: 'blob',
            })
            .then(res => res.data);

    const getObtenerEmbargosById = (idEmbargo: string) =>
        apiAxios
            .get(
                `ObtenerEmbargoById?request=${idEmbargo}`,
                {
                    baseURL: routega2api,
                },
            )
            .then(res => res.data);

    const getObtenerEmbargosByIdentificacion = (idEmbargo: string) =>
        apiAxios
            .get(
                `ObtenerEmbargoByIdentificacion?request=${idEmbargo}`,
                {
                    baseURL: routega2api,
                },
            )
            .then(res => res.data);

    return {
        getTiposEmbargos,
        getTiposProcesosEmbargos,
        getAllEmbargos,
        postInsertEmbargo,
        postUpdateEmbargo,
        deleteEmbargo,
        getEmbargosCuentaConcepto,
        getTiposRetencion,
        postInsertEmbargoCuentaConcepto,
        postUpdateEmbargoCuentaConcepto,
        deleteEmbargoConcepto,
        getAllBeneficiariosEmbago,
        postInsertEmbargoBeneficiario,
        postUpdateEmbargoBeneficiario,
        deleteEmbargoBeneficiario,
        postAllEmbargosByDate,
        postInsertDesembargo,
        getAllDesembargos,
        getObtenerCuentasPorRadicadoWorkManager,
        getObtenerConceptoPorCuenta,
        getObtenerConceptosEmbargoById,
        postObtenerRangoFiltros,
        getObtenerEmbargosById,
        postObtenerRespuesta,
        getObtenerEmbargosByIdentificacion,
        postInsertRadicadoEmbargo,
        InsertarPagoEmbargo,
        ObtenerPagoEmbargo,
        postUpdateEstadoEmbargo

    };
};

export default ApiEmbargo;