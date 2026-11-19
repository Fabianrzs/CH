import { Dispatch } from 'redux';
import { toast } from 'react-toastify';
import swal from 'sweetalert';
import ApiCargueNomina from '../services/api-carguenomina.service';
import { AppActions } from '../types/appactions.type';
import {
    ON_ERROR,
    POST_ARCHIVO_NOMINA,
    GET_ARCHIVO_NOMINA,
    POST_ARCHIVO_NOMINA_SUCCESS,
    OBTENER_ARCHIVOS_CARGADOS,
    OBTENER_ARCHIVOS_CARGADOS_SUCCESS,
    PROCESAR_ARCHIVOS_CARGADOS,
    PROCESAR_ARCHIVOS_CARGADOS_SUCCESS,
    CREAR_SOLICITUD_NOMINA,
    CREAR_SOLICITUD_NOMINA_SUCCESS,
    ACTUALIZAR_SOLICITUD_NOMINA,
    ACTUALIZAR_SOLICITUD_NOMINA_SUCCESS,
    OBTENER_SOLICITUD_NOMINA,
    OBTENER_SOLICITUD_NOMINA_SUCCESS,
    POST_ARCHIVO_NOMINA_SUCCESS_CARGA,
    CONSULTAR_ARCHIVOS_CARGADOS,
    CONSULTAR_ARCHIVOS_CARGADOS_SUCCESS,
    RECHAZAR_ARCHIVO_NOMINA_SUCCESS,
    RECHAZAR_ARCHIVO_NOMINA,
} from '../types/cargue-nomina.action.type';
import {
    ArchivosCargados,
    CargueNominaDto,
    DocumentoCarga,
    EjecutorDto,
} from '../types/cargue-nomina.model.type';
import NotificationService from '../services/notification.service';
import {
    NotificationCardModel,
    NotificationCardModelEnum,
} from '../models/requests/notification-cards-model';
import { AppDispatch } from '../store/store';



const api = ApiCargueNomina();

export const PostArchivoNomina = (formData: FormData): AppActions => ({
    type: POST_ARCHIVO_NOMINA,
    formData: formData,
});

export const PostArchivoNominaSuccess = (
    ejecutor: EjecutorDto,
): AppActions => ({
    type: POST_ARCHIVO_NOMINA_SUCCESS,
    ejecutor,
});


export const PostArchivoNominaSuccessCarga = (
    documentos: DocumentoCarga[],
): AppActions => ({
    type: POST_ARCHIVO_NOMINA_SUCCESS_CARGA,
    documentos,
});

export const GetArchivoNomina = (
    cargueNomina: CargueNominaDto,
): AppActions => ({
    type: GET_ARCHIVO_NOMINA,
    cargueNomina: cargueNomina,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const ObtenerArchivosCargados = (esdId: number): AppActions => ({
    type: OBTENER_ARCHIVOS_CARGADOS,
    esdId,
});

export const ObtenerArchivosCargadosSuccess = (
    documentos: DocumentoCarga[],
): AppActions => ({
    type: OBTENER_ARCHIVOS_CARGADOS_SUCCESS,
    documentos,
});

export const ConsultarArchivosCargados = (esdId: number): AppActions => ({
    type: CONSULTAR_ARCHIVOS_CARGADOS,
    esdId,
});


export const ConsultarArchivosCargadosSuccess = (
    consultaArchivosCargados: ArchivosCargados[],
): AppActions => ({
    type: CONSULTAR_ARCHIVOS_CARGADOS_SUCCESS,
    consultaArchivosCargados,
});

export const RechazarCargueNomina = (esdId: number): AppActions => ({
    type: RECHAZAR_ARCHIVO_NOMINA,
    esdId,
});

export const RechazarCargueNominaSuccess = (
    rechazarCargueNomina: ArchivosCargados[],
): AppActions => ({
    type: RECHAZAR_ARCHIVO_NOMINA_SUCCESS,
    rechazarCargueNomina,
});

export const ProcesarArchivosCargados = (
    documentosPorProcesar: DocumentoCarga[],
): AppActions => ({
    type: PROCESAR_ARCHIVOS_CARGADOS,
    documentosPorProcesar,
});

export const ProcesarArchivosCargadosSuccess = (
    documentosProcesados: ArchivosCargados[],
): AppActions => ({
    type: PROCESAR_ARCHIVOS_CARGADOS_SUCCESS,
    documentosProcesados,
});

export const CrearSolicitudNomina = (
    documentosPorProcesar: DocumentoCarga[],
): AppActions => ({
    type: CREAR_SOLICITUD_NOMINA,
    documentosPorProcesar,
});

export const CrearSolicitudNominaSuccess = (
    documentosProcesados: DocumentoCarga[],
): AppActions => ({
    type: CREAR_SOLICITUD_NOMINA_SUCCESS,
    documentosProcesados,
});
export const ActualizarSolicitudNomina = (
    documentosPorProcesar: DocumentoCarga[],
): AppActions => ({
    type: ACTUALIZAR_SOLICITUD_NOMINA,
    documentosPorProcesar,
});

export const ActualizarSolicitudNominaSuccess = (
    documentosProcesados: DocumentoCarga[],
): AppActions => ({
    type: ACTUALIZAR_SOLICITUD_NOMINA_SUCCESS,
    documentosProcesados,
});
export const ObtenerSolicitudNomina = (
    documentosPorProcesar: DocumentoCarga[],
): AppActions => ({
    type: OBTENER_SOLICITUD_NOMINA,
    documentosPorProcesar,
});

export const ObtenerSolicitudNominaSuccess = (
    documentosProcesados: DocumentoCarga[],
): AppActions => ({
    type: OBTENER_SOLICITUD_NOMINA_SUCCESS,
    documentosProcesados,
});

export const startProcesarArchivosCargados =
    (documentosPorProcesar: any) =>
        async (dispatch: Dispatch<AppActions>) => {
            dispatch(ProcesarArchivosCargados(documentosPorProcesar));
            const response = await api.procesarArchivosCargadosRequest(
                documentosPorProcesar,
            );

            if (response.isAxiosError) {
                dispatch(OnError(response));
                toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.WARNING })

            } else {
                toast(` ${response.returnMessage}`, { type: toast.TYPE.SUCCESS })
                dispatch(ProcesarArchivosCargadosSuccess(response.data));
            }
        };

export const startCrearSolicitudNomina =
    (documentosPorProcesar: any) =>
        async (dispatch: Dispatch<AppActions>) => {
            dispatch(CrearSolicitudNomina(documentosPorProcesar));
            const response = await api.postCrearSolicitudNomina(
                documentosPorProcesar,
            );

            if (response.isAxiosError) {
                toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.WARNING })
                dispatch(OnError(response));
            } else if (response.isSuccess === true && response.data.length === 0) {
                toast('Consulta sin datos disponibles', { type: toast.TYPE.WARNING })
                dispatch(OnError(response))
            } else {
                toast('Solicitud Creada', { type: toast.TYPE.SUCCESS })
                dispatch(CrearSolicitudNominaSuccess(response.data));
            }
        };

export const startActualizarSolicitudNomina =
    (documentosPorProcesar: any) =>
        async (dispatch: Dispatch<AppActions>) => {
            dispatch(ActualizarSolicitudNomina(documentosPorProcesar));
            const response = await api.postActualizarSolicitudNomina(
                documentosPorProcesar,
            );

            if (response.isAxiosError) {
                toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.WARNING })
                dispatch(OnError(response));
            } else if (response.isSuccess === true && response.data.length === 0) {
                toast('Respuesta sin datos disponibles', { type: toast.TYPE.WARNING })
                dispatch(OnError(response))
            } else {
                toast('Solicitud Actualizada', { type: toast.TYPE.SUCCESS })
                dispatch(ActualizarSolicitudNominaSuccess(response.data));
            }
        };

export const startObtenerSolicitudNomina = (documentosPorProcesar: any) =>
    async (dispatch: Dispatch<AppActions>) => {
        dispatch(ObtenerSolicitudNomina(documentosPorProcesar));
        const response = await api.getObtenerSolicitudNomina(
            documentosPorProcesar,
        );

        if (response.isAxiosError) {
            toast(`Error: ${response.returnMessage}`, { type: toast.TYPE.WARNING })
            dispatch(OnError(response));
        } else if (response.isSuccess === true && ((response.data && response.data.length === 0) || response.data === null)) {
            toast('Consulta sin datos disponibles', { type: toast.TYPE.WARNING })
            dispatch(OnError(response))
        } else {
            toast('Consulta de Solicitud Exitosa', { type: toast.TYPE.SUCCESS })
            dispatch(ObtenerSolicitudNominaSuccess(response.data));
        }
    };

export const startRechazarCargueNomina =
    (requestConsulta: any) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(RechazarCargueNomina(requestConsulta));
        const response = await api.postRechazarCargueNominaRequest(requestConsulta);
        if (response.isAxiosError) {
            toast(`Error: ${response.response.data.returnMessage}`, { type: toast.TYPE.ERROR })
            dispatch(OnError(response));
        } else {
            await NotificationService.genericAlert(
                '¡Éxitoso!',
                'Rechazado con Éxito.',
            );
            toast('Rechazado con Éxito', { type: toast.TYPE.SUCCESS })
            dispatch(RechazarCargueNominaSuccess(response.data));
        }
    };

export const startObtenerArchivosCargados =
    (esdId: number) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(ObtenerArchivosCargados(esdId));
        const response = await api.obtenerArchivosCargadosRequest(esdId);
        if (response.isAxiosError) {
            dispatch(OnError(response));
            await NotificationService.showNotification({
                message: response.response.data.returnMessage,
                type: NotificationCardModelEnum.error,
            } as NotificationCardModel);
        } else {
            dispatch(ObtenerArchivosCargadosSuccess(response.data));
        }
    };

export const startConsultarArchivosCargados =
    (requestConsulta: any, showToast = false) =>
        async (dispatch: AppDispatch) => {

            dispatch(ConsultarArchivosCargados(requestConsulta));

            const response = await api.postConsultarArchivosRequest(requestConsulta);

            if (response.isAxiosError) {
                dispatch(OnError(response));
                if (showToast) {
                    toast(`Error: ${response.response.data.returnMessage}`, {
                        type: toast.TYPE.WARNING
                    });
                }
                return;
            }

            if (response.isSuccess === true && response.data.length === 0) {
                dispatch(OnError(response));
                if (showToast) {
                    toast('Consulta sin datos disponibles', {
                        type: toast.TYPE.WARNING
                    });
                }
                return;
            }

            dispatch(ConsultarArchivosCargadosSuccess(response.data));

            if (showToast) {
                toast.success('Consulta Exitosa');
            }
        };


export const startPostArchivoNomina =
    (formData: FormData) =>
        async (dispatch: Dispatch<AppActions>) => {
            dispatch(PostArchivoNomina(formData));

            const response = await api.postCargueNominaRequest(formData);

            if (response?.isAxiosError) {
                dispatch(OnError(response));
                await NotificationService.showNotification({
                    message: response.response?.data?.returnMessage || 'Error',
                    type: NotificationCardModelEnum.error,
                } as NotificationCardModel);
            } else if (!response.data) {
                dispatch(ObtenerArchivosCargados(1));
                await NotificationService.showNotification({
                    message: `Error carga: ${response.returnMessage}`,
                    type: NotificationCardModelEnum.error,
                } as NotificationCardModel);
            } else {
                dispatch(PostArchivoNominaSuccessCarga([response.data]));
                swal({
                    text: 'FINALIZADO, Cargue exitoso en unidad ejecutora',
                    icon: 'success',
                });

            }
        };


