import { Dispatch } from 'react';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import {
    GET_INSURANCE_PARAMETRIZATION,
    GET_INSURANCE_PARAMETRIZATION_SUCCESS,
    GET_INSURANCE_PARAMETRIZATION_FAILURE,
    GET_RATE_PARAMETRIZATION,
    GET_RATE_PARAMETRIZATION_SUCCESS,
    GET_RATE_PARAMETRIZATION_FAILURE,
    UPDATE_RATE_PARAMETRIZATION,
    UPDATE_RATE_PARAMETRIZATION_SUCCESS,
    UPDATE_RATE_PARAMETRIZATION_FAILURE,
    UPDATE_INSURANCE_PARAMETRIZATION,
    UPDATE_INSURANCE_PARAMETRIZATION_SUCCESS,
    UPDATE_INSURANCE_PARAMETRIZATION_FAILURE,
    GET_SETTLEMENT_PARAMETRIZATION,
    GET_SETTLEMENT_PARAMETRIZATION_SUCCESS,
    GET_SETTLEMENT_PARAMETRIZATION_FAILURE,
    UPDATE_SETTLEMENT_PARAMETRIZATION,
    UPDATE_SETTLEMENT_PARAMETRIZATION_SUCCESS,
    UPDATE_SETTLEMENT_PARAMETRIZATION_FAILURE,
    GET_TERM_PARAMETRIZATION,
    GET_TERM_PARAMETRIZATION_SUCCESS,
    GET_TERM_PARAMETRIZATION_FAILURE,
    UPDATE_TERM_PARAMETRIZATION,
    UPDATE_TERM_PARAMETRIZATION_SUCCESS,
    UPDATE_TERM_PARAMETRIZATION_FAILURE,
    OBTENER_PARAMETRO_TRANSACCION,
    CREAR_PARAMETRO_TRANSACCION,
    ACTUALIZAR_PARAMETRO_TRANSACCION,
    ON_ERROR,
    GET_PARAMETRIZATION_PLAZOS_GENERAL,
    GET_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE,
    POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL,
    POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE,
    POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    GET_PARAMETRIZATION_SEGURO_GENERAL,
    GET_PARAMETRIZATION_SEGURO_GENERAL_FAILURE,
    POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL,
    GET_PARAMETRIZATION_SEGURO_MUNICIPIO,
    GET_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO,
    POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO,
    POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO,
    POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    GET_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE,
    GET_PARAMETRIZATION_LIQUIDACION_GENERAL,
    POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL,
    POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE,
    GET_PARAMETRIZATION_SEGURO_REGION_FAILURE,
    GET_PARAMETRIZATION_SEGURO_REGION,
    POST_UPDATE_PARAMETRIZATION_SEGURO_REGION,
    POST_UPDATE_PARAMETRIZATION_SEGURO_REGION_FAILURE,
} from '../types/parametrization.action.type';
import {
    SeguroDto,
    TasaDto,
    PlazosDto,
    ParametroTransaccionDto,
    LiquidacionDto,
    PlazoDto,
    PlazoTipoActorDto,
    PlazoUndejDto,
    SegurosDto,
    SeguroMunicipioDto,
    LiquidacionGeneralDto,
    SeguroRegionDto,
} from '../types/parametrization.model.type';
import ApiParametrization from '../services/api-parametrization.service';

const api = ApiParametrization();

export const GetInsuranceParametrization = (): AppActions => ({
    type: GET_INSURANCE_PARAMETRIZATION,
});

export const GetInsuranceParametrizationSuccess = (
    insurance: SeguroDto,
): AppActions => ({
    type: GET_INSURANCE_PARAMETRIZATION_SUCCESS,
    insurance: insurance,
});

export const GetInsuranceParametrizationFailure = (error: any): AppActions => ({
    type: GET_INSURANCE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const GetRateParametrization = (): AppActions => ({
    type: GET_RATE_PARAMETRIZATION,
});

export const GetRateParametrizationSuccess = (rate: TasaDto): AppActions => ({
    type: GET_RATE_PARAMETRIZATION_SUCCESS,
    rate: rate,
});

export const GetRateParametrizationFailure = (error: any): AppActions => ({
    type: GET_RATE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const GetTermParametrization = (): AppActions => ({
    type: GET_TERM_PARAMETRIZATION,
});

export const GetTermParametrizationSuccess = (term: PlazosDto): AppActions => ({
    type: GET_TERM_PARAMETRIZATION_SUCCESS,
    term: term,
});

export const GetTermParametrizationFailure = (error: any): AppActions => ({
    type: GET_TERM_PARAMETRIZATION_FAILURE,
    error: error,
});

export const UpdateInsuranceParametrization = (): AppActions => ({
    type: UPDATE_INSURANCE_PARAMETRIZATION,
});

export const UpdateInsuranceParametrizationSuccess = (
    insurance: SeguroDto,
): AppActions => ({
    type: UPDATE_INSURANCE_PARAMETRIZATION_SUCCESS,
    insurance: insurance,
});

export const UpdateInsuranceParametrizationFailure = (
    error: any,
): AppActions => ({
    type: UPDATE_INSURANCE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const UpdateRateParametrization = (): AppActions => ({
    type: UPDATE_RATE_PARAMETRIZATION,
});

export const UpdateRateParametrizationSuccess = (
    rate: TasaDto,
): AppActions => ({
    type: UPDATE_RATE_PARAMETRIZATION_SUCCESS,
    rate: rate,
});

export const UpdateRateParametrizationFailure = (error: any): AppActions => ({
    type: UPDATE_RATE_PARAMETRIZATION_FAILURE,
    error: error,
});

export const UpdateTermParametrization = (): AppActions => ({
    type: UPDATE_TERM_PARAMETRIZATION,
});

export const UpdateTermParametrizationSuccess = (
    term: PlazosDto,
): AppActions => ({
    type: UPDATE_TERM_PARAMETRIZATION_SUCCESS,
    term: term,
});

export const UpdateTermParametrizationFailure = (error: any): AppActions => ({
    type: UPDATE_TERM_PARAMETRIZATION_FAILURE,
    error: error,
});

export const ActualizarParametroTransaccion = (
    parametro: ParametroTransaccionDto,
): AppActions => ({
    type: ACTUALIZAR_PARAMETRO_TRANSACCION,
    parametro: parametro,
});

export const ObtenerParametroTransaccion = (
    parametros: ParametroTransaccionDto[],
): AppActions => ({
    type: OBTENER_PARAMETRO_TRANSACCION,
    parametros: parametros,
});

export const CrearParametroTransaccion = (
    parametro: ParametroTransaccionDto,
): AppActions => ({
    type: CREAR_PARAMETRO_TRANSACCION,
    parametro: parametro,
});

export const GetSettlementParametrization = (): AppActions => ({
    type: GET_SETTLEMENT_PARAMETRIZATION,
});

export const GetSettlementParametrizationSuccess = (
    Settlement: LiquidacionDto,
): AppActions => ({
    type: GET_SETTLEMENT_PARAMETRIZATION_SUCCESS,
    Settlement: Settlement,
});

export const GetSettlementParametrizationFailure = (
    error: any,
): AppActions => ({
    type: GET_SETTLEMENT_PARAMETRIZATION_FAILURE,
    error: error,
});

export const UpdateSettlementParametrization = (): AppActions => ({
    type: UPDATE_SETTLEMENT_PARAMETRIZATION,
});

export const UpdateSettlementParametrizationSuccess = (
    Settlement: LiquidacionDto,
): AppActions => ({
    type: UPDATE_SETTLEMENT_PARAMETRIZATION_SUCCESS,
    Settlement: Settlement,
});

export const UpdateSettlementParametrizationFailure = (
    error: any,
): AppActions => ({
    type: UPDATE_SETTLEMENT_PARAMETRIZATION_FAILURE,
    error: error,
});

export const OnError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});


export const GetParametrizationPlazosGeneralSuccess = (
    plazosGeneral: PlazoDto[],
): AppActions => ({
    type: GET_PARAMETRIZATION_PLAZOS_GENERAL,
    plazosGeneral: plazosGeneral,
});

export const GetParametrizationPlazosGeneralFailure = (error: any): AppActions => ({
    type: GET_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE,
    error: error,
});

export const PostUpdateParametrizationPlazosGeneralSuccess = (
    plazoGeneral: PlazoDto,
): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL,
    plazoGeneral: plazoGeneral,
});

export const PostUpdateParametrizationPlazosGeneralFailure = (error: any): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE,
    error: error,
});



export const PostInsertParametrizationPlazoTipoActorSuccess = (
    plazoTipoActor: PlazoTipoActorDto,
): AppActions => ({
    type: POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    plazoTipoActor: plazoTipoActor
});

export const PostInsertParametrizationPlazoTipoActorFailure = (error: any): AppActions => ({
    type: POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    error: error,
});

export const PostParametrizationPlazosTipoActorSuccess = (
    plazosTipoActor: PlazoTipoActorDto[],
): AppActions => ({
    type: POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    plazosTipoActor: plazosTipoActor,
});

export const PostParametrizationPlazosTipoActorFailure = (error: any): AppActions => ({
    type: POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    error: error,
});

export const PostUpdateParametrizationPlazoTipoActorSuccess = (
    plazoTipoActor: PlazoTipoActorDto,
): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    plazoTipoActor: plazoTipoActor
});

export const PostUpdateParametrizationPlazoTipoActorFailure = (error: any): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    error: error,
});

export const PostDeleteParametrizationPlazoTipoActorSuccess = (
    plazoTipoActor: PlazoTipoActorDto,
): AppActions => ({
    type: POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR,
    plazoTipoActor: plazoTipoActor
});

export const PostDeleteParametrizationPlazoTipoActorFailure = (error: any): AppActions => ({
    type: POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE,
    error: error,
});



export const PostInsertParametrizationPlazoUniejSuccess = (
    plazoUniej: PlazoUndejDto,
): AppActions => ({
    type: POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    plazoUniej: plazoUniej
});

export const PostInsertParametrizationPlazoUniejFailure = (error: any): AppActions => ({
    type: POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    error: error,
});

export const PostParametrizationPlazosUniejSuccess = (
    plazosUniej: PlazoUndejDto[],
): AppActions => ({
    type: POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    plazosUniej: plazosUniej
});

export const PostParametrizationPlazosUniejFailure = (error: any): AppActions => ({
    type: POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    error: error,
});

export const PostUpdateParametrizationPlazosUniejSuccess = (
    plazoUniej: PlazoUndejDto,
): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    plazoUniej: plazoUniej
});

export const PostUpdateParametrizationPlazosUniejFailure = (error: any): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    error: error,
});

export const PostDeleteParametrizationPlazoUniejSuccess = (
    plazoUniej: PlazoUndejDto,
): AppActions => ({
    type: POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA,
    plazoUniej: plazoUniej
});

export const PostDeleteParametrizationPlazoUniejFailure = (error: any): AppActions => ({
    type: POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE,
    error: error,
});



export const GetParametrizationSeguroGeneralSuccess = (
    segurosGeneral: SegurosDto[],
): AppActions => ({
    type: GET_PARAMETRIZATION_SEGURO_GENERAL,
    segurosGeneral: segurosGeneral,
});

export const GetParametrizationSeguroGeneralFailure = (error: any): AppActions => ({
    type: GET_PARAMETRIZATION_SEGURO_GENERAL_FAILURE,
    error: error,
});

export const PostUpdateParametrizationSeguroGeneralSuccess = (
    seguroGeneral: SegurosDto,
): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL,
    seguroGeneral: seguroGeneral,
});

export const PostUpdateParametrizationSeguroGeneralFailure = (error: any): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE,
    error: error,
});


export const GetParametrizationSeguroPorMunicipioSuccess = (
    segurosMunicipio: SeguroMunicipioDto[],
): AppActions => ({
    type: GET_PARAMETRIZATION_SEGURO_MUNICIPIO,
    segurosMunicipio: segurosMunicipio,
});

export const GetParametrizationSeguroPorMunicipioFailure = (error: any): AppActions => ({
    type: GET_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    error: error,
});

export const PostInsertParametrizationSeguroPorMunicipioSuccess = (
    seguroMunicipio: SeguroMunicipioDto,
): AppActions => ({
    type: POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO,
    seguroMunicipio: seguroMunicipio
});

export const GetParametrizationSeguroPorRegionFailure = (error: any): AppActions => ({
    type: GET_PARAMETRIZATION_SEGURO_REGION_FAILURE,
    error: error,
});

export const GetParametrizationSeguroPorRegionSuccess = (
    segurosRegion: SeguroRegionDto[],
): AppActions => ({
    type: GET_PARAMETRIZATION_SEGURO_REGION,
    segurosRegion: segurosRegion
});

export const PostUpdateParametrizationSeguroPorRegionFailure = (error: any): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_SEGURO_REGION_FAILURE,
    error: error,
});

export const PostUpdateParametrizationSeguroPorRegionSuccess = (
    request: any,
): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_SEGURO_REGION,
    request: request
});

export const PostInsertParametrizationSeguroPorMunicipioFailure = (error: any): AppActions => ({
    type: POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    error: error,
});

export const PostUpdateParametrizationSeguroPorMunicipioSuccess = (
    seguroMunicipio: SeguroMunicipioDto,
): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO,
    seguroMunicipio: seguroMunicipio
});

export const PostUpdateParametrizationSeguroPorMunicipioFailure = (error: any): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    error: error,
});

export const PostDeleteParametrizationSeguroPorMunicipioSuccess = (
    seguroMunicipio: SeguroMunicipioDto,
): AppActions => ({
    type: POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO,
    seguroMunicipio: seguroMunicipio
});

export const PostDeleteParametrizationSeguroPorMunicipioFailure = (error: any): AppActions => ({
    type: POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    error: error,
});



export const GetParametrizationLiquidacionGeneralSuccess = (
    liquidacionGeneral: LiquidacionGeneralDto,
): AppActions => ({
    type: GET_PARAMETRIZATION_LIQUIDACION_GENERAL,
    liquidacionGeneral: liquidacionGeneral,
});

export const GetParametrizationLiquidacionGeneralFailure = (error: any): AppActions => ({
    type: GET_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE,
    error: error,
});

export const PostUpdateParametrizationLiquidacionGeneralSuccess = (
    liquidacionGeneral: LiquidacionGeneralDto,
): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL,
    liquidacionGeneral: liquidacionGeneral
});

export const PostUpdateParametrizationLiquidacionGeneralFailure = (error: any): AppActions => ({
    type: POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE,
    error: error,
});




export const startGetAllParametrizationPlazosGeneral =
    () => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.GetAllParametrizationPlazosGeneral();
        if (response?.isSuccess == null) {
            dispatch(GetParametrizationPlazosGeneralFailure(response));
        } else {
            dispatch(GetParametrizationPlazosGeneralSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostUpdateParametrizationPlazosGeneral =
    (plazosGeneral: PlazoDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.PostUpdateParametrizationPlazosGeneral(plazosGeneral);
        if (response?.isSuccess == null) {
            dispatch(PostUpdateParametrizationPlazosGeneralFailure(response.data));
        } else {
            dispatch(PostUpdateParametrizationPlazosGeneralSuccess(response.data));
            toast(response.returnMessage);
        }
    };



export const startPostAllParametrizationPlazosTipoActor =
    (tipoCredito: number) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postAllParametrizationPlazosExcepTipoActor(tipoCredito);
        if (response?.isSuccess == null) {
            dispatch(PostParametrizationPlazosTipoActorFailure(response));
        } else {
            dispatch(PostParametrizationPlazosTipoActorSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostInsertParametrizationPlazosExcepTipoActor =
    (plazoTipoActor: PlazoTipoActorDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postInsertParametrizationPlazosExcepTipoActor(plazoTipoActor);
        if (response?.isSuccess == null) {
            dispatch(PostInsertParametrizationPlazoTipoActorFailure(response.data));
        } else {
            dispatch(PostInsertParametrizationPlazoTipoActorSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostUpdateParametrizationPlazoTipoActor =
    (plazoTipoActor: PlazoTipoActorDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postUpdateParametrizationPlazosExcepTipoActor(plazoTipoActor);
        if (response?.isSuccess == null) {
            dispatch(PostUpdateParametrizationPlazoTipoActorFailure(response.data));
        } else {
            dispatch(PostUpdateParametrizationPlazoTipoActorSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostDeleteParametrizationPlazoTipoActor =
    (plazoTipoActor: PlazoTipoActorDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postDeleteParametrizationPlazosExcepTipoActor(plazoTipoActor);
        if (response?.isSuccess == null) {
            dispatch(PostDeleteParametrizationPlazoTipoActorFailure(response.data));
        } else {
            dispatch(PostDeleteParametrizationPlazoTipoActorSuccess(response.data));
            toast(response.returnMessage);
        }
    };



export const startPostAllParametrizationPlazosUniej =
    (tipoCredito: number) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.PostAllParametrizationPlazosExcepUndej(tipoCredito);
        if (response?.isSuccess == null) {
            dispatch(PostParametrizationPlazosUniejFailure(response));
        } else {
            dispatch(PostParametrizationPlazosUniejSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostInsertParametrizationPlazosExcepUniej =
    (plazoUniej: PlazoUndejDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.PostInsertParametrizationPlazosExcepUndej(plazoUniej);
        if (response?.isSuccess == null) {
            dispatch(PostInsertParametrizationPlazoUniejFailure(response.data));
        } else {
            dispatch(PostInsertParametrizationPlazoUniejSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostUpdateParametrizationPlazoUniej =
    (plazoUniej: PlazoUndejDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postUpdateParametrizationPlazosExcepUniej(plazoUniej);
        if (response?.isSuccess == null) {
            dispatch(PostUpdateParametrizationPlazosUniejFailure(response.data));
        } else {
            dispatch(PostUpdateParametrizationPlazosUniejSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostDeleteParametrizationPlazoUniej =
    (plazosUniej: PlazoUndejDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postDeleteParametrizationPlazosExcepUniej(plazosUniej);
        if (response?.isSuccess == null) {
            dispatch(PostDeleteParametrizationPlazoUniejFailure(response.data));
        } else {
            dispatch(PostDeleteParametrizationPlazoUniejSuccess(response.data));
            toast(response.returnMessage);
        }
    };



export const startGetAllParametrizationSeguroGeneral =
    () => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.GetAllParametrizationSegurosGeneral();
        if (response?.isSuccess == null) {
            dispatch(GetParametrizationSeguroGeneralFailure(response));
        } else {
            dispatch(GetParametrizationSeguroGeneralSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostUpdateParametrizationSeguroGeneral =
    (seguroGeneral: SegurosDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.PostUpdateParametrizationSegurosGeneral(seguroGeneral);
        if (response?.isSuccess == null) {
            dispatch(PostUpdateParametrizationSeguroGeneralFailure(response.data));
        } else {
            dispatch(PostUpdateParametrizationSeguroGeneralSuccess(response.data));
            toast(response.returnMessage);
        }
    };



export const startGetAllParametrizationExcepPorMunicipio=
    () => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.GetAllParametrizationSeguroExcepPorMunicipio();
        if (response?.isSuccess == null) {
            dispatch(GetParametrizationSeguroPorMunicipioFailure(response));
        } else {
            dispatch(GetParametrizationSeguroPorMunicipioSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostInsertParametrizationSeguroPorMunicipio =
    (seguroMunicipio: SeguroMunicipioDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.PostInsertParametrizationExcepSeguroPorMunicipio(seguroMunicipio);
        if (response?.isSuccess == null) {
            dispatch(PostInsertParametrizationSeguroPorMunicipioFailure(response.data));
        } else {
            dispatch(PostInsertParametrizationSeguroPorMunicipioSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostUpdateParametrizationSeguroPorMunicipio =
    (seguroMunicipio: SeguroMunicipioDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.PostUpdateParametrizationExcepSeguroPorMunicipio(seguroMunicipio);
        if (response?.isSuccess == null) {
            dispatch(PostUpdateParametrizationSeguroPorMunicipioFailure(response.data));
        } else {
            dispatch(PostUpdateParametrizationSeguroPorMunicipioSuccess(response.data));
            toast(response.returnMessage);
        }
    };

    export const startPostDeleteParametrizationSeguroPorMunicipio =
    (seguroMunicipio: SeguroMunicipioDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.postDeleteParametrizationExcepSeguroPorMunicipio(seguroMunicipio);
        if (response?.isSuccess == null) {
            dispatch(PostDeleteParametrizationSeguroPorMunicipioFailure(response.data));
        } else {
            dispatch(PostDeleteParametrizationSeguroPorMunicipioSuccess(response.data));
            toast(response.returnMessage);
        }
    };



export const startGetAllParametrizationSeguroRegion =
    () => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.GetAllParametrizationSeguroRegion();
        if (response?.isSuccess == null) {
            dispatch(GetParametrizationSeguroPorRegionFailure(response));
        } else {
            dispatch(GetParametrizationSeguroPorRegionSuccess(response.data));
            toast(response.returnMessage);

        }
    };


export const startGetAllParametrizationLiquidacionGeneral =
    () => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.GetAllParametrizationLiquidacionGeneral();
        if (response?.isSuccess == null) {
            dispatch(GetParametrizationLiquidacionGeneralFailure(response));
        } else {
            dispatch(GetParametrizationLiquidacionGeneralSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startPostUpdateParametrizationLiquidacionGeneral =
    (liquidacionGeneral: LiquidacionGeneralDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.PostUpdateParametrizationLiquidacionGeneral(liquidacionGeneral);
        if (response?.isSuccess == null) {
            dispatch(PostUpdateParametrizationLiquidacionGeneralFailure(response.data));
        } else {
            dispatch(PostUpdateParametrizationLiquidacionGeneralSuccess(response.data));
            toast(response.returnMessage);
        }
    };

//

export const startGetInsuranceParametrization =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetInsuranceParametrization());
        const response = await api.GetInsuranceParametrization();
        if (response?.isSuccess == null) {
            dispatch(GetInsuranceParametrizationFailure(response));
        } else {
            dispatch(GetInsuranceParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startGetRateParametrization =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetRateParametrization());
        const response = await api.GetRateParametrization();
        if (response?.isSuccess == null) {
            dispatch(GetRateParametrizationFailure(response));
        } else {
            dispatch(GetRateParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startGetTermParametrization =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetTermParametrization());
        const response = await api.GetTermParametrization();
        if (response?.isSuccess == null) {
            dispatch(GetTermParametrizationFailure(response));
        } else {
            dispatch(GetTermParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startUpdateInsuranceParametrization =
    (insurance: SeguroDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateInsuranceParametrization());
        const response = await api.UpdateInsuranceParametrization(insurance);
        if (response?.isSuccess == null) {
            dispatch(UpdateInsuranceParametrizationFailure(response));
        } else {
            dispatch(UpdateInsuranceParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startUpdateRateParametrization =
    (insurance: TasaDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateRateParametrization());
        const response = await api.UpdateRateParametrization(insurance);
        if (response?.isSuccess == null) {
            dispatch(UpdateRateParametrizationFailure(response));
        } else {
            dispatch(UpdateRateParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startUpdateTermParametrization =
    (insurance: PlazosDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateTermParametrization());
        const response = await api.UpdateTermParametrization(insurance);

        if (response?.isSuccess == null) {
            dispatch(UpdateTermParametrizationFailure(response.data));
        } else {
            dispatch(UpdateTermParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startGetSettlementParametrization =
    () => async (dispatch: Dispatch<AppActions>) => {
        dispatch(GetSettlementParametrization());
        const response = await api.GetSettlementParametrization();

        if (response?.isSuccess == null) {
            dispatch(GetSettlementParametrizationFailure(response));
        } else {
            dispatch(GetSettlementParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startUpdateSettlementParametrization =
    (Settlement: LiquidacionDto) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.UpdateSettlementParametrization(Settlement);

        if (response?.isSuccess == null) {
            dispatch(UpdateSettlementParametrizationFailure(response));
        } else {
            dispatch(UpdateSettlementParametrizationSuccess(response.data));
            toast(response.returnMessage);
        }
    };

export const startCrearParametrosTransaccion =
    (parametro: ParametroTransaccionDto, props: string) =>
        async (dispatch: Dispatch<AppActions>) => {
            const response = await api.CrearParametroTransaccionRequest(parametro)
            if (response?.isSuccess == null) {
                dispatch(OnError(response));
            } else {
                dispatch(CrearParametroTransaccion(response.data));
                toast(response.returnMessage);
            }
        };

export const startActualizarParametrosTransaccion =
    (parametro: ParametroTransaccionDto, props: string) =>
        async (dispatch: Dispatch<AppActions>) => {
            const response = await api.ActualizarParametroTransaccionRequest(parametro)
            if (response?.isSuccess == null) {
                dispatch(OnError(response));
            } else {
                dispatch(ActualizarParametroTransaccion(response.data));
                toast(response.returnMessage);
            }
        };

export const startObtenerParametrosTransaccion =
    (props: string) => async (dispatch: Dispatch<AppActions>) => {
        const response = await api.ObtenerParametroTransaccionRequest()
        if (response?.isSuccess === null) {
            dispatch(OnError(response));
        } else {
            dispatch(ObtenerParametroTransaccion(response.data));
            toast(response.returnMessage);
        }
    };
