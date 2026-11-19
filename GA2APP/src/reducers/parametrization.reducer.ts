import {
    InsuranceForm,
    RateForm,
    TermForm,
    SettlementForm,
    ParametroTransaccion,
    PlazosForm,
    PlazosTipoActorDtoForm,
    PlazosUniejDtoForm,
    SeguroGeneralForm,
    SeguroExcepMunicipioForm,
    LiquidacionGeneralForm,
    SeguroRegionForm,
} from '../pages/parametrization/constants/form.model.type';
import {
    ParametrizationActionTypes,
    GET_INSURANCE_PARAMETRIZATION,
    GET_INSURANCE_PARAMETRIZATION_SUCCESS,
    GET_INSURANCE_PARAMETRIZATION_FAILURE,
    GET_RATE_PARAMETRIZATION,
    GET_RATE_PARAMETRIZATION_SUCCESS,
    GET_RATE_PARAMETRIZATION_FAILURE,
    GET_TERM_PARAMETRIZATION,
    GET_TERM_PARAMETRIZATION_SUCCESS,
    GET_TERM_PARAMETRIZATION_FAILURE,
    UPDATE_TERM_PARAMETRIZATION,
    UPDATE_TERM_PARAMETRIZATION_SUCCESS,
    UPDATE_TERM_PARAMETRIZATION_FAILURE,
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
    CREAR_PARAMETRO_TRANSACCION,
    ACTUALIZAR_PARAMETRO_TRANSACCION,
    OBTENER_PARAMETRO_TRANSACCION,
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
    POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL_FAILURE,
    GET_PARAMETRIZATION_SEGURO_MUNICIPIO,
    GET_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO,
    POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO,
    POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO,
    POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE,
    GET_PARAMETRIZATION_LIQUIDACION_GENERAL,
    GET_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE,
    POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE,
    POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL,
    GET_PARAMETRIZATION_SEGURO_REGION,
    GET_PARAMETRIZATION_SEGURO_REGION_FAILURE,
    POST_UPDATE_PARAMETRIZATION_SEGURO_REGION,
    POST_UPDATE_PARAMETRIZATION_SEGURO_REGION_FAILURE,
} from '../types/parametrization.action.type';
import {
    SeguroDto,
    TasaDto,
    PlazosDto,
    LiquidacionDto,
    ParametroTransaccionDto,
    PlazoDto,
    PlazoTipoActorDto,
    PlazoUndejDto,
    SegurosDto,
    SeguroMunicipioDto,
    LiquidacionGeneralDto,
    SeguroRegionDto,
} from '../types/parametrization.model.type';

export interface ParametrizacionState {
    id: number;
    insurance: SeguroDto;
    rate: TasaDto;
    term: PlazosDto;
    settlement: LiquidacionDto;
    parametro: ParametroTransaccionDto;
    parametros: ParametroTransaccionDto[];
    //
    plazosGeneral: PlazoDto[];
    plazoGeneral: PlazoDto;
    plazosTipoActor: PlazoTipoActorDto[];
    plazoTipoActor: PlazoTipoActorDto;
    plazosUniej: PlazoUndejDto[];
    plazoUniej: PlazoUndejDto;
    segurosGeneral: SegurosDto[];
    seguroGeneral: SegurosDto;
    segurosMunicipio: SeguroMunicipioDto[];
    seguroMunicipio: SeguroMunicipioDto;
    segurosRegion: SeguroRegionDto[];
    seguroRegion: SeguroRegionDto;
    liquidacionGeneral: LiquidacionGeneralDto;
    error: any;
    loading: boolean;
}

const ParametrizacionStateInitial: ParametrizacionState = {
    id: 0,
    insurance: InsuranceForm,
    rate: RateForm,
    term: TermForm,
    settlement: SettlementForm,
    parametro: ParametroTransaccion,
    parametros: [],
    //
    plazosGeneral: [],
    plazoGeneral: PlazosForm,
    plazosTipoActor: [],
    plazoTipoActor: PlazosTipoActorDtoForm,
    plazosUniej: [],
    plazoUniej: PlazosUniejDtoForm,
    segurosGeneral: [],
    seguroGeneral: SeguroGeneralForm,
    segurosMunicipio: [],
    seguroMunicipio: SeguroExcepMunicipioForm,
    segurosRegion: [],
    seguroRegion: SeguroRegionForm,
    liquidacionGeneral: LiquidacionGeneralForm,
    error: null,
    loading: false,
};

export const ParametrizationReducer = (
    state: ParametrizacionState = ParametrizacionStateInitial,
    action: ParametrizationActionTypes,
): ParametrizacionState => {
    switch (action.type) {
        case GET_PARAMETRIZATION_PLAZOS_GENERAL:
            return {
                ...state,
                plazosGeneral: action.plazosGeneral,
                loading: false,
            };
        case GET_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL:
            return {
                ...state,
                plazoGeneral: action.plazoGeneral,
                loading: false,
            };
        case POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR:
            return {
                ...state,
                plazosTipoActor: action.plazosTipoActor,
                loading: false,
            };
        case POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR:
            return {
                ...state,
                plazoTipoActor: action.plazoTipoActor,
                loading: false,
            };
        case POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR:
            return {
                ...state,
                plazoTipoActor: action.plazoTipoActor,
                loading: false,
            };
        case POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR:
            return {
                ...state,
                plazoTipoActor: action.plazoTipoActor,
                loading: false,
            };
        case POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA:
            return {
                ...state,
                plazosUniej: action.plazosUniej,
                loading: false,
            };
        case POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA:
            return {
                ...state,
                plazoUniej: action.plazoUniej,
                loading: false,
            };
        case POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA:
            return {
                ...state,
                plazoUniej: action.plazoUniej,
                loading: false,
            };
        case POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA:
            return {
                ...state,
                plazoUniej: action.plazoUniej,
                loading: false,
            };
        case POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case GET_PARAMETRIZATION_SEGURO_GENERAL:
            return {
                ...state,
                segurosGeneral: action.segurosGeneral,
                loading: false,
            };
        case GET_PARAMETRIZATION_SEGURO_GENERAL_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL:
            return {
                ...state,
                seguroGeneral: action.seguroGeneral,
                loading: false,
            };
        case POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case GET_PARAMETRIZATION_SEGURO_MUNICIPIO:
            return {
                ...state,
                segurosMunicipio: action.segurosMunicipio,
                loading: false,
            };
        case GET_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO:
            return {
                ...state,
                seguroMunicipio: action.seguroMunicipio,
                loading: false,
            };
        case POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO:
            return {
                ...state,
                seguroMunicipio: action.seguroMunicipio,
                loading: false,
            };
        case POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO:
            return {
                ...state,
                seguroMunicipio: action.seguroMunicipio,
                loading: false,
            };
        case POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case GET_PARAMETRIZATION_SEGURO_REGION:
            return {
                ...state,
                segurosRegion: action.segurosRegion,
                loading: false,
            };
        case GET_PARAMETRIZATION_SEGURO_REGION_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case GET_PARAMETRIZATION_LIQUIDACION_GENERAL:
            return {
                ...state,
                liquidacionGeneral: action.liquidacionGeneral,
                loading: false,
            };
        case GET_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL:
            return {
                ...state,
                liquidacionGeneral: action.liquidacionGeneral,
                loading: false,
            };
        case POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case GET_INSURANCE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case GET_INSURANCE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                insurance: action.insurance,
                loading: false,
            };
        case GET_INSURANCE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case GET_RATE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case GET_RATE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                rate: action.rate,
                loading: false,
            };
        case GET_RATE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case GET_TERM_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case GET_TERM_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                term: action.term,
                loading: false,
            };
        case GET_TERM_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_INSURANCE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_INSURANCE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                insurance: action.insurance,
                loading: false,
            };
        case UPDATE_INSURANCE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_RATE_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_RATE_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                rate: action.rate,
                loading: false,
            };
        case UPDATE_RATE_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_TERM_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_TERM_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                term: action.term,
                loading: false,
            };
        case UPDATE_TERM_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case CREAR_PARAMETRO_TRANSACCION:
            return {
                ...state,
                parametro: action.parametro,
                loading: true,
            };
        case GET_SETTLEMENT_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case GET_SETTLEMENT_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                settlement: action.Settlement,
                loading: false,
            };
        case GET_SETTLEMENT_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        case UPDATE_SETTLEMENT_PARAMETRIZATION:
            return {
                ...state,
                loading: true,
            };
        case UPDATE_SETTLEMENT_PARAMETRIZATION_SUCCESS:
            return {
                ...state,
                settlement: action.Settlement,
                loading: false,
            };
        case UPDATE_SETTLEMENT_PARAMETRIZATION_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

        case ACTUALIZAR_PARAMETRO_TRANSACCION:
            return {
                ...state,
                parametro: action.parametro,
                loading: false,
            };

        case OBTENER_PARAMETRO_TRANSACCION:
            return {
                ...state,
                parametros: action.parametros,
                loading: false,
            };

        case ON_ERROR:
            return {
                ...state,
                error: action.error,
                loading: false,
            };
        default:
            return state;
    }
};
