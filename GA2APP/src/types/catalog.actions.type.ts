import {
    GRADEDTO,
    IDENTIFICATIONTYPEDTO,
    EXECUTINGUNITDTO,
    CITYDTO,
    LIVINGPLACEDTO,
    FORCEDTO,
    ESTADOCIVILDTO,
    SEXODTO,
    DEPARTAMENTODTO,
    PAISDTO,
    ACTIVIDADECONOMICA,
    PROFESION,
    ESTADOVIVIENDA,
    TIVIVIENDA,
    TIPOVIVIENDA,
    TYPEHOME,
    TIPOCREDITO,
    PHONETYPE,
    MAILTYPE,
    CURRENCYTYPE,
    CATEGORIES,
    ADDRESSTYPE,
    TYPEREQUEST,
    TYPEMODALITY,
    TYPESUBMODALITY,
    ABONOUNICO,
    TIPOABONO,
    TIPOSUBSIDO,
    ESTADOCARTERA,
    TIPOCUENTA,
    ENTIDADBANCARIA,
    CatalogData,
    VALORCATALOGO,
    ENTIDADEDUCATIVA,
    PROGRAMAEDUCATIVO,
    NIVELEDUCATIVO,
    FORMATDTO,
    AVERAGECOSTDTO,
    ENTIDADSEGUROEDUCATIVO,
    TIPOAFILIACIONDTO,
    ESTADOAFILIACIONDTO,
    CAMPOCALIDADDTO,
    TIPOAFILIACIONPRODECENTEDTO,
    PORCENTAJEDESCUENTODTO,
    OFICINASREGISTRODTO,
    CAUSALBLOQUEODTO,
    REQUESTTYPEREALSTATEREGISTRATIONDTO,
    CAUSALBLOQUEOCUENTA,
    GRUPOSINCONSITENCIASDTO,
    INCONSISTENCIADTO,
    HERRAMIENTAINCONSISTENCIADTO,
    PUNTOSATENCIONDTO,
    IPC,
    ESTADOTAREADTO,
    PROGRAMAPORENTIDADDTO,
    NIVELEDUCATIVOPORPROGRAMADTO,
    MODALIDADAFILIACIONVETERANODTO,
    PAYDTO,
    TYPESFILEDTO,
    STATEFILEDTO,
    LINEDTO

} from './catalog.type';
import { CreditTypeModel } from '../models/credit-type.model';
import { ActivityStatusModel } from '../models/activity-status.model';

// Especificacion de accion
export const GETALL_IDENTIFICATIONTYPES = 'GET_ALL_IDENTIFICATION_TYPES';
export const GETALL_CAUSALES_BLOQUEO = 'GET_ALL_CAUSALES_BLOQUEO';
export const GETALL_ENTIDAD_EDUCATIVA = 'GET_ALL_ENTIDAD_EDUCATIVA';
export const GETALL_PROGRAMA_EDUCATIVO = 'GET_ALL_PROGRAMA_EDUCATIVO';
export const GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD = 'GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD';
export const GETALL_NIVEL_EDUCATIVO_POR_PROGRAMA = 'GETALL_NIVEL_EDUCATIVO_POR_PROGRAMA';
export const GETALL_NIVEL_EDUCATIVO = 'GET_ALL_NIVEL_EDUCATIVO';
export const GETALL_GRADES = 'GET_ALL_GRADES';
export const GET_CREDIT_LINE = 'GET_CREDIT_LINE';
export const GETALL_PAY = 'GET_ALL_PAY';
export const GETALL_CITIES = 'GET_ALL_CITIES';
export const GETALL_CITIES_VENDORS = 'GETALL_CITIES_VENDORS';
export const GETALL_EXECUTINGUNITS = 'GET_ALL_EXECUTING_UNITS';
export const GET_TYPESFILE = 'GET_TYPES_FILE';
export const GET_STATEFILE = 'GET_STATE_FILE';
export const GETALL_LIVINGPLACES = 'GET_ALL_LIVING_PLACES';
export const GETALL_FORCES = 'GET_ALL_FORCES';
export const GET_ALL_SEX_TYPES = 'GET_ALL_SEX_TYPES';
export const GET_ALL_CIVIL_STATES = 'GET_ALL_CIVIL_STATES';
export const GET_ALL_DEPARTMENTS = 'GET_ALL_DEPARTMENTS';
export const GET_BENEFICIARIO_CARTERA = 'GET_BENEFICIARIO_CARTERA';
export const GET_CONCEPTOS_CARTERA = 'GET_CONCEPTOS_CARTERA';
export const GET_TIPOS_COMPROBANTE = 'GET_TIPOS_COMPROBANTE';
export const GET_ALL_COUNTRIES = 'GET_ALL_COUNTRIES';
export const GET_ALL_OFFICES = 'GET_ALL_OFFICES';
export const GET_ALL_ECONOMIC_ACTIVITIES = 'GET_ALL_ECONOMIC_ACTIVITIES';
export const GET_ALL_PROFESSIONS = 'GET_ALL_PROFESSIONS';

export const GET_ACTIVITIES_STATUS = 'GET_ACTIVITIES_STATUS';
export const GET_ACTIVITIES_STATUS_SUCCESS = 'GET_ACTIVITIES_STATUS_SUCCESS';

export const GET_CREDIT_TYPES = 'GET_CREDIT_TYPES';
export const GET_CREDIT_TYPES_SUCCESS = 'GET_CREDIT_TYPES_SUCCESS';

export const GET_CITIES_BY_DEPARTMENT = 'GET_CITIES_BY_DEPARTMENT';
export const GET_CITIES_BY_DEPARTMENT_SUCCESS =
    'GET_CITIES_BY_DEPARTMENT_SUCCESS';

export const GET_PHONES_TYPES = 'GET_PHONES_TYPES';
export const GET_PHONES_TYPES_SUCCESS = 'GET_PHONES_TYPES_SUCCESS';

export const GET_MAIL_TYPES = 'GET_MAIL_TYPES';
export const GET_MAIL_TYPES_SUCCESS = 'GET_MAIL_TYPES_SUCCESS';

export const GET_CURRENCY_TYPES = 'GET_CURRENCY_TYPES';
export const GET_CURRENCY_TYPES_SUCCESS = 'GET_CURRENCY_TYPES_SUCCESS';

export const GET_CATEGORY = 'GET_CATEGORY';
export const GET_CATEGORY_SUCCESS = 'GET_CATEGORY_SUCCESS';

export const ON_ERROR = 'ON_ERROR';
export const GET_ALL_TI_VIVIENDA = 'GET_ALL_TI_VIVIENDA';
export const GET_ALL_TYPE_HOME = 'GET_ALL_TYPE_HOME';
export const GET_ALL_TIPO_VIVIENDA = 'GET_ALL_TIPO_VIVIENDA';
export const GET_ALL_ESTADO_VIVIENDA = 'GET_ALL_ESTADO_VIVIENDA';
export const GET_ALL_TIPO_CREDITO = 'GET_ALL_TIPO_CREDITO';
export const GET_ALL_ESTADO_CARTERA = 'GET_ALL_ESTADO_CARTERA';

export const GET_PAYROLL_CATEGORY_CONCEPT = 'GET_PAYROLL_CATEGORY_CONCEPT';
export const GET_PAYROLL_CATEGORY_CONCEPT_SUCCESS =
    'GET_PAYROLL_CATEGORY_CONCEPT_SUCCESS';

export const GET_EMAIL_DOMAINS = 'GET_EMAIL_DOMAINS';
export const GET_EMAIL_DOMAINS_SUCCESS = 'GET_EMAIL_DOMAINS_SUCCESS';

export const GET_ADDRESS_TYPE = 'GET_ADDRESS_TYPE';
export const GET_ADDRESS_TYPE_SUCCESS = 'GET_ADDRESS_TYPE_SUCCESS';

export const GET_INFO_TURNO = 'GET_INFO_TURNO';

export const GETALL_TYPE_REQUEST = 'GETALL_TYPE_REQUEST';
export const GETALL_TYPE_MODALITY = 'GETALL_TYPE_MODALITY';
export const GETALL_TYPE_SUBMODALITY = 'GETALL_TYPE_SUBMODALITY';
export const GET_ALL_HOME_TYPE = 'GET_ALL_HOME_TYPE';

export const GET_ALL_ABONO_UNICO = 'GET_ALL_ABONO_UNICO';

export const GET_ALL_TIPO_ABONO = 'GET_ALL_TIPO_ABONO';

export const GET_ALL_TIPO_SUBSIDIO = 'GET_ALL_TIPO_SUBSIDIO';

export const GET_ALL_TIPO_CUENTA = 'GET_ALL_TIPO_CUENTA';

export const GET_ALL_ENTIDAD_BANCARIA = 'GET_ALL_ENTIDAD_BANCARIA';

export const GET_ALL_VALOR_CATALOGO_POR_ID = 'GET_ALL_VALOR_CATALOGO_POR_ID';
export const GET_ALL_VALOR_CATALOGO_POR_NOMBRE = 'GET_ALL_VALOR_CATALOGO_POR_NOMBRE';

export const GET_ALL_VALOR_CATALOGO_PARENTESCO = 'GET_ALL_VALOR_CATALOGO_PARENTESCO';

export const GET_FORMAT = 'GET_FORMAT';
export const GET_FORMAT_SUCCESS = 'GET_FORMAT_SUCCESS';
export const GET_FORMAT_FAILURE = 'GET_FORMAT_FAILURE';
export const GET_ALL_ENTIDAD_SEGURO_EDUCATIVO = 'GET_ALL_ENTIDAD_SEGURO_EDUCATIVO';


export const GET_AVERAGE_COST = 'GET_AVERAGE_COST';
export const GET_AVERAGE_COST_SUCCESS = 'GET_AVERAGE_COST_SUCCESS';
export const GET_AVERAGE_COST_FAILURE = 'GET_AVERAGE_COST_FAILURE';

export const GET_INSERT_TYPE_ID_SUCCESS = 'GET_INSERT_TYPE_ID_SUCCESS';
export const GET_INSERT_TYPE_ID_FAILURE = 'GET_INSERT_TYPE_ID_FAILURE';
export const UPDATE_TYPE_ID_SUCCESS = 'UPDATE_TYPE_ID_SUCCESS';

export const INSERT_TYPE_GENERO_SUCCESS = 'INSERT_TYPE_GENERO_SUCCESS';
export const UPDATE_TYPE_GENERO_SUCCESS = 'UPDATE_TYPE_GENERO_SUCCESS';

export const INSERT_TYPE_FORCE_SUCCESS = 'INSERT_TYPE_FORCE_SUCCESS';
export const UPDATE_TYPE_FORCE_SUCCESS = 'UPDATE_TYPE_FORCE_SUCCESS';

export const GET_TYPE_AFFILIATE = 'GET_TYPE_AFFILIATE';
export const GET_TYPE_AFFILIATE_SUCCESS = 'GET_TYPE_AFFILIATE_SUCCESS';
export const GET_TYPE_AFFILIATE_FAILURE = 'GET_TYPE_AFFILIATE_FAILURE';

export const GET_AFFILIATE_STATE = 'GET_AFFILIATE_STATE';
export const GET_AFFILIATE_STATE_SUCCESS = 'GET_AFFILIATE_STATE_SUCCESS';
export const GET_AFFILIATE_STATE_FAILURE = 'GET_AFFILIATE_STATE_FAILURE';

export const GET_QUALITY_FIELD = 'GET_QUALITY_FIELD';
export const GET_QUALITY_FIELD_SUCCESS = 'GET_QUALITY_FIELD_SUCCESS';
export const GET_QUALITY_FIELD_FAILURE = 'GET_QUALITY_FIELD_FAILURE';

export const GET_AFFILIATE_TYPE_PROCEDURE = 'GET_AFFILIATE_TYPE_PROCEDURE';
export const GET_AFFILIATE_TYPE_PROCEDURE_SUCCESS = 'GET_AFFILIATE_TYPE_PROCEDURE_SUCCESS';
export const GET_AFFILIATE_TYPE_PROCEDURE_FAILURE = 'GET_AFFILIATE_TYPE_PROCEDURE_FAILURE';

export const INSERT_TYPE_CATEGORY_SUCCESS = 'INSERT_TYPE_CATEGORY_SUCCESS';
export const UPDATE_TYPE_CATEGORY_SUCCESS = 'UPDATE_TYPE_CATEGORY_SUCCESS';

export const GET_DISCOUNT_PERCENTAGE = 'GET_DISCOUNT_PERCENTAGE';
export const GET_DISCOUNT_PERCENTAGE_SUCCESS = 'GET_DISCOUNT_PERCENTAGE_SUCCESS';
export const GET_DISCOUNT_PERCENTAGE_FAILURE = 'GET_DISCOUNT_PERCENTAGE_FAILURE';
export const GET_IPC = 'GET_IPC';

export const GET_ALL_GROUP_INCONSISTENCIES = 'GET_ALL_GROUP_INCONSISTENCIES';

export const GET_ALL_INCONSISTENCIES_BY_GROUP = 'GET_ALL_INCONSISTENCIES_BY_GROUP';

export const GET_ALL_TOOLS_INCONSISTENCIES = 'GET_ALL_TOOLS_INCONSISTENCIES';

export const GET_ALL_POINTS_ATTENTION = 'GET_ALL_POINTS_ATTENTION';

export const GET_ALL_STATE_REQUEST = 'GET_ALL_STATE_REQUEST';

export const GET_REGISTRATION_OFFICE = 'GET_REGISTRATION_OFFICE';
export const GET_REGISTRATION_OFFICE_SUCCESS = 'GET_REGISTRATION_OFFICE_SUCCESS';
export const GET_REGISTRATION_OFFICE_FAILURE = 'GET_REGISTRATION_OFFICE_FAILURE';

export const GET_CAUSAL_BLOCKAGE = 'GET_CAUSAL_BLOCKAGE';
export const GET_CAUSAL_BLOCKAGE_SUCCESS = 'GET_CAUSAL_BLOCKAGE_SUCCESS';
export const GET_CAUSAL_BLOCKAGE_FAILURE = 'GET_CAUSAL_BLOCKAGE_FAILURE';

export const GET_REQUEST_TYPE_REAL_STATE_REGISTRATION = 'GET_REQUEST_TYPE_REAL_STATE_REGISTRATION';
export const GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_SUCCESS = 'GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_SUCCESS';
export const GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_FAILURE = 'GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_FAILURE';

export const GET_MODALIDAD_AFILIACION_VETERANO = 'GET_MODALIDAD_AFILIACION_VETERANO';
export const GET_MODALIDAD_AFILIACION_VETERANO_SUCCESS= 'GET_MODALIDAD_AFILIACION_VETERANO_SUCCESS';
export const GET_MODALIDAD_AFILIACION_VETERANO_FAILURE = 'GET_MODALIDAD_AFILIACION_VETERANO_FAILURE';

export const GET_USERS = 'GET_USERS';
export const GET_USERS_SUCCESS = 'GET_USERS_SUCCESS';
export const GET_USERS_FAILURE = 'GET_USERS_FAILURE';

export interface GetIPCType {
    type: 'GET_IPC';
    ipc: IPC;
}

export interface GetIdentificationType {
    type: 'GET_ALL_IDENTIFICATION_TYPES';
    identificationTypes: IDENTIFICATIONTYPEDTO[];
}

export interface GetUsersType {
    type: 'GET_USERS';
    users: any[];
}

export interface GetCausalesBloqueoType {
    type: 'GET_ALL_CAUSALES_BLOQUEO';
    causalesBloqueoTypes: CAUSALBLOQUEOCUENTA[];
}

export interface GetGrades {
    type: 'GET_ALL_GRADES';
    grades: GRADEDTO[];
}

export interface GetLine {
    type: 'GET_CREDIT_LINE';
    line: LINEDTO[];
}

export interface GetPay {
    type: 'GET_ALL_PAY';
    pay: PAYDTO[];
}

export interface GetCities {
    type: 'GET_ALL_CITIES';
    cities: CITYDTO[];
}
export interface GetCitiesVendors {
    type: 'GETALL_CITIES_VENDORS';
    citiesVendors: CITYDTO[];
}

export interface GetExecutingUnits {
    type: 'GET_ALL_EXECUTING_UNITS';
    executingUnits: EXECUTINGUNITDTO[];
}
export interface GetAllTypeFiles {
    type: 'GET_TYPES_FILE';
    typesFile: TYPESFILEDTO[];
}
export interface GetAllStateFiles {
    type: 'GET_STATE_FILE';
    stateFile: STATEFILEDTO[];
}
export interface GetLivingPlaces {
    type: 'GET_ALL_LIVING_PLACES';
    livingPlaces: LIVINGPLACEDTO[];
}
export interface GetForces {
    type: 'GET_ALL_FORCES';
    forces: FORCEDTO[];
}

export interface GetCivilStates {
    type: 'GET_ALL_CIVIL_STATES';
    civilstates: ESTADOCIVILDTO[];
}

export interface GetSexTypes {
    type: 'GET_ALL_SEX_TYPES';
    sextypes: SEXODTO[];
}
export interface GetDepartments {
    type: 'GET_ALL_DEPARTMENTS';
    departments: DEPARTAMENTODTO[];
}
export interface GetBeneficiarioCartera {
    type: 'GET_BENEFICIARIO_CARTERA';
    beneficiarioCartera: any;
}
export interface GetConceptosCartera {
    type: 'GET_CONCEPTOS_CARTERA';
    conceptosCartera: any;
}
export interface GetTipoComprobante {
    type: 'GET_TIPOS_COMPROBANTE';
    tipoComprobante: any;
}
export interface GetCountries {
    type: 'GET_ALL_COUNTRIES';
    countries: PAISDTO[];
}

export interface GetEconomicActivities {
    type: 'GET_ALL_ECONOMIC_ACTIVITIES';
    economicActivities: ACTIVIDADECONOMICA[];
}

export interface GetProfessions {
    type: 'GET_ALL_PROFESSIONS';
    professions: PROFESION[];
}

export interface GetAllTipoCredito {
    type: 'GET_ALL_TIPO_CREDITO';
    tipoCredito: TIPOCREDITO[];
}

export interface GetAllTiVivienda {
    type: 'GET_ALL_TI_VIVIENDA';
    tiVivienda: TIVIVIENDA[];
}


export interface GetAllTypeHome {
    type: 'GET_ALL_TYPE_HOME';
    typeHome: TYPEHOME[];
}

export interface GetAllTipoVivienda {
    type: 'GET_ALL_TIPO_VIVIENDA';
    tipoVivienda: TIPOVIVIENDA[];
}

export interface GetAllEstadoCartera {
    type: 'GET_ALL_ESTADO_CARTERA';
    estadoCartera: ESTADOCARTERA[];
}

/* export interface GettiviviendaTypesSuccess {
    type: 'GET_TI_VIVIENDA_TYPES_SUCCESS';
    tiViviendaTypes: TIVIVIENDA[];
} */

export interface GetAllEstadoVivienda {
    type: 'GET_ALL_ESTADO_VIVIENDA';
    estadoVivienda: ESTADOVIVIENDA[];
}

export interface GetActivitiesStatus {
    type: 'GET_ACTIVITIES_STATUS';
}

export interface GetActivitiesStatusSuccess {
    type: 'GET_ACTIVITIES_STATUS_SUCCESS';
    activitiesStatus: ActivityStatusModel[];
}

export interface GetCreditTypes {
    type: 'GET_CREDIT_TYPES';
}

export interface GetCreditTypesSuccess {
    type: 'GET_CREDIT_TYPES_SUCCESS';
    creditTypes: CreditTypeModel[];
}

export interface GetCitiesByDepartment {
    type: 'GET_CITIES_BY_DEPARTMENT';
}

export interface GetCitiesByDepartmentSuccess {
    type: 'GET_CITIES_BY_DEPARTMENT_SUCCESS';
    cities: CITYDTO[];
}

export interface GetPhoneType {
    type: 'GET_PHONES_TYPES';
}

export interface GetPhoneTypeSuccess {
    type: 'GET_PHONES_TYPES_SUCCESS';
    phoneTypes: PHONETYPE[];
}

export interface GetMailType {
    type: 'GET_MAIL_TYPES';
}

export interface GetMailTypeSuccess {
    type: 'GET_MAIL_TYPES_SUCCESS';
    MailTypes: MAILTYPE[];
}

export interface GetCurrencyType {
    type: 'GET_CURRENCY_TYPES';
}

export interface GetCurrencyTypeSuccess {
    type: 'GET_CURRENCY_TYPES_SUCCESS';
    CurrencyTypes: CURRENCYTYPE[];
}

export interface GetCategory {
    type: 'GET_CATEGORY';
}

export interface GetCategorySuccess {
    type: 'GET_CATEGORY_SUCCESS';
    Categories: CATEGORIES[];
}

export interface GetPayrollCategoryConcept {
    type: 'GET_PAYROLL_CATEGORY_CONCEPT';
}

export interface GetPayrollCategoryConceptSuccess {
    type: 'GET_PAYROLL_CATEGORY_CONCEPT_SUCCESS';
    payrollCategoriesConcept: any[];
}

export interface GetEmailDomains {
    type: 'GET_EMAIL_DOMAINS';
}

export interface GetEmailDomainsSuccess {
    type: 'GET_EMAIL_DOMAINS_SUCCESS';
    emailDomains: any[];
}

export interface GetAddressTypes {
    type: 'GET_ADDRESS_TYPE';
}

export interface GetAddressTypesSuccess {
    type: 'GET_ADDRESS_TYPE_SUCCESS';
    addressTypes: ADDRESSTYPE[];
}

export interface GetTypeRequest {
    type: 'GETALL_TYPE_REQUEST';
    typeRequest: TYPEREQUEST[];
}

export interface GetTypeModality {
    type: 'GETALL_TYPE_MODALITY';
    typeModality: TYPEMODALITY[];
}

export interface GetTypeSubModality {
    type: 'GETALL_TYPE_SUBMODALITY';
    typeSubModality: TYPESUBMODALITY[];
}

export interface GetAllAbonoUnico {
    type: 'GET_ALL_ABONO_UNICO';
    abonoUnico: ABONOUNICO[];
}


export interface GetAllTipoAbono {
    type: 'GET_ALL_TIPO_ABONO';
    tipoAbono: TIPOABONO[];
}

export interface GetAllTipoSubsidio {
    type: 'GET_ALL_TIPO_SUBSIDIO';
    tipoSubsidio: TIPOSUBSIDO[];
}


export interface GetAllTipoCuenta {
    type: 'GET_ALL_TIPO_CUENTA';
    tipoCuenta: TIPOCUENTA[];
}

export interface GetAllEntidadBancaria {
    type: 'GET_ALL_ENTIDAD_BANCARIA';
    entidadBancaria: ENTIDADBANCARIA[];
}

export interface GetConsultarInformacionTurno {
	type: 'GET_INFO_TURNO';
	infoTurno: CatalogData[];
}

export interface GetAllEntidadSeguroEducativo {
    type: 'GET_ALL_ENTIDAD_SEGURO_EDUCATIVO';
    entidadSeguroEducativo: ENTIDADSEGUROEDUCATIVO[];
}

export interface GetAllCatalogoPorId {
    type: 'GET_ALL_VALOR_CATALOGO_POR_ID';
    valorCatalogo: VALORCATALOGO[];
}

export interface GetAllCatalogoPorNombre {
    type: 'GET_ALL_VALOR_CATALOGO_POR_NOMBRE';
    nombreCatalogo: string;
    valorCatalogo: VALORCATALOGO[];
}
export interface GetAllCatalogoParentesco {
    type: 'GET_ALL_VALOR_CATALOGO_PARENTESCO';
    parentescoCatalogo: VALORCATALOGO[];
}

export interface GetEntidadEducativa {
    type: 'GET_ALL_ENTIDAD_EDUCATIVA';
    entidadEducativa: ENTIDADEDUCATIVA[];
}

export interface GetProgramaEducativo {
    type: 'GET_ALL_PROGRAMA_EDUCATIVO';
    programaEducativo: PROGRAMAEDUCATIVO[];
}

export interface GetNivelEducativo {
    type: 'GET_ALL_NIVEL_EDUCATIVO';
    nivelEducativo: NIVELEDUCATIVO[];
}



export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export interface GetFormat {
    type: 'GET_FORMAT'
}

export interface GetFormatSuccess {
    type: 'GET_FORMAT_SUCCESS'
    Formato: FORMATDTO[] | null;
}

export interface GetFormatFailure {
    type: 'GET_FORMAT_FAILURE'
    Formato: FORMATDTO[] | null;
}

export interface GetAverageCost {
    type: 'GET_AVERAGE_COST'
}

export interface GetAverageCostSuccess {
    type: 'GET_AVERAGE_COST_SUCCESS'
    MediosDeEnvio: AVERAGECOSTDTO[] | null;
}

export interface GetAverageCostFailure {
    type: 'GET_AVERAGE_COST_FAILURE'
    MediosDeEnvio: AVERAGECOSTDTO[] | null;
}

export interface GetInsertTypeIdSuccess {
    type: 'GET_INSERT_TYPE_ID_SUCCESS'
    IdentificacionType: IDENTIFICATIONTYPEDTO[];
}
export interface GetInsertTypeIdFailure {
    type: 'GET_INSERT_TYPE_ID_FAILURE'
    error: any;
}
export interface UpdateTypeIdSuccess {
    type: 'UPDATE_TYPE_ID_SUCCESS'
    typeId: IDENTIFICATIONTYPEDTO;
}
export interface InsertTypeGeneroSuccess {
    type: 'INSERT_TYPE_GENERO_SUCCESS'
    generotype: any;
}
export interface UpdateTypeGeneroSuccess {
    type: 'UPDATE_TYPE_GENERO_SUCCESS'
    sextype: SEXODTO;
}
export interface InsertTypeForceSuccess {
    type: 'INSERT_TYPE_FORCE_SUCCESS'
    force: FORCEDTO;
}

export interface UpdateTypeForceSuccess {
    type: 'UPDATE_TYPE_FORCE_SUCCESS'
    force: FORCEDTO;
}

export interface GetTypeAffiliate {
    type: 'GET_TYPE_AFFILIATE';
}

export interface GetTypeAffiliateSuccess {
    type: 'GET_TYPE_AFFILIATE_SUCCESS';
    tipoAfiliacion: TIPOAFILIACIONDTO[];
}

export interface GetTypeAffiliateFailure {
    type: 'GET_TYPE_AFFILIATE_FAILURE';
    tipoAfiliacion: TIPOAFILIACIONDTO[] | null;
}
export interface InsertTypeCategorySuccess {
    type: 'INSERT_TYPE_CATEGORY_SUCCESS'
    category: CATEGORIES;
}

export interface UpdateTypeCategorySuccess {
    type: 'UPDATE_TYPE_CATEGORY_SUCCESS'
    category: CATEGORIES;
}

export interface GetAffiliateState {
    type: 'GET_AFFILIATE_STATE';
}

export interface GetAffiliateStateSuccess {
    type: 'GET_AFFILIATE_STATE_SUCCESS';
    estadoAfiliacion: ESTADOAFILIACIONDTO[];
}

export interface GetAffiliateStateFailure {
    type: 'GET_AFFILIATE_STATE_FAILURE';
    estadoAfiliacion: ESTADOAFILIACIONDTO[] | null;
}

export interface GetQualityField {
    type: 'GET_QUALITY_FIELD';
}

export interface GetQualityFieldSuccess {
    type: 'GET_QUALITY_FIELD_SUCCESS';
    campoCalidad: CAMPOCALIDADDTO[];
}

export interface GetQualityFieldFailure {
    type: 'GET_QUALITY_FIELD_FAILURE';
    campoCalidad: CAMPOCALIDADDTO[] | null;
}

export interface GetAffiliateTypeProcedure {
    type: 'GET_AFFILIATE_TYPE_PROCEDURE'
}

export interface GetAffiliateTypeProcedureSuccess {
    type: 'GET_AFFILIATE_TYPE_PROCEDURE_SUCCESS';
    tipoAfiliacionProcedente: TIPOAFILIACIONPRODECENTEDTO[];
}

export interface GetAffiliateTypeProcedureFailure {
    type: 'GET_AFFILIATE_TYPE_PROCEDURE_FAILURE';
    tipoAfiliacionProcedente: TIPOAFILIACIONPRODECENTEDTO[] | null;
}

export interface GetDiscountPercentage {
    type: 'GET_DISCOUNT_PERCENTAGE'
}

export interface GetDiscountPercentageSuccess {
    type: 'GET_DISCOUNT_PERCENTAGE_SUCCESS';
    porcentajeDescuento: PORCENTAJEDESCUENTODTO[];
}

export interface GetDiscountPercentageFailure {
    type: 'GET_DISCOUNT_PERCENTAGE_FAILURE';
    porcentajeDescuento: PORCENTAJEDESCUENTODTO[] | null;
}

export interface GetAllGroupInconsistenciesSuccess {
    type: 'GET_ALL_GROUP_INCONSISTENCIES';
    gruposInconsistencias: GRUPOSINCONSITENCIASDTO[];
}
export interface GetInconsistenciesByGroupSuccess {
    type: 'GET_ALL_INCONSISTENCIES_BY_GROUP';
    inconsistencias: INCONSISTENCIADTO[];
}
export interface GetAllToolsInconsistenciesSuccess {
    type: 'GET_ALL_TOOLS_INCONSISTENCIES';
    herramientasInconsistencia: HERRAMIENTAINCONSISTENCIADTO[];
}

export interface GetAllPointsAttentionSuccess {
    type: 'GET_ALL_POINTS_ATTENTION';
    puntosAtencion: PUNTOSATENCIONDTO[];
}

export interface GetAllStatesRequestSuccess {
    type: 'GET_ALL_STATE_REQUEST';
    estadosTareas: ESTADOTAREADTO[];
}

export interface GetRegistrationOffice {
    type: 'GET_REGISTRATION_OFFICE';
}

export interface GetRegistrationOfficeSuccess {
    type: 'GET_REGISTRATION_OFFICE_SUCCESS';
    oficinasRegistro: OFICINASREGISTRODTO[];
}

export interface GetRegistrationOfficeFailure {
    type: 'GET_REGISTRATION_OFFICE_FAILURE';
    oficinasRegistro: OFICINASREGISTRODTO[] | null;
}

export interface GetCausalBlockage {
    type: 'GET_CAUSAL_BLOCKAGE';
}

export interface GetCausalBlockageSuccess {
    type: 'GET_CAUSAL_BLOCKAGE_SUCCESS';
    causalBloqueo: CAUSALBLOQUEODTO[];
}

export interface GetCausalBlockageFailure {
    type: 'GET_CAUSAL_BLOCKAGE_FAILURE';
    causalBloqueo: CAUSALBLOQUEODTO[] | null;
}

export interface GetRequestTypeRealStateRegistration {
    type: 'GET_REQUEST_TYPE_REAL_STATE_REGISTRATION';
}

export interface GetRequestTypeRealStateRegistrationSuccess {
    type: 'GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_SUCCESS';
    tipoSolicitudMatriculaInmobiliaria: REQUESTTYPEREALSTATEREGISTRATIONDTO[];
}

export interface GetRequestTypeRealStateRegistrationFailure {
    type: 'GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_FAILURE';
    tipoSolicitudMatriculaInmobiliaria: REQUESTTYPEREALSTATEREGISTRATIONDTO[] | null;
}

export interface GetAllEntidadEducativa {
    type: 'GETALL_ENTIDAD_EDUCATIVA';
    entidadEducativa: ENTIDADEDUCATIVA[];
}

export interface GetAllProgramaPorEntidad {
    type: 'GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD';
    programaPorEntidad: PROGRAMAPORENTIDADDTO[];
}

export interface GetAllNivelEducativoPorPrograma {
    type: 'GETALL_NIVEL_EDUCATIVO_POR_PROGRAMA';
    nivelPorPrograma: NIVELEDUCATIVOPORPROGRAMADTO[];
}

export interface GetModalidadAfiliacionVeterano {
    type: 'GET_MODALIDAD_AFILIACION_VETERANO';
}
export interface GetModalidadAfiliacionVeteranoSuccess {
    type: 'GET_MODALIDAD_AFILIACION_VETERANO_SUCCESS';
    ModalidadAfiliacionVeterano: MODALIDADAFILIACIONVETERANODTO[];
}

export interface GetModalidadAfiliacionVeteranoFailure {
    type: 'GET_MODALIDAD_AFILIACION_VETERANO_FAILURE';
    ModalidadAfiliacionVeterano: MODALIDADAFILIACIONVETERANODTO[] | null;
}

export type CatalogActionTypes =
    | GetIdentificationType
    | GetUsersType
    | GetCausalesBloqueoType
    | GetGrades
    | GetLine
    | GetPay
    | GetDepartments
    | GetCountries
    | GetCities
    | GetExecutingUnits
    | GetAllTypeFiles
    | GetAllStateFiles
    | GetLivingPlaces
    | GetForces
    | OnError
    | GetCivilStates
    | GetSexTypes
    | GetEconomicActivities
    | GetProfessions
    | GetAllTiVivienda
    | GetAllTypeHome
    | GetAllTipoVivienda
    | GetAllEstadoCartera
    | GetCreditTypesSuccess
    | GetAllEstadoVivienda
    | GetAllTipoCredito
    | GetProfessions
    | GetActivitiesStatus
    | GetActivitiesStatusSuccess
    | GetCreditTypes
    | GetCreditTypesSuccess
    | GetCitiesByDepartment
    | GetCitiesByDepartmentSuccess
    | GetPhoneType
    | GetPhoneTypeSuccess
    | GetMailType
    | GetMailTypeSuccess
    | GetCurrencyType
    | GetCurrencyTypeSuccess
    | GetCategory
    | GetCategorySuccess
    | GetPayrollCategoryConcept
    | GetPayrollCategoryConceptSuccess
    | GetEmailDomains
    | GetEmailDomainsSuccess
    | GetAddressTypes
    | GetTypeRequest
    | GetTypeModality
    | GetTypeSubModality
    | GetAddressTypesSuccess
    | GetAllAbonoUnico
    | GetAllTipoAbono
    | GetCitiesVendors
    | GetAllTipoCuenta
    | GetAllTipoSubsidio
    | GetAllCatalogoPorId
    | GetAllCatalogoPorNombre
    | GetAllCatalogoParentesco
    | GetEntidadEducativa
    | GetProgramaEducativo
    | GetNivelEducativo
    | GetAllEntidadBancaria
    | GetConsultarInformacionTurno
    | GetFormat
    | GetFormatFailure
    | GetFormatSuccess
    | GetAverageCost
    | GetAverageCostFailure
    | GetAverageCostSuccess
    | GetAllEntidadSeguroEducativo
    | GetInsertTypeIdSuccess
    | GetInsertTypeIdFailure
    | UpdateTypeIdSuccess
    | InsertTypeGeneroSuccess
    | UpdateTypeGeneroSuccess
    | InsertTypeForceSuccess
    | UpdateTypeForceSuccess
    | GetTypeAffiliate
    | GetTypeAffiliateSuccess
    | GetTypeAffiliateFailure
    | GetAffiliateState
    | GetAffiliateStateSuccess
    | GetAffiliateStateFailure
    | GetQualityField
    | GetQualityFieldSuccess
    | GetQualityFieldFailure
    | GetAffiliateTypeProcedure
    | GetAffiliateTypeProcedureSuccess
    | GetAffiliateTypeProcedureFailure
    | InsertTypeCategorySuccess
    | UpdateTypeCategorySuccess
    | UpdateTypeForceSuccess
    | GetDiscountPercentage
    | GetDiscountPercentageSuccess
    | GetDiscountPercentageFailure
    | GetRegistrationOffice
    | GetRegistrationOfficeSuccess
    | GetRegistrationOfficeFailure
    | GetCausalBlockage
    | GetCausalBlockageSuccess
    | GetCausalBlockageFailure
    | GetRequestTypeRealStateRegistration
    | GetRequestTypeRealStateRegistrationSuccess
    | GetRequestTypeRealStateRegistrationFailure
    | GetIPCType
    | GetAllGroupInconsistenciesSuccess
    | GetInconsistenciesByGroupSuccess
    | GetAllToolsInconsistenciesSuccess
    | GetAllPointsAttentionSuccess
    | GetIPCType
    | GetAllStatesRequestSuccess
    | GetAllEntidadEducativa
    | GetAllProgramaPorEntidad
    | GetAllNivelEducativoPorPrograma
    | GetModalidadAfiliacionVeterano
    | GetModalidadAfiliacionVeteranoSuccess
    | GetModalidadAfiliacionVeteranoFailure
    | GetConceptosCartera
    | GetTipoComprobante
    | GetBeneficiarioCartera;


