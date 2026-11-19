import { ClientForm } from '../pages/curriculum/constants/form.mode.type';
import {
    ClientActionTypes,
    GET_BY_ID_CLIENT,
    ON_ERROR,
    UPDATE_CLIENT,
    GET_BY_ID_TYPE_ID_NUMBER,
    GET_BY_ID_TYPE_ID_NUMBER_SUCCESS,
    GET_BY_ID_TYPE_ID_NUMBER_FAILURE,
    UPDATE_CLIENT_SUCCESS,
    UPDATE_CLIENT_FAILURE,
    GET_MOVEMENT_ACCOUNT_CLIENT,
    GET_MOVEMENT_ACCOUNT_CLIENT_SUCCESS,
    GET_MOVEMENT_ACCOUNT_CLIENT_FAILURE,
    GET_ADMINISTRACION_CUENTAS_SUCCESS,
    GET_ADMINISTRACION_CUENTAS_FAILURE,
} from '../types/client.actions.types';
import { AdminCuentasAfiliado, ClienteDto, CuentasMovimientoAfiliado } from '../types/client.model.type';
import { TemporalClientDto } from '../types/temporalClient.model.type';

export interface ClientState {
    id: number;
    client: ClienteDto;
    temporalClient: TemporalClientDto | null;
    movementsAccountClient: CuentasMovimientoAfiliado[];
    error: any;
    loading: boolean;
    admincuentas: AdminCuentasAfiliado;
}

const ClienteStateInitial: ClientState = {
    id: 0,
    client: ClientForm,
    temporalClient: null,
    movementsAccountClient: [],
    error: null,
    loading: false,
    admincuentas:{} as AdminCuentasAfiliado,
};

export const ClientReducer = (
    state: ClientState = ClienteStateInitial,
    action: ClientActionTypes,
): ClientState => {
    switch (action.type) {

        case GET_ADMINISTRACION_CUENTAS_SUCCESS:
            return {
                ...state,
                admincuentas: action.AdminCuentas,
                loading: false,
            };

            case GET_ADMINISTRACION_CUENTAS_FAILURE:
                return {
                    ...state,
                    error: action.error,
                    loading: false,
                };



        case GET_BY_ID_CLIENT:
            return {
                ...state,
                client: action.client,
            };       
        case GET_BY_ID_TYPE_ID_NUMBER:
            return {
                ...state,
                client: ClientForm,
                loading: true,
            };
        case GET_BY_ID_TYPE_ID_NUMBER_SUCCESS:
            return {
                ...state,
                client: action.client,
                loading: false,
            };
        case GET_BY_ID_TYPE_ID_NUMBER_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_CLIENT:
            return {
                ...state,
                client: action.client,
                loading: true,
            };
        case UPDATE_CLIENT_SUCCESS:
            return {
                ...state,
                client: action.client,
                loading: false,
            };
        case UPDATE_CLIENT_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case GET_MOVEMENT_ACCOUNT_CLIENT:
            return {
                ...state,
                loading: true,
            };
        case GET_MOVEMENT_ACCOUNT_CLIENT_SUCCESS:
            return {
                ...state,
                movementsAccountClient: action.movementsAccountClient,
                loading: false,
            };
        case GET_MOVEMENT_ACCOUNT_CLIENT_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case ON_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        default:
            return state;
    }
};
