import { Dispatch } from "react";
import ApiAuditorias from "../services/api-auditorias.servise";
import AuthService from "../services/auth.service";
import { AppActions } from "../types/appactions.type";
import { POST_INSERT_AUDITORIA_SUCCESS,POST_INSERT_AUDITORIA_FAILURE,GET_LOG_AUDITORIA_FAILURE, GET_LOG_AUDITORIA_SUCCESS, GET_TABLAS_AUDITORIA_FAILURE, GET_TABLAS_AUDITORIA_SUCCESS,GET_LOG_AUDITORIA_DETALLE } from "../types/auditoria.actions.type";
import { Auditoria, ConsultaAuditoria, LogAuditorias, TablasAuditoria } from "../types/Auditoria.model.type";


const api=ApiAuditorias();

export const GetTablasAuditoriaSuccess= (tablas: TablasAuditoria[]): AppActions => ({
    type: GET_TABLAS_AUDITORIA_SUCCESS,
    Tablas:tablas,
});

export const GetTablasAuditoriaFailure= (error: any): AppActions => ({
    type: GET_TABLAS_AUDITORIA_FAILURE ,
    error: error,
});

export const GetLogAuditoriaSuccess= (log: LogAuditorias): AppActions => ({
    type: GET_LOG_AUDITORIA_SUCCESS,
    Log:log,
});

export const GetLogAuditoriaFailure= (error: any): AppActions => ({
    type: GET_LOG_AUDITORIA_FAILURE,
    error: error,
});

export const SetLogAuditoriaDetalle= (auditoria: Auditoria): AppActions => ({
    type: GET_LOG_AUDITORIA_DETALLE,
    Detalle:auditoria,
});

export const PostInsertAsesoriaSuccess = (
    auditoria: Auditoria,
): AppActions => ({
    type: POST_INSERT_AUDITORIA_SUCCESS,
    Auditoria: auditoria
});

export const PostInsertAsesoriaFailure= (error: any): AppActions => ({
    type: POST_INSERT_AUDITORIA_FAILURE,
    error: error,
});


export const startGetTablasAuditoria = () => async (dispatch: Dispatch<AppActions>) => {

    const response = await api.GetTablasAuditoria();

    if (response?.isSuccess == null || !response?.isSuccess) {
       dispatch(GetTablasAuditoriaFailure(response.data));
    } else {
        dispatch(GetTablasAuditoriaSuccess(response.data));
    }

};

export const startGetLogAuditoria = (auditoria: ConsultaAuditoria) => async (dispatch: Dispatch<AppActions>) => {

    const response = await api.GetLogAuditoria(auditoria);

    if (response?.isSuccess == null || !response?.isSuccess) {
       dispatch(GetLogAuditoriaFailure(response.data));
    } else {
       dispatch(GetLogAuditoriaSuccess(response.data));
    }
};

export const startPostAuditoria =
(auditoria: Auditoria) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.InsertAuditoria(auditoria);
        if (response?.isSuccess == null) {
            dispatch(PostInsertAsesoriaFailure(response));
        } else {
            dispatch(PostInsertAsesoriaSuccess(response.data));
        }
    } catch (e) {
        dispatch(PostInsertAsesoriaFailure('server-error'));
    }
};
