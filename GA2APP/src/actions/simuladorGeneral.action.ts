import { Dispatch } from 'redux';
import { toast } from 'react-toastify';
import ApiSimuladorGeneral from '../services/api-simuladorGeneral.service';
import { AppActions } from "../types/appactions.type";
import { ACTUALIZAR_SIMULADOR_GENERAL, 
         ACTUALIZAR_SIMULADOR_GENERAL_FAILURE,
         OBTENER_SIMULADOR_GENERAL, 
         OBTENER_SIMULADOR_GENERAL_FAILURE } from "../types/simuladorGeneral.action.type";
import { SimuladorGeneralDto } from "../types/simuladorGeneral.model.type";

const api = ApiSimuladorGeneral ();

export const GetSimuladorGeneral = (
    simulador: SimuladorGeneralDto,    
): AppActions => ({
    type: OBTENER_SIMULADOR_GENERAL,
    simulador: simulador
})

export const GetSimuladorGeneralFailure = (
    error: any
): AppActions => ({
    type: OBTENER_SIMULADOR_GENERAL_FAILURE,
    error: error,
});

export const PostUpdateSimuladorGeneral = (
    simulador: SimuladorGeneralDto,    
): AppActions => ({
    type: ACTUALIZAR_SIMULADOR_GENERAL,
    simulador: simulador
})

export const PostUpdateSimuladorGeneralFailure = (
    error: any,    
): AppActions => ({
    type: ACTUALIZAR_SIMULADOR_GENERAL_FAILURE,
    error: error
})

export const startGetSimuladorGeneral = () => async (dispatch: Dispatch<AppActions>) => {   
    const response = await api.getParametrosGenerales();
    if (response?.isSuccess == null) {
        dispatch(PostUpdateSimuladorGeneralFailure(response.data));
    } else {
        dispatch(GetSimuladorGeneral(response.data));
        toast(response.returnMessage);
    }
};

export const startPostUpdateSimuladorGeneral =
    (simulador: SimuladorGeneralDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postUpdateParametrosGenerales(simulador);
        if (response?.isSuccess == null) {
            dispatch(PostUpdateSimuladorGeneralFailure(response.data));
        } else {
            dispatch(PostUpdateSimuladorGeneral(response.data)); 
            toast(response.returnMessage);
        }
    };