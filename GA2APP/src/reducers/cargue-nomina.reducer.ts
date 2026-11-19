import { CargueNomina } from '../pages/cargue_nomina/constants/cargue-nomina.model.type';
import {
    CargueNominaActionTypes,
    ON_ERROR,
    POST_ARCHIVO_NOMINA,
    GET_ARCHIVO_NOMINA,
    POST_ARCHIVO_NOMINA_SUCCESS,
    OBTENER_ARCHIVOS_CARGADOS_SUCCESS,
    OBTENER_ARCHIVOS_CARGADOS,
    CONSULTAR_ARCHIVOS_CARGADOS,
    CONSULTAR_ARCHIVOS_CARGADOS_SUCCESS,
    PROCESAR_ARCHIVOS_CARGADOS,
    PROCESAR_ARCHIVOS_CARGADOS_SUCCESS,
    POST_ARCHIVO_NOMINA_SUCCESS_CARGA,
    RECHAZAR_ARCHIVO_NOMINA,
    RECHAZAR_ARCHIVO_NOMINA_SUCCESS,
    CREAR_SOLICITUD_NOMINA_SUCCESS,
    CREAR_SOLICITUD_NOMINA,
    OBTENER_SOLICITUD_NOMINA,
    OBTENER_SOLICITUD_NOMINA_SUCCESS,
    ACTUALIZAR_SOLICITUD_NOMINA,
    ACTUALIZAR_SOLICITUD_NOMINA_SUCCESS
} from '../types/cargue-nomina.action.type';
import {
    ArchivosCargados,
    CargueNominaDto,
    DocumentoCarga,
    EjecutorDto,
} from '../types/cargue-nomina.model.type';

export interface CargueNominaState {
    cargueNomina: CargueNominaDto;
    formData: FormData | null;
    error: any;
    loading: boolean;
    ejecutor: EjecutorDto | null;
    documentosCarga: DocumentoCarga[];
    esdId: number;
    documentosPorProcesar: DocumentoCarga[];
    documentosProcesados: DocumentoCarga[];
    consultaArchivosCargadosN: ArchivosCargados[];
    rechazarCargueNomina: ArchivosCargados[];
}

const CargueNominaInitialState: CargueNominaState = {
    cargueNomina: CargueNomina,
    error: null,
    loading: false,
    ejecutor: null,
    formData: null,
    documentosCarga: [],
    esdId: 0,
    documentosPorProcesar: [],
    documentosProcesados: [],
    consultaArchivosCargadosN: [],
    rechazarCargueNomina: [],

};

export const CargueNominaReducer = (
    state: CargueNominaState = CargueNominaInitialState,
    action: CargueNominaActionTypes,
): CargueNominaState => {
    switch (action.type) {
        case POST_ARCHIVO_NOMINA:
            return {
                ...state,
                formData: action.formData,
                loading: true,
            };
        case POST_ARCHIVO_NOMINA_SUCCESS:
            return {
                ...state,
                ejecutor: action.ejecutor,
                loading: false,
            };
        case POST_ARCHIVO_NOMINA_SUCCESS_CARGA:
            return {
                ...state,
                loading: false,
                documentosCarga: action.documentos
            };
        case OBTENER_ARCHIVOS_CARGADOS:
            return {
                ...state,
                loading: true,
                esdId: action.esdId,
            };
        case OBTENER_ARCHIVOS_CARGADOS_SUCCESS:
            return {
                ...state,
                documentosCarga: action.documentos,
                loading: false,
            };
        case CONSULTAR_ARCHIVOS_CARGADOS:
            return {
                ...state,
                loading: true,
                esdId: action.esdId,
            };
        case CONSULTAR_ARCHIVOS_CARGADOS_SUCCESS:
            const newData = action.consultaArchivosCargados
            .map(item => ({ ...item, errores: '' }));
            return {
                ...state,
                consultaArchivosCargadosN: newData,
                loading: false,
            };
        case RECHAZAR_ARCHIVO_NOMINA:
            return {
                ...state,
                loading: true,
                esdId: action.esdId,
            }; 
        case RECHAZAR_ARCHIVO_NOMINA_SUCCESS:

            return {
                ...state,
                rechazarCargueNomina: action.rechazarCargueNomina,
                loading: false,
            };
        case PROCESAR_ARCHIVOS_CARGADOS:
            return {
                ...state,
                loading: true,
                documentosPorProcesar: action.documentosPorProcesar,
            };
        case PROCESAR_ARCHIVOS_CARGADOS_SUCCESS:
            return {
                ...state,
                consultaArchivosCargadosN: action.documentosProcesados,
                loading: false,
            };
        case CREAR_SOLICITUD_NOMINA:
            return {
                ...state,
                loading: true,
                documentosPorProcesar: action.documentosPorProcesar,
            };
        case CREAR_SOLICITUD_NOMINA_SUCCESS:
            return {
                ...state,
                documentosCarga: action.documentosProcesados,
                loading: false,
            };
        case OBTENER_SOLICITUD_NOMINA:
            return {
                ...state,
                loading: true,
                documentosPorProcesar: action.documentosPorProcesar,
            };
        case OBTENER_SOLICITUD_NOMINA_SUCCESS:
            return {
                ...state,
                documentosCarga: action.documentosProcesados,
                loading: false,
            };
        case ACTUALIZAR_SOLICITUD_NOMINA:
            return {
                ...state,
                loading: true,
                documentosPorProcesar: action.documentosPorProcesar,
            };
        case ACTUALIZAR_SOLICITUD_NOMINA_SUCCESS:
            return {
                ...state,
                documentosCarga: action.documentosProcesados,
                loading: false,
            };
        case GET_ARCHIVO_NOMINA:
            return {
                ...state,
                cargueNomina: action.cargueNomina,
                loading: false,
            };
        case ON_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        default:
            return state;
    }
    return state;
};
