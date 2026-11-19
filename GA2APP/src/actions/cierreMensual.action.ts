import { Dispatch } from "react";
import { toast } from "react-toastify";
import swal from 'sweetalert';
import ApiCierreMensual from "../services/api-cierremensual.service";
import { AppActions } from "../types/appactions.type";
import { GET_IPC } from "../types/catalog.actions.type";
import { IPC } from "../types/catalog.type";
import { CierreMensualModel, ListaCierreMensual } from "../types/cierre-mensual.model.type";
import {GET_CIERRE_MENSUAL_SUCCESS , GET_CIERRE_MENSUAL_FAILURE} from '../types/cierreMensual.actions.types';


const api=ApiCierreMensual();

export const GetIPCSuccess= (ipc: IPC): AppActions => ({
    type: GET_IPC,
    ipc:ipc,
});

export const GetListaCierreMensuallSuccess= (lista: ListaCierreMensual[]): AppActions => ({
    type: GET_CIERRE_MENSUAL_SUCCESS,
    CierreMensual:lista,
});

export const GetListaCierreMensualFailure = (error: any): AppActions => ({
    type: GET_CIERRE_MENSUAL_FAILURE,
    error: error,
});



export const startSetCierreMensual = (data: CierreMensualModel) => async (dispatch: Dispatch<AppActions>) => {

    const response = await api.setCierreMensual(data);
    if (response?.isSuccess !== null ) {
        if(response?.isSuccess){ 
            swal({
                text: 'El cierre para el mes de abril del 2022 se está procesando.',
                icon: 'success',
            });           
        }else{
            toast(response.returnMessage);
        }
    } else {
        toast('Error: respuesta errada del servidor');
    } 

};

export const startgetListaProgramacionCierre = () => async (dispatch: Dispatch<AppActions>) => {

    const response = await api.getListaProgramacionCierre();
    if (response?.isSuccess !== null ) {
        if(response?.isSuccess){        
         dispatch(GetListaCierreMensuallSuccess(response.data)); 
        }else{
            dispatch(GetListaCierreMensualFailure(response.returnMessage)); 
            toast(response.returnMessage);
        }
    } else {
        toast('Error: respuesta errada del servidor');
        dispatch(GetListaCierreMensualFailure("Error: respuesta errada del servidor")); 
    } 

};


export const startgetIPC = () => async (dispatch: Dispatch<AppActions>) => {
    const response = await api.getIPC();  
    if (response?.isSuccess !== null ) {
        if(response?.isSuccess){   
            dispatch(GetIPCSuccess(response.data));     
        }else{
            toast(response.returnMessage);
        }
    } else {
        toast('Error: respuesta errada del servidor');
    } 
};