import { JuzgadoDto } from "./court.model.type";

export const POST_ALL_COURTS_BY_DEPARTAMENT_SUCCESS = 'POST_ALL_COURTS_BY_DEPARTAMENT_SUCCESS';
export const POST_ALL_COURTS_BY_CITY_SUCCESS = 'POST_ALL_COURTS_BY_CITY_SUCCESS';
export const POST_INSERT_COURT_SUCCESS = 'POST_INSERT_COURT_SUCCESS';
export const POST_UPDATE_COURT_SUCCESS = 'POST_UPDATE_COURT_SUCCESS';
export const DELETE_COURT_SUCCESS = 'DELETE_COURT_SUCCESS';
export const ON_ERROR = 'ON_ERROR';

export interface PostAllCourtsByDepartamentSuccess {
    type: 'POST_ALL_COURTS_BY_DEPARTAMENT_SUCCESS';
    courts: JuzgadoDto[];
}

export interface PostAllCourtsByCitySuccess {
    type: 'POST_ALL_COURTS_BY_CITY_SUCCESS';
    courts: JuzgadoDto[];
}

export interface PostInsertCourtSuccess {
    type: 'POST_INSERT_COURT_SUCCESS';
    court: JuzgadoDto;
}

export interface PostUpdateCourtSuccess {
    type: 'POST_UPDATE_COURT_SUCCESS';
    court: JuzgadoDto;
}
export interface deleteCourtSuccess {
    type: 'DELETE_COURT_SUCCESS';
    court: JuzgadoDto;
}
export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export type CourtsType = 
 | PostAllCourtsByDepartamentSuccess
 | PostAllCourtsByCitySuccess
 | PostInsertCourtSuccess
 | PostUpdateCourtSuccess
 | deleteCourtSuccess
 | OnError;