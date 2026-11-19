import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import {
    GET_BY_ID_CLIENT,
    ON_ERROR,
    UPDATE_CLIENT,
    UPDATE_CLIENT_SUCCESS,
    UPDATE_CLIENT_FAILURE,
    GET_BY_ID_TYPE_ID_NUMBER,
    GET_BY_ID_TYPE_ID_NUMBER_SUCCESS,
    GET_BY_ID_TYPE_ID_NUMBER_FAILURE,
    GET_MOVEMENT_ACCOUNT_CLIENT,
    GET_MOVEMENT_ACCOUNT_CLIENT_SUCCESS,
    GET_MOVEMENT_ACCOUNT_CLIENT_FAILURE,
    GET_ADMINISTRACION_CUENTAS_SUCCESS,
    GET_ADMINISTRACION_CUENTAS_FAILURE,
    POST_INCLUSION_SALDOS,
    POST_INCLUSION_SALDOS_FAILURE,
    POST_INCLUSION_SALDOS_SUCCESS,
    GET_PERMISO_ACTUALIZACION
} from '../types/client.actions.types';
import { AdminCuentasAfiliado, ClienteDto, CuentasMovimientoAfiliado, InclusionSaldoRequestDto, PermisoActualizacion } from '../types/client.model.type';
import ApiClients from '../services/api-clients.service';
import NotificationService from '../services/notification.service';
import { AdministracionParametroModel } from '../models/requests/create-simulation-client';
import history from '../utils/history.router';
import { cleanBackendMessage } from '../utils/cleanBackendMessage';

const api = ApiClients();

export const GetClientById = (client: ClienteDto): AppActions => ({
    type: GET_BY_ID_CLIENT,
    client: client,
});

export const UpdateClient = (client: ClienteDto): AppActions => ({
    type: UPDATE_CLIENT,
    client: client,
});

export const UpdateClientSuccess = (client: ClienteDto): AppActions => ({
    type: UPDATE_CLIENT_SUCCESS,
    client: client,
});

export const UpdateClientFailure = (error: any): AppActions => ({
    type: UPDATE_CLIENT_FAILURE,
    error: error,
});

export const GetClientByIdentificationTypeIdentificationNumber =
    (): AppActions => ({
        type: GET_BY_ID_TYPE_ID_NUMBER,
    });

export const GetClientByIdentificationTypeIdentificationNumberSuccess = (
    client: ClienteDto,
): AppActions => ({
    type: GET_BY_ID_TYPE_ID_NUMBER_SUCCESS,
    client: client,
});

export const GetClientByIdentificationTypeIdentificationNumberFailure = (
    error: any,
): AppActions => ({
    type: GET_BY_ID_TYPE_ID_NUMBER_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const GetMovementAccountClient = (): AppActions => ({
    type: GET_MOVEMENT_ACCOUNT_CLIENT,
});

export const GetMovementAccountClientSuccess = (movementsAccountClient: CuentasMovimientoAfiliado[]): AppActions => ({
    type: GET_MOVEMENT_ACCOUNT_CLIENT_SUCCESS,
    movementsAccountClient: movementsAccountClient,
});

export const GetMovementAccountClientFailure = (error: any): AppActions => ({
    type: GET_MOVEMENT_ACCOUNT_CLIENT_FAILURE,
    error: error,
});

export const GetPermisoActualizacion = (permiso: PermisoActualizacion): AppActions => ({
    type: GET_PERMISO_ACTUALIZACION,
    permiso: permiso
});



export const GetAdministracionCuentasSuccess = (data: AdminCuentasAfiliado): AppActions => ({
    type: GET_ADMINISTRACION_CUENTAS_SUCCESS,
    AdminCuentas: data,
});

export const GetAdministracionCuentasFailure = (error: any): AppActions => ({
    type: GET_ADMINISTRACION_CUENTAS_FAILURE,
    error: error,
});



export const startGetClientByIdentificationTypeIdentificationNumber =
    (props: string, idType: number, idNumber: string) =>
        async (dispatch: Dispatch<AppActions>) => {
            try {
                dispatch(GetClientByIdentificationTypeIdentificationNumber());
                const response =
                    await api.getClientByIdentificationTypeIdentificationNumberRequest(
                        idType,
                        idNumber,
                    );
                if (!response?.isSuccess) {
                    dispatch(
                        GetClientByIdentificationTypeIdentificationNumberFailure(
                            response,
                        ),
                    );
                    toast('Error: respuesta errada del servidor');
                } else {
                    dispatch(
                        GetClientByIdentificationTypeIdentificationNumberSuccess(
                            response.data,
                        ),
                    );
                    if (response.data == null) {
                        toast('Afiliado no encontrado');
                    }
                }
            } catch (e) {

                dispatch(
                    GetClientByIdentificationTypeIdentificationNumberFailure(null),
                );
                toast('Error: respuesta errada del servidor');
            }
        };

export const startGetClientById =
    (id: number, props: string) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.GetClientByIdequest(id)
        if (response?.isSuccess === null) {
            dispatch(OnError(response));
        } else {
            dispatch(GetClientById(response.data));
        }
    };

// export const startUpdateClient =
//     (client: ClienteDto, props: string) =>
//         async (dispatch: Dispatch<AppActions>) => {

//             dispatch(UpdateClient(client));
//             const response = await api.UpdateClientRequest(client);
//             if (response?.isSuccess === null) {
//                 dispatch(UpdateClientFailure(client));
//                 toast('Error: respuesta errada del servidor');
//             } else {
//                 dispatch(UpdateClientSuccess(response.data));
//                 await NotificationService.genericAlert(
//                     'Guardado con éxito',
//                     'Se realizó guardado con éxito .',
//                 );
//             }
//         };

export const startUpdateClient =
	(client: ClienteDto, props: string) =>
		async (dispatch: Dispatch<AppActions>) => {
			dispatch(UpdateClient(client));

			const response = await api.UpdateClientRequest(client);

			if (!response || response.isSuccess !== true) {
				dispatch(UpdateClientFailure(client));

				const cleanMessage = cleanBackendMessage(
					response?.returnMessage ||
					'No se pudo actualizar la información del cliente.',
				);

				await NotificationService.genericAlwarning(
					'Advertencia',
					cleanMessage,
				);

				return response;
			}

			dispatch(UpdateClientSuccess(response.data));

			await NotificationService.genericAlert(
				'Guardado con éxito',
				'Se realizó guardado con éxito.',
			);

			return response;
		};

export const startValidatePermisoActualizacion =
    (perfil: any) =>
        async (dispatch: Dispatch<AppActions>) => {
            const response = await api.getPermisoActualizacion(perfil);
            if (response?.isSuccess) {
                if (response?.data === null) {
                    NotificationService.genericAlwarning(
                        'Advertencia',
                        'No cuenta con permisos para realizar esta acción.',
                    );
                } else {
                    dispatch(GetPermisoActualizacion(response?.data));
                }

            };
        }

export const startGetMovementsAccountsClient =
    (identification: string) =>
        async (dispatch: Dispatch<AppActions>) => {
            dispatch(GetMovementAccountClient());
            const response = await api.getMovementsAccountsClient(identification);
            if (response?.isSuccess == null) {
                dispatch(GetMovementAccountClientFailure(response?.returnMessage));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(GetMovementAccountClientSuccess(response.data));
            }
        };


export const startGetAdministracionCuentasAfiliado =
    (data: AdministracionParametroModel) =>
        async (dispatch: Dispatch<AppActions>) => {
            try {
                dispatch(GetMovementAccountClient());
                const response = await api.getAdministracionCuentasAfiliado(data);
                if (response?.isSuccess) {
                    dispatch(GetAdministracionCuentasSuccess(response.data));
                    history.push("/ControlAdministracionCuentas");
                } else {
                    toast("Error. El afiliado no existe. Por favor validar", { type: toast.TYPE.ERROR })
                    dispatch(GetAdministracionCuentasFailure(response?.returnMessage));
                }
            } catch (e) {

                dispatch(GetAdministracionCuentasFailure(null));
                toast('Error: respuesta errada del servidor');
            }
        };

export const PostInclusionSaldos = (): AppActions => ({
    type: POST_INCLUSION_SALDOS,
});

export const PostInclusionSaldosFailure = (inclusionSaldos: InclusionSaldoRequestDto | null): AppActions => ({
    type: POST_INCLUSION_SALDOS_FAILURE,
    inclusionSaldos: null
});

export const PostInclusionSaldosSuccess = (inclusionSaldos: InclusionSaldoRequestDto): AppActions => ({
    type: POST_INCLUSION_SALDOS_SUCCESS,
    inclusionSaldos: inclusionSaldos,
});

export const startPostInclusionSaldos = (inclusionSaldos: InclusionSaldoRequestDto) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(PostInclusionSaldos());
            const response =
                await api.postInclusionSaldos(inclusionSaldos)
            if (!response?.isSuccess) {
                dispatch(
                    PostInclusionSaldosFailure(
                        response,
                    ),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    PostInclusionSaldosSuccess(
                        response.data,
                    ),
                );
                toast('Inclusión de saldos agregada correctamente');
            }
        } catch (e) {

            dispatch(
                PostInclusionSaldosFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };
