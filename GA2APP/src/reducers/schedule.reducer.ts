import { CronogramaActionsType, GET_EXECUTING_UNITS_SCHEDULE, GET_EXECUTING_UNITS_SCHEDULE_FAILURE, GET_EXECUTING_UNITS_SCHEDULE_SUCCESS, EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE, EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_SUCCESS,
    EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_FAILURE, DELETE_EXECUTING_UNITS_SCHEDULE, DELETE_EXECUTING_UNITS_SCHEDULE_SUCCESS, DELETE_EXECUTING_UNITS_SCHEDULE_FAILURE, ADD_EXECUTING_UNIT_SCHEDULE, ADD_EXECUTING_UNIT_SCHEDULE_FAILURE, ADD_EXECUTING_UNIT_SCHEDULE_SUCCESS } from '../types/cronograma.actions.type';
import { UnidadEjecutoraCronogramaDTO } from '../types/cronograma.type';

export interface ScheduleState {
    executingUnitsSchedule: UnidadEjecutoraCronogramaDTO[] | null;
    executingUnitSchedule: UnidadEjecutoraCronogramaDTO | null;
    error: any;
    loading: boolean;
    loadingEdit: boolean;
    firstLoading: boolean;
}

const ScheduleStateInitial: ScheduleState = {
    executingUnitsSchedule: null,
    executingUnitSchedule: null,
    error: null,
    loading: false,
    loadingEdit: false,
    firstLoading: false,
}

export const ScheduleReducer = (
    state: ScheduleState = ScheduleStateInitial,
    action: CronogramaActionsType,
): ScheduleState => {

    switch (action.type) {
        case GET_EXECUTING_UNITS_SCHEDULE:
            return {
                ...state,
                executingUnitsSchedule: null,
                firstLoading: true,
            };
        case GET_EXECUTING_UNITS_SCHEDULE_FAILURE:
            return {
                ...state,
                executingUnitsSchedule: null,
                firstLoading: false,
            };
        case GET_EXECUTING_UNITS_SCHEDULE_SUCCESS:
            return {
                ...state,
                executingUnitsSchedule: action.unidadEjecutoraCronograma,
                firstLoading: false,
            };
        case EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE:
            return {
                ...state,
                loadingEdit: true,
            };
        case EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_FAILURE:
            return {
                ...state,
                executingUnitsSchedule: null,
                loadingEdit: false,
            };
        case EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_SUCCESS:
            return {
                ...state,
                loadingEdit: false,
            };
        case DELETE_EXECUTING_UNITS_SCHEDULE:
            return {
                ...state,
                loading: true,
            };
        case DELETE_EXECUTING_UNITS_SCHEDULE_FAILURE:
            return {
                ...state,
                executingUnitsSchedule: action.unidadesEjecutorasCronograma,
                loading: false,
            };
        case DELETE_EXECUTING_UNITS_SCHEDULE_SUCCESS:
            return {
                ...state,
                loading: false,
                executingUnitsSchedule: action.unidadesEjecutorasCronograma,
            };
        case ADD_EXECUTING_UNIT_SCHEDULE:
                return {
                    ...state,
                        executingUnitSchedule: null,
                        firstLoading: true,
                    };
        case ADD_EXECUTING_UNIT_SCHEDULE_FAILURE:
                return {
                        ...state,
                        executingUnitSchedule: null,
                        firstLoading: false,
                    };
        case ADD_EXECUTING_UNIT_SCHEDULE_SUCCESS:
                return {
                            ...state,
                            executingUnitsSchedule: action.agregarUnidadEjecutoraCronograma,
                            firstLoading: false,
                        };

        default:
            return state;
    }
};
