import {
  EntidadesEducativasActionType,
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
  ADD_CAMPUS_SUCCESS,
  UPDATE_CAMPUS_SUCCESS,
  DELETE_CAMPUS_SUCCESS,
  ADD_PROGRAMS_SUCCESS,
  DELETE_PROGRAMA_SUCCESS,
  UPDATE_PROGRAMA_SUCCESS,
  GET_EDUCATIONAL_ENTITIES_DETAIL,
  GET_EDUCATIONAL_ENTITIES_DETAIL_FAILURE,
  GET_EDUCATIONAL_ENTITIES_DETAIL_SUCCESS,
  GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD,
  GET_FILES_EDUCATIONAL_ENTITIES,
  GET_FILES_EDUCATIONAL_ENTITIES_SUCCESS,
  GET_FILES_EDUCATIONAL_ENTITIES_FAILURE,
  GET_HISTORY,
  GET_HISTORY_SUCCESS,
  GET_HISTORY_FAILURE,
  UPDATE_FIDECOMISO_SUCCESS,
  FIDECOMISO_SUCCESS,
  FIDEICOMISO_FAILURE,
  GET_TRAMITE_ENTITIES,
  GET_TRAMITE_ENTITIES_SUCCESS,
  GET_TRAMITE_ENTITIES_FAILURE,
} from '../types/educational.entities.actions.type';
import {
  BloqueosEntidadesEducativas,
  CuentaEntidadesEducativas,
  EntidadesEducativasFiltro,
  ProgramasEducativos,
  SedeEntidadEducativa,
  EntidadesEducativasAgregar,
  ArchivosEntidadesEducativas,
  HistorialCambios,
  Fidecomiso,
  TramiteEntity,
} from '../types/educational.entities.type';

export interface EducationalEntitiesState {
  educationalEntities: EntidadesEducativasFiltro[] | null;
  sedesEntidadEducativa: SedeEntidadEducativa[];
  programasEducativos: ProgramasEducativos[] | null;
  bloqueos: BloqueosEntidadesEducativas[];
  cuentas: CuentaEntidadesEducativas[];
  agregarEntidadEducativa: EntidadesEducativasAgregar | null;
  idEntidad: string;
  detalleEntidadEducativa: EntidadesEducativasFiltro;
  loading: boolean;
  dataRowDetalleEntidad: EntidadesEducativasFiltro;
  archivos: ArchivosEntidadesEducativas[] | null;
  tramite: TramiteEntity[] | null;
  historialCambios: HistorialCambios[] | null;
  fidecomisos: Fidecomiso[] | null;
}

const EducationalEntitiesStateInitial: EducationalEntitiesState = {
  educationalEntities: [],
  sedesEntidadEducativa: [],
  programasEducativos: [],
  bloqueos: [],
  cuentas: [],
  loading: false,
  agregarEntidadEducativa: null,
  idEntidad: '',
  detalleEntidadEducativa: {} as EntidadesEducativasFiltro,
  dataRowDetalleEntidad: {} as EntidadesEducativasFiltro,
  archivos: [],
  tramite: [],
  historialCambios: [],
  fidecomisos: []
};

export const EducationalEntitiesReducer = (
  state: EducationalEntitiesState = EducationalEntitiesStateInitial,
  action: EntidadesEducativasActionType,
): EducationalEntitiesState => {
  switch (action.type) {
    case SHOW_ROW_DETALLE_SUCCESS:
      return {
        ...state,
        dataRowDetalleEntidad: action.dataRowDetalleEntidad,
      };
    case GET_EDUCATIONAL_ENTITIES_FILTER:
      return {
        ...state,
        loading: true,
      };
    case GET_EDUCATIONAL_ENTITIES_FILTER_SUCCESS:
      return {
        ...state,
        educationalEntities: action.EntidadesEducativas,
        loading: false,
      };
    case GET_EDUCATIONAL_ENTITIES_FILTER_FAILURE:
      return {
        ...state,
        educationalEntities: null,
        loading: false,
      };
    case UPDATE_EDUCATIONAL_ENTITIES:
      return {
        ...state,
        loading: true,
      };
    case UPDATE_EDUCATIONAL_ENTITIES_SUCCESS:
      return {
        ...state,
        loading: false,
      };
    case UPDATE_EDUCATIONAL_ENTITIES_FAILURE:
      return {
        ...state,
        loading: false,
      };
    case GET_EDUCATIONAL_ENTITIES_CAMPUS:
      return {
        ...state,
        loading: true,
      };
    case GET_EDUCATIONAL_ENTITIES_CAMPUS_SUCCESS:
      return {
        ...state,
        sedesEntidadEducativa: action.sedesEntidadEducativa,
        loading: false,
      };
    case GET_EDUCATIONAL_ENTITIES_CAMPUS_FAILURE:
      return {
        ...state,
        sedesEntidadEducativa: [],
        loading: false,
      };
    case  GET_TRAMITE_ENTITIES:
      return {
        ...state,
        loading: true,
    };
    case GET_TRAMITE_ENTITIES_SUCCESS:
      return {
        ...state,
        tramite: action.tramite,
        loading: false,
    };
    case GET_TRAMITE_ENTITIES_FAILURE:
      return {
        ...state,
        tramite: null,
        loading: false,
    };
    case GET_HISTORY:
        return {
          ...state,
          loading: true,
      };
    case GET_HISTORY_SUCCESS:
      return {
        ...state,
        historialCambios: action.historialCambios,
        loading: false,
    };
    case GET_HISTORY_FAILURE:
      return {
        ...state,
        historialCambios: null,
        loading: false,
      };
    case GET_EDUCATIONAL_PROGRAMS:
      return {
        ...state,
        loading: true,
      };
    case GET_EDUCATIONAL_PROGRAMS_SUCCESS:
      return {
        ...state,
        programasEducativos: action.programasEducativos,
        loading: false,
      };
    case GET_EDUCATIONAL_PROGRAMS_FAILURE:
      return {
        ...state,
        programasEducativos: null,
        loading: false,
      };
    case GET_BLOCKS:
      return {
        ...state,
        loading: true,
      };
    case GET_BLOCKS_SUCCESS:
      return {
        ...state,
        bloqueos: action.bloqueos,
        loading: false,
      };
    case GET_BLOCKS_FAILURE:
      return {
        ...state,
        bloqueos: [],
        loading: false,
      };
    case LOCK_ENTIDADES:
      return {
        ...state,
        loading: true,
      };
    case LOCK_ENTIDADES_SUCCESS:
      return {
        ...state,
        bloqueos: state.bloqueos?.concat(action.bloqueo),
        loading: false,
      };
    case LOCK_ENTIDADES_FAILURE:
      return {
        ...state,
        bloqueos: [],
        loading: false,
      };
    case GET_ACCOUNTS_EDUCATIONAL_ENTITIES:
      return {
        ...state,
        loading: true,
      };
    case GET_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS:
      return {
        ...state,
        cuentas: action.cuentas,
        loading: false,
      };
    case GET_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE:
      return {
        ...state,
        loading: false,
      };
    case ADD_ACCOUNTS_EDUCATIONAL_ENTITIES:
    case UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES:
    case DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES:
      return {
        ...state,
        loading: true,
      };
    case ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS:
      return {
        ...state,
        cuentas: state.cuentas?.concat(action.cuenta),
        loading: false,
      };
    case UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS:
      return {
        ...state,
        cuentas: state.cuentas?.map(c => {
          if (c.idCuentaBancaria !== action.cuenta.idCuentaBancaria)
            return c;
          return action.cuenta;
        }),
        loading: false,
      };
    case DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_SUCCESS:
      return {
        ...state,
        cuentas: state.cuentas.filter(c => c.idCuentaBancaria !== action.idCuenta),
        loading: false,
      };
    case ADD_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE:
    case UPDATE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE:
    case DELETE_ACCOUNTS_EDUCATIONAL_ENTITIES_FAILURE:
      return {
        ...state,
        loading: false,
      };
    case ADD_EDUCATIONAL_ENTITIES:
      return {
        ...state,
        loading: true,
      };
    case ADD_EDUCATIONAL_ENTITIES_SUCCESS:
      return {
        ...state,
        agregarEntidadEducativa: action.agregarEntidadEducativa,
        loading: false,
      };
    case ADD_EDUCATIONAL_ENTITIES_FAILURE:
      return {
        ...state,
        agregarEntidadEducativa: null,
        loading: false,
      };
    case ADD_CAMPUS_SUCCESS:
      return {
        ...state,
        sedesEntidadEducativa: state.sedesEntidadEducativa.concat(action.sede),
        loading: false,
      };
    case UPDATE_CAMPUS_SUCCESS:
      return {
        ...state,
        sedesEntidadEducativa: state.sedesEntidadEducativa.map(e => {
          if (e.idSedeEntidad !== action.sede.idSedeEntidad)
            return e;
          return {...e, nombreSede: action.sede.nombreSede, ciudadId: action.sede.ciudadId, nombreCiudad: action.sede.nombreCiudad};
        }),
        loading: false,
      };
    case DELETE_CAMPUS_SUCCESS:
      return {
        ...state,
        sedesEntidadEducativa: state.sedesEntidadEducativa.filter(se => se.idSedeEntidad !== action.idSede),
        loading: false,
      };
    case ADD_PROGRAMS_SUCCESS:
      return {
        ...state,
        programasEducativos: state.programasEducativos?.concat(action.programs) || [],
        loading: false,
      };
    case UPDATE_PROGRAMA_SUCCESS:
      return {
        ...state,
        programasEducativos: state.programasEducativos?.map(p => {
          if (p.idPrograma !== action.programaEducativo.id)
            return p;
          return {...p, programa: action.programaEducativo.programa};
        }) || [],
        loading: false,
      };
    case DELETE_PROGRAMA_SUCCESS:
      return {
        ...state,
        programasEducativos: state.programasEducativos?.filter(pe => pe.idPrograma !== action.idProgramaEducativo) || [],
        loading: false,
      };
      case FIDECOMISO_SUCCESS:
        return {
          ...state,
          loading: true,
        };
    case UPDATE_FIDECOMISO_SUCCESS:
      return {
        ...state,
        fidecomisos: state.fidecomisos,
        loading: false,
      };
    case FIDEICOMISO_FAILURE:
        return {
          ...state,
          fidecomisos: null,
          loading: false,
        };
    case GET_EDUCATIONAL_ENTITIES_DETAIL:
      return {
        ...state,
        loading: true,
      };
    case GET_EDUCATIONAL_ENTITIES_DETAIL_FAILURE:
      return {
        ...state,
        detalleEntidadEducativa: {
          ...state.detalleEntidadEducativa,
          programas: []
        },
        loading: false,
      };
    case GET_EDUCATIONAL_ENTITIES_DETAIL_SUCCESS:
      return {
        ...state,
        detalleEntidadEducativa: action.detalleEntidadEducativa,
        loading: false,
      };
    case GET_FILES_EDUCATIONAL_ENTITIES:
      return {
        ...state,
        loading: true,
      };
    case GET_FILES_EDUCATIONAL_ENTITIES_FAILURE:
      return {
        ...state,
        archivos: [],
        loading: false,
      };
    case GET_FILES_EDUCATIONAL_ENTITIES_SUCCESS:
      return {
        ...state,
        archivos: action.archivos,
        loading: false,
      };
    case GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD:
      return {
        ...state,
        detalleEntidadEducativa: {
          ...state.detalleEntidadEducativa,
          programas: action.programaPorEntidad,
        }
      };
    default:
      return state;
  }
};