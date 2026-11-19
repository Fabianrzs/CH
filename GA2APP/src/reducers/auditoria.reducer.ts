import { GET_TABLAS_AUDITORIA_SUCCESS,GET_TABLAS_AUDITORIA_FAILURE,GET_LOG_AUDITORIA_SUCCESS,GET_LOG_AUDITORIA_FAILURE,GET_LOG_AUDITORIA_DETALLE ,AuditoriaActionsTypes  } from '../types/auditoria.actions.type'
import { Auditoria, LogAuditorias, TablasAuditoria } from '../types/Auditoria.model.type';

export interface AuditoriaState {

  tablas:TablasAuditoria[];
  logs:LogAuditorias;
  detalle:Auditoria;
  error: any;
  loading: boolean;

}

const InitialState: AuditoriaState = {
    tablas:[],
    logs:{} as LogAuditorias,
    detalle:{} as Auditoria,
    error: null,
    loading: false,
}


export const AuditoriaReducer = (
    state: AuditoriaState=InitialState,
    action: AuditoriaActionsTypes,
): AuditoriaState => {
    switch (action.type) {

        case GET_TABLAS_AUDITORIA_SUCCESS:
            return {
                ...state,
                tablas: action.Tablas,
                loading: false,
 
            };
        
          case GET_LOG_AUDITORIA_DETALLE:
            return {
                ...state,
                detalle: action.Detalle,
            };
           

        case GET_TABLAS_AUDITORIA_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
                
            };

        case GET_LOG_AUDITORIA_SUCCESS:
            return {
                ...state,
                logs: action.Log,
                loading: false,
            };

            case GET_LOG_AUDITORIA_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: true,
            };
        default:
            return state;
    }
};