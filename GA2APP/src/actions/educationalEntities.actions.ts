import { toast } from 'react-toastify';
import { Dispatch } from 'redux';
import ApiEntidadesEducativas from '../services/api-entidadesEducativas.service';
import NotificationService from '../services/notification.service';
import { AppActions } from '../types/appactions.type';
import {
	GET_ACCOUNTS_EDUCATIONAL_ENTITIES,
	GET_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	GET_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
	ADD_ACCOUNTS_EDUCATIONAL_ENTITIES,
	ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
	UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES,
	UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
	DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES,
	DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
	GET_BLOCKS,
	GET_BLOCKS_FAILURE,
	GET_BLOCKS_SUCCESS,
	GET_EDUCATIONAL_ENTITIES_CAMPUS,
	GET_EDUCATIONAL_ENTITIES_CAMPUS_FAILURE,
	GET_EDUCATIONAL_ENTITIES_CAMPUS_SUCCESS,
	GET_EDUCATIONAL_ENTITIES_FILTER,
	GET_EDUCATIONAL_ENTITIES_FILTER_FAILURE,
	GET_EDUCATIONAL_ENTITIES_FILTER_SUCCESS,
	GET_EDUCATIONAL_PROGRAMS,
	GET_EDUCATIONAL_PROGRAMS_FAILURE,
	GET_EDUCATIONAL_PROGRAMS_SUCCESS,
	LOCK_ENTIDADES,
	LOCK_ENTIDADES_FAILURE,
	LOCK_ENTIDADES_SUCCESS,
	UPDATE_EDUCATIONAL_ENTITIES,
	UPDATE_EDUCATIONAL_ENTITIES_FAILURE,
	UPDATE_EDUCATIONAL_ENTITIES_SUCCESS,
	ADD_EDUCATIONAL_ENTITIES,
	ADD_EDUCATIONAL_ENTITIES_SUCCESS,
	ADD_EDUCATIONAL_ENTITIES_FAILURE,
	SHOW_ROW_DETALLE_SUCCESS,
	ADD_PROGRAMS,
	ADD_PROGRAMS_SUCCESS,
	ADD_PROGRAMS_FAILURE,
	DELETE_PROGRAMA_SUCCESS,
	ADD_CAMPUS,
	ADD_CAMPUS_SUCCESS,
	ADD_CAMPUS_FAILURE,
	UPDATE_CAMPUS,
	UPDATE_CAMPUS_SUCCESS,
	UPDATE_CAMPUS_FAILURE,
	DELETE_CAMPUS,
	DELETE_CAMPUS_SUCCESS,
	DELETE_CAMPUS_FAILURE,
	GET_EDUCATIONAL_ENTITIES_DETAIL,
	GET_EDUCATIONAL_ENTITIES_DETAIL_SUCCESS,
	GET_EDUCATIONAL_ENTITIES_DETAIL_FAILURE,
	UPDATE_PROGRAMA_SUCCESS,
	GET_FILES_EDUCATIONAL_ENTITIES,
	GET_FILES_EDUCATIONAL_ENTITIES_SUCCESS,
	GET_FILES_EDUCATIONAL_ENTITIES_FAILURE,
	DELETE_EDUCATIONAL_ENTITIES,
	DELETE_EDUCATIONAL_ENTITIES_SUCCESS,
	DELETE_EDUCATIONAL_ENTITIES_FAILURE,
	GET_REPORT_EDUCATIONAL_ENTITIES_FAILURE,
	GET_REPORT_EDUCATIONAL_ENTITIES,
	GET_TRAMITE_ENTITIES_FAILURE,
	GET_TRAMITE_ENTITIES_SUCCESS,
	GET_TRAMITE_ENTITIES,
	GET_HISTORY,
	GET_HISTORY_SUCCESS,
	GET_HISTORY_FAILURE,
	FIDECOMISO_SUCCESS,
	UPDATE_FIDECOMISO_SUCCESS,
	FIDEICOMISO_FAILURE
} from '../types/educational.entities.actions.type';

import {
	ArchivosEntidadesEducativas,
	BloqueosEntidadesEducativas,
	CuentaEntidadesEducativas,
	EntidadesEducativasAgregar,
	EntidadesEducativasFiltro,
	EntidadesEducativasFiltroRequest,
	Fidecomiso,
	HistorialCambios,
	HistorialEntity,
	ProgramaEducativo,
	ProgramasEducativos,
	SedeEntidadEducativa,
	TramiteEntity,
} from '../types/educational.entities.type';
import history from '../utils/history.router';

const api = ApiEntidadesEducativas();

export const UpdateEducationalEntities = (): AppActions => ({
	type: UPDATE_EDUCATIONAL_ENTITIES,
});

export const ShowDetalleEntidadEducativaAction = (
	dataRowDetalleEntidad: EntidadesEducativasFiltro,
): AppActions => ({
	type: SHOW_ROW_DETALLE_SUCCESS,
	dataRowDetalleEntidad: dataRowDetalleEntidad,
});

export const UpdateEducationalEntitiesSuccess = (): AppActions => ({
	type: UPDATE_EDUCATIONAL_ENTITIES_SUCCESS,
});

export const UpdateEducationalEntitiesFailure = (): AppActions => ({
	type: UPDATE_EDUCATIONAL_ENTITIES_FAILURE,
});

export const startUpdateEducationalEntities =
	(entidadEducativa: EntidadesEducativasFiltro) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				dispatch(UpdateEducationalEntities());
				const response = await api.updateEntidadEducativa(entidadEducativa);
				if (!response?.isSuccess) {
					dispatch(UpdateEducationalEntitiesFailure());
					toast('Error: respuesta errada del servidor');
				} else {
					dispatch(UpdateEducationalEntitiesSuccess());
					try {
						dispatch(GetHistory());
						const response = await api.GetChangeHistory(entidadEducativa?.idEntidad);
						if (!response?.isSuccess) {
							dispatch(GetHistoryFailure());
							toast('Error: respuesta errada del servidor');
						} else {
							dispatch(GetHistorySuccess(response?.data));
							toast('Entidad actualizada correctamente.');
						}
					} catch (e) {
						dispatch(GetHistoryFailure());
						toast('Error: respuesta errada del servidor');
					}
					startGetArchivosPorEntidad(entidadEducativa.idEntidad);
				}
			} catch (e) {

				dispatch(GetEducationalEntitiesFilterFailure(null));
				toast('Error: respuesta errada del servidor');
			}
		};

export const GetEducationalEntitiesFilter = (): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_FILTER,
});

export const GetEducationalEntitiesFilterSuccess = (
	entidadesEducativas: EntidadesEducativasFiltro[],
): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_FILTER_SUCCESS,
	EntidadesEducativas: entidadesEducativas,
});

export const GetEducationalEntitiesFilterFailure = (
	entidadesEducativas: EntidadesEducativasFiltro[] | null,
): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_FILTER_FAILURE,
	EntidadesEducativas: entidadesEducativas,
});

export const startGetEducationalEntitiesFilter =
	(entidadEducativa: EntidadesEducativasFiltroRequest) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				dispatch(GetEducationalEntitiesFilter());
				const response = await api.getEntidadesEducativasFiltroRequest(
					entidadEducativa,
				);
				if (!response?.isSuccess) {
					dispatch(GetEducationalEntitiesFilterFailure(response));
					toast('Error: respuesta errada del servidor');
				} else {
					dispatch(GetEducationalEntitiesFilterSuccess(response.data));
					if (response.data == null) {
						toast('Afiliado no encontrado.');
					}
				}
			} catch (e) {

				dispatch(GetEducationalEntitiesFilterFailure(null));
				toast('Error: respuesta errada del servidor');
			}
		};

export const GetSedesEntidadEducativa = (): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_CAMPUS,
});

export const GetSedesEntidadEducativaSuccess = (
	sedesEntidadesEducativas: SedeEntidadEducativa[],
): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_CAMPUS_SUCCESS,
	sedesEntidadEducativa: sedesEntidadesEducativas,
});

export const GetSedesEntidadEducativaFailure = (
	sedesEntidadesEducativas: SedeEntidadEducativa[] | null,
): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_CAMPUS_FAILURE,
	SedesEntidadEducativa: sedesEntidadesEducativas,
});

export const startGetSedesEntidadEducativa =
	(idEntidadEducativa: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetSedesEntidadEducativa());
			const response = await api.GetSedesEntidadEducativa(
				idEntidadEducativa,
			);
			if (!response?.isSuccess) {
				dispatch(GetSedesEntidadEducativaFailure(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetSedesEntidadEducativaSuccess(response.data));
			}
		} catch (e) {

			dispatch(GetEducationalEntitiesFilterFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetProgramasEducativos = (): AppActions => ({
	type: GET_EDUCATIONAL_PROGRAMS,
});

export const GetProgramasEducativosSuccess = (
	programasEducativos: ProgramasEducativos[],
): AppActions => ({
	type: GET_EDUCATIONAL_PROGRAMS_SUCCESS,
	programasEducativos: programasEducativos,
});

export const GetProgramasEducativosFailure = (
	programasEducativos: ProgramasEducativos[] | null,
): AppActions => ({
	type: GET_EDUCATIONAL_PROGRAMS_FAILURE,
	programasEducativos: programasEducativos,
});

export const startGetProgramasEducativos =
	(idEntidadEducativa: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetProgramasEducativos());
			const response = await api.GetProgramasEntidadEducativa(
				idEntidadEducativa,
			);
			if (!response?.isSuccess) {
				dispatch(GetProgramasEducativosFailure(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetProgramasEducativosSuccess(response.data));
			}
		} catch (e) {

			dispatch(GetProgramasEducativosFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};

export const LockEntidades = (): AppActions => ({
	type: LOCK_ENTIDADES,
});

export const LockEntidadesSuccess = (
	bloqueo: BloqueosEntidadesEducativas,
): AppActions => ({
	type: LOCK_ENTIDADES_SUCCESS,
	bloqueo: bloqueo,
});

export const LockEntidadesFailure = (
	bloqueo: BloqueosEntidadesEducativas,
): AppActions => ({
	type: LOCK_ENTIDADES_FAILURE,
	bloqueo: bloqueo,
});

export const startLockOrUnlockEntidades =
	(bloqueoEntidadEducativa: BloqueosEntidadesEducativas, lock: boolean) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				dispatch(LockEntidades());

				let response;
				if (lock) {
					response = await api.lockEntidades(bloqueoEntidadEducativa);
				} else {
					response = await api.unlockEntidades(bloqueoEntidadEducativa);
				}
				if (!response?.isSuccess) {
					dispatch(LockEntidadesFailure(response));
					toast('Error: respuesta errada del servidor');
				} else {
					dispatch(LockEntidadesSuccess(response.data));
					try {
						dispatch(GetBloqueos());
						const response = await api.GetBloqueos(bloqueoEntidadEducativa?.idEntidad);
						if (!response?.isSuccess) {
							dispatch(GetBloqueosFailure(response));
							toast('Error: respuesta errada del servidor');
						} else {
							dispatch(GetBloqueosSuccess(response.data));
							await NotificationService.genericAlert(
								lock ? 'Bloqueo' : 'Desbloqueo',
								'La operación se realizó con éxito.',
							);
							

							try {
								dispatch(GetHistory());
								const response = await api.GetChangeHistory(bloqueoEntidadEducativa?.idEntidad);
								if (!response?.isSuccess) {
									dispatch(GetHistoryFailure());
									toast('Error: respuesta errada del servidor');
								} else {
									dispatch(GetHistorySuccess(response?.data));
								}
							} catch (e) {
								dispatch(GetHistoryFailure());
								toast('Error: respuesta errada del servidor');
							}

						}
					} catch (e) {
						dispatch(GetBloqueosFailure(null));
						toast('Error: respuesta errada del servidor');
					}

				}
			} catch (e) {

				dispatch(GetBloqueosFailure(null));
				toast('Error: respuesta errada del servidor');
			}
		};

export const GetBloqueos = (): AppActions => ({
	type: GET_BLOCKS,
});

export const GetBloqueosSuccess = (
	bloqueos: BloqueosEntidadesEducativas[],
): AppActions => ({
	type: GET_BLOCKS_SUCCESS,
	bloqueos: bloqueos,
});

export const GetBloqueosFailure = (
	bloqueos: BloqueosEntidadesEducativas[] | null,
): AppActions => ({
	type: GET_BLOCKS_FAILURE,
	bloqueos: bloqueos,
});

export const startGetBloqueos =
	(idEntidadEducativa: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetBloqueos());
			const response = await api.GetBloqueos(idEntidadEducativa);
			if (!response?.isSuccess) {
				dispatch(GetBloqueosFailure(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetBloqueosSuccess(response.data));
			}
		} catch (e) {

			dispatch(GetBloqueosFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetCuentas = (): AppActions => ({
	type: GET_ACCOUNTS_EDUCATIONAL_ENTITIES,
});

export const GetCuentasSuccess = (
	cuentas: CuentaEntidadesEducativas[],
): AppActions => ({
	type: GET_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	cuentas: cuentas,
});

export const GetCuentasFailure = (
	cuentas: CuentaEntidadesEducativas[] | null,
): AppActions => ({
	type: GET_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
	cuentas: cuentas,
});


export const startGetCuentas =
	(idEntidadEducativa: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetCuentas());
			const response = await api.GetCuentas(idEntidadEducativa);
			if (!response?.isSuccess) {
				dispatch(GetCuentasFailure(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetCuentasSuccess(response.data));
			}
		} catch (e) {

			dispatch(GetCuentasFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};

export const AddCuenta = (): AppActions => ({
	type: ADD_ACCOUNTS_EDUCATIONAL_ENTITIES,
});

export const AddCuentaSuccess = (
	cuentas: CuentaEntidadesEducativas,
): AppActions => ({
	type: ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	cuenta: cuentas,
});

export const AddCuentaFailure = (): AppActions => ({
	type: ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
});

export const startAddCuenta =
	(cuentaEntidad: CuentaEntidadesEducativas) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(AddCuenta());
			const response = await api.AddCuenta(cuentaEntidad);
			if (!response?.isSuccess) {
				dispatch(AddCuentaFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(AddCuentaSuccess(response.data));
				await NotificationService.genericAlert(
					'Cuenta creado',
					'La operación se realizó con éxito.',
				);
			}
		} catch (e) {
			dispatch(AddCuentaFailure());
			toast('Error: respuesta errada del servidor');
		}
	};

export const UpdateCuenta = (): AppActions => ({
	type: UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES,
});

export const UpdateCuentaSuccess = (
	cuentas: CuentaEntidadesEducativas,
): AppActions => ({
	type: UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	cuenta: cuentas,
});

export const UpdateCuentaFailure = (): AppActions => ({
	type: UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
});

export const startUpdateCuenta =
	(cuentaEntidad: CuentaEntidadesEducativas) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(UpdateCuenta());
			const response = await api.UpdateCuenta(cuentaEntidad);
			if (!response?.isSuccess) {
				dispatch(UpdateCuentaFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(UpdateCuentaSuccess(response.data));
				await NotificationService.genericAlert(
					'Cuenta actualizada',
					'La operación se realizó con éxito.',
				);
			}
		} catch (e) {
			dispatch(UpdateCuentaFailure());
			toast('Error: respuesta errada del servidor');
		}
	};

export const DeleteCuenta = (): AppActions => ({
	type: DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES,
});

export const DeleteCuentaSuccess = (
	idCuentaBancaria: string,
	cuenta: CuentaEntidadesEducativas,
): AppActions => ({
	type: DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS,
	idCuenta: idCuentaBancaria,
	cuenta: cuenta
});

export const DeleteCuentaFailure = (): AppActions => ({
	type: DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE,
});

export const startDeleteCuenta =
	(idCuentaBancaria: string, idEntidad: string, idUsuario: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(DeleteCuenta());
			const request = {
				idCuentaBancaria: idCuentaBancaria,
				modificadoPor: idUsuario
			}
			const response = await api.DeleteFidecomiso(request);
			if (!response?.isSuccess) {
				dispatch(DeleteCuentaFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(DeleteCuentaSuccess(idCuentaBancaria, response.data));
				try {
					dispatch(GetCuentas());
					const response = await api.GetCuentas(idEntidad);
					if (!response?.isSuccess) {
						dispatch(GetCuentasFailure(response));
						toast('Error: respuesta errada del servidor');
					} else {
						dispatch(GetCuentasSuccess(response.data));
						try {
							dispatch(GetHistory());
							const response = await api.GetChangeHistory(idEntidad);
							if (!response?.isSuccess) {
								dispatch(GetHistoryFailure());
								toast('Error: respuesta errada del servidor');
							} else {
								dispatch(GetHistorySuccess(response?.data));
								await NotificationService.genericAlert(
									'Cuenta eliminada',
									'La operación se realizó con éxito.',
								);
							}
						} catch (e) {
							dispatch(GetHistoryFailure());
							toast('Error: respuesta errada del servidor');
						}
					}
				} catch (e) {

					dispatch(GetCuentasFailure(null));
					toast('Error: respuesta errada del servidor');
				}

			}
		} catch (e) {
			dispatch(DeleteCuentaFailure());
			toast('Error: respuesta errada del servidor');
		}
	};

export const AddEducationalEntities = (): AppActions => ({
	type: ADD_EDUCATIONAL_ENTITIES,
});

export const AddEducationalEntitiesSuccess = (
	agregarEntidadEducativa: EntidadesEducativasAgregar,
): AppActions => ({
	type: ADD_EDUCATIONAL_ENTITIES_SUCCESS,
	agregarEntidadEducativa: agregarEntidadEducativa,
});

export const AddEducationalEntitiesFailure = (
	agregarEntidadEducativa: EntidadesEducativasAgregar | null,
): AppActions => ({
	type: ADD_EDUCATIONAL_ENTITIES_FAILURE,
	agregarEntidadEducativa: agregarEntidadEducativa,
});


export const startAddEducationalEntities =
	(agregarEntidadEducativa: EntidadesEducativasAgregar) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				dispatch(AddEducationalEntities());
				const response = await api.AddEducationalEntities(
					agregarEntidadEducativa,
				);

				if (!response?.isSuccess) {
					dispatch(AddEducationalEntitiesFailure(response));
					toast('Error: respuesta errada del servidor');
				}
				if (response.returnMessage !== null) {
					dispatch(
						AddEducationalEntitiesFailure(
							response.data,
						),
					);

					let mensaje = '';
					if (response.returnMessage && response.returnMessage.includes('Ya se encuentra registrada una entidad con ese nit')) {
						mensaje = 'La entidad ya se encuentra registrada en el sistema';
					} else {
						mensaje = response.returnMessage;
					}
					await NotificationService.genericAlertError(
						'Error',
						mensaje,
					);
					history.push('/EntidadesEducativas')
				} else {
					dispatch(AddEducationalEntitiesSuccess(response.data));
					await NotificationService.genericAlert(
						'Actualizado con éxito',
						'Se realizó con éxito la actualización.',
					);
					history.push('/EntidadesEducativas');
				}
			} catch (e) {

				dispatch(AddEducationalEntitiesFailure(null));
				toast('Error: respuesta errada del servidor');
			}
		};

export const startAddEducationalEntitiesFromRepaes =
	(agregarEntidadEducativa: EntidadesEducativasAgregar) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				dispatch(AddEducationalEntities());

				const response = await api.AddEducationalEntities(
					agregarEntidadEducativa,
				);

				const tieneMensajeError =
					typeof response?.returnMessage === 'string' &&
					response.returnMessage.trim().length > 0;

				const fueExitosa =
					response?.isSuccess === true &&
					!tieneMensajeError;

				if (!fueExitosa) {
					dispatch(
						AddEducationalEntitiesFailure(
							response?.data ?? null,
						),
					);

					return response;
				}

				dispatch(
					AddEducationalEntitiesSuccess(response.data),
				);

				return response;
			} catch (error) {
				console.error(
					'Error al crear entidad educativa desde REPAES:',
					error,
				);

				dispatch(AddEducationalEntitiesFailure(null));

				return null;
			}
		};


export const DeleteEntidadEducativa = (): AppActions => ({
	type: DELETE_EDUCATIONAL_ENTITIES,
});
export const GetReporteEducativa = (): AppActions => ({
	type: GET_REPORT_EDUCATIONAL_ENTITIES,
});
export const GetReporteEducativaFailure = (): AppActions => ({
	type: GET_REPORT_EDUCATIONAL_ENTITIES_FAILURE,
});
export const GetReporteEducativaSuccess = (): AppActions => ({
	type: GET_REPORT_EDUCATIONAL_ENTITIES_FAILURE,
});
export const DeleteEntidadEducativaSuccess = (
	idEntidad: string,
	entidadEducativa: EntidadesEducativasFiltro,
): AppActions => ({
	type: DELETE_EDUCATIONAL_ENTITIES_SUCCESS,
	idEntidad: idEntidad,
	entidadEducativa: entidadEducativa
});

export const DeleteEntidadEducativaFailure = (): AppActions => ({
	type: DELETE_EDUCATIONAL_ENTITIES_FAILURE,
});

export const startDeleteEntidadEducativa =
	(idEntidad: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(DeleteEntidadEducativa());
			const response = await api.DeleteEntidadEducativa(idEntidad);
			if (!response?.isSuccess) {
				dispatch(DeleteEntidadEducativaFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(DeleteEntidadEducativaSuccess(idEntidad, response.data));
				await NotificationService.genericAlert(
					'Entidad Educativa eliminada',
					'La operación se realizó con éxito.',
				);
			}
		} catch (e) {
			dispatch(DeleteEntidadEducativaFailure());
			toast('Error: respuesta errada del servidor');
		}
	};

export const startReportEntidadEducativa =
	(entidad: EntidadesEducativasFiltro) => async (dispatch: Dispatch<AppActions>) => {

		try {
			dispatch(GetReporteEducativa());
			const response = await api.ReportEntidadEducativa(entidad);
			if (!response?.isSuccess) {
				dispatch(DeleteEntidadEducativaFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetReporteEducativaSuccess());
				await NotificationService.genericAlert(
					'Se ha descargado el archivo',
					'La operación se realizó con éxito.',
				);
			}
		} catch (e) {
			dispatch(GetReporteEducativaFailure());
			toast('Error: respuesta errada del servidor');
		}
	};
export const AddPrograms = (): AppActions => ({
	type: ADD_PROGRAMS,
});

export const AddProgramsSuccess = (
	programa: ProgramasEducativos,
): AppActions => ({
	type: ADD_PROGRAMS_SUCCESS,
	programs: programa,
});

export const AddProgramsFailure = (
	programa: ProgramasEducativos | null,
): AppActions => ({
	type: ADD_PROGRAMS_FAILURE,
	programs: programa,
});

export const startAddPrograms =
	(programa: ProgramasEducativos) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				dispatch(AddPrograms());
				const response = await api.AddProgram(programa);
				if (!response?.isSuccess) {
					dispatch(AddProgramsFailure(response));
					toast('Error: respuesta errada del servidor');
				} else {
					dispatch(AddProgramsSuccess(response.data));
					await NotificationService.genericAlert(
						'Agregado con éxito',
						'Se realizó con éxito la adición del programa.',
					);
				}
			} catch (e) {

				dispatch(AddEducationalEntitiesFailure(null));
				toast('Error: respuesta errada del servidor');
			}
		};

export const UpdateProgramsSuccess = (
	programa: ProgramaEducativo,
): AppActions => ({
	type: UPDATE_PROGRAMA_SUCCESS,
	programaEducativo: programa,
});

export const startUpdatePrograms =
	(programa: ProgramaEducativo) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.UpdateProgram(programa);
				if (response?.isSuccess) {
					dispatch(UpdateProgramsSuccess(response.data));

					try {
						dispatch(GetProgramasEducativos());
						const response = await api.GetProgramasEntidadEducativa(
							programa?.idEntidad,
						);
						if (!response?.isSuccess) {
							dispatch(GetProgramasEducativosFailure(response));
							toast('Error: respuesta errada del servidor');
						} else {
							dispatch(GetProgramasEducativosSuccess(response.data));

							try {
								dispatch(GetHistory());
								const response = await api.GetChangeHistory(programa?.idEntidad);
								if (!response?.isSuccess) {
									dispatch(GetHistoryFailure());
									toast('Error: respuesta errada del servidor');
								} else {
									dispatch(GetHistorySuccess(response?.data));
									await NotificationService.genericAlert(
										'Actualizado con éxito',
										'Se realizó con éxito la actualización del programa.',
									);
								}
							} catch (e) {
								dispatch(GetHistoryFailure());
								toast('Error: respuesta errada del servidor');
							}
						}
					} catch (e) {

						dispatch(GetProgramasEducativosFailure(null));
						toast('Error: respuesta errada del servidor');
					}

				}
			} catch (e) {

				dispatch(AddEducationalEntitiesFailure(null));
				toast('Error: respuesta errada del servidor');
			}
		};

export const DeleteProgramSuccess = (
	idProgramaEducativo: string,
): AppActions => ({
	type: DELETE_PROGRAMA_SUCCESS,
	idProgramaEducativo: idProgramaEducativo,
});

export const startDeleteProgram =
	(idProgramaEducativo: string, idEntidad: string, idUsuario: string) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const request = {
					idProgramaEducativo: idProgramaEducativo,
					modificadoPor: idUsuario
				}
				const response = await api.DeleteProgram(request);
				if (response?.isSuccess) {
					dispatch(DeleteProgramSuccess(idProgramaEducativo));
					try {
						dispatch(GetHistory());
						const response = await api.GetChangeHistory(idEntidad);
						if (!response?.isSuccess) {
							dispatch(GetHistoryFailure());
							toast('Error: respuesta errada del servidor');
						} else {
							dispatch(GetHistorySuccess(response?.data));
							await NotificationService.genericAlert(
								'Eliminado con éxito',
								'Se realizó con éxito la eliminación del programa.',
							);
						}
					} catch (e) {
						dispatch(GetHistoryFailure());
						toast('Error: respuesta errada del servidor');
					}
				}
			} catch (e) {
				toast(`Error: respuesta errada del servidor ${e}`);
			}
		};
export const startGetTramiteEntity =
	(idEntidadEducativa: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetTramite());
			const response = await api.startGetTramiteEntity(idEntidadEducativa);
			if (!response?.isSuccess) {
				dispatch(GetTramiteFailure(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetTramiteSuccess(response.data));

				// Mensajes solicitudes Procesadas
				if (response?.data.length >= 2) {
					const solicitudesProcesadas = response?.data.filter((solicitud: any) => solicitud?.estado === 'PROCESADA');
					if (solicitudesProcesadas && solicitudesProcesadas.length >= 2) {
						toast('La entidad registra dos o más pagos', { type: toast.TYPE.ERROR });
					} else if (solicitudesProcesadas && solicitudesProcesadas.length === 1) {
						toast('La entidad registra un pago', { type: toast.TYPE.ERROR });
					} else if (solicitudesProcesadas && solicitudesProcesadas.length === 0) {
						toast('La entidad no registra pagos', { type: toast.TYPE.ERROR });
					}
				}
			}
		} catch (e) {

			dispatch(GetTramiteFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};
export const GetTramite = (): AppActions => ({
	type: GET_TRAMITE_ENTITIES,
});
export const GetTramiteSuccess = (
	tramite: TramiteEntity[],
): AppActions => ({
	type: GET_TRAMITE_ENTITIES_SUCCESS,
	tramite: tramite,
});

export const GetTramiteFailure = (
	tramite: TramiteEntity[] | null,
): AppActions => ({
	type: GET_TRAMITE_ENTITIES_FAILURE,
	tramite: tramite,
});
export const AddCampus = (): AppActions => ({
	type: ADD_CAMPUS,
});

export const AddCampusSuccess = (campus: SedeEntidadEducativa): AppActions => ({
	type: ADD_CAMPUS_SUCCESS,
	sede: campus,
});

export const AddCampusFailure = (
	campus: SedeEntidadEducativa | null,
): AppActions => ({
	type: ADD_CAMPUS_FAILURE,
	sede: campus,
});

export const startAddCampus =
	(sede: SedeEntidadEducativa) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(AddCampus());
			const response = await api.AddCampus(sede);
			if (!response?.isSuccess) {
				dispatch(AddCampusFailure(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(AddCampusSuccess(response.data));
				await NotificationService.genericAlert(
					'Agregado con éxito',
					'Se realizó con éxito la adición de la sede.',
				);
			}
		} catch (e) {

			dispatch(AddEducationalEntitiesFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};

export const UpdateCampus = (): AppActions => ({
	type: UPDATE_CAMPUS,
});

export const UpdateCampusSuccess = (campus: SedeEntidadEducativa): AppActions => ({
	type: UPDATE_CAMPUS_SUCCESS,
	sede: campus,
});

export const UpdateCampusFailure = (): AppActions => ({
	type: UPDATE_CAMPUS_FAILURE,
});

export const startUpdateCampus =
	(sede: SedeEntidadEducativa) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(UpdateCampus());
			const response = await api.UpdateCampus(sede);
			if (!response?.isSuccess) {
				dispatch(UpdateCampusFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(UpdateCampusSuccess(response.data));
				try {
					dispatch(GetHistory());
					const response = await api.GetChangeHistory(sede?.idEntidad);
					if (!response?.isSuccess) {
						dispatch(GetHistoryFailure());
						toast('Error: respuesta errada del servidor');
					} else {
						dispatch(GetHistorySuccess(response?.data));
						await NotificationService.genericAlert(
							'Actualizado con éxito',
							'Se realizó con éxito la actualizacón de la sede.',
						);

					}
				} catch (e) {
					dispatch(GetHistoryFailure());
					toast('Error: respuesta errada del servidor');
				}
			}
		} catch (e) {

			dispatch(AddEducationalEntitiesFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};

export const DeleteCampus = (): AppActions => ({
	type: DELETE_CAMPUS,
});

export const DeleteCampusSuccess = (idSede: string): AppActions => ({
	type: DELETE_CAMPUS_SUCCESS,
	idSede: idSede,
});

export const DeleteCampusFailure = (): AppActions => ({
	type: DELETE_CAMPUS_FAILURE,
});

export const startDeleteCampus =
	(idSede: string, idEntidad: string, idUsuario: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(DeleteCampus());
			const request = {
				idSede: idSede,
				modificadoPor: idUsuario
			}
			const response = await api.DeleteCampus(request);
			if (!response?.isSuccess) {
				dispatch(DeleteCampusFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(DeleteCampusSuccess(idSede));
				try {
					dispatch(GetHistory());
					const response = await api.GetChangeHistory(idEntidad);
					if (!response?.isSuccess) {
						dispatch(GetHistoryFailure());
						toast('Error: respuesta errada del servidor');
					} else {
						dispatch(GetHistorySuccess(response?.data));
						await NotificationService.genericAlert(
							'Eliminado con éxito',
							'Se realizó con éxito la eliminación de la sede.',
						);
					}
				} catch (e) {
					dispatch(GetHistoryFailure());
					toast('Error: respuesta errada del servidor');
				}
			}
		} catch (e) {

			dispatch(AddEducationalEntitiesFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetEducationalEntitiesDetail = (): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_DETAIL,
});

export const GetEducationalEntitiesDetailSuccess = (detalle: EntidadesEducativasFiltro): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_DETAIL_SUCCESS,
	detalleEntidadEducativa: detalle,
});

export const GetEducationalEntitiesDetailFailure = (
	detalle: EntidadesEducativasFiltro,
): AppActions => ({
	type: GET_EDUCATIONAL_ENTITIES_DETAIL_FAILURE,
	detalleEntidadEducativa: detalle,
});

export const startGetEducationalEntitiesDetail = (idEntidad: string) => async (dispatch: Dispatch<AppActions>) => {
	try {
		dispatch(GetEducationalEntitiesDetail());
		const response = await api.GetEducationalEntityDetail(idEntidad);
		if (!response?.isSuccess) {
			dispatch(GetEducationalEntitiesDetailFailure(response));
			toast('Error: respuesta errada del servidor');
		} else {
			dispatch(GetEducationalEntitiesDetailSuccess(response.data));
		}
	} catch (e) {

		dispatch(GetEducationalEntitiesDetailFailure({} as EntidadesEducativasFiltro));
		toast('Error: respuesta errada del servidor');
	}
};

export const GetArchivosPorEntidad = (): AppActions => ({
	type: GET_FILES_EDUCATIONAL_ENTITIES,
});

export const GetArchivosPorEntidadSuccess = (archivos: ArchivosEntidadesEducativas[]): AppActions => ({
	type: GET_FILES_EDUCATIONAL_ENTITIES_SUCCESS,
	archivos: archivos,
});

export const GetArchivosPorEntidadFailure = (): AppActions => ({
	type: GET_FILES_EDUCATIONAL_ENTITIES_FAILURE,
});

export const startGetArchivosPorEntidad = (idEntidad: string) => async (dispatch: Dispatch<AppActions>) => {
	try {
		dispatch(GetArchivosPorEntidad());
		const response = await api.GetFilesByEntity(idEntidad);
		if (!response?.isSuccess) {
			dispatch(GetArchivosPorEntidadFailure());
			toast('Error: respuesta errada del servidor');
		} else {
			dispatch(GetArchivosPorEntidadSuccess(response.data));
		}
	} catch (e) {

		dispatch(GetEducationalEntitiesDetailFailure({} as EntidadesEducativasFiltro));
		toast('Error: respuesta errada del servidor');
	}
};

export const startPostCrearHistorialEntidad = (historialEntity: HistorialEntity) =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch({ type: GET_EDUCATIONAL_ENTITIES_DETAIL });
			const response = await api.PostCrearHistorialEntidad(historialEntity);

			if (!response?.data?.isSuccess) {
				toast('Error: respuesta errada del servidor');
			} else {
				toast('Historial creado correctamente.');
			}
		} catch (error) {
			dispatch(GetEducationalEntitiesDetailFailure({} as EntidadesEducativasFiltro));
			toast('Error: No se pudo crear el historial.');
		}
	};

export const GetHistory = (): AppActions => ({
	type: GET_HISTORY,
});

export const GetHistorySuccess = (historialCambios: HistorialCambios[]): AppActions => ({
	type: GET_HISTORY_SUCCESS,
	historialCambios: historialCambios,
});

export const GetHistoryFailure = (): AppActions => ({
	type: GET_HISTORY_FAILURE,
});

export const startHistorialCambiosEntidades =
	(idEntidad: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetHistory());
			const response = await api.GetChangeHistory(idEntidad);
			if (!response?.isSuccess) {
				dispatch(GetHistoryFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetHistorySuccess(response?.data));
			}
		} catch (e) {
			dispatch(GetHistoryFailure());
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetFidecomisos = (): AppActions => ({
	type: FIDECOMISO_SUCCESS,
});

export const GetFidecomisosSuccess = (fidecomiso: Fidecomiso[]): AppActions => ({
	type: UPDATE_FIDECOMISO_SUCCESS,
	fidecomiso: fidecomiso,
});

export const GetFidecomisoFailure = (): AppActions => ({
	type: FIDEICOMISO_FAILURE,
});


export const startUpdateFideicomisos =
	(fideicomiso: Fidecomiso, idEntidad: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetFidecomisos());
			const response = await api.UpdateFidecomiso(fideicomiso, idEntidad);
			if (!response?.isSuccess) {
				dispatch(GetFidecomisoFailure());
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetFidecomisosSuccess(response.data));

				try {
					dispatch(GetCuentas());
					const response = await api.GetCuentas(idEntidad);
					if (!response?.isSuccess) {
						dispatch(GetCuentasFailure(response));
						toast('Error: respuesta errada del servidor');
					} else {
						dispatch(GetCuentasSuccess(response.data));
						try {
							dispatch(GetHistory());
							const response = await api.GetChangeHistory(idEntidad);
							if (!response?.isSuccess) {
								dispatch(GetHistoryFailure());
								toast('Error: respuesta errada del servidor');
							} else {
								dispatch(GetHistorySuccess(response?.data));
								await NotificationService.genericAlert(
									'Actualizado con éxito',
									'Se realizó con éxito la actualización de fideicomiso.',
								);

							}
						} catch (e) {
							dispatch(GetHistoryFailure());
							toast('Error: respuesta errada del servidor');
						}
					}
				} catch (e) {

					dispatch(GetCuentasFailure(null));
					toast('Error: respuesta errada del servidor');
				}
			}
		} catch (e) {

			dispatch(AddEducationalEntitiesFailure(null));
			toast('Error: respuesta errada del servidor');
		}
	};