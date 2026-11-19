import {
    ArchivosCargados,
    CargueNominaDto,
    DocumentoCarga,
    EjecutorDto,
} from './cargue-nomina.model.type';

// Especificacion de accion
export const POST_ARCHIVO_NOMINA = 'POST_ARCHIVO_NOMINA';
export const GET_ARCHIVO_NOMINA = 'GET_ARCHIVO_NOMINA';
export const POST_ARCHIVO_NOMINA_SUCCESS = 'POST_ARCHIVO_NOMINA_SUCCESS';
export const POST_ARCHIVO_NOMINA_SUCCESS_CARGA = 'POST_ARCHIVO_NOMINA_SUCCESS_CARGA';

export const ON_ERROR = 'ON_ERROR';
export const OBTENER_ARCHIVOS_CARGADOS = 'OBTENER_ARCHIVOS_CARGADOS';
export const OBTENER_ARCHIVOS_CARGADOS_SUCCESS =
    'OBTENER_ARCHIVOS_CARGADOS_SUCCESS';
export const CONSULTAR_ARCHIVOS_CARGADOS = 'CONSULTAR_ARCHIVOS_CARGADOS';
export const CONSULTAR_ARCHIVOS_CARGADOS_SUCCESS = 'CONSULTAR_ARCHIVOS_CARGADOS_SUCCESS';
export const RECHAZAR_ARCHIVO_NOMINA = 'RECHAZAR_ARCHIVO_NOMINA';
export const RECHAZAR_ARCHIVO_NOMINA_SUCCESS = 'RECHAZAR_ARCHIVO_NOMINA_SUCCESS';
export const PROCESAR_ARCHIVOS_CARGADOS = 'PROCESAR_ARCHIVOS_CARGADOS';
export const PROCESAR_ARCHIVOS_CARGADOS_SUCCESS ='PROCESAR_ARCHIVOS_CARGADOS_SUCCESS';
export const CREAR_SOLICITUD_NOMINA ='CREAR_SOLICITUD_NOMINA';
export const CREAR_SOLICITUD_NOMINA_SUCCESS ='CREAR_SOLICITUD_NOMINA_SUCCESS';
export const ACTUALIZAR_SOLICITUD_NOMINA ='ACTUALIZAR_SOLICITUD_NOMINA';
export const ACTUALIZAR_SOLICITUD_NOMINA_SUCCESS ='ACTUALIZAR_SOLICITUD_NOMINA_SUCCESS';
export const OBTENER_SOLICITUD_NOMINA ='OBTENER_SOLICITUD_NOMINA';
export const OBTENER_SOLICITUD_NOMINA_SUCCESS ='OBTENER_SOLICITUD_NOMINA_SUCCESS';

export interface PostArchivoNomina {
    type: 'POST_ARCHIVO_NOMINA';
    formData: FormData;
}

export interface PostArchivoNominaSuccess {
    type: 'POST_ARCHIVO_NOMINA_SUCCESS';
    ejecutor: EjecutorDto;
}

export interface PostArchivoNominaSuccessCarga {
    type: 'POST_ARCHIVO_NOMINA_SUCCESS_CARGA';
    documentos: DocumentoCarga[];
}

export interface GetArchivoNomina {
    type: 'GET_ARCHIVO_NOMINA';
    cargueNomina: CargueNominaDto;
}

export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export interface ObtenerArchivosCargados {
    type: 'OBTENER_ARCHIVOS_CARGADOS';
    esdId: number;
}

export interface ObtenerArchivosCargadosSuccess {
    type: 'OBTENER_ARCHIVOS_CARGADOS_SUCCESS';
    documentos: DocumentoCarga[];
}

export interface ConsultarArchivosCargados {
    type: 'CONSULTAR_ARCHIVOS_CARGADOS';
    esdId: number;
}

export interface ConsultarArchivosCargadosSuccess {
    type: 'CONSULTAR_ARCHIVOS_CARGADOS_SUCCESS';
    consultaArchivosCargados: ArchivosCargados[];
}
export interface RechazarCargueNomina {
    type: 'RECHAZAR_ARCHIVO_NOMINA';
    esdId: number;
}

export interface RechazarCargueNominaSuccess {
    type: 'RECHAZAR_ARCHIVO_NOMINA_SUCCESS';
    rechazarCargueNomina: ArchivosCargados[];
}

export interface ProcesarArchivosCargados {
    type: 'PROCESAR_ARCHIVOS_CARGADOS';
    documentosPorProcesar: DocumentoCarga[];
}

export interface ProcesarArchivosCargadosSuccess {
    type: 'PROCESAR_ARCHIVOS_CARGADOS_SUCCESS';
    documentosProcesados: ArchivosCargados[];
}

export interface CrearSolicitudNomina {
    type: 'CREAR_SOLICITUD_NOMINA';
    documentosPorProcesar: DocumentoCarga[];
}

export interface CrearSolicitudNominaSuccess {
    type: 'CREAR_SOLICITUD_NOMINA_SUCCESS';
    documentosProcesados: DocumentoCarga[];
}

export interface ActualizarSolicitudNomina {
    type: 'ACTUALIZAR_SOLICITUD_NOMINA';
    documentosPorProcesar: DocumentoCarga[];
}

export interface ActualizarSolicitudNominaSuccess {
    type: 'ACTUALIZAR_SOLICITUD_NOMINA_SUCCESS';
    documentosProcesados: DocumentoCarga[];
}

export interface ObtenerSolicitudNomina {
    type: 'OBTENER_SOLICITUD_NOMINA';
    documentosPorProcesar: DocumentoCarga[];
}

export interface ObtenerSolicitudNominaSuccess {
    type: 'OBTENER_SOLICITUD_NOMINA_SUCCESS';
    documentosProcesados: DocumentoCarga[];
}

export type CargueNominaActionTypes =
    | PostArchivoNomina
    | PostArchivoNominaSuccess
    | GetArchivoNomina
    | OnError
    | ObtenerArchivosCargados
    | ObtenerArchivosCargadosSuccess
    | ConsultarArchivosCargados
    | ConsultarArchivosCargadosSuccess
    | RechazarCargueNominaSuccess
    | RechazarCargueNomina
    | ProcesarArchivosCargados
    | PostArchivoNominaSuccessCarga
    | ProcesarArchivosCargadosSuccess
    | CrearSolicitudNomina
    | CrearSolicitudNominaSuccess
    | ObtenerSolicitudNomina
    | ObtenerSolicitudNominaSuccess
    | ActualizarSolicitudNomina
    | ActualizarSolicitudNominaSuccess;
