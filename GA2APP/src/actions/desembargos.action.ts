import { Dispatch } from "react";
import ApiDesembargo from "../services/api-desembargos.service";
import { AppActions } from "../types/appactions.type";
import { DesembargoDto, ObtenerDesembargoDto, TipoProcesoDesembargoDto } from "../types/desembargo.model.type";
import {
    GET_ALL_TIPOS_DESEMBARGOS_SUCCESS, 
    GET_ALL_TIPOS_PROCESOS_DESEMBARGOS_SUCCESS,
    GET_ALL_DESEMBARGOS_CUENTA_SUCCESS,
    GET_ALL_DESEMBARGOS_SUCCESS,
    GET_ALL_UPDATE_DESEMBARGOS_SUCCESS,
    GET_ALL_TIPOS_RETENCION_SUCCESS,
    GET_ALL_DESEMBARGOS_CUENTA_CONCEPTOS_SUCCESS,
    GET_ALL_CONCEPTOS_SUCCESS,
    POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS,
    POST_INSERT_DESEMBARGOS_SUCCESS,
    POST_UPDATE_DESEMBARGOS_SUCCESS,
    POST_ALL_DESEMBARGOS_BY_DATE_SUCCESS,
    POST_INSERT_DESEMBARGOS_CONCEPTO_SUCCESS,
    POST_UPDATE_DESEMBARGOS_CONCEPTO_SUCCESS,
    DELETE_DESEMBARGOS_SUCCESS, 
    DELETE_DESEMBARGOS_CONCEPTO_SUCCESS,
    ON_ERROR 
} from "../types/desembargos.actions.type";

const api = ApiDesembargo();

export const GetAllTiposDesembargosSuccess = (
    tiposDesembargo: DesembargoDto[],
): AppActions => ({
    type: GET_ALL_TIPOS_DESEMBARGOS_SUCCESS,
    tiposDesembargo: tiposDesembargo
});

export const GetAllTiposProcesosDesembargosSuccess = (
    tiposProcesos: TipoProcesoDesembargoDto[],
): AppActions => ({
    type: GET_ALL_TIPOS_PROCESOS_DESEMBARGOS_SUCCESS,
    tiposProcesos: tiposProcesos
});

export const GetAllDesembargosSuccess = (
    desembargos: DesembargoDto[],
): AppActions => ({
    type: GET_ALL_DESEMBARGOS_SUCCESS,
    desembargos: desembargos
});

export const PostAllDesembargosByDateSuccess = (
    desembargos: DesembargoDto[],
): AppActions => ({
    type: POST_ALL_DESEMBARGOS_BY_DATE_SUCCESS,
    desembargos: desembargos 
});

export const GetAllUpdateDesembargosSuccess = (
    desembargos: DesembargoDto[],
): AppActions => ({
    type: GET_ALL_UPDATE_DESEMBARGOS_SUCCESS,
    desembargos: desembargos
});

export const PostInsertDesembargosSuccess = (
    desembargo: DesembargoDto,
): AppActions => ({
    type: POST_INSERT_DESEMBARGOS_SUCCESS,
    desembargo: desembargo
});

export const PostUpdateDesembargosSuccess = (
    desembargo: DesembargoDto,
): AppActions => ({
    type: POST_UPDATE_DESEMBARGOS_SUCCESS,
    desembargo: desembargo
});

export const DeleteDesembargosSuccess = (
    desembargo: DesembargoDto,
): AppActions => ({
    type: DELETE_DESEMBARGOS_SUCCESS,
    desembargo: desembargo
});


export const GetAllDesembargosCuentaConceptosSuccess = (
    desembargosConcepto: ObtenerDesembargoDto[],
): AppActions => ({
    type: GET_ALL_DESEMBARGOS_CUENTA_CONCEPTOS_SUCCESS,
    desembargosConcepto: desembargosConcepto
});

export const PostInsertDesembargosConceptoSuccess = (
    desembargoConcepto: ObtenerDesembargoDto,
): AppActions => ({
    type: POST_INSERT_DESEMBARGOS_CONCEPTO_SUCCESS,
    desembargoConcepto: desembargoConcepto
});

export const PostUpdateDesembargosConceptoSuccess = (
    desembargoConcepto: ObtenerDesembargoDto,
): AppActions => ({
    type: POST_UPDATE_DESEMBARGOS_CONCEPTO_SUCCESS,
    desembargoConcepto: desembargoConcepto
});

export const DeleteDesembargosConceptoSuccess = (
    desembargoConcepto: ObtenerDesembargoDto,
): AppActions => ({
    type: DELETE_DESEMBARGOS_CONCEPTO_SUCCESS,
    desembargoConcepto: desembargoConcepto
});


export const PostInsertDesembargoSuccess = (
    desembargoConcepto: ObtenerDesembargoDto,
): AppActions => ({
    type: POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS,
    desembargoConcepto: desembargoConcepto
});

export const GetAllDesembargosCuentaSuccess = (
    desembargosConcepto: ObtenerDesembargoDto[],
): AppActions => ({
    type: GET_ALL_DESEMBARGOS_CUENTA_SUCCESS,
    desembargosConcepto: desembargosConcepto
});

export const GetAllConceptosSuccess = (
    conceptos: any,
): AppActions => ({
    type: GET_ALL_CONCEPTOS_SUCCESS,
    conceptos: conceptos
});

export const SetError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});


export const startGetTiposDesembargo =
    () => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getAllDesembargos();
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllTiposDesembargosSuccess(response.data));
            }
        } catch (error) {
            dispatch(SetError('server-error'));
        }
    };

export const startGetTiposProcesoDesembargo =
    () => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getAllDesembargos();
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllTiposProcesosDesembargosSuccess(response.data));
            }
        } catch (error) {
            dispatch(SetError('server-error'));
        }
    };

export const startGetDesembargos =
    () => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getAllDesembargos();
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllDesembargosSuccess(response.data));
            }
        } catch (error) {
            dispatch(SetError('server-error'));
        }
    };

export const startGetDesembargosByDate =
    (fechaInicial: Date, fechaFinal: Date) => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.postAllDesembargosByDate(fechaInicial,fechaFinal);
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(PostAllDesembargosByDateSuccess(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };