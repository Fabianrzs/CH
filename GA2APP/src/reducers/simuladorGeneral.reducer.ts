import { SimuladorGeneralForm } from "../pages/generalParameterization/constants/simuladorGeneralForm.model.type";
import { ACTUALIZAR_SIMULADOR_GENERAL, 
         ACTUALIZAR_SIMULADOR_GENERAL_FAILURE,
         OBTENER_SIMULADOR_GENERAL, 
         OBTENER_SIMULADOR_GENERAL_FAILURE, 
         SimuladorGeneralActionTypes } from "../types/simuladorGeneral.action.type";
import { SimuladorGeneralDto } from "../types/simuladorGeneral.model.type";

export interface SimuladorGeneralState {
    simulador: SimuladorGeneralDto;
    error: any;
    loading: boolean;
}

const SimuladorGeneralStateInicial: SimuladorGeneralState = {
    simulador: SimuladorGeneralForm,
    error: null,
    loading: false,
}

export const SimuladorGeneralReducer = (
    state: SimuladorGeneralState = SimuladorGeneralStateInicial,
    action: SimuladorGeneralActionTypes
): SimuladorGeneralState => {
    switch (action.type){
        case OBTENER_SIMULADOR_GENERAL:
            return {
                ...state,
                simulador: action.simulador,
                loading: true
            };
        case OBTENER_SIMULADOR_GENERAL_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false
            };       
        case ACTUALIZAR_SIMULADOR_GENERAL:
            return {
                ...state,
                simulador: action.simulador,
                loading: true
            };
        case ACTUALIZAR_SIMULADOR_GENERAL_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false
            };
        default:           
            return state;  
    }

};