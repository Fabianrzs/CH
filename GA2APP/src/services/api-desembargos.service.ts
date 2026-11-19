import axios from "axios";
import StorageService from "./storage.service";
import { routebuaapi, routega2api } from '../utils/baserouteapi.route';
import { BusquedaDesembargos, DesembargoDto, ObtenerDesembargoDto } from "../types/desembargo.model.type";
import { convertToInsertDesembargoConcepto } from "../pages/desembargosConceptos/constants/conversionesDesembargosConcepto";
import { BusquedaEmbargos } from "../types/embargo.model.type";

const ApiDesembargo = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getTiposDesembargos = () => 
    apiAxios             
        .get(`ObtenerDesembargo`,{
            baseURL: routega2api,
        })
        .then(res => res.data);
        
    const getTiposProcesosDesembargos = () => 
    apiAxios             
        .get(`ObtenerDesembargo`,{
            baseURL: routega2api,
        })
        .then(res => res.data);

    const getAllDesembargos = () => 
    apiAxios             
        .get(`ObtenerDesembargo`,{
            baseURL: routega2api,
        })
        .then(res => res.data);

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
    
    const postAllDesembargosByDate = (fechaInicial: Date, fechaFinal: Date) => 
    apiAxios             
        .post(`InsertarDesembargo`,{
            fechaInicial: fechaInicial,
            fechaFinal: fechaFinal
        },{
            baseURL: routega2api,
        })
        .then(res => res.data);
        
    const postInsertDesembargo = async(desembargo: DesembargoDto) =>
    apiAxios             
        .post(`InsertarDesembargo`,desembargo,{
            baseURL: routega2api,
        })
        .then(res => res.data);
    
    const postUpdateDesembargo = (desembargo: DesembargoDto) => 
    apiAxios             
        .post(`InsertarDesembargo`,desembargo,{
            baseURL: routega2api,
        })
        .then(res => res.data);
    
    const deleteDesembargo = (desembargo: ObtenerDesembargoDto) => 
    apiAxios      
        .delete(`EliminarDesembargo?request=${desembargo.id}`,{
            baseURL: routega2api,
        })
        .then(res => res.data);

    const getDesembargosCuentaConcepto = () => 
    apiAxios             
        .get(`ObtenerDesembargo`,{
            baseURL: routega2api,
        })
        .then(res => res.data);
    
    const getTiposRetencion = () => 
    apiAxios             
        .get(`ObtenerDesembargo`,{
            baseURL: routega2api,
        })
        .then(res => res.data);

    const postInsertDesembargoCuentaConcepto = (desembargoConcepto: ObtenerDesembargoDto, conceptoId: number) => 
    apiAxios             
        .post(`InsertarDesembargo`,convertToInsertDesembargoConcepto(desembargoConcepto, conceptoId),{
            baseURL: routega2api,
        })
        .then(res => res.data); 

    const postUpdateDesembargoCuentaConcepto = (desembargoConcepto: ObtenerDesembargoDto) => 
    apiAxios             
        .post(`InsertarDesembargo`,desembargoConcepto,{
            baseURL: routega2api,
        })
        .then(res => res.data);
        
    const deleteDesembargoConcepto = (desembargoConcepto: ObtenerDesembargoDto) => 
    apiAxios      
        .delete(`EliminarDesembargoCuentaConcepto?request=${desembargoConcepto.id}`,{
            baseURL: routega2api,
        })
        .then(res => res.data);
        
    const getObtenerCuentasPorRadicadoWorkManager = (radicado: string) =>
    apiAxios
        .get(
            `ObtenerDesembargo?request=${radicado}`,
            {
                baseURL: routega2api,
            },
        )
        .then(res => res.data);

    const getObtenerConceptoPorCuenta = (idCuenta: number) =>
    apiAxios
        .get(
            `ObtenerDesembargo?idCuenta=${idCuenta}`,
            {
                baseURL: routega2api,
            },
        )
        .then(res => res.data);

    const getObtenerConceptosDesembargoById = (idDesembargo: string) =>
    apiAxios
        .get(
            `ObtenerDesembargo?request=${idDesembargo}`,
            {
                baseURL: routega2api,
            },
        )
        .then(res => res.data);

    const postObtenerRespuesta = (solicitudId: string) => 
    apiAxios             
    .post(`InsertarDesembargo?request=${solicitudId}`,{},{
        baseURL: routega2api,
        responseType: 'blob',
    })
    .then(res => res.data); 

    const getObtenerDesembargosById= (idEmbargo: string) =>
    apiAxios
        .get(
            `ObtenerDesembargo?request=${idEmbargo}`,
            {
                baseURL: routega2api,
            },
        )
        .then(res => res.data);

    const postObtenerRangoFiltros = (filtros: BusquedaEmbargos) => 
    apiAxios             
        .post(`ObtenerEmbargosRangoFiltros`,filtros,{
            baseURL: routega2api,
        })
        .then(res => res.data);

    const postObtenerDesembargoRangoFiltros = (filtros: BusquedaEmbargos) => 
    apiAxios             
        .post(`ObtenerEmbargosADesembargar`,filtros,{
            baseURL: routega2api,
        })
        .then(res => res.data);
    

    return {
        getTiposDesembargos,
        getTiposProcesosDesembargos,
        getAllDesembargos,
        postAllDesembargosByDate,
        postInsertDesembargo,
        postUpdateDesembargo,
        deleteDesembargo,
        getDesembargosCuentaConcepto,
        getTiposRetencion,
        postInsertDesembargoCuentaConcepto,
        postUpdateDesembargoCuentaConcepto,
        deleteDesembargoConcepto,
        getObtenerCuentasPorRadicadoWorkManager,
        getObtenerConceptoPorCuenta,
        getObtenerConceptosDesembargoById,
        postObtenerRespuesta,
        getObtenerDesembargosById,
        postObtenerRangoFiltros,
        getClientByIdentificationTypeIdentificationNumberRequest,
        postObtenerDesembargoRangoFiltros
    };
};

export default ApiDesembargo;