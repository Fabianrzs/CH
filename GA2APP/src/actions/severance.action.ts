import { Dispatch } from "react";
import SeveranceAdapter from "../adapters/severance.adapter";
import { CreditBasicDataModel } from "../models/credit-basic-data.model";
import { RequestSeverancetModel } from "../models/request-severance.model";
import { RequestRequestSeveranceModel } from "../models/requests/request-request-severance.model";
import SeveranceApi from "../services/api-severance.service";
import { AppActions } from "../types/appactions.type";
import {
    GETALL_POST_REQUEST_SEVERANCE,
    GET_REQUEST_SEVERANCE,
    CLEAR_DETAIL_REQUEST_SEVERANCE,
    ON_ERROR,
    GET_INFORMATION_BASIC_SEVERANCE,
    GETALL_POST_INFORMATION_BASIC_SEVERANCE
} from "../types/severance.actions.type";

const api = SeveranceApi();

export const SetError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});


export const clearDetailRequestSeverance = (): AppActions => ({
    type: CLEAR_DETAIL_REQUEST_SEVERANCE,
});

const GetRequestSeverance = (): AppActions => ({
    type: GET_REQUEST_SEVERANCE,
});

export const GetAllPostRequestSeverance = (requestSeverance: RequestSeverancetModel): AppActions => ({
    type: GETALL_POST_REQUEST_SEVERANCE,
    requestSeverance,
});

export const startGetAllPostRequestSeverance =
    (data: RequestRequestSeveranceModel) => async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetRequestSeverance());
            const response = await api.getAllRequestSeverance(data);
            if (response?.isSuccess === false) {
                dispatch(SetError(response));
            } else {
                const dataAdapted = SeveranceAdapter.adaptProcessRequestSeverance(
                    response.data,
                );
                dispatch(GetAllPostRequestSeverance(dataAdapted));
            }
        } catch (e) {
           
            dispatch(SetError('server-error'));
        }
    };



const GetInformaionBasicSeverance = (): AppActions => ({
    type: GET_INFORMATION_BASIC_SEVERANCE,
});

export const GetAllPostInformationBasicSeverance = (informationBasicSeverance: CreditBasicDataModel): AppActions => ({
    type: GETALL_POST_INFORMATION_BASIC_SEVERANCE,
    informationBasicSeverance,
});

export const startGetAllPostInformationBasicSeverance =
    (numeroIdentificacion: number) => async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetInformaionBasicSeverance());
            const response = await api.getAllInformationBasicServerance(numeroIdentificacion);
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllPostInformationBasicSeverance(response.data));
            }
        } catch (e) {
           
            dispatch(SetError('server-error'));
        }
    };


