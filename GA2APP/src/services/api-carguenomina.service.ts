import axios, { AxiosError, AxiosResponse } from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { DocumentoCarga } from '../types/cargue-nomina.model.type';


const ApiCargueNomina = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: routega2api,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const procesarArchivosCargadosRequest = async (
        documentosPorProcesar: DocumentoCarga[],
    ) => 
        apiAxios
            .post('procesararchivoscargados', documentosPorProcesar)
            .then(res => res.data)
            .catch(error => error as AxiosError);

const postCargueNominaRequest = async (
    formData: FormData,
   
) => {
    try {
        const response = await apiAxios.post('cargarchivonomina', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
             
        });
        return response.data;
    } catch (error: any) {
        return error as AxiosError;
    }
};


    const obtenerArchivosCargadosRequest = async (esdId: number) => 
           apiAxios
                .get(`obtenerarchivoscargados`, {
                    params: { request: esdId }
                })
                .then(res => res.data)
                .catch(error => error as AxiosError);
                
    const postConsultarArchivosRequest = async (request: any) => 
        apiAxios
            .post('consultararchivoscargados', request )
            .then(res => res.data)
            .catch(error => error as AxiosError);
            
    const postRechazarCargueNominaRequest = async (request: any) => 
        apiAxios
            .post('rechazarcarguenomina', request )
            .then(res => res.data)
            .catch(error => error as AxiosError);

    const postCrearSolicitudNomina = async (request: any) => 
        apiAxios
            .post('solicitudNomina', request )
            .then(res => res.data)
            .catch(error => error as AxiosError);

    const getObtenerSolicitudNomina = async (esdId: any) => 
        apiAxios
            .get(`obtenerSolicitudNomina`, {params: { request: esdId }})
            .then(res => res.data)
            .catch(error => error as AxiosError);

    const postActualizarSolicitudNomina = async (request: any) => 
        apiAxios
            .post('ActualizaSolicitudNomina', request )
            .then(res => res.data)
            .catch(error => error as AxiosError);

            
    const PostAprobarDocumentoCargueNominaSolicitud = async (request: any) => 
    apiAxios
        .post(`AprobarDocumentoCargueNominaSolicitud?request=${request}` )
        .then(res => res.data)
        .catch(error => error as AxiosError);

    const postConsultarArchivoCorregidoSolicitudNomina = async (id: number) => 
    apiAxios
        .post(`Reportes/ConsultarArchivoCorregidoSolicitudNomina?request=${id}` )
        .then(res => res.data)
        .catch(error => error as AxiosError);

    const getObtenerListaCausalesRechazo = async () => 
    apiAxios
        .get(`ConsultarCausalesRechazoNomina`)
        .then(res => res.data)
        .catch(error => error as AxiosError);


        const descarcarNominaTxt = (id: any): Promise<AxiosResponse<Blob>> =>
        apiAxios.get(`descargarArchivoNomina/${id}`, {
          responseType: 'blob',
        });

        return {
            procesarArchivosCargadosRequest,
            postCargueNominaRequest,
            obtenerArchivosCargadosRequest,
            postConsultarArchivosRequest,
            postRechazarCargueNominaRequest,
            postCrearSolicitudNomina,
            getObtenerSolicitudNomina,
            postActualizarSolicitudNomina,
            PostAprobarDocumentoCargueNominaSolicitud,
            postConsultarArchivoCorregidoSolicitudNomina,
            getObtenerListaCausalesRechazo,
            descarcarNominaTxt
        };
    }

    export default ApiCargueNomina;
