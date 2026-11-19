import { ListaCierreMensual } from "./cierre-mensual.model.type";

export const GET_CIERRE_MENSUAL_SUCCESS = 'GET_CIERRE_MENSUAL_SUCCESS';
export const GET_CIERRE_MENSUAL_FAILURE = 'GET_CIERRE_MENSUAL_FAILURE';



export interface GetCierreMensualSuccess {
    type: 'GET_CIERRE_MENSUAL_SUCCESS';
    CierreMensual: ListaCierreMensual[];
}


export interface GetCierreMensualFailure {
    type: 'GET_CIERRE_MENSUAL_FAILURE';
    error: any;
}

export type CierreMensualActionTypes =
|GetCierreMensualSuccess
|GetCierreMensualFailure;