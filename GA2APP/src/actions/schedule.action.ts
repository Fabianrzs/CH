import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import ApiSchedule from '../services/api-cronograma.service';
import { GET_EXECUTING_UNITS_SCHEDULE, GET_EXECUTING_UNITS_SCHEDULE_FAILURE, GET_EXECUTING_UNITS_SCHEDULE_SUCCESS, EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE, EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_SUCCESS,
       EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_FAILURE, DELETE_EXECUTING_UNITS_SCHEDULE, DELETE_EXECUTING_UNITS_SCHEDULE_SUCCESS, DELETE_EXECUTING_UNITS_SCHEDULE_FAILURE, ADD_EXECUTING_UNIT_SCHEDULE, ADD_EXECUTING_UNIT_SCHEDULE_SUCCESS, ADD_EXECUTING_UNIT_SCHEDULE_FAILURE} from '../types/cronograma.actions.type';
import { UnidadEjecutoraCronogramaDTO, UnidadEjecutoraCronogramaEditarDTO } from '../types/cronograma.type';
import NotificationService from '../services/notification.service';

const api = ApiSchedule();

export const GetExecutingUnitsSchedule = (): AppActions => ({
    type: GET_EXECUTING_UNITS_SCHEDULE,
});

export const GetExecutingUnitsScheduleSuccess = (unidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[]): AppActions => ({
    type: GET_EXECUTING_UNITS_SCHEDULE_SUCCESS,
    unidadEjecutoraCronograma: unidadEjecutoraCronograma,
});

export const GetExecutingUnitsScheduleFailure = (unidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[] | null) : AppActions => ({
    type: GET_EXECUTING_UNITS_SCHEDULE_FAILURE,
    unidadEjecutoraCronograma: unidadEjecutoraCronograma,
});

export const EditReportDateExecutingUnitsSchedule = (): AppActions => ({
    type: EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE,
});

export const EditReportDateExecutingUnitsScheduleSuccess = (UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaDTO[]): AppActions => ({
    type: EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_SUCCESS,
    UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaEditar,
});

export const EditReportDateExecutingUnitsScheduleFailure = (UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaDTO[] | null) : AppActions => ({
    type: EDIT_REPORT_DATE_EXECUTING_UNITS_SCHEDULE_FAILURE,
    UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaEditar,
});


export const DeleteExecutingUnitsSchedule = (): AppActions => ({
    type: DELETE_EXECUTING_UNITS_SCHEDULE,
});

export const DeleteExecutingUnitsScheduleSuccess = (UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaDTO[]): AppActions => ({
    type: DELETE_EXECUTING_UNITS_SCHEDULE_SUCCESS,
    unidadesEjecutorasCronograma: UnidadEjecutoraCronogramaEditar,
});

export const DeleteExecutingUnitsScheduleFailure = (UnidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaDTO[] | null) : AppActions => ({
    type: DELETE_EXECUTING_UNITS_SCHEDULE_FAILURE,
    unidadesEjecutorasCronograma: UnidadEjecutoraCronogramaEditar,
});

export const AddExecutingUnitSchedule = (): AppActions => ({
    type: ADD_EXECUTING_UNIT_SCHEDULE,
});

export const AddExecutingUnitScheduleSuccess = (agregarUnidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[]): AppActions => ({
    type: ADD_EXECUTING_UNIT_SCHEDULE_SUCCESS,
    agregarUnidadEjecutoraCronograma: agregarUnidadEjecutoraCronograma,
});

export const AddExecutingUnitScheduleFailure = (agregarUnidadEjecutoraCronograma: UnidadEjecutoraCronogramaDTO[] | null) : AppActions => ({
    type: ADD_EXECUTING_UNIT_SCHEDULE_FAILURE,
    agregarUnidadEjecutoraCronograma: agregarUnidadEjecutoraCronograma,
});

export const startGetExecutingUnitsSchedule = () =>
        async (dispatch: Dispatch<AppActions>) => {
            try {
                dispatch(GetExecutingUnitsSchedule());
                const response =
                    await api.getExecutingUnitsSchedule();
                if (!response?.isSuccess) {
                    dispatch(
                        GetExecutingUnitsScheduleFailure(
                            response.data,
                        ),
                    );
                    toast('Error: respuesta errada del servidor');
                } else {
                    dispatch(
                        GetExecutingUnitsScheduleSuccess(
                            response.data,
                        ),
                    );
                    if (response.data == null) {
                        toast('No se encontaron unidades ejecutoras.');
                    }
                }
            } catch (e) {
               
                dispatch(
                    GetExecutingUnitsScheduleFailure (null),
                );
                toast('Error: respuesta errada del servidor');
            }
    };

export const StartEditReportDateExecutingUnitsSchedule = (unidadEjecutoraCronogramaEditar: UnidadEjecutoraCronogramaEditarDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(EditReportDateExecutingUnitsSchedule());
            const response =
                await api.editExecutingUnitSchedule(unidadEjecutoraCronogramaEditar);
            if (!response?.isSuccess) {
                dispatch(
                    EditReportDateExecutingUnitsScheduleFailure(
                        response.data,
                    ),
                );
                await NotificationService.genericAlert(
                    'Error', 
                    'Error interno del servidor', 
                    'error',
                );
            } else {
                dispatch(
                    EditReportDateExecutingUnitsScheduleSuccess(
                        response.data,
                    ),
                );
                await NotificationService.genericAlert(
                    '¡Éxito!', 
                    'La información se actualizó exitosamente', 
                );
            }
        } catch (e) {
           
            dispatch(
                EditReportDateExecutingUnitsScheduleFailure (null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

    export const startDeleteExecutingUnitsSchedule = (unidadEjecutoraCronogramaEliminar: UnidadEjecutoraCronogramaDTO | undefined) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(DeleteExecutingUnitsSchedule());
            const response =
                await api.deleteExecutingUnitSchedule(unidadEjecutoraCronogramaEliminar);
            if (!response?.isSuccess) {
                dispatch(
                    DeleteExecutingUnitsScheduleFailure(response?.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    DeleteExecutingUnitsScheduleSuccess(response?.data),
                );
                if (response.data == null) {
                    toast('Unidad ejecutora eliminada correctamente.');
                }
            }
        } catch (e) {
           
            dispatch(
                DeleteExecutingUnitsScheduleFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    }

    export const startAddExecutingUnitSchedule = (agregarUnidadEjecutora: UnidadEjecutoraCronogramaDTO) =>
        async (dispatch: Dispatch<AppActions>) => {
            try {
                dispatch(AddExecutingUnitSchedule());
                const response =
                    await api.addExecutingUnitSchedule(agregarUnidadEjecutora);
               if (!response?.isSuccess) {
                    dispatch(
                        AddExecutingUnitScheduleFailure(
                            response.data,
                        ),
                    );
                    toast('Error: respuesta errada del servidor');
                } else {
                    dispatch(
                        AddExecutingUnitScheduleSuccess(
                            response.data,
                        ),
                    );
                    if (response.data == null) {
                        toast('No se encontaron unidades ejecutoras.');
                    }
                }
            } catch (e) {
               
                dispatch(
                    AddExecutingUnitScheduleFailure (null),
                );
                toast('Error: respuesta errada del servidor');
            }
    };


