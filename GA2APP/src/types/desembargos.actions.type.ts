import { DesembargoDto, ObtenerDesembargoDto, TipoProcesoDesembargoDto } from "./desembargo.model.type";

export const GET_ALL_TIPOS_DESEMBARGOS_SUCCESS = 'GET_ALL_TIPOS_DESEMBARGOS_SUCCESS';
export const GET_ALL_TIPOS_PROCESOS_DESEMBARGOS_SUCCESS = 'GET_ALL_TIPOS_PROCESOS_DESEMBARGOS_SUCCESS';
export const GET_ALL_DESEMBARGOS_CUENTA_SUCCESS = 'GET_ALL_DESEMBARGOS_CUENTA_SUCCESS';

export const GET_ALL_DESEMBARGOS_SUCCESS = 'GET_ALL_DESEMBARGOS_SUCCESS';
export const POST_ALL_DESEMBARGOS_BY_DATE_SUCCESS = 'POST_ALL_DESEMBARGOS_BY_DATE_SUCCESS';
export const GET_ALL_UPDATE_DESEMBARGOS_SUCCESS = 'GET_ALL_UPDATE_DESEMBARGOS_SUCCESS';
export const POST_INSERT_DESEMBARGOS_SUCCESS = 'POST_INSERT_DESEMBARGOS_SUCCESS';
export const POST_UPDATE_DESEMBARGOS_SUCCESS = 'POST_UPDATE_DESEMBARGOS_SUCCESS';
export const DELETE_DESEMBARGOS_SUCCESS = 'DELETE_DESEMBARGOS_SUCCESS';

export const GET_ALL_TIPOS_RETENCION_SUCCESS = 'GET_ALL_TIPOS_RETENCION_SUCCESS';
export const GET_ALL_DESEMBARGOS_CUENTA_CONCEPTOS_SUCCESS = 'GET_ALL_DESEMBARGOS_CUENTA_CONCEPTOS_SUCCESS';
export const POST_INSERT_DESEMBARGOS_CONCEPTO_SUCCESS = 'POST_INSERT_DESEMBARGOS_CONCEPTO_SUCCESS';
export const POST_UPDATE_DESEMBARGOS_CONCEPTO_SUCCESS = 'POST_UPDATE_DESEMBARGOS_CONCEPTO_SUCCESS';
export const DELETE_DESEMBARGOS_CONCEPTO_SUCCESS = 'DELETE_DESEMBARGOS_CONCEPTO_SUCCESS';



export const POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS = 'POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS';

export const GET_ALL_CONCEPTOS_SUCCESS = 'GET_ALL_CONCEPTOS_SUCCESS';

export const ON_ERROR = 'ON_ERROR';

export interface GetAllTiposDesembargosSuccess {
    type: 'GET_ALL_TIPOS_DESEMBARGOS_SUCCESS';
    tiposDesembargo: DesembargoDto[];
}
export interface GetAllTiposProcesosDesembargosSuccess {
    type: 'GET_ALL_TIPOS_PROCESOS_DESEMBARGOS_SUCCESS';
    tiposProcesos: TipoProcesoDesembargoDto[];
}
export interface GetAllDesembargosSuccess {
    type: 'GET_ALL_DESEMBARGOS_SUCCESS';
    desembargos: DesembargoDto[];
}
export interface PostAllDesembargosByDateSuccess {
    type: 'POST_ALL_DESEMBARGOS_BY_DATE_SUCCESS';
    desembargos: DesembargoDto[];
}
export interface GetAllUpdateDesembargosSuccess {
    type: 'GET_ALL_UPDATE_DESEMBARGOS_SUCCESS';
    desembargos: DesembargoDto[];
}
export interface PostInsertDesembargosSuccess {
    type: 'POST_INSERT_DESEMBARGOS_SUCCESS';
    desembargo: DesembargoDto;
}
export interface PostUpdateDesembargosSuccess {
    type: 'POST_UPDATE_DESEMBARGOS_SUCCESS';
    desembargo: DesembargoDto;
}
export interface DeleteDesembargosSuccess {
    type: 'DELETE_DESEMBARGOS_SUCCESS';
    desembargo: DesembargoDto;
}


export interface GetAllDesembargosCuentaConceptosSuccess {
    type: 'GET_ALL_DESEMBARGOS_CUENTA_CONCEPTOS_SUCCESS';
    desembargosConcepto: ObtenerDesembargoDto[];
}
export interface PostInsertDesembargosConceptoSuccess {
    type: 'POST_INSERT_DESEMBARGOS_CONCEPTO_SUCCESS';
    desembargoConcepto: ObtenerDesembargoDto;
}
export interface PostUpdateDesembargosConceptoSuccess {
    type: 'POST_UPDATE_DESEMBARGOS_CONCEPTO_SUCCESS';
    desembargoConcepto: ObtenerDesembargoDto;
}
export interface DeleteDesembargosConceptoSuccess {
    type: 'DELETE_DESEMBARGOS_CONCEPTO_SUCCESS';
    desembargoConcepto: ObtenerDesembargoDto;
}


export interface PostInsertDesembargoSuccess {
    type: 'POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS';
    desembargoConcepto: ObtenerDesembargoDto;
}

export interface GetAllDesembargosCuentaSuccess {
    type: 'GET_ALL_DESEMBARGOS_CUENTA_SUCCESS';
    desembargosConcepto: ObtenerDesembargoDto[];
}


export interface GetAllConceptosSuccess {
    type: 'GET_ALL_CONCEPTOS_SUCCESS';
    conceptos: any;
}
export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export type DesembargosType =
 | GetAllTiposDesembargosSuccess
 | GetAllDesembargosCuentaSuccess
 | GetAllTiposProcesosDesembargosSuccess
 | GetAllDesembargosCuentaSuccess
 | GetAllDesembargosSuccess
 | PostAllDesembargosByDateSuccess
 | GetAllUpdateDesembargosSuccess
 | PostInsertDesembargosSuccess
 | PostUpdateDesembargosSuccess
 | DeleteDesembargosSuccess
 | GetAllDesembargosCuentaConceptosSuccess
 | PostInsertDesembargosConceptoSuccess
 | PostUpdateDesembargosConceptoSuccess
 | DeleteDesembargosConceptoSuccess
 | PostInsertDesembargoSuccess
 | GetAllConceptosSuccess
 | OnError;