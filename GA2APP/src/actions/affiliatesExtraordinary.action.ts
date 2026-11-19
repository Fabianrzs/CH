import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import ApiTypeAffiliate from '../services/api-typeAffiliate.service';
import { AppActions } from '../types/appactions.type';
import {
    CHECK_AFFILIATE_EXISTS,
    CHECK_AFFILIATE_EXISTS_FAILURE,
    CHECK_AFFILIATE_EXISTS_SUCCESS,
    DELETE_AFFILIATE,
    DELETE_AFFILIATE_FAILURE,
    DELETE_AFFILIATE_SUCCESS,
    GET_AFFILIATE_EXTRAORDINARY_DETAIL,
    GET_AFFILIATE_EXTRAORDINARY_DETAIL_FAILURE,
    GET_AFFILIATE_EXTRAORDINARY_DETAIL_SUCCESS,
    GET_EXTRAORDINARY_AFFILIATE, GET_EXTRAORDINARY_AFFILIATE_FAILURE,
    GET_EXTRAORDINARY_AFFILIATE_SUCCESS,
    POST_NEW_EXTRAORDINARY_AFFILIATE,
    POST_NEW_EXTRAORDINARY_AFFILIATE_FAILURE,
    POST_NEW_EXTRAORDINARY_AFFILIATE_SUCCESS,
    POST_UPDATE_EXTRAORDINARY_AFFILIATE,
    POST_UPDATE_EXTRAORDINARY_AFFILIATE_FAILURE,
    POST_UPDATE_EXTRAORDINARY_AFFILIATE_SUCCESS
} from '../types/typeAffiliate.action.type';
import { AfiliacionesExtraordinariasDTO, AfiliacionesExtraordinariasDTOInicial, ConsultaAfiliacionesRequestDTO } from '../types/typeAffiliate.type';
import history from '../utils/history.router';

const api = ApiTypeAffiliate();

export const GetAffiliateExtraordinary = (): AppActions => ({
    type: GET_EXTRAORDINARY_AFFILIATE,
});

export const GetAffiliateExtraordinarySuccess = (afiliacionExtraordinaria: AfiliacionesExtraordinariasDTO[]): AppActions => ({
    type: GET_EXTRAORDINARY_AFFILIATE_SUCCESS,
    afiliacionExtraordinaria: afiliacionExtraordinaria,
});

export const GetAffiliateExtraordinaryFailure = (afiliacionExtraordinaria: AfiliacionesExtraordinariasDTO[] | null): AppActions => ({
    type: GET_EXTRAORDINARY_AFFILIATE_FAILURE,
    afiliacionExtraordinaria: afiliacionExtraordinaria,
});


export const startGetExtraordinaryAffiliate = (afiliacionExtraordinaria: ConsultaAfiliacionesRequestDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetAffiliateExtraordinary());
            const response =
                await api.GetAffiliateExtraordinary(afiliacionExtraordinaria);
            if (!response?.isSuccess) {
                dispatch(
                    GetAffiliateExtraordinaryFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    GetAffiliateExtraordinarySuccess(
                        response.data,
                    ),
                );
                if (response.data == null) {
                    toast('No se encontró una nueva afiliación.');
                }
            }
        } catch (e) {
         
            dispatch(
                GetAffiliateExtraordinaryFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const PostNewExtraordinaryAffiliate = (): AppActions => ({
    type: POST_NEW_EXTRAORDINARY_AFFILIATE,
});

export const PostNewExtraordinaryAffiliateSuccess = (insertarNuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO): AppActions => ({
    type: POST_NEW_EXTRAORDINARY_AFFILIATE_SUCCESS,
    insertarNuevaAfiliacionExtraordinaria: insertarNuevaAfiliacionExtraordinaria,
});

export const PostNewExtraordinaryAffiliateFailure = (insertarNuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO): AppActions => ({
    type: POST_NEW_EXTRAORDINARY_AFFILIATE_FAILURE,
    insertarNuevaAfiliacionExtraordinaria: insertarNuevaAfiliacionExtraordinaria,
});

export const startPostNewExtraordinaryAffiliate = (insertarNuevaAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(PostNewExtraordinaryAffiliate());
            const response =
                await api.PostNewExtraordinaryAffiliate(insertarNuevaAfiliacionExtraordinaria);
            if (!response?.isSuccess) {
                dispatch(
                    PostNewExtraordinaryAffiliateFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    PostNewExtraordinaryAffiliateSuccess(
                        response.data,
                    ),
                );
                if (response.data == null) {
                    toast('Afiliación agregada correctamente.');
                } else {
                    toast('Registro insertado correctamente.');
                }
            }
        } catch (e) {
         
            dispatch(
                PostNewExtraordinaryAffiliateFailure(AfiliacionesExtraordinariasDTOInicial),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const PostUpdateExtraordinaryAffiliate = (): AppActions => ({
    type: POST_UPDATE_EXTRAORDINARY_AFFILIATE,
});

export const PostUpdateExtraordinaryAffiliateSuccess = (actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO): AppActions => ({
    type: POST_UPDATE_EXTRAORDINARY_AFFILIATE_SUCCESS,
    actualizarAfiliacionExtraordinaria: actualizarAfiliacionExtraordinaria,
});

export const PostUpdateExtraordinaryAffiliateFailure = (actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO): AppActions => ({
    type: POST_UPDATE_EXTRAORDINARY_AFFILIATE_FAILURE,
    actualizarAfiliacionExtraordinaria: actualizarAfiliacionExtraordinaria,
});

export const startPostUpdateExtraordinaryAffiliate = (actualizarAfiliacionExtraordinaria: AfiliacionesExtraordinariasDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(PostUpdateExtraordinaryAffiliate());
            const response =
                await api.PostUpdateExtraordinaryAffiliate(actualizarAfiliacionExtraordinaria);
            if (!response?.isSuccess) {
                dispatch(
                    PostUpdateExtraordinaryAffiliateFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    PostUpdateExtraordinaryAffiliateSuccess(
                        response.data,
                    ),
                );
                if (response.data == null) {
                    toast('No se actualizó correctamente la afiliación.');
                } else {
                    toast('Registro actualizado correctamente.');
                }
            }
        } catch (e) {
          
            dispatch(
                PostUpdateExtraordinaryAffiliateFailure(AfiliacionesExtraordinariasDTOInicial),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const GetAffiliateExtraordinaryDetail = (): AppActions => ({
    type: GET_AFFILIATE_EXTRAORDINARY_DETAIL,
});

export const GetAffiliateExtraordinaryDetailSuccess = (detalleAfiliacion: AfiliacionesExtraordinariasDTO): AppActions => ({
    type: GET_AFFILIATE_EXTRAORDINARY_DETAIL_SUCCESS,
    detalleAfiliacion: detalleAfiliacion,
});

export const GetAffiliateExtraordinaryDetailFailure = (detalleAfiliacion: AfiliacionesExtraordinariasDTO | null): AppActions => ({
    type: GET_AFFILIATE_EXTRAORDINARY_DETAIL_FAILURE,
    detalleAfiliacion: detalleAfiliacion,
});

export const startGetAffiliateExtraordinaryDetail = (detalleAfiliacion: ConsultaAfiliacionesRequestDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(GetAffiliateExtraordinaryDetail());
            const response =
                await api.GetAffiliateExtraordinary(detalleAfiliacion);
            if (!response?.isSuccess) {
                dispatch(
                    GetAffiliateExtraordinaryDetailFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    GetAffiliateExtraordinaryDetailSuccess(
                        response.data[0],
                    ),
                );
                if (response.data == null) {
                    toast('No se actualizó correctamente la afiliación.');
                } else {
                    history.push('/reintegro');
                }
            }
        } catch (e) {
          
            dispatch(
                GetAffiliateExtraordinaryDetailFailure(AfiliacionesExtraordinariasDTOInicial),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const CheckAffiliateExists = (): AppActions => ({
    type: CHECK_AFFILIATE_EXISTS,
});

export const CheckAffiliateExistsSuccess = (afiliacionExiste: boolean): AppActions => ({
    type: CHECK_AFFILIATE_EXISTS_SUCCESS,
    AfiliacionExiste: afiliacionExiste,
});

export const CheckAffiliateExistsFailure = (afiliacionExiste: boolean | null): AppActions => ({
    type: CHECK_AFFILIATE_EXISTS_FAILURE,
    AfiliacionExiste: afiliacionExiste,
});

export const startCheckAffiliateExists = (afiliacionExiste: ConsultaAfiliacionesRequestDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(CheckAffiliateExists());
            const response =
                await api.GetAffiliateExtraordinary(afiliacionExiste);
            if (!response?.isSuccess) {
                dispatch(
                    CheckAffiliateExistsFailure(false),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    CheckAffiliateExistsSuccess(
                        response.data.length > 0,
                    ),
                );
                if (response.data == null) {
                    toast('No se actualizó correctamente la afiliación.');
                } else {
                    toast('Registro actualizado correctamente.');
                }
            }
        } catch (e) {
          
            dispatch(
                GetAffiliateExtraordinaryDetailFailure(AfiliacionesExtraordinariasDTOInicial),
            );
            toast('Error: respuesta errada del servidor');
        }
    };

export const DeleteAffiliate = (): AppActions => ({
    type: DELETE_AFFILIATE,
});

export const DeleteAffiliateSuccess = (eliminarAfiliacion: AfiliacionesExtraordinariasDTO): AppActions => ({
    type: DELETE_AFFILIATE_SUCCESS,
    eliminarAfiliacion: eliminarAfiliacion,
});

export const DeleteAffiliateFailure = (eliminarAfiliacion: AfiliacionesExtraordinariasDTO | null): AppActions => ({
    type: DELETE_AFFILIATE_FAILURE,
    eliminarAfiliacion: eliminarAfiliacion,
});


export const startDeleteAffiliate = (eliminarAfiliacion: AfiliacionesExtraordinariasDTO) =>
    async (dispatch: Dispatch<AppActions>) => {
        try {
            dispatch(DeleteAffiliate());
            const response =
                await api.DeleteAffiliate(eliminarAfiliacion);
            if (!response?.isSuccess) {
                dispatch(
                    DeleteAffiliateFailure(response.data),
                );
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(
                    DeleteAffiliateSuccess(
                        response.data,
                    ),
                );
                if (response.data == null) {
                    toast('No se pudo eliminar la afiliación.');
                }
            }
        } catch (e) {
          
            dispatch(
                DeleteAffiliateFailure(null),
            );
            toast('Error: respuesta errada del servidor');
        }
    };
