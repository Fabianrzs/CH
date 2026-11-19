import { SimuladorGeneralDto } from "./simuladorGeneral.model.type";

export const OBTENER_SIMULADOR_GENERAL = 'OBTENER_SIMULADOR_GENERAL';
export const OBTENER_SIMULADOR_GENERAL_FAILURE = 'OBTENER_SIMULADOR_GENERAL_FAILURE';
export const ACTUALIZAR_SIMULADOR_GENERAL_SUCCESS = 'ACTUALIZAR_SIMULADOR_GENERAL_SUCCESS';
export const ACTUALIZAR_SIMULADOR_GENERAL = 'ACTUALIZAR_SIMULADOR_GENERAL';
export const ACTUALIZAR_SIMULADOR_GENERAL_FAILURE = 'ACTUALIZAR_SIMULADOR_GENERAL_FAILURE';

export interface ObtenerSimuladorGeneral {
    type: 'OBTENER_SIMULADOR_GENERAL'; 
    simulador: SimuladorGeneralDto;  
}

export interface ObtenerSimuladorGeneralFailure {
    type: 'OBTENER_SIMULADOR_GENERAL_FAILURE'; 
    error: any;   
}
export interface ActualizarSimuladorGeneral {
    type: 'ACTUALIZAR_SIMULADOR_GENERAL'; 
    simulador: SimuladorGeneralDto;  
}

export interface ActualizarSimuladorGeneralFailure {
    type: 'ACTUALIZAR_SIMULADOR_GENERAL_FAILURE'; 
    error: any;   
}

export type SimuladorGeneralActionTypes = 

    | ObtenerSimuladorGeneral
    | ObtenerSimuladorGeneralFailure
    | ActualizarSimuladorGeneral
    | ActualizarSimuladorGeneralFailure