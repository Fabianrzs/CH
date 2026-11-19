import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import ApiAddress from '../components/modals/address/services/address.services';
import { OBTENER_DIRECCIONES_ABECEDARIO, OBTENER_DIRECCIONES_ABECEDARIO_SUCCESS, OBTENER_DIRECCIONES_CARDINALES, OBTENER_DIRECCIONES_CARDINALES_SUCCESS, OBTENER_DIRECCIONES_TIPOS, OBTENER_DIRECCIONES_TIPOS_SUCCESS, OBTENER_DIRECCIONES_BIS, OBTENER_DIRECCIONES_BIS_SUCCESS } from '../types/address.actions.type';
import {
    DireccionAbecedario,
    DireccionBis,
    DireccionCardinal,
    DireccionTipo,
} from '../types/address.type';
import { AppActions } from '../types/appactions.type';
import { ON_ERROR } from '../types/client.actions.types';

const api = ApiAddress();

export const ObtenerDireccionAbecedarios = (): AppActions => ({
    type: OBTENER_DIRECCIONES_ABECEDARIO,
});

export const ObtenerDireccionAbecedariosSuccess = (
    direccionesAbecedarios: DireccionAbecedario[],
): AppActions => ({
    type: OBTENER_DIRECCIONES_ABECEDARIO_SUCCESS,
    direccionesAbecedarios,
});

export const ObtenerDireccionCardinales = (): AppActions => ({
    type: OBTENER_DIRECCIONES_CARDINALES,
});

export const ObtenerDireccionCardinalesSuccess = (
    direccionesCardinales: DireccionCardinal[],
): AppActions => ({
    type: OBTENER_DIRECCIONES_CARDINALES_SUCCESS,
    direccionesCardinales,
});

export const ObtenerDireccionesTipos = (): AppActions => ({
    type: OBTENER_DIRECCIONES_TIPOS,
});

export const ObtenerDireccionesTiposSuccess = (
    direccionesTipos: DireccionTipo[],
): AppActions => ({
    type: OBTENER_DIRECCIONES_TIPOS_SUCCESS,
    direccionesTipos,
});

export const OnError = (error: Error): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startObtenerDireccionesTipos = (props: string): any => async (
    dispatch: Dispatch<AppActions>,
) => {
    try {
        dispatch(ObtenerDireccionesTipos());
        const response = await api.ObtenerDireccionesTiposRequest();
        if (!response?.isSuccess) {
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(ObtenerDireccionesTiposSuccess(response.data));
        }
    } catch (error) {
        dispatch(OnError(error as Error))
        toast('Error: respuesta errada del servidor');
    }
};

export const startObtenerDireccionesCardinales = (props: string): any => async (
    dispatch: Dispatch<AppActions>,
) => {
    try {
        dispatch(ObtenerDireccionesTipos());
        const response = await api.ObtenerDireccionesCardinalRequest();
        if (!response?.isSuccess) {
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(ObtenerDireccionCardinalesSuccess(response.data));
        }
    } catch (error) {
        dispatch(OnError(error as Error))
        toast('Error: respuesta errada del servidor');
    }
};

export const startObtenerDireccionesAbecedarios = (props: string): any => async (
    dispatch: Dispatch<AppActions>,
) => {
    try {
        dispatch(ObtenerDireccionesTipos());
        const response = await api.ObtenerDireccionesAbecedariosRequest();
        if (!response?.isSuccess) {
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(ObtenerDireccionAbecedariosSuccess(response.data));
        }
    } catch (error) {
        dispatch(OnError(error as Error))
        toast('Error: respuesta errada del servidor');
    }
};

export const ObtenerDireccionBis = (): AppActions => ({
    type: OBTENER_DIRECCIONES_BIS,
});

export const ObtenerDireccionBisSuccess = (
    direccionesBis: DireccionBis[],
): AppActions => ({
    type: OBTENER_DIRECCIONES_BIS_SUCCESS,
    direccionesBis,
});

export const startObtenerDireccionesBis = (props: string): any => async (
    dispatch: Dispatch<AppActions>,
) => {
    try {
        dispatch(ObtenerDireccionesTipos());
        const response = await api.ObtenerDireccionesBisRequest();
        if (!response?.isSuccess) {
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(ObtenerDireccionBisSuccess(response.data));
        }
    } catch (error) {
        dispatch(OnError(error as Error))
        toast('Error: respuesta errada del servidor');
    }
};
