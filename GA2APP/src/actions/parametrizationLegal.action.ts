import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import {
    GET_LEGAL_GMF_PARAMETRIZATION,
    GET_LEGAL_GMF_PARAMETRIZATION_FAILURE,
    GET_LEGAL_GMF_PARAMETRIZATION_SUCCESS,
    UPDATE_LEGAL_GMF_PARAMETRIZATION,
    UPDATE_LEGAL_GMF_PARAMETRIZATION_SUCCESS,
    UPDATE_LEGAL_GMF_PARAMETRIZATION_FAILURE,
    GET_LEGAL_RELIEF_PARAMETRIZATION,
    GET_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS,
    GET_LEGAL_RELIEF_PARAMETRIZATION_FAILURE,
    UPDATE_LEGAL_RELIEF_PARAMETRIZATION,
    UPDATE_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS,
    UPDATE_LEGAL_RELIEF_PARAMETRIZATION_FAILURE,
    GET_LEGAL_RATE_PARAMETRIZATION,
    GET_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    GET_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    UPDATE_LEGAL_RATE_PARAMETRIZATION,
    UPDATE_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    UPDATE_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    INSERT_LEGAL_RATE_PARAMETRIZATION,
    INSERT_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    INSERT_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    DELETE_LEGAL_RATE_PARAMETRIZATION,
    DELETE_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    DELETE_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    ON_ERROR,
} from '../types/parametrizationLegal.action.type';
import {
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
} from '../types/parametrizationLegal.model.type';
import ApiParametrizationLegal from '../services/api-parametrizationLegal.service';
import NotificationService from '../services/notification.service';

const api = ApiParametrizationLegal();

export const GetLegalReliefParametrization = (): AppActions => ({
    type: GET_LEGAL_RELIEF_PARAMETRIZATION,
});

export const GetLegalReliefParametrizationSuccess = (
    LegalRelief: LegalAlivioDto,
): AppActions => ({
    type: GET_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS,
    LegalRelief: LegalRelief,
});

export const GetLegalReliefParametrizationFailure = (
    error: any,
): AppActions => ({
    type: GET_LEGAL_RELIEF_PARAMETRIZATION_FAILURE,
    error: error,
});

export const UpdateLegalReliefParametrization = (): AppActions => ({
    type: UPDATE_LEGAL_RELIEF_PARAMETRIZATION,
});

export const UpdateLegalReliefParametrizationSuccess = (
    LegalRelief: LegalAlivioDto,
): AppActions => ({
    type: UPDATE_LEGAL_RELIEF_PARAMETRIZATION_SUCCESS,
    LegalRelief: LegalRelief,
});

export const UpdateLegalReliefParametrizationFailure = (
    error: any,
): AppActions => ({
    type: UPDATE_LEGAL_RELIEF_PARAMETRIZATION_FAILURE,
    error: error,
});

export const GetLegalRateParametrization = (): AppActions => ({
    type: GET_LEGAL_RATE_PARAMETRIZATION,
});

export const GetLegalRateParametrizationSuccess = (
    LegalRates: LegalTasaDto[],
): AppActions => ({
    type: GET_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    legalRates: LegalRates,
});

export const GetLegalRateParametrizationFailure = (error: any): AppActions => ({
    type: GET_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const UpdateLegalRateParametrization = (): AppActions => ({
    type: UPDATE_LEGAL_RATE_PARAMETRIZATION,
});

export const UpdateLegalRateParametrizationSuccess = (
    LegalRate: LegalTasaDto,
): AppActions => ({
    type: UPDATE_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    LegalRate: LegalRate,
});

export const UpdateLegalRateParametrizationFailure = (
    error: any,
): AppActions => ({
    type: UPDATE_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const InsertLegalRateParametrization = (): AppActions => ({
    type: INSERT_LEGAL_RATE_PARAMETRIZATION,
});

export const InsertLegalRateParametrizationSuccess = (
    LegalRate: LegalTasaDto,
): AppActions => ({
    type: INSERT_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    LegalRate: LegalRate,
});

export const InsertLegalRateParametrizationFailure = (
    error: any,
): AppActions => ({
    type: INSERT_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const DeleteLegalRateParametrization = (): AppActions => ({
    type: DELETE_LEGAL_RATE_PARAMETRIZATION,
});

export const DeleteLegalRateParametrizationSuccess = (
    LegalRate: LegalTasaDto,
): AppActions => ({
    type: DELETE_LEGAL_RATE_PARAMETRIZATION_SUCCESS,
    LegalRate: LegalRate,
});

export const DeleteLegalRateParametrizationFailure = (
    error: any,
): AppActions => ({
    type: DELETE_LEGAL_RATE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const GetLegalGmfParametrization = (): AppActions => ({
    type: GET_LEGAL_GMF_PARAMETRIZATION,
});

export const GetLegalGmfParametrizationSuccess = (
    LegalGmf: LegalGmfDto,
): AppActions => ({
    type: GET_LEGAL_GMF_PARAMETRIZATION_SUCCESS,
    LegalGmf: LegalGmf,
});

export const GetLegalGmfParametrizationFailure = (error: any): AppActions => ({
    type: GET_LEGAL_GMF_PARAMETRIZATION_FAILURE,
    error: error,
});

export const UpdateLegalGmfParametrization = (): AppActions => ({
    type: UPDATE_LEGAL_GMF_PARAMETRIZATION,
});

export const UpdateLegalGmfParametrizationSuccess = (
    LegalGmf: LegalGmfDto,
): AppActions => ({
    type: UPDATE_LEGAL_GMF_PARAMETRIZATION_SUCCESS,
    LegalGmf: LegalGmf,
});

export const UpdateLegalGmfParametrizationFailure = (
    error: any,
): AppActions => ({
    type: UPDATE_LEGAL_GMF_PARAMETRIZATION_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startGetLegalReliefParametrization =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetLegalReliefParametrization());
        const response = await api.GetLegalReliefParametrization();
        if (response?.isSuccess == null) {
            dispatch(GetLegalReliefParametrizationFailure(response));
        } else {
            dispatch(GetLegalReliefParametrizationSuccess(response.data));
        }
    };

export const startUpdateLegalReliefParametrization =
    (legalRelief: LegalAlivioDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateLegalReliefParametrization());
        const response = await api.UpdateLegalReliefParametrization(
            legalRelief,
        );

        if (response?.isSuccess == null) {
            dispatch(UpdateLegalReliefParametrizationFailure(response.data));
        } else {
            dispatch(UpdateLegalReliefParametrizationSuccess(response.data));
            toast.success('Información Actualizada');
        }
    };

export const startGetLegalRateParametrization =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetLegalRateParametrization());
        const response = await api.GetLegalRateParametrization();
        if (response?.isSuccess == null) {
            dispatch(GetLegalRateParametrizationFailure(response));
        } else {
            dispatch(GetLegalRateParametrizationSuccess(response.data));
        }
    };

export const startUpdateLegalRateParametrization =
    (legalRate: LegalTasaDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateLegalRateParametrization());
        const response = await api.UpdateLegalRateParametrization(legalRate);

        if (response?.isSuccess == null) {
            dispatch(UpdateLegalRateParametrizationFailure(response.data));
        } else {
            dispatch(UpdateLegalRateParametrizationSuccess(response.data));
        }
    };

export const startInsertLegalRateParametrization =
    (legalRate: LegalTasaDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(InsertLegalRateParametrization());
        const response = await api.InsertLegalRateParametrization(legalRate);

        if (response?.isSuccess == null) {
            dispatch(InsertLegalRateParametrizationFailure(response.data));
        } else {
            dispatch(InsertLegalRateParametrizationSuccess(response.data));
        }
    };

export const startDeleteLegalRateParametrization =
    (legalRate: LegalTasaDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(DeleteLegalRateParametrization());
        const response = await api.DeleteLegalRateParametrization(legalRate);

        if (response?.isSuccess == null) {
            dispatch(DeleteLegalRateParametrizationFailure(response.data));
        } else {
            dispatch(DeleteLegalRateParametrizationSuccess(response.data));
            await NotificationService.genericAlert(
                'Eliminado con éxito',
                'Se realizó con éxito la eliminación del registro.',
            );
        }
    };

export const startGetLegalGmfParametrization =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetLegalGmfParametrization());
        const response = await api.GetLegalGmfParametrization();
        if (response?.isSuccess == null) {
            dispatch(GetLegalGmfParametrizationFailure(response));
        } else {
            dispatch(GetLegalGmfParametrizationSuccess(response.data));
        }
    };

export const startUpdateLegalGmfParametrization =
    (legalGmf: LegalGmfDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateLegalGmfParametrization());
        const response = await api.UpdateLegalGmfParametrization(legalGmf);

        if (response?.isSuccess == null) {
            dispatch(UpdateLegalGmfParametrizationFailure(response.data));
        } else {
            dispatch(UpdateLegalGmfParametrizationSuccess(response.data));
            toast.success('Información Actualizada');
        }
    };
