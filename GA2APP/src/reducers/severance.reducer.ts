import CreditAdapter from '../adapters/credit.adapter';
import { RequestSeverancetModel } from '../models/request-severance.model';
import { ResponseSeveranceModel } from '../models/requests/response-severance.model';
import {
    ON_ERROR
} from '../types/credit.actions.type';
import {
    CLEAR_DETAIL_REQUEST_SEVERANCE,
    GETALL_POST_INFORMATION_BASIC_SEVERANCE,
    GETALL_POST_REQUEST_SEVERANCE,
    GET_INFORMATION_BASIC_SEVERANCE,
    GET_REQUEST_SEVERANCE,
    SeveranceActionsType } from '../types/severance.actions.type';

export interface SeveranceState {
    loading: boolean;
    error: any;
    responseSeveranceModel: ResponseSeveranceModel | any
    requestSeverance: RequestSeverancetModel;
    informationBasicSeverance: any
}


const DetailInitialState = {
    error: null,
    loading: false,
    requestSeverance: CreditAdapter.adaptProcessRequestSeverance({}),
    informationBasicSeverance: null,
    responseSeveranceModel: null
};

const InitialState: SeveranceState = {
    ...DetailInitialState,
};

const SeveranceReducer = (
    state: SeveranceState = InitialState,
    action: SeveranceActionsType,
): SeveranceState => {
    switch (action.type) {
        case ON_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case CLEAR_DETAIL_REQUEST_SEVERANCE:
            return {
                ...state,
                ...DetailInitialState,
            };
        case GET_REQUEST_SEVERANCE:
            return {
                ...state,
                loading: true,
                error: null
            };
        case GETALL_POST_REQUEST_SEVERANCE:
            return {
                ...state,
                loading: false,
                requestSeverance: action.requestSeverance,
            };
        case GET_INFORMATION_BASIC_SEVERANCE:
            return {
                ...state,
                loading: true,
                error: null
            };
        case GETALL_POST_INFORMATION_BASIC_SEVERANCE:
            return {
                ...state,
                loading: false,
                informationBasicSeverance: action.informationBasicSeverance,
            };
        default:
            return state;
    }
};

export default SeveranceReducer;
