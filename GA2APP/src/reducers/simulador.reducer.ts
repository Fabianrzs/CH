import { SimuladorDto, 
         SimuladorActorDto,
         SimuladorUndEjDto,
         TipoActorDto,
         UnidadEjecutoraDto } 
        from "../types/simulador.model.type";
import { ACTUALIZAR_SIMULADOR, 
         ACTUALIZAR_SIMULADOR_ACTOR,
         ACTUALIZAR_SIMULADOR_UNIEJ,
         CREAR_SIMULADOR, 
         CREAR_SIMULADOR_ACTOR, 
         CREAR_SIMULADOR_UNIEJ, 
         DELETE_SIMULADOR, 
         DELETE_SIMULADOR_ACTOR, 
         DELETE_SIMULADOR_UNIEJ, 
         ELIMINAR_SIMULADOR, 
         ELIMINAR_SIMULADOR_ACTOR, 
         ELIMINAR_SIMULADOR_UNIEJ, 
         GET_SIMULADOR, 
         GET_SIMULADOR_ACTOR, 
         GET_SIMULADOR_ACTOR_FAILURE, 
         GET_SIMULADOR_FAILURE, 
         GET_SIMULADOR_UNIEJ, 
         GET_SIMULADOR_UNIEJ_FAILURE, 
         GET_TIPO_ACTOR, 
         GET_TIPO_ACTOR_FAILURE, 
         GET_UNIDAD_EJECUTORA, 
         GET_UNIDAD_EJECUTORA_FAILURE, 
         INSERT_SIMULADOR, 
         INSERT_SIMULADOR_ACTOR, 
         INSERT_SIMULADOR_UNIEJ, 
         OBTENER_SIMULADOR, 
         OBTENER_SIMULADOR_ACTOR, 
         OBTENER_SIMULADOR_UNIEJ, 
         OBTENER_TIPO_ACTOR, 
         OBTENER_UNIDAD_EJECUTORA, 
         SimuladorActionTypes, 
         UPDATE_SIMULADOR,
         UPDATE_SIMULADOR_ACTOR,
         UPDATE_SIMULADOR_ACTOR_FAILURE,
         UPDATE_SIMULADOR_FAILURE,
         UPDATE_SIMULADOR_UNIEJ,
         UPDATE_SIMULADOR_UNIEJ_FAILURE} from "../types/simulador.action.type";
import { SimuladorActorForm, 
         SimuladorForm, 
         SimuladorUndEjForm } from "../pages/parametrizationMortgageCredit/constans/simuladorform.model.type";
         
export interface SimuladorState {
    simulador: SimuladorDto;
    simuladorGeneral:  SimuladorDto [];
    simuladorActor: SimuladorActorDto;
    simuladorActores: SimuladorActorDto [];
    simuladorUniej: SimuladorUndEjDto;
    simuladorUniEjecutoras: SimuladorUndEjDto [];
    simuladorTipoActor: TipoActorDto [];
    simuladorunidadEjecutora: UnidadEjecutoraDto [];
    error: any;
    loading: boolean;
}

const SimuladorStateInicial: SimuladorState = {
    simulador: SimuladorForm,
    simuladorGeneral: [],    
    simuladorActor: SimuladorActorForm,
    simuladorActores: [],
    simuladorUniej: SimuladorUndEjForm,
    simuladorUniEjecutoras: [],
    simuladorTipoActor: [],
    simuladorunidadEjecutora: [],  
    error: null,  
    loading: false,
};

export const SimuladorReducer = (    
    state: SimuladorState = SimuladorStateInicial,
    action: SimuladorActionTypes,
): SimuladorState => {
    switch (action.type){
        case GET_SIMULADOR: 
        return {
            ...state,
            loading: true,
        };
        case GET_SIMULADOR_FAILURE:
        return {
            ...state,
            loading: false,
            error: action.error,
        };
        case UPDATE_SIMULADOR: 
        return {
            ...state,
            loading: true,
        };
        case UPDATE_SIMULADOR_FAILURE: 
        return {
            ...state,
                loading: false,
                error: action.error,
        };
        case INSERT_SIMULADOR: 
        return {
            ...state,
            loading: true,
        };
        case DELETE_SIMULADOR: 
        return {
            ...state,
            loading: true,
        };
        case CREAR_SIMULADOR: 
            return {
                ...state,
                simulador: action.simulador,
                loading: false,
            };
        case ACTUALIZAR_SIMULADOR:
            return {
                ...state,
                simulador: action.simulador,
                loading: false, 
            };
        case OBTENER_SIMULADOR:            
            return {
                ...state,
                simuladorGeneral: action.simuladorGeneral,
                loading: false,
            }          
        case ELIMINAR_SIMULADOR:
            return {
                ...state,
                simulador: action.simulador,
                loading: false,
            };
        case GET_SIMULADOR_ACTOR: 
        return {
            ...state,
            loading: true,
        };
        case GET_SIMULADOR_ACTOR_FAILURE:
        return {
            ...state,
            loading: false,
            error: action.error,
        };
        case UPDATE_SIMULADOR_ACTOR: 
            return {
                ...state,
                loading: true,
            };
        case UPDATE_SIMULADOR_ACTOR_FAILURE: 
            return {
                ...state,
                    loading: false,
                    error: action.error,
            };
        case INSERT_SIMULADOR_ACTOR: 
        return {
            ...state,
            loading: true,
        };
        case DELETE_SIMULADOR_ACTOR: 
        return {
            ...state,
            loading: true,
        };
        case CREAR_SIMULADOR_ACTOR: 
            return {
                ...state,
                simuladorActor: action.simuladorActor,
            };
        case ACTUALIZAR_SIMULADOR_ACTOR: 
            return {
                ...state,
                simuladorActor: action.simuladorActor,
            };
        case OBTENER_SIMULADOR_ACTOR: 
            return {
                ...state,
                simuladorActores: action.simuladorActores,
                loading: false
            };
        case ELIMINAR_SIMULADOR_ACTOR: 
            return {
                ...state,
                simuladorActor: action.simuladorActor,
            };
        case GET_SIMULADOR_UNIEJ: 
            return {
                ...state,
                loading: true,
            };
        case GET_SIMULADOR_UNIEJ_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case UPDATE_SIMULADOR_UNIEJ: 
            return {
                ...state,
                loading: true,
            };
        case UPDATE_SIMULADOR_UNIEJ_FAILURE: 
            return {
                ...state,
                    loading: false,
                    error: action.error,
            };
        case INSERT_SIMULADOR_UNIEJ: 
            return {
                ...state,
                loading: true,
            };
        case DELETE_SIMULADOR_UNIEJ: 
            return {
                ...state,
                loading: true,
            };
        case CREAR_SIMULADOR_UNIEJ: 
            return {
                ...state,
                simuladorUniej: action.simuladorUniej,
            };
        case ACTUALIZAR_SIMULADOR_UNIEJ: 
            return {
                ...state,
                simuladorUniej: action.simuladorUniej,
            };
        case OBTENER_SIMULADOR_UNIEJ: 
            return {
                ...state,
                simuladorUniEjecutoras: action.simuladorUniEjecutoras,
                loading: false,
            };
        case ELIMINAR_SIMULADOR_UNIEJ: 
            return {
                ...state,
                simuladorUniej: action.simuladorUniej,
            }; 
        case GET_TIPO_ACTOR: 
            return {
                ...state,
                loading: true,
            };       
        case OBTENER_TIPO_ACTOR: 
            return {
                ...state,
                simuladorTipoActor: action.simuladorTipoActor,
                loading: false,
            };  
        case GET_TIPO_ACTOR_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };   
        case GET_UNIDAD_EJECUTORA: 
        return {
            ...state,
            loading: true,
        };     
        case OBTENER_UNIDAD_EJECUTORA: 
            return {
                ...state,
                simuladorunidadEjecutora: action.simuladorunidadEjecutora, 
                loading: false,               
            };   
        case GET_UNIDAD_EJECUTORA_FAILURE:
        return {
            ...state,
            loading: false,
            error: action.error,
        };        
        default:           
            return state;  
    }
};