import { ADD_NOVELTY_BLOCKS, 
         ADD_NOVELTY_BLOCKS_FAILURE, 
         ADD_NOVELTY_BLOCKS_SUCCESS, 
         ADD_PROPIETARY_HISTORY, 
         ADD_PROPIETARY_HISTORY_FAILURE, 
         ADD_PROPIETARY_HISTORY_SUCCESS, 
         ADD_REAL_STATE_REGISTRATIONS, 
         ADD_REAL_STATE_REGISTRATIONS_FAILURE, 
         ADD_REAL_STATE_REGISTRATIONS_SUCCESS, 
         DELETE_NOVELTY_BLOCKS, 
         DELETE_NOVELTY_BLOCKS_FAILURE, 
         DELETE_NOVELTY_BLOCKS_SUCCESS, 
         DELETE_PROPIETARY_HISTORY, 
         DELETE_PROPIETARY_HISTORY_FAILURE, 
         DELETE_PROPIETARY_HISTORY_SUCCESS, 
         DELETE_REAL_STATE_REGISTRATIONS, 
         DELETE_REAL_STATE_REGISTRATIONS_FAILURE, 
         DELETE_REAL_STATE_REGISTRATIONS_SUCCESS, 
         GET_BASIC_INFORMATION, 
         GET_BASIC_INFORMATION_FAILURE, 
         GET_BASIC_INFORMATION_SUCCESS, 
         GET_BLOCK, GET_BLOCK_FAILURE, 
         GET_BLOCK_SUCCESS, 
         GET_OPERATIONS_BLOCKS, 
         GET_OPERATIONS_BLOCKS_BY_ID, 
         GET_OPERATIONS_BLOCKS_BY_ID_FAILURE, 
         GET_OPERATIONS_BLOCKS_BY_ID_SUCCESS, 
         GET_OPERATIONS_BLOCKS_FAILURE, 
         GET_OPERATIONS_BLOCKS_SUCCESS, 
         GET_PROPIETARY_HISTORY, 
         GET_PROPIETARY_HISTORY_FAILURE, 
         GET_PROPIETARY_HISTORY_SUCCESS, 
         GET_REAL_STATE_REGISTRATIONS, 
         GET_REAL_STATE_REGISTRATIONS_FAILURE, 
         GET_REAL_STATE_REGISTRATIONS_SUCCESS, 
         MatriculasInmobiliariasActionTypes,
         UPDATE_NOVELTY_BLOCKS,
         UPDATE_NOVELTY_BLOCKS_FAILURE,
         UPDATE_NOVELTY_BLOCKS_SUCCESS,
         UPDATE_PROPIETARY_HISTORY,
         UPDATE_PROPIETARY_HISTORY_FAILURE,
         UPDATE_PROPIETARY_HISTORY_SUCCESS,
         UPDATE_REAL_STATE_REGISTRATIONS,
         UPDATE_REAL_STATE_REGISTRATIONS_FAILURE,
         UPDATE_REAL_STATE_REGISTRATIONS_SUCCESS, } from '../types/realStateRegistrations.action.type';
import { AgregarMatriculaDTO, 
         CausalesByidTypeDTO, 
         HistorialPropietariosDTO, 
         MatriculasInmobiliariasDTO, 
         CausalMatriculaInmobiliariaDto,
         MatriculasInmobiliariasInitial, 
         NovedadesBloqueosDeleteDTO, 
         NovedadesInsertarDTO, 
         OperacionesMatriculasDTO } from '../types/realStateRegistrations.type';

export interface RealStateRegistrationsState {
    matriculasInmobiliarias: MatriculasInmobiliariasDTO[] | null;
    informacionBasica: MatriculasInmobiliariasDTO,
    historialPropietarios: HistorialPropietariosDTO[];
    bloqueos: NovedadesInsertarDTO[];
    bloqueoInsertar: NovedadesInsertarDTO | null;
    operacionesBloqueos: OperacionesMatriculasDTO[] | null;
    causalesPortipo: CausalesByidTypeDTO[] | null;
    agregarMatricula: AgregarMatriculaDTO | null,
    eliminarMatricula: MatriculasInmobiliariasDTO | null;
    actualizarMatricula: MatriculasInmobiliariasDTO | null;
    actualizarHistorialPropietarios: HistorialPropietariosDTO | null;
    novedadesBloqueosEliminar: NovedadesBloqueosDeleteDTO[] | null;
    actualizarNovevadesBloqueos: NovedadesInsertarDTO | null;
    causales : CausalMatriculaInmobiliariaDto[] | null;
    error: any;
    loading: boolean;
}

const realStateRegistrationsStateInitial: RealStateRegistrationsState = {
    matriculasInmobiliarias: [],
    informacionBasica: MatriculasInmobiliariasInitial,
    historialPropietarios: [],
    bloqueos: [],
    bloqueoInsertar: null,
    operacionesBloqueos: [],
    causalesPortipo: [],
    agregarMatricula: null,
    eliminarMatricula: null,
    actualizarMatricula: null,
    actualizarHistorialPropietarios: null,
    novedadesBloqueosEliminar: [],
    actualizarNovevadesBloqueos: null,
    causales: null,
    error: null,
    loading: false,
}

export const RealStateRegistrationsReducer = (
    state: RealStateRegistrationsState = realStateRegistrationsStateInitial,
    action: MatriculasInmobiliariasActionTypes,
): RealStateRegistrationsState => {

    switch (action.type) {
        case GET_REAL_STATE_REGISTRATIONS:
            return {
                ...state,
                loading: true,
            };
        case GET_REAL_STATE_REGISTRATIONS_FAILURE:
            return {
                ...state,
                matriculasInmobiliarias: null,
                loading: false,
            };
        case GET_REAL_STATE_REGISTRATIONS_SUCCESS:
            return {
                ...state,
                matriculasInmobiliarias: action.matriculasInmobiliarias,
                loading: false,
            };
        case GET_BASIC_INFORMATION:
            return {
                ...state,
                loading: true,
            };
        case GET_BASIC_INFORMATION_FAILURE:
            return {
                ...state,
                informacionBasica: MatriculasInmobiliariasInitial,
                loading: false,
            };
        case GET_BASIC_INFORMATION_SUCCESS:
            return {
                ...state,
                informacionBasica: action.informacionBasica,
                loading: false,
            };
        case GET_PROPIETARY_HISTORY:
            return {
                ...state,
                loading: true,
            };
        case GET_PROPIETARY_HISTORY_FAILURE:
            return {
                ...state,
                historialPropietarios: [],
                loading: false,
            };
        case GET_PROPIETARY_HISTORY_SUCCESS:
            return {
                ...state,
                historialPropietarios: action.historialPropietarios,
                loading: false,
            };
        case GET_BLOCK:
            return {
                ...state,
                loading: true,
            };
        case GET_BLOCK_FAILURE:
            return {
                ...state,
                bloqueos: [],
                loading: false,
            };
        case GET_BLOCK_SUCCESS:
            return {
                ...state,
                bloqueos: action.bloqueos,
                loading: false,
            };

        case DELETE_NOVELTY_BLOCKS:
            return {
                ...state,
                loading: true,
            };
        case DELETE_NOVELTY_BLOCKS_FAILURE:
            return {
                ...state,
                novedadesBloqueosEliminar: [],
                loading: false,
            };
        case DELETE_NOVELTY_BLOCKS_SUCCESS:
            return {
                ...state,
                novedadesBloqueosEliminar

                    : action.novedadesBloqueosEliminar,
                loading: false,
            };



        case GET_OPERATIONS_BLOCKS:
            return {
                ...state,
                loading: true,
            };
        case GET_OPERATIONS_BLOCKS_FAILURE:
            return {
                ...state,
                operacionesBloqueos: [],
                loading: false,
            };
        case GET_OPERATIONS_BLOCKS_SUCCESS:
            return {
                ...state,
                operacionesBloqueos: action.operacionesBloqueos,
                loading: false,
            };

        case DELETE_PROPIETARY_HISTORY:
            return {
                ...state,
                loading: true,
            };
        case DELETE_PROPIETARY_HISTORY_FAILURE:
            return {
                ...state,
                historialPropietarios: [],
                loading: false,
            };
        case DELETE_PROPIETARY_HISTORY_SUCCESS:
            return {
                ...state,
                informacionBasica : {
                    ...state.informacionBasica,
                    propietarios: [...state.informacionBasica.propietarios.filter(x => x.prpId !== action.historialPropietariosEliminar?.prpId)]
                },
                loading: false,
            };

        case ADD_NOVELTY_BLOCKS:
            return {
                ...state,
                loading: true,
            };
        case ADD_NOVELTY_BLOCKS_FAILURE:
            return {
                ...state,
                bloqueoInsertar: null,
                loading: false,
            };
        case ADD_NOVELTY_BLOCKS_SUCCESS:
            return {
                ...state,
                informacionBasica: {
                    ...state.informacionBasica,
                    novedades: action.agregarNovedadesBloqueos,
                },
                loading: false,
            };
        case ADD_REAL_STATE_REGISTRATIONS:
            return {
                ...state,
                loading: true,
            };
        case ADD_REAL_STATE_REGISTRATIONS_FAILURE:
            return {
                ...state,
                agregarMatricula: null,
                loading: false,
            };
        case ADD_REAL_STATE_REGISTRATIONS_SUCCESS:
            return {
                ...state,
                agregarMatricula: action.agregarMatricula,
                loading: false,
            };

        case DELETE_REAL_STATE_REGISTRATIONS:
            return {
                ...state,
                loading: true,
            };
        case DELETE_REAL_STATE_REGISTRATIONS_FAILURE:
            return {
                ...state,
                eliminarMatricula: null,
                loading: false,
            };
        case DELETE_REAL_STATE_REGISTRATIONS_SUCCESS:
            return {
                ...state,
                matriculasInmobiliarias: [...state.matriculasInmobiliarias?.filter(x => x.maiId !== action.eliminarMatricula.maiId) as MatriculasInmobiliariasDTO[]],
                loading: false,
            };

        case UPDATE_REAL_STATE_REGISTRATIONS:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_REAL_STATE_REGISTRATIONS_FAILURE:
            return {
                ...state,
                actualizarMatricula: null,
                loading: false,
            };
        case UPDATE_REAL_STATE_REGISTRATIONS_SUCCESS:
            return {
                ...state,
                actualizarMatricula: action.actualizarMatricula,
                loading: false,
            };

        case UPDATE_PROPIETARY_HISTORY:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_PROPIETARY_HISTORY_FAILURE:
            return {
                ...state,
                actualizarHistorialPropietarios: null,
                loading: false,
            }; 
        // eslint-disable-next-line no-lone-blocks
        case UPDATE_PROPIETARY_HISTORY_SUCCESS: {
                return {
                    ...state, 
                    loading: false,
                    informacionBasica: {
                        ...state.informacionBasica,
                        propietarios: [...state.informacionBasica.propietarios.map(p => p.prpId === action.actualizarHistorialPropietarios.prpId ? {
                            ...p,
                            prpTipoIdentificacion: action.actualizarHistorialPropietarios.prpTipoIdentificacion,
                            prpIdTipoIdentificacion: action.actualizarHistorialPropietarios.prpIdTipoIdentificacion,
                            prpNumeroIdentificacion: action.actualizarHistorialPropietarios.prpNumeroIdentificacion,
                            prpNombreRazonSocial: action.actualizarHistorialPropietarios.prpNombreRazonSocial,
                            correo: action.actualizarHistorialPropietarios.correo,
                            telefono: action.actualizarHistorialPropietarios.telefono,
                        } : p)]
                    }
                }

            };

        case UPDATE_NOVELTY_BLOCKS:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_NOVELTY_BLOCKS_FAILURE:
            return {

                ...state,
                actualizarNovevadesBloqueos: null,
                loading: false,
            };
        case UPDATE_NOVELTY_BLOCKS_SUCCESS:
            return {
                ...state,
                actualizarNovevadesBloqueos: action.actualizarNovevadesBloqueos,
                loading: false,
            };

	case GET_OPERATIONS_BLOCKS_BY_ID:
            return {
                ...state,
                loading: true,
            };
        case GET_OPERATIONS_BLOCKS_BY_ID_SUCCESS:
            return {
                ...state,
                causalesPortipo: action.causalesPortipo,
                loading: false,
            };
        case GET_OPERATIONS_BLOCKS_BY_ID_FAILURE:
            return {
                ...state,
                causalesPortipo: null,
                loading: false,
            };  
        case ADD_PROPIETARY_HISTORY:
            return {
                ...state,
                loading: true,
            };
        case ADD_PROPIETARY_HISTORY_FAILURE:
            return {
                ...state,
                loading: false,
            };
        case ADD_PROPIETARY_HISTORY_SUCCESS:
            return {
                ...state,
                informacionBasica:{
                    ...state.informacionBasica,
                    propietarios: [...state.informacionBasica.propietarios.concat(action.historialPropietarios)]
                },
                loading: false,
            };
	    
        default:
            return state;
    }
};