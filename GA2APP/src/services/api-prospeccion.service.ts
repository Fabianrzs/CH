import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';

const Prospeccion = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}`,

        timeout: 800000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });



    const postConsultarVigenciaSimulacion = async (request: any) =>
        apiAxios
            .post(`prospeccion/ConsultarVigenciaSimulacion?request=${request}`, {},)
            .then((res: any) => res.data);

    const postCrearSolicitudCredito = async (request: any) =>
        apiAxios
            .post('prospeccion/CrearSolicitudCredito', request)
            .then((res: any) => res.data);



    const postObtenerMatriculasCreditoPorSolicitud = (socId: string) =>
        apiAxios
            .post(`prospeccion/ObtenerMatriculasCreditoporSolicitud?request=${socId}`, {},)
            .then((res: any) => res.data);

    const postActualizarValoresCategoria = (request: any) =>
        apiAxios
            .post('Categoria/ActualizarValoresCategoria', request,)
            .then((res: any) => res.data);

    const postObtenerValoresCategoria = (request: any) =>
        apiAxios
            .post(`Categoria/ObtenerValoresCategoria?Anio=${request.anio}`)
            .then((res: any) => res.data);

    const postCrearValoresCategoria = (request: any) =>
        apiAxios
            .post('Categoria/CrearValoresCategoria', request)
            .then((res: any) => res.data);

    const searchProspeccion = (data: any) =>
        apiAxios
            .post('prospeccion/ConsultarProspeccionPorNumeroDocumento', data)
            .then((res: any) => res.data);

    const getConsultarDatosProspeccionByNumeroDocumento = (request: any) =>
        apiAxios
            .get(`prospeccion/ConsultarDatosProspeccionByNumeroDocumento?request=${request}`,)
            .then((res: any) => res.data);

    const postConsultarLineaCredito = (data: any) =>
        apiAxios
            .post('prospeccion/ConsultarLineaCreditoObligacion', data)
            .then((res: any) => res.data);


    const postValoresProspeccion = (data: any) =>
        apiAxios
            .post('prospeccion/ConsultarValoresProspeccion', data)
            .then((res: any) => res.data);


    const postRestaurarLineasCreditoObligacion = (data: any) =>
        apiAxios
            .post('prospeccion/RestaurarLineasCreditoObligacion', data)
            .then((res: any) => res.data);

    const postRestaurarSimulacionPlanCliente = (data: any) =>
        apiAxios
            .post('prospeccion/RestaurarSimulacionPlanCliente', data)
            .then((res: any) => res.data);

    const getConsultarPeritoDetalle = (idSolicitud: string) =>
        apiAxios
            .get(`prospeccion/ConsultarPeritoDetalle?request=${idSolicitud}`)
            .then((res: any) => res.data);



          

    const getListaDocumentosACargar = (idSolicitud: string) =>

        apiAxios
            .get(`prospeccion/ObtenerDocumentosRequeridosSolicCredito?idSolicitud=${idSolicitud}`)
            .then((res: any) => res.data);


    const postRegistrarSeguroDatosCredito = (request: any) =>
        apiAxios
            .post('prospeccion/RegistrarSeguroDatosCredito', request)
            .then((res: any) => res.data);


    const postConsultarSegurosCredito = (request: any) =>
        apiAxios
            .post('prospeccion/ConsultarSegurosCredito', request)
            .then((res: any) => res.data);

    const postObtenerRecursosConelados = (request: any) =>
        apiAxios
            .post('prospeccion/ConsultarRecursosCongeladosProspeccion', request)
            .then((res: any) => res.data);
    const postConsultarConceptosCongeladosCalculados = (request: any) =>
        apiAxios
            .post('prospeccion/ConsultarConceptosCongeladosCalculados', request)
            .then((res: any) => res.data);
    const postActualizarAnioSalarial = (request: any) =>
        apiAxios
            .post('prospeccion/ActualizarAnioSalarial', request)
            .then((res: any) => res.data);


    const getObtenerAnioSalarial = () =>
        apiAxios
            .get(`prospeccion/ObtenerAnioSalarial`)
            .then((res: any) => res.data);

    return {
        postCrearSolicitudCredito,
        postObtenerMatriculasCreditoPorSolicitud,
        postConsultarVigenciaSimulacion,
        postActualizarValoresCategoria,
        postObtenerValoresCategoria,
        postCrearValoresCategoria,
        searchProspeccion,
        postValoresProspeccion,
        getConsultarDatosProspeccionByNumeroDocumento,
        postConsultarLineaCredito,
        postRestaurarLineasCreditoObligacion,
        getConsultarPeritoDetalle,
        postRestaurarSimulacionPlanCliente,
        getListaDocumentosACargar,
        postRegistrarSeguroDatosCredito,
        postConsultarSegurosCredito,
        postObtenerRecursosConelados,
        postConsultarConceptosCongeladosCalculados,
        postActualizarAnioSalarial,
        getObtenerAnioSalarial,
   

    };
};

export default Prospeccion;
