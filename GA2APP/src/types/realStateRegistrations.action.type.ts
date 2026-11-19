import { AgregarMatriculaDTO, CausalesByidTypeDTO, HistorialNovedadesMatriculasInmobiliarias, HistorialPropietariosDTO, MatriculasInmobiliariasDTO, NovedadesBloqueosDeleteDTO, NovedadesInsertarDTO, OperacionesMatriculasDTO} from "./realStateRegistrations.type";


export const GET_REAL_STATE_REGISTRATIONS = 'GET_REAL_STATE_REGISTRATIONS';
export const GET_REAL_STATE_REGISTRATIONS_SUCCESS = 'GET_REAL_STATE_REGISTRATIONS_SUCCESS';
export const GET_REAL_STATE_REGISTRATIONS_FAILURE = 'GET_REAL_STATE_REGISTRATIONS_FAILURE';

export const GET_BASIC_INFORMATION = 'GET_BASIC_INFORMATION';
export const GET_BASIC_INFORMATION_SUCCESS = 'GET_BASIC_INFORMATION_SUCCESS';
export const GET_BASIC_INFORMATION_FAILURE = 'GET_BASIC_INFORMATION_FAILURE';
 
export const GET_PROPIETARY_HISTORY = 'GET_PROPIETARY_HISTORY';
export const GET_PROPIETARY_HISTORY_SUCCESS = 'GET_PROPIETARY_HISTORY_SUCCESS';
export const GET_PROPIETARY_HISTORY_FAILURE = 'GET_PROPIETARY_HISTORY_FAILURE';

export const ADD_PROPIETARY_HISTORY = 'ADD_PROPIETARY_HISTORY';
export const ADD_PROPIETARY_HISTORY_SUCCESS = 'ADD_PROPIETARY_HISTORY_SUCCESS';
export const ADD_PROPIETARY_HISTORY_FAILURE = 'ADD_PROPIETARY_HISTORY_FAILURE';

export const GET_BLOCK = 'GET_BLOCK';
export const GET_BLOCK_SUCCESS = 'GET_BLOCK_SUCCESS';
export const GET_BLOCK_FAILURE = 'GET_BLOCK_FAILURE';

export const GET_OPERATIONS_BLOCKS = 'GET_OPERATIONS_BLOCKS';
export const GET_OPERATIONS_BLOCKS_SUCCESS = 'GET_OPERATIONS_BLOCKS_SUCCESS';
export const GET_OPERATIONS_BLOCKS_FAILURE = 'GET_OPERATIONS_BLOCKS_FAILURE';
export const GET_OPERATIONS_BLOCKS_BY_ID = 'GET_OPERATIONS_BLOCKS_BY_ID';
export const GET_OPERATIONS_BLOCKS_BY_ID_SUCCESS = 'GET_OPERATIONS_BLOCKS_BY_ID_SUCCESS';
export const GET_OPERATIONS_BLOCKS_BY_ID_FAILURE = 'GET_OPERATIONS_BLOCKS_BY_ID_FAILURE';
export const DELETE_PROPIETARY_HISTORY = 'DELETE_PROPIETARY_HISTORY';
export const DELETE_PROPIETARY_HISTORY_SUCCESS = 'DELETE_PROPIETARY_HISTORY_SUCCESS';
export const DELETE_PROPIETARY_HISTORY_FAILURE = 'DELETE_PROPIETARY_HISTORY_FAILURE';

export const DELETE_NOVELTY_BLOCKS = 'DELETE_NOVELTY_BLOCKS';
export const DELETE_NOVELTY_BLOCKS_SUCCESS = 'DELETE_NOVELTY_BLOCKS_SUCCESS';
export const DELETE_NOVELTY_BLOCKS_FAILURE = 'DELETE_NOVELTY_BLOCKS_FAILURE';

export const ADD_NOVELTY_BLOCKS = 'ADD_NOVELTY_BLOCKS';
export const ADD_NOVELTY_BLOCKS_SUCCESS = 'ADD_NOVELTY_BLOCKS_SUCCESS';
export const ADD_NOVELTY_BLOCKS_FAILURE = 'ADD_NOVELTY_BLOCKS_FAILURE';

export const ADD_REAL_STATE_REGISTRATIONS = 'ADD_REAL_STATE_REGISTRATIONS';
export const ADD_REAL_STATE_REGISTRATIONS_SUCCESS = 'ADD_REAL_STATE_REGISTRATIONS_SUCCESS';
export const ADD_REAL_STATE_REGISTRATIONS_FAILURE = 'ADD_REAL_STATE_REGISTRATIONS_FAILURE';

export const DELETE_REAL_STATE_REGISTRATIONS = 'DELETE_REAL_STATE_REGISTRATIONS';
export const DELETE_REAL_STATE_REGISTRATIONS_SUCCESS = 'DELETE_REAL_STATE_REGISTRATIONS_SUCCESS';
export const DELETE_REAL_STATE_REGISTRATIONS_FAILURE = 'DELETE_REAL_STATE_REGISTRATIONS_FAILURE';

export const UPDATE_REAL_STATE_REGISTRATIONS = 'UPDATE_REAL_STATE_REGISTRATIONS';
export const UPDATE_REAL_STATE_REGISTRATIONS_SUCCESS = 'UPDATE_REAL_STATE_REGISTRATIONS_SUCCESS';
export const UPDATE_REAL_STATE_REGISTRATIONS_FAILURE = 'UPDATE_REAL_STATE_REGISTRATIONS_FAILURE';

export const UPDATE_PROPIETARY_HISTORY = 'UPDATE_PROPIETARY_HISTORY';
export const UPDATE_PROPIETARY_HISTORY_SUCCESS = 'UPDATE_PROPIETARY_HISTORY_SUCCESS';
export const UPDATE_PROPIETARY_HISTORY_FAILURE = 'UPDATE_PROPIETARY_HISTORY_FAILURE';

export const UPDATE_NOVELTY_BLOCKS = 'UPDATE_NOVELTY_BLOCKS';
export const UPDATE_NOVELTY_BLOCKS_SUCCESS = 'UPDATE_NOVELTY_BLOCKS_SUCCESS';
export const UPDATE_NOVELTY_BLOCKS_FAILURE = 'UPDATE_NOVELTY_BLOCKS_FAILURE';


export interface GetRealStateRegistrations{
    type: 'GET_REAL_STATE_REGISTRATIONS';
}

export interface GetRealStateRegistrationsSuccess {
    type: 'GET_REAL_STATE_REGISTRATIONS_SUCCESS';
    matriculasInmobiliarias: MatriculasInmobiliariasDTO[];
}

export interface GetRealStateRegistrationsFailure {
    type: 'GET_REAL_STATE_REGISTRATIONS_FAILURE';
    matriculasInmobiliarias: MatriculasInmobiliariasDTO[] | null;
}

export interface GetBasicInformation{
    type: 'GET_BASIC_INFORMATION';
}

export interface GetBasicInformationSuccess {
    type: 'GET_BASIC_INFORMATION_SUCCESS';
    informacionBasica: MatriculasInmobiliariasDTO;
}

export interface GetBasicInformationFailure {
    type: 'GET_BASIC_INFORMATION_FAILURE';
    informacionBasica: MatriculasInmobiliariasDTO | null;
}

export interface GetPropietaryHistory{
    type: 'GET_PROPIETARY_HISTORY'
}

export interface GetPropietaryHistorySuccess {
    type: 'GET_PROPIETARY_HISTORY_SUCCESS';
    historialPropietarios: HistorialPropietariosDTO[];
}

export interface GetPropietaryHistoryFailure {
    type: 'GET_PROPIETARY_HISTORY_FAILURE'
    historialPropietarios: HistorialPropietariosDTO  [] | null;
}

export interface AddPropietaryHistory{
    type: 'ADD_PROPIETARY_HISTORY'
}

export interface AddPropietaryHistorySuccess {
    type: 'ADD_PROPIETARY_HISTORY_SUCCESS';
    historialPropietarios: HistorialPropietariosDTO;
}

export interface AddPropietaryHistoryFailure {
    type: 'ADD_PROPIETARY_HISTORY_FAILURE'
    historialPropietarios: HistorialPropietariosDTO | null;
}

export interface DeletePropietaryHistory{
    type: 'DELETE_PROPIETARY_HISTORY';
}

export interface DeletePropietaryHistorySuccess {
    type: 'DELETE_PROPIETARY_HISTORY_SUCCESS';
    historialPropietariosEliminar: HistorialPropietariosDTO;
}

export interface DeletePropietaryHistoryFailure {
    type: 'DELETE_PROPIETARY_HISTORY_FAILURE';
    historialPropietariosEliminar: HistorialPropietariosDTO | null;
}

export interface GetBlock{
    type: 'GET_BLOCK'
}

export interface GetBlockSuccess {
    type: 'GET_BLOCK_SUCCESS';
    bloqueos: NovedadesInsertarDTO[];
}

export interface GetBlockFailure {
    type: 'GET_BLOCK_FAILURE'
    bloqueos: NovedadesInsertarDTO  [] | null;
}

export interface GetOperationsBlocks{
    type: 'GET_OPERATIONS_BLOCKS';
}

export interface GetOperationsBlocksSuccess {
    type: 'GET_OPERATIONS_BLOCKS_SUCCESS';
    operacionesBloqueos: OperacionesMatriculasDTO [];
}

export interface GetOperationsBlocksFailure {
    type: 'GET_OPERATIONS_BLOCKS_FAILURE';
    operacionesBloqueos: OperacionesMatriculasDTO [] | null;
}

export interface DeleteNoveltyBlocks{
    type: 'DELETE_NOVELTY_BLOCKS';
}

export interface DeleteNoveltyBlocksSuccess {
    type: 'DELETE_NOVELTY_BLOCKS_SUCCESS';
    novedadesBloqueosEliminar: NovedadesBloqueosDeleteDTO[];
}

export interface DeleteNoveltyBlocksFailure {
    type: 'DELETE_NOVELTY_BLOCKS_FAILURE';
    novedadesBloqueosEliminar: NovedadesBloqueosDeleteDTO[] | null;
}

export interface AddNoveltyBlocks{
    type: 'ADD_NOVELTY_BLOCKS';
}

export interface AddNoveltyBlocksSuccess {
    type: 'ADD_NOVELTY_BLOCKS_SUCCESS';
    agregarNovedadesBloqueos: HistorialNovedadesMatriculasInmobiliarias[];
}

export interface AddNoveltyBlocksFailure {
    type: 'ADD_NOVELTY_BLOCKS_FAILURE';
    agregarNovedadesBloqueos: HistorialNovedadesMatriculasInmobiliarias[] | null;
}

export interface AddRealStateRegistrations{
    type: 'ADD_REAL_STATE_REGISTRATIONS';
}

export interface AddRealStateRegistrationsSuccess {
    type: 'ADD_REAL_STATE_REGISTRATIONS_SUCCESS';
    agregarMatricula: AgregarMatriculaDTO;
}

export interface AddRealStateRegistrationsFailure {
    type: 'ADD_REAL_STATE_REGISTRATIONS_FAILURE';
    agregarMatricula: AgregarMatriculaDTO | null;
}

export interface DeleteRealStateRegistrations{
    type: 'DELETE_REAL_STATE_REGISTRATIONS';
}

export interface DeleteRealStateRegistrationsSuccess {
    type: 'DELETE_REAL_STATE_REGISTRATIONS_SUCCESS';
    eliminarMatricula: MatriculasInmobiliariasDTO;
}

export interface DeleteRealStateRegistrationsFailure {
    type: 'DELETE_REAL_STATE_REGISTRATIONS_FAILURE';
    eliminarMatricula: MatriculasInmobiliariasDTO | null;
}

export interface UpdateRealStateRegistrations{
    type: 'UPDATE_REAL_STATE_REGISTRATIONS';
}

export interface UpdateRealStateRegistrationsSuccess {
    type: 'UPDATE_REAL_STATE_REGISTRATIONS_SUCCESS';
    actualizarMatricula: MatriculasInmobiliariasDTO;
}

export interface UpdateRealStateRegistrationsFailure {
    type: 'UPDATE_REAL_STATE_REGISTRATIONS_FAILURE';
    actualizarMatricula: MatriculasInmobiliariasDTO | null;
}

export interface UpdatePropietaryHistory{
    type: 'UPDATE_PROPIETARY_HISTORY';
}

export interface UpdatePropietaryHistorySuccess {
    type: 'UPDATE_PROPIETARY_HISTORY_SUCCESS';
    actualizarHistorialPropietarios: HistorialPropietariosDTO;
}

export interface UpdatePropietaryHistoryFailure {
    type: 'UPDATE_PROPIETARY_HISTORY_FAILURE'; 
    actualizarHistorialPropietarios: HistorialPropietariosDTO | null;
}

export interface UpdateNoveltyBlocks{
    type: 'UPDATE_NOVELTY_BLOCKS';
}

export interface UpdateNoveltyBlocksSuccess {
    type: 'UPDATE_NOVELTY_BLOCKS_SUCCESS';
    actualizarNovevadesBloqueos: NovedadesInsertarDTO;
}

export interface UpdateNoveltyBlocksFailure {
    type: 'UPDATE_NOVELTY_BLOCKS_FAILURE';
    actualizarNovevadesBloqueos: NovedadesInsertarDTO | null;
}

export interface GetOperationsBlockById{
    type: 'GET_OPERATIONS_BLOCKS_BY_ID';
}

export interface GetOperationsBlockByIdSuccess {
    type: 'GET_OPERATIONS_BLOCKS_BY_ID_SUCCESS';
    causalesPortipo: CausalesByidTypeDTO[];
}

export interface GetOperationsBlockByIdFailure {
    type: 'GET_OPERATIONS_BLOCKS_BY_ID_FAILURE';
    causalesPortipo: CausalesByidTypeDTO[] | null;
}

export type MatriculasInmobiliariasActionTypes =
    | GetRealStateRegistrations
    | GetRealStateRegistrationsFailure
    | GetRealStateRegistrationsSuccess
    | GetBasicInformation
    | GetBasicInformationSuccess
    | GetBasicInformationFailure
    | GetPropietaryHistory
    | GetPropietaryHistorySuccess
    | GetPropietaryHistoryFailure
    | AddPropietaryHistory
    | AddPropietaryHistorySuccess
    | AddPropietaryHistoryFailure
    | GetBlock
    | GetBlockSuccess
    | GetBlockFailure
    | GetOperationsBlocks
    | GetOperationsBlocksSuccess
    | GetOperationsBlocksFailure
    | DeletePropietaryHistory
    | DeletePropietaryHistorySuccess
    | DeletePropietaryHistoryFailure
    | DeleteNoveltyBlocks
    | DeleteNoveltyBlocksSuccess
    | DeleteNoveltyBlocksFailure
    | AddNoveltyBlocks
    | AddNoveltyBlocksSuccess
    | AddNoveltyBlocksFailure
    | AddRealStateRegistrations
    | AddRealStateRegistrationsSuccess
    | AddRealStateRegistrationsFailure
    | DeleteRealStateRegistrations
    | DeleteRealStateRegistrationsSuccess
    | DeleteRealStateRegistrationsFailure
    | UpdateRealStateRegistrations
    | UpdateRealStateRegistrationsSuccess
    | UpdateRealStateRegistrationsFailure
    | UpdatePropietaryHistory
    | UpdatePropietaryHistorySuccess
    | UpdatePropietaryHistoryFailure
    | UpdateNoveltyBlocks
    | UpdateNoveltyBlocksSuccess
    | UpdateNoveltyBlocksFailure
    | GetOperationsBlockById
    | GetOperationsBlockByIdSuccess
    | GetOperationsBlockByIdFailure

