import { Tarea } from './proceso.type';

export const CREATE_TAREA = 'CREATE_TAREA';
export const UPDATE_TAREA = 'UPDATE_TAREA';
export const DELETE_TAREA = 'CREATE_TAREA';
export const GET_BY_ID_TAREA = 'GET_BY_ID_TAREA';
export const GET_ALL_TAREA = 'GET_ALL_TAREA';

export interface CreateTareaActionType {
    type: typeof CREATE_TAREA;
    tarea: Tarea;
}

export interface UpdateTareaActionType {
    type: typeof UPDATE_TAREA;
    tarea: Tarea;
}

export interface DeleteTareaActionType {
    type: typeof DELETE_TAREA;
    tarea: Tarea;
}

export interface GetByIdTareaActionType {
    type: typeof GET_BY_ID_TAREA;
    tareaId: string;
}

export interface CrearTareaActionType {
    type: typeof GET_ALL_TAREA;
    tareas: Tarea[];
}
