import { ListaCierreMensual } from '../types/cierre-mensual.model.type';
import {CierreMensualActionTypes, GET_CIERRE_MENSUAL_FAILURE,GET_CIERRE_MENSUAL_SUCCESS } from '../types/cierreMensual.actions.types';


export interface CierreMensualState {
    lista: ListaCierreMensual[];
    error: any;
    loading: boolean;
}

const CierreMensualStateInitial: CierreMensualState= { 
    lista: [],
    error: null,
    loading: false,  
};


export const CierreMensualReducer = (
    state: CierreMensualState = CierreMensualStateInitial,
    action: CierreMensualActionTypes,
): CierreMensualState => {
    switch (action.type) {

        case GET_CIERRE_MENSUAL_SUCCESS:
            return {
                ...state,
                lista: action.CierreMensual,
                loading: false,
            };

            case GET_CIERRE_MENSUAL_FAILURE:
                return {
                    ...state,
                    error: action.error,
                    loading: false,
                };

        default:
            return state;
    }
};


