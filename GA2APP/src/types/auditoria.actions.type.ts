import { Auditoria, LogAuditorias, TablasAuditoria } from "./Auditoria.model.type";

export const GET_TABLAS_AUDITORIA_SUCCESS = 'GET_TABLAS_AUDITORIA_SUCCESS';
export const GET_TABLAS_AUDITORIA_FAILURE = 'GET_TABLAS_AUDITORIA_FAILURE';
export const GET_LOG_AUDITORIA_SUCCESS = 'GET_LOG_AUDITORIA_SUCCESS';
export const GET_LOG_AUDITORIA_FAILURE = 'GET_LOG_AUDITORIA_FAILURE';
export const GET_LOG_AUDITORIA_DETALLE = 'GET_LOG_AUDITORIA_DETALLE';
export const POST_INSERT_AUDITORIA_SUCCESS='POST_INSERT_AUDITORIA_SUCCESS';
export const POST_INSERT_AUDITORIA_FAILURE='POST_INSERT_AUDITORIA_FAILURE';

export interface GetTablasAuditoria {
    type:  typeof GET_TABLAS_AUDITORIA_SUCCESS;
    Tablas: TablasAuditoria[];     
}

export interface GetLogAuditoria {
    type:  typeof GET_LOG_AUDITORIA_SUCCESS;
    Log: LogAuditorias;     
}

export interface GetTablasAuditoriaFailure {
    type: typeof GET_TABLAS_AUDITORIA_FAILURE;
    error: any;
}

export interface GetLogAuditoriaFailure {
    type: typeof GET_LOG_AUDITORIA_FAILURE;
    error: any;
}

export interface GetLogAuditoriaDetalle {
    type: typeof GET_LOG_AUDITORIA_DETALLE;
    Detalle: Auditoria;
}

export interface PostInsertAsesoriaSuccess {
    type: typeof POST_INSERT_AUDITORIA_SUCCESS;
    Auditoria: Auditoria;
}

export interface PostInsertAsesoriaFailure {
    type: typeof POST_INSERT_AUDITORIA_FAILURE;
    error: any;
}

export type AuditoriaActionsTypes =  
    |GetTablasAuditoria
    |GetLogAuditoria
    |GetTablasAuditoriaFailure
    |GetLogAuditoriaFailure
    |PostInsertAsesoriaSuccess
    |PostInsertAsesoriaFailure
    |GetLogAuditoriaDetalle;
    



