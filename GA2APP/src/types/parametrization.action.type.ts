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
} from './parametrization.model.type';

// Especificacion de accion Plazos
export const GET_PARAMETRIZATION_PLAZOS_GENERAL = 'GET_PARAMETRIZATION_PLAZOS_GENERAL';
export const GET_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE = 'GET_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE';
export const POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL = 'POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL';
export const POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE = 'POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE';

// Plazos Tipo Afiliado
export const POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR = 'POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
export const POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE = 'POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';
export const POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR = 'POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
export const POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE = 'POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';
export const POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR = 'POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
export const POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE = 'POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';
export const POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR = 'POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
export const POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE = 'POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';

// Plazos Unidad Ejecutora
export const POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA = 'POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
export const POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE = 'POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';
export const POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA = 'POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
export const POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE = 'POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';
export const POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA = 'POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
export const POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE = 'POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';
export const POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA = 'POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
export const POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE = 'POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';

// Seguro General
export const GET_PARAMETRIZATION_SEGURO_GENERAL = 'GET_PARAMETRIZATION_SEGURO_GENERAL';
export const GET_PARAMETRIZATION_SEGURO_GENERAL_FAILURE = 'GET_PARAMETRIZATION_SEGURO_GENERAL_FAILURE';
export const POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL = 'POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL';
export const POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL_FAILURE = 'POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL_FAILURE';

// Excepcion seguro por municipio
export const GET_PARAMETRIZATION_SEGURO_MUNICIPIO = 'GET_PARAMETRIZATION_SEGURO_MUNICIPIO';
export const GET_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE = 'GET_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';
export const POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO = 'POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO';
export const POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE = 'POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';
export const POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO = 'POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO';
export const POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE = 'POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';
export const POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO = 'POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO';
export const POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE = 'POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';

// seguro por región

export const GET_PARAMETRIZATION_SEGURO_REGION = 'GET_PARAMETRIZATION_SEGURO_REGION';
export const GET_PARAMETRIZATION_SEGURO_REGION_FAILURE = 'GET_PARAMETRIZATION_SEGURO_REGION_FAILURE';
export const POST_UPDATE_PARAMETRIZATION_SEGURO_REGION = 'POST_UPDATE_PARAMETRIZATION_SEGURO_REGION';
export const POST_UPDATE_PARAMETRIZATION_SEGURO_REGION_FAILURE = 'POST_UPDATE_PARAMETRIZATION_SEGURO_REGION_FAILURE';

// Liquidacion General
export const GET_PARAMETRIZATION_LIQUIDACION_GENERAL = 'GET_PARAMETRIZATION_LIQUIDACION_GENERAL';
export const GET_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE = 'GET_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE';
export const POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL = 'POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL';
export const POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE = 'POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE';

export const GET_INSURANCE_PARAMETRIZATION = 'GET_INSURANCE_PARAMETRIZATION';
export const GET_INSURANCE_PARAMETRIZATION_SUCCESS =
    'GET_INSURANCE_PARAMETRIZATION_SUCCESS';
export const GET_INSURANCE_PARAMETRIZATION_FAILURE =
    'GET_INSURANCE_PARAMETRIZATION_FAILURE';
export const GET_RATE_PARAMETRIZATION = 'GET_RATE_PARAMETRIZATION';
export const GET_RATE_PARAMETRIZATION_SUCCESS =
    'GET_RATE_PARAMETRIZATION_SUCCESS';
export const GET_RATE_PARAMETRIZATION_FAILURE =
    'GET_RATE_PARAMETRIZATION_FAILURE';
export const GET_TERM_PARAMETRIZATION = 'GET_TERM_PARAMETRIZATION';
export const GET_TERM_PARAMETRIZATION_SUCCESS =
    'GET_TERM_PARAMETRIZATION_SUCCESS';
export const GET_TERM_PARAMETRIZATION_FAILURE =
    'GET_TERM_PARAMETRIZATION_FAILURE';
export const UPDATE_INSURANCE_PARAMETRIZATION =
    'UPDATE_INSURANCE_PARAMETRIZATION';
export const UPDATE_INSURANCE_PARAMETRIZATION_SUCCESS =
    'UPDATE_INSURANCE_PARAMETRIZATION_SUCCESS';
export const UPDATE_INSURANCE_PARAMETRIZATION_FAILURE =
    'UPDATE_INSURANCE_PARAMETRIZATION_FAILURE';
export const UPDATE_RATE_PARAMETRIZATION_SUCCESS =
    'UPDATE_RATE_PARAMETRIZATION_SUCCESS';
export const UPDATE_RATE_PARAMETRIZATION_FAILURE =
    'UPDATE_RATE_PARAMETRIZATION_FAILURE';
export const UPDATE_TERM_PARAMETRIZATION_SUCCESS =
    'UPDATE_TERM_PARAMETRIZATION_SUCCESS';
export const UPDATE_TERM_PARAMETRIZATION_FAILURE =
    'UPDATE_TERM_PARAMETRIZATION_FAILURE';
export const ACTUALIZAR_PARAMETRO_TRANSACCION =
    'ACTUALIZAR_PARAMETRO_TRANSACCION';
export const OBTENER_PARAMETRO_TRANSACCION = 'OBTENER_PARAMETRO_TRANSACCION';
export const CREAR_PARAMETRO_TRANSACCION = 'CREAR_PARAMETRO_TRANSACCION';
export const GET_SETTLEMENT_PARAMETRIZATION_SUCCESS =
    'GET_SETTLEMENT_PARAMETRIZATION_SUCCESS';
export const GET_SETTLEMENT_PARAMETRIZATION_FAILURE =
    'GET_SETTLEMENT_PARAMETRIZATION_FAILURE';
export const UPDATE_SETTLEMENT_PARAMETRIZATION_SUCCESS =
    'UPDATE_SETTLEMENT_PARAMETRIZATION_SUCCESS';
export const UPDATE_SETTLEMENT_PARAMETRIZATION_FAILURE =
    'UPDATE_SETTLEMENT_PARAMETRIZATION_FAILURE';
export const UPDATE_RATE_PARAMETRIZATION = 'UPDATE_RATE_PARAMETRIZATION';
export const UPDATE_TERM_PARAMETRIZATION = 'UPDATE_TERM_PARAMETRIZATION';
export const GET_SETTLEMENT_PARAMETRIZATION = 'GET_SETTLEMENT_PARAMETRIZATION';
export const UPDATE_SETTLEMENT_PARAMETRIZATION =
    'UPDATE_SETTLEMENT_PARAMETRIZATION';
export const ON_ERROR = 'ON_ERROR';

// Plazos generales
export interface GetParametrizationPlazosGeneralSuccess {
    type: 'GET_PARAMETRIZATION_PLAZOS_GENERAL';
    plazosGeneral: PlazoDto[];
}
export interface GetParametrizationPlazosGeneralFailure {
    type: 'GET_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE';
    error: any;
}
export interface PostUpdateParametrizationPlazosGeneralSuccess {
    type: 'POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL';
    plazoGeneral: PlazoDto;
}
export interface PostUpdateParametrizationPlazosGeneralFailure {
    type: 'POST_UPDATE_PARAMETRIZATION_PLAZOS_GENERAL_FAILURE';
    error: any;
}

// Plazos tipo actor

export interface PostParametrizationPlazosTipoActorSuccess {
    type: 'POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
    plazosTipoActor: PlazoTipoActorDto[];
}
export interface PostParametrizationPlazosTipoActorFailure {
    type: 'POST_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';
    error: any;
}
export interface PostInsertParametrizationPlazoTipoActorSuccess {
    type: 'POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
    plazoTipoActor: PlazoTipoActorDto;
}
export interface PostInsertParametrizationPlazoTipoActorFailure {
    type: 'POST_INSERT_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';
    error: any;
}
export interface PostUpdateParametrizationPlazoTipoActorSuccess {
    type: 'POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
    plazoTipoActor: PlazoTipoActorDto;
}
export interface PostUpdateParametrizationPlazoTipoActorFailure {
    type: 'POST_UPDATE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';
    error: any;
}
export interface PostDeleteParametrizationPlazoTipoActorSuccess {
    type: 'POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR';
    plazoTipoActor: PlazoTipoActorDto;
}
export interface PostDeleteParametrizationPlazoTipoActorFailure {
    type: 'POST_DELETE_PARAMETRIZATION_PLAZOS_TIPO_ACTOR_FAILURE';
    error: any;
}

// Plazos unidad ejecutora
export interface PostParametrizationPlazosUniejSuccess {
    type: 'POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
    plazosUniej: PlazoUndejDto[];
}
export interface PostParametrizationPlazosUniejFailure {
    type: 'POST_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';
    error: any;
}
export interface PostInsertParametrizationPlazoUniejSuccess {
    type: 'POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
    plazoUniej: PlazoUndejDto;
}
export interface PostInsertParametrizationPlazoUniejFailure {
    type: 'POST_INSERT_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';
    error: any;
}
export interface PostUpdateParametrizationPlazosUniejSuccess {
    type: 'POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
    plazoUniej: PlazoUndejDto;
}
export interface PostUpdateParametrizationPlazosUniejFailure {
    type: 'POST_UPDATE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';
    error: any;
}
export interface PostDeleteParametrizationPlazoUniejSuccess {
    type: 'POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA';
    plazoUniej: PlazoUndejDto;
}
export interface PostDeleteParametrizationPlazoUniejFailure {
    type: 'POST_DELETE_PARAMETRIZATION_PLAZOS_UNIDAD_EJECUTORA_FAILURE';
    error: any;
}

// Seguro General

export interface GetParametrizationSeguroGeneralSuccess {
    type: 'GET_PARAMETRIZATION_SEGURO_GENERAL';
    segurosGeneral: SegurosDto[];
}
export interface GetParametrizationSeguroGeneralFailure {
    type: 'GET_PARAMETRIZATION_SEGURO_GENERAL_FAILURE';
    error: any;
}
export interface PostUpdateParametrizationSeguroGeneralSuccess {
    type: 'POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL';
    seguroGeneral: SegurosDto;
}
export interface PostUpdateParametrizationSeguroGeneralFailure {
    type: 'POST_UPDATE_PARAMETRIZATION_SEGURO_GENERAL_FAILURE';
    error: any;
}

// Seguros por región
export interface GetParametrizationSeguroPorMunicipioSuccess {
    type: 'GET_PARAMETRIZATION_SEGURO_MUNICIPIO';
    segurosMunicipio: SeguroMunicipioDto[];
}
export interface GetParametrizationSeguroPorMunicipioFailure {
    type: 'GET_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';
    error: any;
}

export interface PostUpdateParametrizationSeguroPorRegionSuccess {
    type: 'POST_UPDATE_PARAMETRIZATION_SEGURO_REGION';
    request: any;
}
export interface PostUpdateParametrizationSeguroPorRegionFailure {
    type: 'POST_UPDATE_PARAMETRIZATION_SEGURO_REGION_FAILURE';
    error: any;
}

export interface PostInsertParametrizationSeguroPorMunicipioSuccess {
    type: 'POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO';
    seguroMunicipio: SeguroMunicipioDto;
}
export interface PostInsertParametrizationSeguroPorMunicipioFailure {
    type: 'POST_INSERT_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';
    error: any;
}
export interface PostUpdateParametrizationSeguroPorMunicipioSuccess {
    type: 'POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO';
    seguroMunicipio: SeguroMunicipioDto;
}
export interface PostUpdateParametrizationSeguroPorMunicipioFailure {
    type: 'POST_UPDATE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';
    error: any;
}
export interface PostDeleteParametrizationSeguroPorMunicipioSuccess {
    type: 'POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO';
    seguroMunicipio: SeguroMunicipioDto;
}
export interface PostDeleteParametrizationSeguroPorMunicipioFailure {
    type: 'POST_DELETE_PARAMETRIZATION_SEGURO_MUNICIPIO_FAILURE';
    error: any;
}

// Seguros por Ubicación

export interface GetParametrizationSeguroPorRegionSuccess {
    type: 'GET_PARAMETRIZATION_SEGURO_REGION';
    segurosRegion: SeguroRegionDto[];
}
export interface GetParametrizationSeguroPorRegionFailure {
    type: 'GET_PARAMETRIZATION_SEGURO_REGION_FAILURE';
    error: any;
}

// Liquidacion General
export interface GetParametrizationLiquidacionGeneralSuccess {
    type: 'GET_PARAMETRIZATION_LIQUIDACION_GENERAL';
    liquidacionGeneral: LiquidacionGeneralDto;
}
export interface GetParametrizationLiquidacionGeneralFailure {
    type: 'GET_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE';
    error: any;
}
export interface PostUpdateParametrizationLiquidacionGeneralSuccess {
    type: 'POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL';
    liquidacionGeneral: LiquidacionGeneralDto;
}
export interface PostUpdateParametrizationLiquidacionGeneralFailure {
    type: 'POST_UPDATE_PARAMETRIZATION_LIQUIDACION_GENERAL_FAILURE';
    error: any;
}

// 

export interface GetInsuranceParametrization {
    type: 'GET_INSURANCE_PARAMETRIZATION';
}

export interface GetInsuranceParametrizationSuccess {
    type: 'GET_INSURANCE_PARAMETRIZATION_SUCCESS';
    insurance: SeguroDto;
}

export interface GetInsuranceParametrizationFailure {
    type: 'GET_INSURANCE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface GetRateParametrization {
    type: 'GET_RATE_PARAMETRIZATION';
}

export interface GetRateParametrizationSuccess {
    type: 'GET_RATE_PARAMETRIZATION_SUCCESS';
    rate: TasaDto;
}

export interface GetRateParametrizationFailure {
    type: 'GET_RATE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface GetTermParametrization {
    type: 'GET_TERM_PARAMETRIZATION';
}

export interface GetTermParametrizationSuccess {
    type: 'GET_TERM_PARAMETRIZATION_SUCCESS';
    term: PlazosDto;
}

export interface GetTermParametrizationFailure {
    type: 'GET_TERM_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface UpdateInsuranceParametrization {
    type: 'UPDATE_INSURANCE_PARAMETRIZATION';
}

export interface UpdateInsuranceParametrizationSuccess {
    type: 'UPDATE_INSURANCE_PARAMETRIZATION_SUCCESS';
    insurance: SeguroDto;
}

export interface UpdateInsuranceParametrizationFailure {
    type: 'UPDATE_INSURANCE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface UpdateRateParametrization {
    type: 'UPDATE_RATE_PARAMETRIZATION';
}

export interface UpdateRateParametrizationSuccess {
    type: 'UPDATE_RATE_PARAMETRIZATION_SUCCESS';
    rate: TasaDto;
}

export interface UpdateRateParametrizationFailure {
    type: 'UPDATE_RATE_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface UpdateTermParametrization {
    type: 'UPDATE_TERM_PARAMETRIZATION';
}

export interface UpdateTermParametrizationSuccess {
    type: 'UPDATE_TERM_PARAMETRIZATION_SUCCESS';
    term: PlazosDto;
}

export interface UpdateTermParametrizationFailure {
    type: 'UPDATE_TERM_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface GetSettlementParametrization {
    type: 'GET_SETTLEMENT_PARAMETRIZATION';
}

export interface GetSettlementParametrizationSuccess {
    type: 'GET_SETTLEMENT_PARAMETRIZATION_SUCCESS';
    Settlement: LiquidacionDto;
}

export interface GetSettlementParametrizationFailure {
    type: 'GET_SETTLEMENT_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface UpdateSettlementParametrization {
    type: 'UPDATE_SETTLEMENT_PARAMETRIZATION';
}

export interface UpdateSettlementParametrizationSuccess {
    type: 'UPDATE_SETTLEMENT_PARAMETRIZATION_SUCCESS';
    Settlement: LiquidacionDto;
}

export interface UpdateSettlementParametrizationFailure {
    type: 'UPDATE_SETTLEMENT_PARAMETRIZATION_FAILURE';
    error: any;
}

export interface CrearParametroTransaccion {
    type: 'CREAR_PARAMETRO_TRANSACCION';
    parametro: ParametroTransaccionDto;
}

export interface ActualizarParametroTransaccion {
    type: 'ACTUALIZAR_PARAMETRO_TRANSACCION';
    parametro: ParametroTransaccionDto;
}

export interface ObtenerParametroTransaccion {
    type: 'OBTENER_PARAMETRO_TRANSACCION';
    parametros: ParametroTransaccionDto[];
}

export interface OnErrorInsurance {
    type: 'ON_ERROR';
    error: any;
}

export type ParametrizationActionTypes =
    | GetInsuranceParametrization
    | GetInsuranceParametrizationSuccess
    | GetInsuranceParametrizationFailure
    | UpdateInsuranceParametrization
    | UpdateInsuranceParametrizationSuccess
    | UpdateInsuranceParametrizationFailure
    | GetRateParametrization
    | GetRateParametrizationSuccess
    | GetRateParametrizationFailure
    | UpdateRateParametrization
    | UpdateRateParametrizationSuccess
    | UpdateRateParametrizationFailure
    | GetTermParametrization
    | GetTermParametrizationSuccess
    | GetTermParametrizationFailure
    | UpdateTermParametrization
    | UpdateTermParametrizationSuccess
    | UpdateTermParametrizationFailure
    | CrearParametroTransaccion
    | ActualizarParametroTransaccion
    | ObtenerParametroTransaccion
    | GetSettlementParametrization
    | GetSettlementParametrizationSuccess
    | GetSettlementParametrizationFailure
    | UpdateSettlementParametrization
    | UpdateSettlementParametrizationSuccess
    | UpdateSettlementParametrizationFailure
    | OnErrorInsurance
    | GetParametrizationPlazosGeneralSuccess
    | GetParametrizationPlazosGeneralFailure
    | PostUpdateParametrizationPlazosGeneralSuccess
    | PostUpdateParametrizationPlazosGeneralFailure
    | PostInsertParametrizationPlazoTipoActorSuccess
    | PostInsertParametrizationPlazoTipoActorFailure
    | PostParametrizationPlazosTipoActorSuccess
    | PostParametrizationPlazosTipoActorFailure
    | PostUpdateParametrizationPlazoTipoActorSuccess
    | PostUpdateParametrizationPlazoTipoActorFailure
    | PostDeleteParametrizationPlazoTipoActorSuccess
    | PostDeleteParametrizationPlazoTipoActorFailure
    | PostParametrizationPlazosUniejSuccess
    | PostParametrizationPlazosUniejFailure
    | PostInsertParametrizationPlazoUniejSuccess
    | PostInsertParametrizationPlazoUniejFailure
    | PostUpdateParametrizationPlazosUniejSuccess
    | PostUpdateParametrizationPlazosUniejFailure
    | PostDeleteParametrizationPlazoUniejSuccess
    | PostDeleteParametrizationPlazoUniejFailure
    | GetParametrizationSeguroGeneralSuccess
    | GetParametrizationSeguroGeneralFailure
    | PostUpdateParametrizationSeguroGeneralSuccess
    | PostUpdateParametrizationSeguroGeneralFailure
    | GetParametrizationSeguroPorMunicipioSuccess
    | GetParametrizationSeguroPorMunicipioFailure
    | GetParametrizationSeguroPorRegionSuccess
    | GetParametrizationSeguroPorRegionFailure
    | PostUpdateParametrizationSeguroPorRegionSuccess
    | PostUpdateParametrizationSeguroPorRegionFailure
    | GetParametrizationSeguroPorMunicipioFailure
    | PostInsertParametrizationSeguroPorMunicipioSuccess
    | PostInsertParametrizationSeguroPorMunicipioFailure
    | PostUpdateParametrizationSeguroPorMunicipioSuccess
    | PostUpdateParametrizationSeguroPorMunicipioFailure
    | PostDeleteParametrizationSeguroPorMunicipioSuccess
    | PostDeleteParametrizationSeguroPorMunicipioFailure
    | GetParametrizationLiquidacionGeneralSuccess
    | GetParametrizationLiquidacionGeneralFailure
    | PostUpdateParametrizationLiquidacionGeneralSuccess
    | PostUpdateParametrizationLiquidacionGeneralFailure;
