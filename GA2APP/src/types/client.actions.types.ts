import { AdminCuentasAfiliado, ClienteDto, CuentasMovimientoAfiliado, InclusionSaldoRequestDto, PermisoActualizacion } from './client.model.type';

// Especificacion de accion
export const GET_BY_ID_CLIENT = 'GET_BY_ID_CLIENT';

export const GET_BY_ID_TYPE_ID_NUMBER = 'GET_BY_ID_TYPE_ID_NUMBER';
export const GET_BY_ID_TYPE_ID_NUMBER_SUCCESS ='GET_BY_ID_TYPE_ID_NUMBER_SUCCESS';
export const GET_BY_ID_TYPE_ID_NUMBER_FAILURE ='GET_BY_ID_TYPE_ID_NUMBER_FAILURE';
export const UPDATE_CLIENT = 'UPDATE_CLIENT';
export const UPDATE_CLIENT_SUCCESS = 'UPDATE_CLIENT_SUCCESS';
export const UPDATE_CLIENT_FAILURE = 'UPDATE_CLIENT_FAILURE';
export const GET_MOVEMENT_ACCOUNT_CLIENT = 'GET_MOVEMENT_ACCOUNT_CLIENT';
export const GET_MOVEMENT_ACCOUNT_CLIENT_SUCCESS = 'GET_MOVEMENT_ACCOUNT_CLIENT_SUCCESS';
export const GET_MOVEMENT_ACCOUNT_CLIENT_FAILURE = 'GET_MOVEMENT_ACCOUNT_CLIENT_FAILURE';

export const GET_ADMINISTRACION_CUENTAS_SUCCESS='GET_ADMINISTRACION_CUENTAS_SUCCESS'
export const GET_ADMINISTRACION_CUENTAS_FAILURE='GET_ADMINISTRACION_CUENTAS_FAILURE'
export const GET_PERMISO_ACTUALIZACION='GET_PERMISO_ACTUALIZACION'

export const POST_INCLUSION_SALDOS='POST_INCLUSION_SALDOS'
export const POST_INCLUSION_SALDOS_SUCCESS='POST_INCLUSION_SALDOS_SUCCESS'
export const POST_INCLUSION_SALDOS_FAILURE='POST_INCLUSION_SALDOS_FAILURE'

export const ON_ERROR = 'ON_ERROR';

export interface GetAdministracionCuentasSuccess {
    type: 'GET_ADMINISTRACION_CUENTAS_SUCCESS';
    AdminCuentas: AdminCuentasAfiliado;
}

export interface GetAdministracionCuentasFailure {
    type: 'GET_ADMINISTRACION_CUENTAS_FAILURE';
    error: any;
}

export interface GetPermisoActualizacion {
    type: 'GET_PERMISO_ACTUALIZACION';
    permiso:PermisoActualizacion;
}


export interface GetClientById {
    type: 'GET_BY_ID_CLIENT';
    client: ClienteDto;
}

export interface UpdateClient {
    type: 'UPDATE_CLIENT';
    client: ClienteDto;
}

export interface UpdateClientSuccess {
    type: 'UPDATE_CLIENT_SUCCESS';
    client: ClienteDto;
}

export interface UpdateClientFailure {
    type: 'UPDATE_CLIENT_FAILURE';
    error: any;
}

export interface GetClientByIdTypeIdNumber {
    type: 'GET_BY_ID_TYPE_ID_NUMBER';
}

export interface GetClientByIdTypeIdNumberSuccess {
    type: 'GET_BY_ID_TYPE_ID_NUMBER_SUCCESS';
    client: ClienteDto;
}

export interface GetClientByIdTypeIdNumberFailure {
    type: 'GET_BY_ID_TYPE_ID_NUMBER_FAILURE';
    error: any;
}

export interface GetMovementAccountClient {
    type: 'GET_MOVEMENT_ACCOUNT_CLIENT';
}

export interface GetMovementAccountClientSuccess {
    type: 'GET_MOVEMENT_ACCOUNT_CLIENT_SUCCESS';
    movementsAccountClient: CuentasMovimientoAfiliado[];
}

export interface GetMovementAccountClientFailure {
    type: 'GET_MOVEMENT_ACCOUNT_CLIENT_FAILURE';
    error: any;
}

export interface PostInclusionSaldos {
    type: 'POST_INCLUSION_SALDOS';
}

export interface PostInclusionSaldosSuccess {
    type: 'POST_INCLUSION_SALDOS_SUCCESS';
    inclusionSaldos: InclusionSaldoRequestDto;
}

export interface PostInclusionSaldosFailure {
    type: 'POST_INCLUSION_SALDOS_FAILURE';
    inclusionSaldos: InclusionSaldoRequestDto | null;
}

export interface OnErrorClient {
    type: 'ON_ERROR';
    error: any;
}

export type ClientActionTypes =
    | GetClientById
    | GetClientByIdTypeIdNumber
    | GetClientByIdTypeIdNumberSuccess
    | GetClientByIdTypeIdNumberFailure
    | UpdateClient
    | UpdateClientSuccess
    | UpdateClientFailure
    | GetMovementAccountClient
    | GetMovementAccountClientSuccess
    | GetMovementAccountClientFailure
    | OnErrorClient
    | GetAdministracionCuentasSuccess
    | GetAdministracionCuentasFailure
    | PostInclusionSaldos
    | PostInclusionSaldosFailure
    | GetPermisoActualizacion
    | PostInclusionSaldosSuccess;
