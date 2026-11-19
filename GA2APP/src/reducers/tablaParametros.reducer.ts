import { DataEditForm } from '../pages/TablasParametros/constants/tablaParametroform.model.type';
import {
    DataActionTypes,
    GET_DATA,
    GET_DATA_SUCCESS,
    GET_DATA_FAILURE,
    UPDATE_DATA,
    UPDATE_DATA_SUCCESS,
    UPDATE_DATA_FAILURE,
    INSERT_DATA,
    INSERT_DATA_SUCCESS,
    INSERT_DATA_FAILURE,
    DELETE_DATA,
    DELETE_DATA_SUCCESS,
    DELETE_DATA_FAILURE,
    ON_ERROR,
} from '../types/tablaParametro.actions.type';
import { DataEdit } from '../types/tablaParametro.model.type';

export interface DataData {
    id: number;
    alldata: DataEdit[];
    data: DataEdit;
    error: any;
    loading: boolean;
}

const DataDataInitial: DataData = {
    id: 0,
    alldata: [],
    data: DataEditForm,
    error: null,
    loading: false,
};

export const DataReducer = (
    data: DataData = DataDataInitial,
    action: DataActionTypes,
): DataData => {
    switch (action.type) {
        case GET_DATA:
            return {
                ...data,
                loading: true,
            };
        case GET_DATA_SUCCESS:
            return {
                ...data,
                alldata: action.alldata,
                loading: false,
            };
        case GET_DATA_FAILURE:
            return {
                ...data,
                loading: false,
                error: action.error,
            };
        case UPDATE_DATA:
            return {
                ...data,
                loading: true,
            };
        case UPDATE_DATA_SUCCESS:
            return {
                ...data,
                data: action.data,
                loading: false,
            };
        case UPDATE_DATA_FAILURE:
            return {
                ...data,
                loading: false,
                error: action.error,
            };
        case INSERT_DATA:
            return {
                ...data,
                loading: true,
            };
        case INSERT_DATA_SUCCESS:
            return {
                ...data,
                data: action.data,
                loading: false,
            };
        case INSERT_DATA_FAILURE:
            return {
                ...data,
                loading: false,
                error: action.error,
            };
        case DELETE_DATA:
            return {
                ...data,
                loading: true,
            };
        case DELETE_DATA_SUCCESS:
            return {
                ...data,
                data: action.data,
                loading: false,
            };
        case DELETE_DATA_FAILURE:
            return {
                ...data,
                loading: false,
                error: action.error,
            };
        case ON_ERROR:
            return {
                ...data,
                error: action.error,
                loading: false,
            };
        default:
            return data;
    }
};