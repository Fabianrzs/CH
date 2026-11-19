import {NovedadesCronogramaDto, novedades} from './unidadEjecutora.model.type'

export const GET_ALL_UNIDADES_EJECUTORAS = "GET_ALL_UNIDADES_EJECUTORAS";

export const GET_NOVEDADES = "GET_NOVEDADES";

export interface GetUnidadesEjecutoras {
    type: 'GET_ALL_UNIDADES_EJECUTORAS';
    unidadesEjecutoras: NovedadesCronogramaDto;
}

export interface GetNovedades {
    type: 'GET_NOVEDADES';
    novedadesData: novedades;
}



export type UnidadEjecutoraType = 

    | GetUnidadesEjecutoras
    | GetNovedades;