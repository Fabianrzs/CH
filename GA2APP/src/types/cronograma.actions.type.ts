import { UnidadEjecutoraCronogramaDTO} from "./cronograma.type";


export const GET_EXECUTING_UNITS_SCHEDULE = 'GET_EXECUTING_UNITS_SCHEDULE';
export const GET_EXECUTING_UNITS_SCHEDULE_SUCCESS = 'GET_EXECUTING_UNITS_SCHEDULE_SUCCESS';
export const GET_EXECUTING_UNITS_SCHEDULE_FAILURE = 'GET_EXECUTING_UNITS_SCHEDULE_FAILURE';
export const EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE = 'EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE';
export const EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_SUCCESS = 'EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_SUCCESS';
export const EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_FAILURE = 'EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_FAILURE';
export const DELETE_EXECUTING_UNITS_SCHEDULE = 'DELETE_EXECUTING_UNITS_SCHEDULE';
export const DELETE_EXECUTING_UNITS_SCHEDULE_SUCCESS = 'DELETE_EXECUTING_UNITS_SCHEDULE_SUCCESS';
export const DELETE_EXECUTING_UNITS_SCHEDULE_FAILURE = 'DELETE_EXECUTING_UNITS_SCHEDULE_FAILURE';
export const ADD_EXECUTING_UNIT_SCHEDULE = 'ADD_EXECUTING_UNIT_SCHEDULE';
export const ADD_EXECUTING_UNIT_SCHEDULE_SUCCESS = 'ADD_EXECUTING_UNIT_SCHEDULE_SUCCESS';
export const ADD_EXECUTING_UNIT_SCHEDULE_FAILURE = 'ADD_EXECUTING_UNIT_SCHEDULE_FAILURE';

export interface GetExecutingUnitsSchedule{
    type: 'GET_EXECUTING_UNITS_SCHEDULE';
}

export interface GetExecutingUnitsScheduleSuccess {
    type: 'GET_EXECUTING_UNITS_SCHEDULE_SUCCESS';
    unidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[];
}

export interface GetExecutingUnitsScheduleFailure {
    type: 'GET_EXECUTING_UNITS_SCHEDULE_FAILURE';
    unidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[] | null;
}

export interface EditReportDateExecutingUnitsSchedule {
    type: 'EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE'
}

export interface EditReportDateExecutingUnitsScheduleSuccess {
    type: 'EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_SUCCESS';
    UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaDTO[];
}


export interface EditReportDateExecutingUnitsScheduleFailure {
    type: 'EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_FAILURE';
    UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaDTO[] | null;
}

export interface DeleteExecutingUnitsSchedule{
    type: 'DELETE_EXECUTING_UNITS_SCHEDULE';
}

export interface DeleteExecutingUnitsScheduleSuccess {
    type: 'DELETE_EXECUTING_UNITS_SCHEDULE_SUCCESS';
    unidadesEjecutorasCronograma: UnidadEjecutoraCronogramaDTO[] | null;
}

export interface DeleteExecutingUnitsScheduleFailure {
    type: 'DELETE_EXECUTING_UNITS_SCHEDULE_FAILURE';
    unidadesEjecutorasCronograma: UnidadEjecutoraCronogramaDTO[] | null;
}

export interface AddExecutingUnitSchedule{
    type: 'ADD_EXECUTING_UNIT_SCHEDULE';
}

export interface AddExecutingUnitScheduleSuccess {
    type: 'ADD_EXECUTING_UNIT_SCHEDULE_SUCCESS';
    agregarUnidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[];
}

export interface AddExecutingUnitScheduleFailure {
    type: 'ADD_EXECUTING_UNIT_SCHEDULE_FAILURE';
    agregarUnidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[] | null;
}


export type CronogramaActionsType =
    | GetExecutingUnitsSchedule
    | GetExecutingUnitsScheduleFailure
    | GetExecutingUnitsScheduleSuccess
    | EditReportDateExecutingUnitsSchedule
    | EditReportDateExecutingUnitsScheduleSuccess
    | EditReportDateExecutingUnitsScheduleFailure
    | DeleteExecutingUnitsSchedule
    | DeleteExecutingUnitsScheduleFailure
    | DeleteExecutingUnitsScheduleSuccess
    | AddExecutingUnitSchedule
    | AddExecutingUnitScheduleFailure
    | AddExecutingUnitScheduleSuccess
