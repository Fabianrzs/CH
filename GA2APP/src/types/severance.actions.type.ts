
import { CreditBasicDataModel } from "../models/credit-basic-data.model";
import { RequestSeverancetModel } from "../models/request-severance.model";

export const ON_ERROR = 'ON_ERROR';
export const GETALL_POST_REQUEST_SEVERANCE = 'GETALL_POST_REQUEST_SEVERANCE';
export const GET_REQUEST_SEVERANCE = 'GET_REQUEST_SEVERANCE';
export const CLEAR_DETAIL_REQUEST_SEVERANCE = 'CLEAR_DETAIL_REQUEST_SEVERANCE';


export const GET_INFORMATION_BASIC_SEVERANCE = 'GET_INFORMATION_BASIC_SEVERANCE';
export const GETALL_POST_INFORMATION_BASIC_SEVERANCE = 'GETALL_POST_INFORMATION_BASIC_SEVERANCE';


export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export interface ClearDetailRequestSeverance {
    type: 'CLEAR_DETAIL_REQUEST_SEVERANCE';
}

export interface GetRequestSeverance {
    type: 'GET_REQUEST_SEVERANCE';
}

export interface GetAllPostRequestSeverance {
    type: 'GETALL_POST_REQUEST_SEVERANCE';
    requestSeverance: RequestSeverancetModel;
}

export interface GetInformationBasicSeverance {
    type: 'GET_INFORMATION_BASIC_SEVERANCE';
}

export interface GetAllPostInformationBasciSeverance {
    type: 'GETALL_POST_INFORMATION_BASIC_SEVERANCE';
    informationBasicSeverance: CreditBasicDataModel;
}

export type SeveranceActionsType =
    | GetAllPostRequestSeverance
    | GetRequestSeverance
    | ClearDetailRequestSeverance
    | GetInformationBasicSeverance
    | GetAllPostInformationBasciSeverance
    | OnError;
