import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import ApiRealStateRegistrations from '../services/api-realstateregistrations.service';
import history from '../utils/history.router';
import { AppActions } from '../types/appactions.type';
import {
    GET_REAL_STATE_REGISTRATIONS, 
    GET_REAL_STATE_REGISTRATIONS_SUCCESS, 
    GET_REAL_STATE_REGISTRATIONS_FAILURE,
    GET_BASIC_INFORMATION, 
    GET_BASIC_INFORMATION_SUCCESS, 
    GET_BASIC_INFORMATION_FAILURE, 
    GET_PROPIETARY_HISTORY,
    GET_PROPIETARY_HISTORY_SUCCESS, 
    GET_PROPIETARY_HISTORY_FAILURE, 
    GET_BLOCK, 
    GET_BLOCK_SUCCESS, 
    GET_BLOCK_FAILURE,
    DELETE_PROPIETARY_HISTORY_FAILURE, 
    DELETE_PROPIETARY_HISTORY_SUCCESS, 
    DELETE_PROPIETARY_HISTORY, 
    DELETE_NOVELTY_BLOCKS,
    DELETE_NOVELTY_BLOCKS_SUCCESS, 
    DELETE_NOVELTY_BLOCKS_FAILURE, 
    ADD_NOVELTY_BLOCKS, 
    ADD_NOVELTY_BLOCKS_SUCCESS,
    ADD_NOVELTY_BLOCKS_FAILURE, 
    GET_OPERATIONS_BLOCKS, 
    GET_OPERATIONS_BLOCKS_SUCCESS, 
    GET_OPERATIONS_BLOCKS_FAILURE,
    ADD_REAL_STATE_REGISTRATIONS, 
    ADD_REAL_STATE_REGISTRATIONS_SUCCESS, 
    ADD_REAL_STATE_REGISTRATIONS_FAILURE, 
    DELETE_REAL_STATE_REGISTRATIONS, 
    DELETE_REAL_STATE_REGISTRATIONS_SUCCESS, 
    DELETE_REAL_STATE_REGISTRATIONS_FAILURE, 
    UPDATE_REAL_STATE_REGISTRATIONS, 
    UPDATE_REAL_STATE_REGISTRATIONS_SUCCESS, 
    UPDATE_REAL_STATE_REGISTRATIONS_FAILURE, 
    UPDATE_PROPIETARY_HISTORY, 
    UPDATE_PROPIETARY_HISTORY_SUCCESS, 
    UPDATE_PROPIETARY_HISTORY_FAILURE, 
    UPDATE_NOVELTY_BLOCKS, 
    UPDATE_NOVELTY_BLOCKS_SUCCESS, 
    UPDATE_NOVELTY_BLOCKS_FAILURE,
    GET_OPERATIONS_BLOCKS_BY_ID,
    GET_OPERATIONS_BLOCKS_BY_ID_SUCCESS,
    GET_OPERATIONS_BLOCKS_BY_ID_FAILURE,
    ADD_PROPIETARY_HISTORY,
    ADD_PROPIETARY_HISTORY_FAILURE,
    ADD_PROPIETARY_HISTORY_SUCCESS,
} from '../types/realStateRegistrations.action.type';
import { AgregarMatriculaDTO, CausalesByidTypeDTO, HistorialNovedadesMatriculasInmobiliarias, HistorialPropietariosDTO, MatriculasInmobiliariasDTO, MatriculasInmobiliariasFilterDTO, NovedadesBloqueosDeleteDTO, NovedadesInsertarDTO, OperacionesMatriculasDTO } from '../types/realStateRegistrations.type';
import NotificationService from '../services/notification.service';

const apiRegistrations = ApiRealStateRegistrations();
const api = ApiRealStateRegistrations();


export const GetRealStateRegistrations = (): AppActions => ({
    type: GET_REAL_STATE_REGISTRATIONS,
});

export const GetRealStateRegistrationsSuccess = (matriculasInmobiliarias: MatriculasInmobiliariasDTO[]): AppActions => ({
    type: GET_REAL_STATE_REGISTRATIONS_SUCCESS,
    matriculasInmobiliarias: matriculasInmobiliarias,
});

export const GetRealStateRegistrationsFailure = (matriculasInmobiliarias: MatriculasInmobiliariasDTO[] | null): AppActions => ({
    type: GET_REAL_STATE_REGISTRATIONS_FAILURE,
    matriculasInmobiliarias: matriculasInmobiliarias,
});


export const startGetRealStateRegistrations = (matriculas: MatriculasInmobiliariasFilterDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetRealStateRegistrations());
            const response =
                await apiRegistrations.GetRealStateRegistrations(matriculas);
            if (!response?.isSuccess) {
                dispatch(
                    GetRealStateRegistrationsFailure(response.data),
                );
                toast('Error: respuesta errada del servidor', { type: toast.TYPE.ERROR });
            } else {
                dispatch(
                    GetRealStateRegistrationsSuccess(
                        response.data,
                    ),
                );
                if (response.data.length === 0) {
                    toast('No se encontraron matrículas inmobiliarias.',{ type: toast.TYPE.ERROR });
                }
            }
        } catch (e) {
           
            dispatch(
                GetRealStateRegistrationsFailure(null),
            );
            toast('Error: respuesta errada del servidor',{ type: toast.TYPE.ERROR });
        }
    };

export const GetBasicInformation = (): AppActions => ({
    type: GET_BASIC_INFORMATION,
});

export const GetBasicInformationSuccess = (informacionBasica: MatriculasInmobiliariasDTO): AppActions => ({
    type: GET_BASIC_INFORMATION_SUCCESS,
    informacionBasica: informacionBasica,
});

export const GetBasicInformationFailure = (informacionBasica: MatriculasInmobiliariasDTO | null): AppActions => ({
    type: GET_BASIC_INFORMATION_FAILURE,
    informacionBasica: informacionBasica,
});


export const startGetBasicInformation = (idInformacionBasica: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetBasicInformation());
            const response =
                await apiRegistrations.GetBasicInformation(idInformacionBasica);
            if (!response?.isSuccess) {
                dispatch(
                    GetBasicInformationFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    GetBasicInformationSuccess(
                        response.data,
                    ),
                );
                history.push('/MatriculasInmobiliarias')
                if (response.data == null) {
                    toast('No se pudo guardar la información con éxito.');
                }
            }
        } catch (e) {
           
            dispatch(
                GetBasicInformationFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const GetPropietaryHistory = (): AppActions => ({
    type: GET_PROPIETARY_HISTORY,
});

export const GetPropietaryHistorySuccess = (historialPropietarios: HistorialPropietariosDTO[]): AppActions => ({
    type: GET_PROPIETARY_HISTORY_SUCCESS,
    historialPropietarios: historialPropietarios,
});

export const GetPropietaryHistoryFailure = (historialPropietarios: HistorialPropietariosDTO[] | null): AppActions => ({
    type: GET_PROPIETARY_HISTORY_FAILURE,
    historialPropietarios: historialPropietarios,
});



export const startGetPropietaryHistory = (idHistorialPropietarios: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetPropietaryHistory());
            const response =
                await apiRegistrations.GetPropietaryHistory(idHistorialPropietarios);
            if (!response?.isSuccess) {
                dispatch(
                    GetPropietaryHistoryFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    GetPropietaryHistorySuccess(
                        response.data,
                    ),
                );
                if (response.data == null) {
                    toast('No se actualizó correctamente el historial.');
                }
            }
        } catch (e) {
           
            dispatch(
                GetPropietaryHistoryFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const AddPropietaryHistory = (): AppActions => ({
    type: ADD_PROPIETARY_HISTORY,
});

export const AddPropietaryHistorySuccess = (historialPropietarios: HistorialPropietariosDTO): AppActions => ({
    type: ADD_PROPIETARY_HISTORY_SUCCESS,
    historialPropietarios: historialPropietarios,
});

export const AddPropietaryHistoryFailure = (historialPropietarios: HistorialPropietariosDTO | null): AppActions => ({
    type: ADD_PROPIETARY_HISTORY_FAILURE,
    historialPropietarios: historialPropietarios,
});

export const startAddPropietaryHistory = (historialPropietarios: HistorialPropietariosDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(AddPropietaryHistory());
            const response =
                await apiRegistrations.AddPropietaryHistory(historialPropietarios);
            if (!response?.isSuccess) {
                dispatch(
                    GetPropietaryHistoryFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    AddPropietaryHistorySuccess(
                        response.data,
                    ),
                );
                toast('Propietario agregado con éxito.');
            }
        } catch (e) {
           
            dispatch(
                GetPropietaryHistoryFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const DeletePropietaryHistory = (): AppActions => ({
    type: DELETE_PROPIETARY_HISTORY,
});

export const DeletePropietaryHistorySuccess = (historialPropietariosEliminar: HistorialPropietariosDTO): AppActions => ({
    type: DELETE_PROPIETARY_HISTORY_SUCCESS,
    historialPropietariosEliminar: historialPropietariosEliminar,
});

export const DeletePropietaryHistoryFailure = (historialPropietariosEliminar: HistorialPropietariosDTO | null): AppActions => ({
    type: DELETE_PROPIETARY_HISTORY_FAILURE,
    historialPropietariosEliminar: historialPropietariosEliminar,
});
 
export const startDeletePropietaryHistory = (historialPropietariosEliminar: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(DeletePropietaryHistory());
            const response = await api.DeletePropietaryHistory(historialPropietariosEliminar);
         
        } catch (e) {
           
            dispatch(
                DeletePropietaryHistoryFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    }


export const GetBlock = (): AppActions => ({
    type: GET_BLOCK,
});

export const GetBlockSuccess = (bloqueos: NovedadesInsertarDTO[]): AppActions => ({
    type: GET_BLOCK_SUCCESS,
    bloqueos: bloqueos,
});

export const GetBlockFailure = (bloqueos: NovedadesInsertarDTO[] | null): AppActions => ({
    type: GET_BLOCK_FAILURE,
    bloqueos: bloqueos,
});


export const startGetBloqueos = (idMatricula: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetBlock());
            const response =
                await apiRegistrations.GetBloqueos(idMatricula);
            if (!response?.isSuccess) {
                dispatch(
                    GetBlockFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    GetBlockSuccess(
                        response.data,
                    ),
                );
                if (response.data == null) {
                    toast('No se pudo guardar la novedad.');
                }
            }
        } catch (e) {
           
            dispatch(
                GetPropietaryHistoryFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const GetOperationsBlocks = (): AppActions => ({
    type: GET_OPERATIONS_BLOCKS,
});

export const GetOperationsBlocksSuccess = (operacionesBloqueos: OperacionesMatriculasDTO[]): AppActions => ({
    type: GET_OPERATIONS_BLOCKS_SUCCESS,
    operacionesBloqueos: operacionesBloqueos,
});

export const GetOperationsBlocksFailure = (operacionesBloqueos: OperacionesMatriculasDTO[] | null): AppActions => ({
    type: GET_OPERATIONS_BLOCKS_FAILURE,
    operacionesBloqueos: operacionesBloqueos,
});


export const startGetOperacionesBloqueos = () =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetOperationsBlocks());
            const response =
                await apiRegistrations.GetOperacionesBloqueos();
            if (!response?.isSuccess) {
                dispatch(
                    GetOperationsBlocksFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    GetOperationsBlocksSuccess(
                        response.data,
                    ),
                );
                if (response.data.length === 0) {
                    toast('No se pudo guardar la operación.');
                }
            }
        } catch (e) {
           
            dispatch(
                GetOperationsBlocksFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };


export const DeleteNoveltyBlocks = (): AppActions => ({
    type: DELETE_NOVELTY_BLOCKS,
});

export const DeleteNoveltyBlocksSuccess = (novedadesBloqueosEliminar: NovedadesBloqueosDeleteDTO[]): AppActions => ({
    type: DELETE_NOVELTY_BLOCKS_SUCCESS,
    novedadesBloqueosEliminar: novedadesBloqueosEliminar,
});

export const DeleteNoveltyBlocksFailure = (novedadesBloqueosEliminar: NovedadesBloqueosDeleteDTO[] | null): AppActions => ({
    type: DELETE_NOVELTY_BLOCKS_FAILURE,
    novedadesBloqueosEliminar: novedadesBloqueosEliminar,
});

export const startDeleteNoveltyBlocks = (novedadesBloqueosEliminar: string) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(DeleteNoveltyBlocks());
            const response =
                await api.DeleteNoveltyBlocks(novedadesBloqueosEliminar);
            if (!response?.isSuccess) {
                dispatch(
                    DeleteNoveltyBlocksFailure(response?.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    DeleteNoveltyBlocksSuccess(response?.data),
                );
                if (response.data == null) {
                    toast('Matrícula eliminada correctamente.');
                }
            }
        } catch (e) {
           
            dispatch(
                DeleteNoveltyBlocksFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    }

export const AddNoveltyBlocks = (): AppActions => ({
    type: ADD_NOVELTY_BLOCKS,
});

export const AddNoveltyBlocksSuccess = (agregarNovedadesBloqueos: HistorialNovedadesMatriculasInmobiliarias[]): AppActions => ({
    type: ADD_NOVELTY_BLOCKS_SUCCESS,
    agregarNovedadesBloqueos: agregarNovedadesBloqueos,
});

export const AddNoveltyBlocksFailure = (agregarNovedadesBloqueos: HistorialNovedadesMatriculasInmobiliarias[] | null): AppActions => ({
    type: ADD_NOVELTY_BLOCKS_FAILURE,
    agregarNovedadesBloqueos: agregarNovedadesBloqueos,
}); 

export const startAddNoveltyBlocks = (agregarNovedadOperacion: HistorialNovedadesMatriculasInmobiliarias) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(AddNoveltyBlocks());
            const response =
                await api.AddNoveltyBlocks(agregarNovedadOperacion);
            if (!response?.isSuccess) {
                dispatch(
                    AddNoveltyBlocksFailure(response?.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    AddNoveltyBlocksSuccess(response?.data),
                );
                toast('Novedad agregada correctamente.');

                try {
                    dispatch(GetBasicInformation());
                    const response =
                        await apiRegistrations.GetBasicInformation(agregarNovedadOperacion?.idMatricula);
                    if (!response?.isSuccess) {
                        dispatch(
                            GetBasicInformationFailure(response.data),
                        );
                        toast('Error: respuesta errada del servidor');
                    } else {
                        dispatch(
                            GetBasicInformationSuccess(
                                response.data,
                            ),
                        );
                        if (response.data == null) {
                            toast('No se pudo guardar la información con éxito.');
                        }
                    }
                } catch (e) {
                   
                    dispatch(
                        GetBasicInformationFailure(null),
                    );
                    toast('Error: respuesta errada del servidor');
                }
            }
        } catch (e) {
           
            dispatch(
                AddNoveltyBlocksFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    }

export const AddRealStateRegistrations = (): AppActions => ({
    type: ADD_REAL_STATE_REGISTRATIONS,
});

export const AddRealStateRegistrationsSuccess = (agregarMatricula: AgregarMatriculaDTO): AppActions => ({
    type: ADD_REAL_STATE_REGISTRATIONS_SUCCESS,
    agregarMatricula: agregarMatricula,
});

export const AddRealStateRegistrationsFailure = (agregarMatricula: AgregarMatriculaDTO | null): AppActions => ({
    type: ADD_REAL_STATE_REGISTRATIONS_FAILURE,
    agregarMatricula: agregarMatricula,
});


// export const startAddRealStateRegistrations = (agregarMatricula: AgregarMatriculaDTO) =>
//     async (dispatch: Dispatch<AppActions>) => {
//         try {
//             dispatch(AddRealStateRegistrations());
//             const response =
//                 await apiRegistrations.AddRealStateRegistrations(agregarMatricula);
//             if (!response?.isSuccess) {
//                 dispatch(
//                     AddRealStateRegistrationsFailure(response.data),
//                 );
//                 toast('Error: respuesta errada del servidor');
                
//             } 
//             if(response.returnMessage !== null)
//             {
//                 dispatch(
//                     AddRealStateRegistrationsFailure(
//                         response.data,
//                     ),
//                 );
//                 await NotificationService.genericAlertError(
//                     'Error',
//                     response.returnMessage,
//                 );
//                 history.push('/Matriculas')
//             }
//             else {
               
//                 dispatch(
//                     AddRealStateRegistrationsSuccess(
//                         response.data,
//                     ),
//                 );
//                 await NotificationService.genericAlert(
//                     'Agregado con éxito',
//                     'Se agregó la matrícula con éxito.',
//                 );
//                 history.push('/Matriculas')
//             }
//         } catch (e) {
           
//             dispatch(
//                 AddRealStateRegistrationsFailure(null),
//             );
//             toast('Error: respuesta errada del servidor');
//         }
//     };

export const startAddRealStateRegistrations = (agregarMatricula: AgregarMatriculaDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(AddRealStateRegistrations());
            const response = await apiRegistrations.AddRealStateRegistrations(agregarMatricula);

            if (!response?.isSuccess) {
                if (response.returnMessage === 'Matricula ya existente') {
                    dispatch(AddRealStateRegistrationsFailure(null));
                    await NotificationService.genericAlertError(
                        'Matrícula Duplicada',
                        'El número de matrícula ya está registrado. Verifica la información.',
                    );
                } else {
                    dispatch(AddRealStateRegistrationsFailure(response.data));
                    toast('Error: respuesta errada del servidor');
                }
                history.push('/Matriculas');
                return;
            }

            dispatch(AddRealStateRegistrationsSuccess(response.data));
            await NotificationService.genericAlert(
                'Agregado con éxito',
                'Se agregó la matrícula con éxito.',
            );
            history.push('/Matriculas');
        } catch (error: any) {
            if (error.response) {
                if (error.response.status === 409 && error.response.data?.returnMessage) {
                    
                    toast('Matrícula ya existe', { type: toast.TYPE.ERROR });
                    dispatch(AddRealStateRegistrationsFailure(null));
                } else {
                    console.error("Error del servidor:", error.response.data);
                    toast(`Error: ${error.response.data?.message || 'Problema con la solicitud'}`);
                }
            } else if (error.request) {
                console.error("Sin respuesta del servidor:", error.request);
                toast('Error: El servidor no respondió.', { type: toast.TYPE.ERROR });
            } else {
                console.error("Error desconocido:", error.message);
                toast(`Error inesperado: ${error.message}`, { type: toast.TYPE.ERROR });
            }
            dispatch(AddRealStateRegistrationsFailure(null));
        }
    };



export const DeleteRealStateRegistrations = (): AppActions => ({
    type: DELETE_REAL_STATE_REGISTRATIONS,
});

export const DeleteRealStateRegistrationsSuccess = (eliminarMatriculaInmobiliaria: MatriculasInmobiliariasDTO): AppActions => ({
    type: DELETE_REAL_STATE_REGISTRATIONS_SUCCESS,
    eliminarMatricula: eliminarMatriculaInmobiliaria,
});

export const DeleteRealStateRegistrationsFailure = (eliminarMatriculaInmobiliaria: MatriculasInmobiliariasDTO | null): AppActions => ({
    type: DELETE_REAL_STATE_REGISTRATIONS_FAILURE,
    eliminarMatricula: eliminarMatriculaInmobiliaria,
});

export const startDeleteRealStateRegistrations = (eliminarMatricula: MatriculasInmobiliariasDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(DeleteRealStateRegistrations());
            const response = await api.DeleteRealStateRegistrations(eliminarMatricula);
            if (response.data == null) {
                dispatch(
                    DeleteRealStateRegistrationsFailure(response?.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    DeleteRealStateRegistrationsSuccess(response?.data),
                );
                if (response.data != null) {
                    toast('Matrícula eliminada correctamente.');
                }
            }
        } catch (e) {
           
            dispatch(
                DeleteRealStateRegistrationsFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    }

export const UpdateRealStateRegistrations = (): AppActions => ({
    type: UPDATE_REAL_STATE_REGISTRATIONS,
});

export const UpdateRealStateRegistrationsSuccess = (actualizarMatriculaInmobiliaria: MatriculasInmobiliariasDTO): AppActions => ({
    type: UPDATE_REAL_STATE_REGISTRATIONS_SUCCESS,
    actualizarMatricula: actualizarMatriculaInmobiliaria,
    
});

export const UpdateRealStateRegistrationsFailure = (actualizarMatriculaInmobiliaria: MatriculasInmobiliariasDTO | null): AppActions => ({
    type: UPDATE_REAL_STATE_REGISTRATIONS_FAILURE,
    actualizarMatricula: actualizarMatriculaInmobiliaria,
});

export const startUpdateRealStateRegistrations = (actualizarMatricula: MatriculasInmobiliariasDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(UpdateRealStateRegistrations());
            const response =
                await api.UpdateRealStateRegistrations(actualizarMatricula);
            if (!response?.isSuccess) {
                dispatch(
                    UpdateRealStateRegistrationsFailure(response?.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    UpdateRealStateRegistrationsSuccess(response?.data),
                );
                toast('Información actualizada');
            }
        } catch (e) {
           
            dispatch(
                UpdateRealStateRegistrationsFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    }

export const UpdatePropietaryHistory = (): AppActions => ({
    type: UPDATE_PROPIETARY_HISTORY,
});

export const UpdatePropietaryHistorySuccess = (actualizarHistorialPropietarios: HistorialPropietariosDTO): AppActions => ({
    type: UPDATE_PROPIETARY_HISTORY_SUCCESS,
    actualizarHistorialPropietarios: actualizarHistorialPropietarios,
});

export const UpdatePropietaryHistoryFailure = (actualizarHistorialPropietarios: HistorialPropietariosDTO | null): AppActions => ({
    type: UPDATE_PROPIETARY_HISTORY_FAILURE,
    actualizarHistorialPropietarios: actualizarHistorialPropietarios,
});

export const startUpdatePropietaryHistory = (actualizarHistorialPropietarios: HistorialPropietariosDTO) =>
    async (dispatch: Dispatch<AppActions>): Promise<Response> => {
        try {
            dispatch(UpdatePropietaryHistory());
            const response = await api.UpdatePropietaryHistory(actualizarHistorialPropietarios);
            if (!response?.isSuccess) {
                dispatch(UpdatePropietaryHistoryFailure(response?.data));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(UpdatePropietaryHistorySuccess(response?.data));
                toast('Historial modificado correctamente.');
            }
            return response; 
        } catch (e) {
           
            dispatch(UpdatePropietaryHistoryFailure(null));
            toast('Error: respuesta errada del servidor');
            throw e; 
        }
    };


export const UpdateNoveltyBlocks = (): AppActions => ({
    type: UPDATE_NOVELTY_BLOCKS,
});

export const UpdateNoveltyBlocksSuccess = (actualizarNovevadesBloqueos: NovedadesInsertarDTO): AppActions => ({
    type: UPDATE_NOVELTY_BLOCKS_SUCCESS,
    actualizarNovevadesBloqueos: actualizarNovevadesBloqueos,
});

export const UpdateNoveltyBlocksFailure = (actualizarHistorialPropietarios: NovedadesInsertarDTO | null): AppActions => ({
    type: UPDATE_NOVELTY_BLOCKS_FAILURE,
    actualizarNovevadesBloqueos: actualizarHistorialPropietarios,
});

export const startUpdateNoveltyBlocks = (actualizarNovevadesBloqueos: NovedadesInsertarDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(UpdateNoveltyBlocks());
            const response =
                await api.UpdateNoveltyBlocks(actualizarNovevadesBloqueos);
            if (!response?.isSuccess) {
                dispatch(
                    UpdateNoveltyBlocksFailure(response?.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    UpdateNoveltyBlocksSuccess(response?.data),
                );
                if (response.data == null) {
                    toast('Novedad eliminada correctamente.');
                }
            }
        } catch (e) {
           
            dispatch(
                UpdateNoveltyBlocksFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    }
    
    export const GetOperationsBlockById = (): AppActions => ({
    type: GET_OPERATIONS_BLOCKS_BY_ID,
});
        
export const GetOperationsBlockByIdSuccess = (causalesPortipo: CausalesByidTypeDTO[]): AppActions => ({
    type: GET_OPERATIONS_BLOCKS_BY_ID_SUCCESS,
    causalesPortipo: causalesPortipo,
});

export const GetOperationsBlockByIdFailure = (causalesPortipo: CausalesByidTypeDTO[] | null): AppActions => ({
    type: GET_OPERATIONS_BLOCKS_BY_ID_FAILURE,
    causalesPortipo: causalesPortipo,
});
        

export const startGetBloqueosById = (typeIdOperation: string) =>
async (dispatch: Dispatch<AppActions>) => {
    try {
        dispatch(GetOperationsBlockById());
        const response =
            await apiRegistrations.GetCausalTypeById(typeIdOperation);
        if (!response?.isSuccess) {
            dispatch(
                GetOperationsBlockByIdFailure(response.data),
            );
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(
                GetOperationsBlockByIdSuccess(
                    response.data,
                ),
            );
            if (response.data == null) {
                toast('No se encontraron causales.');
            }
        }
    } catch (e) {
       
        dispatch(
            GetOperationsBlockByIdFailure(null),
        );
        toast('Error: respuesta errada del servidor');
    }
};
