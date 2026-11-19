import { NovedadesAfiliadoDto } from "./novedad.model.type";

export const GET_ALL_CONCEPTOS_SUCCESS = 'GET_ALL_CONCEPTOS_SUCCESS';

export const NOVEDADES_GET_NOVEDADES_AFILIADO_SUCCESS = 'NOVEDADES_GET_NOVEDADES_AFILIADO_SUCCESS';

export const NOVEDADES_POST_INSERTAR_NOVEDAD_AFILIADO_SUCCESS = 'NOVEDADES_POST_INSERTAR_NOVEDAD_AFILIADO_SUCCESS';

export const ON_ERROR = 'ON_ERROR';

export interface GetAllConceptosSuccess {
    type: 'GET_ALL_CONCEPTOS_SUCCESS';
    conceptos: any;
}

export interface GetNovedadesAfiliadoSuccess {
    type: 'NOVEDADES_GET_NOVEDADES_AFILIADO_SUCCESS';
    novedadesConcepto: NovedadesAfiliadoDto;
}

export interface postInsertarNovedadAfiliadoSuccess {
    type: 'NOVEDADES_POST_INSERTAR_NOVEDAD_AFILIADO_SUCCESS';
    novedadesConcepto: NovedadesAfiliadoDto;
}


export interface OnErrorAction {
    type: 'ON_ERROR';
    error: any;
}

export type NovedadesType =
    | GetAllConceptosSuccess
    | GetNovedadesAfiliadoSuccess
    | postInsertarNovedadAfiliadoSuccess
    | OnErrorAction;
 