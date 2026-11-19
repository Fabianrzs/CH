import {
    CatalogActionTypes,
    GETALL_CITIES,
    GETALL_EXECUTINGUNITS,
    GET_TYPESFILE,
    GET_STATEFILE,
    GETALL_FORCES,
    GETALL_GRADES,
    GET_CREDIT_LINE,
    GETALL_IDENTIFICATIONTYPES,
    GETALL_CAUSALES_BLOQUEO,
    GETALL_LIVINGPLACES,
    GET_ALL_CIVIL_STATES,
    GET_ALL_SEX_TYPES,
    GET_ALL_COUNTRIES,
    GET_ALL_DEPARTMENTS,
    GET_BENEFICIARIO_CARTERA,
    GET_CONCEPTOS_CARTERA,
    GET_ALL_TI_VIVIENDA,
    GET_ALL_TIPO_VIVIENDA,
    GET_ALL_ESTADO_CARTERA,
    GET_ALL_ESTADO_VIVIENDA,
    ON_ERROR,
    GET_ALL_ECONOMIC_ACTIVITIES,
    GET_ALL_PROFESSIONS,
    GET_ACTIVITIES_STATUS,
    GET_ACTIVITIES_STATUS_SUCCESS,
    GET_CREDIT_TYPES,
    GET_CREDIT_TYPES_SUCCESS,
    GET_CITIES_BY_DEPARTMENT,
    GET_CITIES_BY_DEPARTMENT_SUCCESS,
    GET_PHONES_TYPES,
    GET_PHONES_TYPES_SUCCESS,
    GET_MAIL_TYPES,
    GET_MAIL_TYPES_SUCCESS,
    GET_CURRENCY_TYPES_SUCCESS,
    GET_CURRENCY_TYPES,
    GET_CATEGORY,
    GET_CATEGORY_SUCCESS,
    GET_PAYROLL_CATEGORY_CONCEPT_SUCCESS,
    GET_PAYROLL_CATEGORY_CONCEPT,
    GET_EMAIL_DOMAINS,
    GET_EMAIL_DOMAINS_SUCCESS,
    GET_ADDRESS_TYPE,
    GET_ADDRESS_TYPE_SUCCESS,
    GET_ALL_TYPE_HOME,
    GET_ALL_ABONO_UNICO,
    GET_ALL_TIPO_ABONO,
    GET_ALL_TIPO_SUBSIDIO,
    GET_ALL_TIPO_CREDITO,
    GETALL_TYPE_REQUEST,
    GETALL_TYPE_MODALITY,
    GETALL_TYPE_SUBMODALITY,
    GETALL_CITIES_VENDORS,
    GET_ALL_TIPO_CUENTA,
    GET_ALL_ENTIDAD_BANCARIA,
    GET_INFO_TURNO,
    GET_ALL_VALOR_CATALOGO_POR_ID,
    GET_ALL_VALOR_CATALOGO_POR_NOMBRE,
    GET_ALL_VALOR_CATALOGO_PARENTESCO,
    GETALL_ENTIDAD_EDUCATIVA,
    GETALL_PROGRAMA_EDUCATIVO,
    GETALL_NIVEL_EDUCATIVO,
    GET_FORMAT_FAILURE,
    GET_FORMAT_SUCCESS,
    GET_FORMAT,
    GET_AVERAGE_COST_FAILURE,
    GET_AVERAGE_COST,
    GET_AVERAGE_COST_SUCCESS,
    GET_ALL_ENTIDAD_SEGURO_EDUCATIVO,
    GET_INSERT_TYPE_ID_SUCCESS,
    GET_INSERT_TYPE_ID_FAILURE,
    UPDATE_TYPE_ID_SUCCESS,
    INSERT_TYPE_GENERO_SUCCESS,
    UPDATE_TYPE_GENERO_SUCCESS,
    INSERT_TYPE_FORCE_SUCCESS,
    UPDATE_TYPE_FORCE_SUCCESS,
    GET_TYPE_AFFILIATE,
    GET_TYPE_AFFILIATE_FAILURE,
    GET_TYPE_AFFILIATE_SUCCESS,
    GET_AFFILIATE_STATE,
    GET_AFFILIATE_STATE_SUCCESS,
    GET_AFFILIATE_STATE_FAILURE,
    GET_QUALITY_FIELD,
    GET_QUALITY_FIELD_SUCCESS,
    GET_QUALITY_FIELD_FAILURE,
    GET_AFFILIATE_TYPE_PROCEDURE,
    GET_AFFILIATE_TYPE_PROCEDURE_SUCCESS,
    GET_AFFILIATE_TYPE_PROCEDURE_FAILURE,
    INSERT_TYPE_CATEGORY_SUCCESS,
    UPDATE_TYPE_CATEGORY_SUCCESS,
    GET_DISCOUNT_PERCENTAGE,
    GET_DISCOUNT_PERCENTAGE_SUCCESS,
    GET_DISCOUNT_PERCENTAGE_FAILURE,
    GET_REGISTRATION_OFFICE,
    GET_REGISTRATION_OFFICE_SUCCESS,
    GET_REGISTRATION_OFFICE_FAILURE,
    GET_CAUSAL_BLOCKAGE,
    GET_CAUSAL_BLOCKAGE_SUCCESS,
    GET_CAUSAL_BLOCKAGE_FAILURE,
    GET_REQUEST_TYPE_REAL_STATE_REGISTRATION,
    GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_SUCCESS,
    GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_FAILURE,
    GET_ALL_GROUP_INCONSISTENCIES,
    GET_ALL_TOOLS_INCONSISTENCIES,
    GET_ALL_POINTS_ATTENTION,
    GET_IPC,
    GET_ALL_STATE_REQUEST,
    GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD,
    GET_MODALIDAD_AFILIACION_VETERANO,
    GET_MODALIDAD_AFILIACION_VETERANO_SUCCESS,
    GET_MODALIDAD_AFILIACION_VETERANO_FAILURE,
    GETALL_PAY,
    GET_TIPOS_COMPROBANTE,
    GET_USERS

} from '../types/catalog.actions.type';
import {
    CITYDTO,
    ESTADOCIVILDTO,
    EXECUTINGUNITDTO,
    FORCEDTO,
    GRADEDTO,
    IDENTIFICATIONTYPEDTO,
    LIVINGPLACEDTO,
    SEXODTO,
    DEPARTAMENTODTO,
    PAISDTO,
    ACTIVIDADECONOMICA,
    PROFESION,
    TIPOCREDITO,
    TIVIVIENDA,
    TIPOVIVIENDA,
    ESTADOVIVIENDA,
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
    TYPEHOME,
    TIPOCUENTA,
    ENTIDADBANCARIA,
    CatalogData,
    VALORCATALOGO,
    ENTIDADEDUCATIVA,
    PROGRAMAEDUCATIVO,
    NIVELEDUCATIVO,
    AVERAGECOSTDTO,
    FORMATDTO,
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
    TIPOVINCULACIONDTO,
    TYPESFILEDTO,
    STATEFILEDTO,
    LINEDTO,

} from '../types/catalog.type';
import { CreditTypeModel } from '../models/credit-type.model';
import { ActivityStatusModel } from '../models/activity-status.model';
import { TypeCategoriesForm, TypeForceForm, TypeIdForm, TypeTipoGeneroForm } from '../pages/parametrizationTypeId/constants/catalogForm.model.type';
import { CATALOG_NAME } from '../constants/constants';


export interface CatalogState {
    IdentificationType: IDENTIFICATIONTYPEDTO[];
    causalesBloqueoTypes: CAUSALBLOQUEOCUENTA[];
    Grade: GRADEDTO[];
    Line: LINEDTO[];
    Pay: PAYDTO[];
    City: CITYDTO[];
    ExecutingUnit: EXECUTINGUNITDTO[];
    TypesFile: TYPESFILEDTO[];
    StateFile: STATEFILEDTO[];
    LivingPlace: LIVINGPLACEDTO[];
    Force: FORCEDTO[];
    error: any;
    civilstates: ESTADOCIVILDTO[];
    sextypes: SEXODTO[];
    countries: PAISDTO[];
    departments: DEPARTAMENTODTO[];
    beneficiarioCartera:any[];
    conceptosCartera:any[];
    tipoComprobante:any[];
    economicActivities: ACTIVIDADECONOMICA[];
    professions: PROFESION[];
    tiVivienda: TIVIVIENDA[];
    tipoVivienda: TIPOVIVIENDA[];
    estadoCartera: ESTADOCARTERA[];
    tipoCredito: TIPOCREDITO[];
    estadoVivienda: ESTADOVIVIENDA[];
    activitiesStatus: ActivityStatusModel[];
    creditTypes: CreditTypeModel[];
    phoneTypes: PHONETYPE[];
    mailTypes: MAILTYPE[];
    currencyTypes: CURRENCYTYPE[];
    categories: CATEGORIES[];
    loading: boolean;
    payrollCategoriesConcept: any[];
    emailDomains: any[];
    addressTypes: ADDRESSTYPE[];
    typeHome: TYPEHOME[];
    typeRequest: TYPEREQUEST[];
    typeModality: TYPEMODALITY[];
    typeSubModality: TYPESUBMODALITY[];
    abonoUnico: ABONOUNICO[];
    tipoAbono: TIPOABONO[];
    tipoSubsidio: TIPOSUBSIDO[];
    citiesVendors: CITYDTO[];
    tipoCuenta: TIPOCUENTA[];
    entidadBancaria: ENTIDADBANCARIA[];
    CatalogData: CatalogData[];
    valorCatalogo: VALORCATALOGO[];
    parentescoCatalogo: VALORCATALOGO[];
    tipoCuentaCatalogo: VALORCATALOGO[];
    cuentaAcreditada: VALORCATALOGO[];
    tipoPersona: VALORCATALOGO[];
    EstadoEntidad: VALORCATALOGO[];
    entidadEducativa: ENTIDADEDUCATIVA[];
    programaEducativo: PROGRAMAEDUCATIVO[];
    programaPorEntidad: PROGRAMAPORENTIDADDTO[];
    nivelEducativo: NIVELEDUCATIVO[];
    MediosDeEnvio: AVERAGECOSTDTO[] | null;
    Formato: FORMATDTO[] | null;
    typeId: IDENTIFICATIONTYPEDTO;
    generotype: any;
    sextype: SEXODTO;
    force: FORCEDTO;
    entidadSeguroEducativo: ENTIDADSEGUROEDUCATIVO[];
    tipoAfiliacion: TIPOAFILIACIONDTO[];
    tiposVinculacion: TIPOVINCULACIONDTO[];
    estadoAfiliacion: ESTADOAFILIACIONDTO[];
    campoCalidad: CAMPOCALIDADDTO[];
    tipoAfiliacionProcedente: TIPOAFILIACIONPRODECENTEDTO[];
    category: CATEGORIES;
    porcentajeDescuento: PORCENTAJEDESCUENTODTO[];
    gruposInconsistencias: GRUPOSINCONSITENCIASDTO[];
    incosistencias: INCONSISTENCIADTO[];
    herramientasInconsistencia: HERRAMIENTAINCONSISTENCIADTO[];
    puntosAtencion: PUNTOSATENCIONDTO[];
    ipc: IPC;
    estadosTareas: ESTADOTAREADTO[];
    oficinasRegistro: OFICINASREGISTRODTO[];
    causalBloqueo: CAUSALBLOQUEODTO[];
    tipoSolicitudMatriculaInmobiliaria: REQUESTTYPEREALSTATEREGISTRATIONDTO[],
    nivelPorPrograma: NIVELEDUCATIVOPORPROGRAMADTO[],
    ModalidadAfiliacionVeterano: MODALIDADAFILIACIONVETERANODTO[];
    users:any[]
}

const InitialState: CatalogState = {
    causalesBloqueoTypes: [],
    IdentificationType: [],
    Grade: [],
    Line: [],
    Pay: [],
    City: [],
    ExecutingUnit: [],
    TypesFile: [],
    StateFile: [],
    LivingPlace: [],
    Force: [],
    error: null,
    civilstates: [],
    sextypes: [],
    countries: [],
    departments: [],
    beneficiarioCartera: [],
    conceptosCartera: [],
    tipoComprobante: [],
    economicActivities: [],
    professions: [],
    tipoCredito: [],
    tiVivienda: [],
    tipoVivienda: [],
    estadoCartera: [],
    estadoVivienda: [],
    activitiesStatus: [],
    creditTypes: [],
    phoneTypes: [],
    mailTypes: [],
    currencyTypes: [],
    categories: [],
    loading: false,
    payrollCategoriesConcept: [],
    emailDomains: [],
    addressTypes: [],
    typeHome: [],
    typeRequest: [],
    typeModality: [],
    typeSubModality: [],
    abonoUnico: [],
    tipoAbono: [],
    tipoSubsidio: [],
    citiesVendors: [],
    tipoCuenta: [],
    entidadBancaria: [],
    CatalogData: [],
    valorCatalogo: [],
    tipoCuentaCatalogo: [],
    cuentaAcreditada: [],
    tipoPersona: [],
    EstadoEntidad:[],
    parentescoCatalogo: [],
    entidadEducativa: [],
    programaEducativo: [],
    programaPorEntidad: [],
    nivelEducativo: [],
    MediosDeEnvio: [],
    Formato: [],
    typeId: TypeIdForm,
    generotype: null,
    sextype: TypeTipoGeneroForm,
    force: TypeForceForm,
    entidadSeguroEducativo: [],
    tipoAfiliacion: [],
    estadoAfiliacion: [],
    campoCalidad: [],
    tipoAfiliacionProcedente: [],
    category: TypeCategoriesForm,
    porcentajeDescuento: [],
    oficinasRegistro: [],
    causalBloqueo: [],
    tipoSolicitudMatriculaInmobiliaria: [],
    gruposInconsistencias: [],
    incosistencias: [],
    herramientasInconsistencia: [],
    puntosAtencion: [],
    ipc: {} as IPC,
    estadosTareas: [],
    nivelPorPrograma: [],
    ModalidadAfiliacionVeterano: [],
    tiposVinculacion: [],
    users:[]
};

export const CatalogReducer = (
    state: CatalogState = InitialState,
    action: CatalogActionTypes,
): CatalogState => {
    switch (action.type) {
        case GETALL_CAUSALES_BLOQUEO:
            return {
                ...state,
                causalesBloqueoTypes: action.causalesBloqueoTypes,
            };
        case GETALL_IDENTIFICATIONTYPES:
            return {
                ...state,
                IdentificationType: action.identificationTypes,
            };
        case GET_USERS:
            return {
                ...state,
                users: action.users,
            };
        case GET_ALL_TI_VIVIENDA:
            return {
                ...state,
                tiVivienda: action.tiVivienda,
            };
        case GET_ALL_TIPO_VIVIENDA:
            return {
                ...state,
                tipoVivienda: action.tipoVivienda,
            };
        case GET_ALL_ESTADO_CARTERA:
            return {
                ...state,
                estadoCartera: action.estadoCartera,
            };
        case GET_ALL_ESTADO_VIVIENDA:
            return {
                ...state,
                estadoVivienda: action.estadoVivienda,
            };
        case GET_ALL_TIPO_CREDITO:
            return {
                ...state,
                tipoCredito: action.tipoCredito,
            };
        case GETALL_GRADES:
            return {
                ...state,
                Grade: action.grades,
            };
        case GET_CREDIT_LINE:
            return {
                ...state,
                Line: action.line,
            };
        case GETALL_PAY:
            return {
                ...state,
                Pay: action.pay,
            };
        case GETALL_CITIES:
            return {
                ...state,
                City: action.cities,
            };
        case GETALL_CITIES_VENDORS:
            return {
                ...state,
                citiesVendors: action.citiesVendors,
            };
        case GET_CITIES_BY_DEPARTMENT:
            return {
                ...state,
                loading: true,
            };
        case GET_CITIES_BY_DEPARTMENT_SUCCESS:
            return {
                ...state,
                City: action.cities,
                loading: false,
            };
        case GETALL_FORCES:
            return {
                ...state,
                Force: action.forces,
            };
        case GETALL_EXECUTINGUNITS:
            return {
                ...state,
                ExecutingUnit: action.executingUnits,
            };
        case GET_TYPESFILE:
            return {
                ...state,
                TypesFile: action.typesFile,
            };
        case GET_STATEFILE:
            return {
                ...state,
                StateFile: action.stateFile,
            };
        case GETALL_LIVINGPLACES:
            return {
                ...state,
                LivingPlace: action.livingPlaces,
            };
        case GET_ALL_CIVIL_STATES:
            return {
                ...state,
                civilstates: action.civilstates,
            };
        case GET_ALL_SEX_TYPES:
            return {
                ...state,
                sextypes: action.sextypes,
            };
        case GET_ALL_COUNTRIES:
            return {
                ...state,
                countries: action.countries,
            };
        case GET_ALL_DEPARTMENTS:
            return {
                ...state,
                departments: action.departments,
            };
        case GET_BENEFICIARIO_CARTERA:
            return {
                ...state,
                beneficiarioCartera: action.beneficiarioCartera,
            };
        case GET_CONCEPTOS_CARTERA:
            return {
                ...state,
                conceptosCartera: action.conceptosCartera,
            };
        case GET_TIPOS_COMPROBANTE:
            return {
                ...state,
                tipoComprobante: action.tipoComprobante,
            };
        case GET_ALL_ECONOMIC_ACTIVITIES:
            return {
                ...state,
                economicActivities: action.economicActivities,
            };
        case GET_ALL_PROFESSIONS:
            return {
                ...state,
                professions: action.professions,
            };
        case GET_ACTIVITIES_STATUS:
            return {
                ...state,
                loading: true,
            };
        case GET_ACTIVITIES_STATUS_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                activitiesStatus: action.activitiesStatus,
            };
        case GET_CREDIT_TYPES:
            return {
                ...state,
                loading: true,
            };
        case GET_CREDIT_TYPES_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                creditTypes: action.creditTypes,
            };
        case GET_PHONES_TYPES:
            return {
                ...state,
                loading: true,
            };
        case GET_PHONES_TYPES_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                phoneTypes: action.phoneTypes,
            };
        case GET_MAIL_TYPES:
            return {
                ...state,
                loading: true,
            };
        case GET_MAIL_TYPES_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                mailTypes: action.MailTypes,
            };
        case GET_CURRENCY_TYPES:
            return {
                ...state,
                loading: true,
            };
        case GET_CURRENCY_TYPES_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                currencyTypes: action.CurrencyTypes,
            };
        case GET_CATEGORY:
            return {
                ...state,
                loading: true,
            };
        case GET_CATEGORY_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                categories: action.Categories,
            };
        case GET_PAYROLL_CATEGORY_CONCEPT:
            return {
                ...state,
                error: null,
                loading: true,
            };
        case GET_PAYROLL_CATEGORY_CONCEPT_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                payrollCategoriesConcept: action.payrollCategoriesConcept,
            };
        case GET_EMAIL_DOMAINS:
            return {
                ...state,
                error: null,
                loading: true,
            };
        case GET_EMAIL_DOMAINS_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                emailDomains: action.emailDomains,
            };
        case GET_ADDRESS_TYPE:
            return {
                ...state,
                error: null,
                loading: true,
            };
        case GET_ADDRESS_TYPE_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                addressTypes: action.addressTypes,
            };
        case GETALL_TYPE_REQUEST:
            return {
                ...state,
                loading: true,
                typeRequest: action.typeRequest,
            };
        case GETALL_TYPE_MODALITY:
            return {
                ...state,
                loading: true,
                typeModality: action.typeModality,
            };
        case GETALL_TYPE_SUBMODALITY:
            return {
                ...state,
                loading: true,
                typeSubModality: action.typeSubModality,
            };
        case GET_ALL_TYPE_HOME:
            return {
                ...state,
                error: null,
                loading: false,
                typeHome: action.typeHome,
            };

        case GET_ALL_ABONO_UNICO:
            return {
                ...state,
                error: null,
                loading: false,
                abonoUnico: action.abonoUnico,
            };

        case GET_ALL_TIPO_ABONO:
            return {
                ...state,
                error: null,
                loading: false,
                tipoAbono: action.tipoAbono,
            };


        case GET_ALL_TIPO_SUBSIDIO:
            return {
                ...state,
                error: null,
                loading: false,
                tipoSubsidio: action.tipoSubsidio,
            };

        case GET_ALL_TIPO_CUENTA:
            return {
                ...state,
                error: null,
                loading: false,
                tipoCuenta: action.tipoCuenta,
            }
        case GET_ALL_ENTIDAD_BANCARIA:
            return {
                ...state,
                error: null,
                loading: false,
                entidadBancaria: action.entidadBancaria,
            }
        case GET_INFO_TURNO:
            return {
                ...state,
                error: null,
                loading: false,
                CatalogData: action.infoTurno,
            }
        case GET_ALL_ENTIDAD_SEGURO_EDUCATIVO:
            return {
                ...state,
                entidadSeguroEducativo: action.entidadSeguroEducativo,
            }
        case GET_ALL_VALOR_CATALOGO_POR_ID:
            return {
                ...state,
                error: null,
                loading: false,
                valorCatalogo: action.valorCatalogo,
            }
        case GET_ALL_VALOR_CATALOGO_POR_NOMBRE:
            const newState = {
                ...state,
                error: null,
                loading: false,
            }
            switch (action.nombreCatalogo) {
                case CATALOG_NAME.TIPO_CUENTA:
                    newState.tipoCuentaCatalogo = action.valorCatalogo;
                    break;
                case CATALOG_NAME.CUENTA_ACREDITADA:
                    newState.cuentaAcreditada = action.valorCatalogo;
                    break;
                case CATALOG_NAME.TIPO_PERSONA:
                    newState.tipoPersona = action.valorCatalogo;
                    break;
                case CATALOG_NAME.ESTADO_ENTIDAD:
                    newState.EstadoEntidad = action.valorCatalogo;
                    break;    
                default:
                    newState.valorCatalogo = action.valorCatalogo;
                    break;
            }
            return newState;
        case GET_ALL_VALOR_CATALOGO_PARENTESCO:
            return {
                ...state,
                error: null,
                loading: false,
                parentescoCatalogo: action.parentescoCatalogo,
            }
        case GETALL_ENTIDAD_EDUCATIVA:
            return {
                ...state,
                entidadEducativa: action.entidadEducativa,
            };
        case GETALL_PROGRAMA_EDUCATIVO:
            return {
                ...state,
                programaEducativo: action.programaEducativo,
            };
        case GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD:
            return {
                ...state,
                programaPorEntidad: action.programaPorEntidad,
            };
        case GETALL_NIVEL_EDUCATIVO:
            return {
                ...state,
                nivelEducativo: action.nivelEducativo,
            };
        case GET_INSERT_TYPE_ID_SUCCESS:
            return {
                ...state,
                loading: false,
                IdentificationType: action.IdentificacionType,
            };
        case GET_INSERT_TYPE_ID_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case UPDATE_TYPE_ID_SUCCESS:
            return {
                ...state,
                loading: false,
                typeId: action.typeId,
            };
        case INSERT_TYPE_GENERO_SUCCESS:
            return {
                ...state,
                loading: false,
                generotype: action.generotype,
            };
        case UPDATE_TYPE_GENERO_SUCCESS:
            return {
                ...state,
                loading: false,
                sextype: action.sextype,
            };
        case INSERT_TYPE_FORCE_SUCCESS:
            return {
                ...state,
                loading: false,
                force: action.force,
            };
        case UPDATE_TYPE_FORCE_SUCCESS:
            return {
                ...state,
                loading: false,
                force: action.force,
            };
        case INSERT_TYPE_CATEGORY_SUCCESS:
            return {
                ...state,
                loading: false,
                category: action.category,
            };
        case UPDATE_TYPE_CATEGORY_SUCCESS:
            return {
                ...state,
                loading: false,
                category: action.category,
            };
        case ON_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            }

        case GET_FORMAT:
            return {
                ...state,
                loading: true,
            }

        case GET_FORMAT_SUCCESS:
            return {
                ...state,
                loading: false,
                Formato: action.Formato
            }

        case GET_FORMAT_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.Formato,
            }

        case GET_AVERAGE_COST:
            return {
                ...state,
                loading: true,
            }

        case GET_AVERAGE_COST_SUCCESS:
            return {
                ...state,
                loading: false,
                MediosDeEnvio: action.MediosDeEnvio
            }

        case GET_AVERAGE_COST_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.MediosDeEnvio,
            }
        case GET_TYPE_AFFILIATE:
            return {
                ...state,
                loading: true,
            }

        case GET_TYPE_AFFILIATE_SUCCESS:
            return {
                ...state,
                loading: false,
                tipoAfiliacion: action.tipoAfiliacion
            }

        case GET_TYPE_AFFILIATE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.tipoAfiliacion,
            }

        case GET_AFFILIATE_STATE:
            return {
                ...state,
                loading: true,
            }

        case GET_AFFILIATE_STATE_SUCCESS:
            return {
                ...state,
                loading: false,
                estadoAfiliacion: action.estadoAfiliacion
            }

        case GET_AFFILIATE_STATE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.estadoAfiliacion,
            }

        case GET_QUALITY_FIELD:
            return {
                ...state,
                loading: true,
            }

        case GET_QUALITY_FIELD_SUCCESS:
            return {
                ...state,
                loading: false,
                campoCalidad: action.campoCalidad
            }

        case GET_QUALITY_FIELD_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.campoCalidad,
            }

        case GET_AFFILIATE_TYPE_PROCEDURE:
            return {
                ...state,
                loading: true,
            }

        case GET_AFFILIATE_TYPE_PROCEDURE_SUCCESS:
            return {
                ...state,
                loading: false,
                tipoAfiliacionProcedente: action.tipoAfiliacionProcedente
            }

        case GET_AFFILIATE_TYPE_PROCEDURE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.tipoAfiliacionProcedente,
            }

        case GET_DISCOUNT_PERCENTAGE:
            return {
                ...state,
                loading: true,
            }

        case GET_DISCOUNT_PERCENTAGE_SUCCESS:
            return {
                ...state,
                loading: false,
                porcentajeDescuento: action.porcentajeDescuento
            }

        case GET_DISCOUNT_PERCENTAGE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.porcentajeDescuento,
            }

        case GET_ALL_GROUP_INCONSISTENCIES:
            return {
                ...state,
                loading: false,
                gruposInconsistencias: action.gruposInconsistencias
            }
        case GET_ALL_TOOLS_INCONSISTENCIES:
            return {
                ...state,
                loading: false,
                herramientasInconsistencia: action.herramientasInconsistencia
            }
        case GET_ALL_POINTS_ATTENTION:
            return {
                ...state,
                loading: false,
                puntosAtencion: action.puntosAtencion
            }
        case GET_IPC:
            return {
                ...state,
                ipc: action.ipc,
            };

        case GET_ALL_STATE_REQUEST:
            return {
                ...state,
                error: null,
                loading: false,
                estadosTareas: action.estadosTareas,
            };

        case GET_REGISTRATION_OFFICE:
            return {
                ...state,
                loading: true,
            }

        case GET_REGISTRATION_OFFICE_SUCCESS:
            return {
                ...state,
                loading: false,
                oficinasRegistro: action.oficinasRegistro
            }

        case GET_REGISTRATION_OFFICE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.oficinasRegistro,
            }

        case GET_CAUSAL_BLOCKAGE:
            return {
                ...state,
                loading: true,
            }

        case GET_CAUSAL_BLOCKAGE_SUCCESS:
            return {
                ...state,
                loading: false,
                causalBloqueo: action.causalBloqueo
            }

        case GET_CAUSAL_BLOCKAGE_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.causalBloqueo,
            }

        case GET_REQUEST_TYPE_REAL_STATE_REGISTRATION:
            return {
                ...state,
                loading: true,
            }

        case GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_SUCCESS:
            return {
                ...state,
                loading: false,
                tipoSolicitudMatriculaInmobiliaria: action.tipoSolicitudMatriculaInmobiliaria
            }

        case GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.tipoSolicitudMatriculaInmobiliaria,
            }


        case GET_MODALIDAD_AFILIACION_VETERANO:
            return {
                ...state,
                loading: true,
            }

        case GET_MODALIDAD_AFILIACION_VETERANO_SUCCESS:
            return {
                ...state,
                loading: false,
                ModalidadAfiliacionVeterano: action.ModalidadAfiliacionVeterano
            }

        case GET_MODALIDAD_AFILIACION_VETERANO_FAILURE:
            return {
                ...state,
                loading: false,
                error: action.ModalidadAfiliacionVeterano,
            }

        default:
            return state;
    }
};
