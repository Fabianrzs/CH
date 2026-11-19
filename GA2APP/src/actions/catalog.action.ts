import { Dispatch } from 'redux';
import { toast } from 'react-toastify';
import history from '../utils/history.router';
import { AppActions } from '../types/appactions.type';


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
	TIVIVIENDA,
	TIPOVIVIENDA,
	ESTADOVIVIENDA,
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
	TYPEHOME,
	TIPOCUENTA,
	ENTIDADBANCARIA,
	VALORCATALOGO,
	ENTIDADEDUCATIVA,
	NIVELEDUCATIVO,
	PROGRAMAEDUCATIVO,
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
	ESTADOTAREADTO,
	PROGRAMAPORENTIDADDTO,
	NIVELEDUCATIVOPORPROGRAMADTO,
	MODALIDADAFILIACIONVETERANODTO,
	PAYDTO,
	TYPESFILEDTO,
	STATEFILEDTO,
	LINEDTO,
} from '../types/catalog.type';
import {
	GETALL_CITIES,
	GETALL_EXECUTINGUNITS,
	GET_TYPESFILE,
	GET_STATEFILE,
	GETALL_FORCES,
	GETALL_GRADES,
	GET_CREDIT_LINE,
	GETALL_IDENTIFICATIONTYPES,
	GET_USERS,
	GETALL_LIVINGPLACES,
	GET_ALL_CIVIL_STATES,
	GET_ALL_SEX_TYPES,
	GET_ALL_DEPARTMENTS,
	ON_ERROR,
	GET_ALL_COUNTRIES,
	GET_ALL_PROFESSIONS,
	GET_ALL_ECONOMIC_ACTIVITIES,
	GET_ALL_TI_VIVIENDA,
	GET_ALL_TIPO_VIVIENDA,
	GET_ALL_ESTADO_VIVIENDA,
	GET_ALL_TIPO_CREDITO,
	GET_ALL_ESTADO_CARTERA,
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
	GET_CURRENCY_TYPES,
	GET_CURRENCY_TYPES_SUCCESS,
	GET_CATEGORY_SUCCESS,
	GET_CATEGORY,
	GET_PAYROLL_CATEGORY_CONCEPT_SUCCESS,
	GET_PAYROLL_CATEGORY_CONCEPT,
	GET_EMAIL_DOMAINS,
	GET_EMAIL_DOMAINS_SUCCESS,
	GET_ADDRESS_TYPE,
	GET_ADDRESS_TYPE_SUCCESS,
	GET_ALL_ABONO_UNICO,
	GET_ALL_TIPO_ABONO,
	GET_ALL_TIPO_SUBSIDIO,
	GET_ALL_TYPE_HOME,
	GETALL_TYPE_REQUEST,
	GETALL_TYPE_MODALITY,
	GETALL_TYPE_SUBMODALITY,
	GETALL_CITIES_VENDORS,
	GET_ALL_TIPO_CUENTA,
	GET_ALL_ENTIDAD_BANCARIA,
	GET_ALL_VALOR_CATALOGO_POR_ID,
	GET_ALL_VALOR_CATALOGO_POR_NOMBRE,
	GET_ALL_VALOR_CATALOGO_PARENTESCO,
	GETALL_ENTIDAD_EDUCATIVA,
	GETALL_PROGRAMA_EDUCATIVO,
	GETALL_NIVEL_EDUCATIVO,
	GET_AVERAGE_COST,
	GET_AVERAGE_COST_SUCCESS,
	GET_AVERAGE_COST_FAILURE,
	GET_FORMAT,
	GET_FORMAT_SUCCESS,
	GET_FORMAT_FAILURE,
	GET_ALL_ENTIDAD_SEGURO_EDUCATIVO,
	GET_INSERT_TYPE_ID_SUCCESS,
	GET_INSERT_TYPE_ID_FAILURE,
	UPDATE_TYPE_ID_SUCCESS,
	INSERT_TYPE_GENERO_SUCCESS,
	UPDATE_TYPE_GENERO_SUCCESS,
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
	GET_AFFILIATE_TYPE_PROCEDURE_FAILURE,
	GET_AFFILIATE_TYPE_PROCEDURE_SUCCESS,
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

	GETALL_CAUSALES_BLOQUEO,
	GET_ALL_GROUP_INCONSISTENCIES,
	GET_ALL_INCONSISTENCIES_BY_GROUP,
	GET_ALL_TOOLS_INCONSISTENCIES,
	GET_ALL_POINTS_ATTENTION,
	GET_ALL_STATE_REQUEST,
	GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD,
	GETALL_NIVEL_EDUCATIVO_POR_PROGRAMA,
	GET_MODALIDAD_AFILIACION_VETERANO,
	GET_MODALIDAD_AFILIACION_VETERANO_SUCCESS,
	GET_MODALIDAD_AFILIACION_VETERANO_FAILURE,
	GETALL_PAY,
	GET_BENEFICIARIO_CARTERA,
	GET_CONCEPTOS_CARTERA,
	GET_TIPOS_COMPROBANTE,

} from '../types/catalog.actions.type';
import { ActivityStatusModel } from '../models/activity-status.model';
import CatalogApi from '../services/api-catalog.service';
import LoginApi from '../services/api-login.service'
import CatalogAdapter from '../adapters/catalog.adapter';
import { CreditTypeModel } from '../models/credit-type.model';
import NotificationService from '../services/notification.service';
import { NotificationCardModel, NotificationCardModelEnum } from '../models/requests/notification-cards-model';
import { ProgramasEducativos } from '../types/educational.entities.type';

const api = CatalogApi();

export const GetAllIdentificationTypes = (
	identificationTypes: IDENTIFICATIONTYPEDTO[],
): AppActions => ({
	type: GETALL_IDENTIFICATIONTYPES,
	identificationTypes,
});

export const GetAllUsers = (
	users: any[],
): AppActions => ({
	type: GET_USERS,
	users,
});

export const GetAllCausalesBloqueoCuenta = (
	causalesBloqueoTypes: CAUSALBLOQUEOCUENTA[],
): AppActions => ({
	type: GETALL_CAUSALES_BLOQUEO,
	causalesBloqueoTypes,
});

export const SetError = (error: any): AppActions => ({
	type: ON_ERROR,
	error: error,
});

export const startGetAllIdentificationTypes =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllTypeDocuments()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllIdentificationTypes(response.data));
		}
	};

export const startGetAllUsers =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const apiLogin = LoginApi();
		const response = await apiLogin.getAllUsersRequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllUsers(response.data));
		}
	};

export const startGetAllTypeDocuments =
	() => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllTypeDocuments();
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllIdentificationTypes(response.data));
		}
	};

export const startGetAllCausalesBloqueoCuenta =
	() => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllCausalesBloqueoCuenta();
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllCausalesBloqueoCuenta(response.data));
		}
	};

export const startActualizaBloqueoCuentas =
	(data: any) => async () => {

		const response = await api.ActualizaBloqueoCuentas(data);
		if (response?.isSuccess == null || !response.isSuccess) {

			await NotificationService.showNotification({
				message: response.returnMessage,
				image: null,
				type: NotificationCardModelEnum.error,
			} as NotificationCardModel);

		} else {

			await NotificationService.showNotification({
				message: "Cuenta Bloqueada Exitosamente.",
				image: null,
				type: NotificationCardModelEnum.success,
			} as NotificationCardModel);

		
		}
	};

export const startCrearCuentas =
	(data: any) => async () => {

		const response = await api.CrearNuevaCuenta(data);
		if (response?.isSuccess == null || !response.isSuccess) {

			await NotificationService.showNotification({
				message: response.returnMessage,
				image: null,
				type: NotificationCardModelEnum.error,
			} as NotificationCardModel);

		} else {

			await NotificationService.showNotification({
				message: "Cuenta Creada exitosamente!.",
				image: null,
				type: NotificationCardModelEnum.success,
			} as NotificationCardModel);
			history.push('/ControlAdministracionCuentas');
		}
	};

export const GetAllPay = (pay: PAYDTO[]): AppActions => ({
	type: GETALL_PAY,
	pay,
});

export const startGetPayApplication =
	() => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAlternativosAplicacionPagos();
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllPay(response.data));
		}
	};

export const GetAllGrades = (grades: GRADEDTO[]): AppActions => ({
	type: GETALL_GRADES,
	grades,
});

export const startGetAllGrades =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getGrados();
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllGrades(response.data));
		}
	};

export const GetCreditLine = (line: LINEDTO[]): AppActions => ({
	type: GET_CREDIT_LINE,
	line,
});

export const starGetCreditLine =
	(props: number) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getCreditLine(props);
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetCreditLine(response.data));
		}
	};

export const GetAllCities = (cities: CITYDTO[]): AppActions => ({
	type: GETALL_CITIES,
	cities,
});

export const startGetAllCities =
	(stateID?: string) => async (dispatch: Dispatch<AppActions>) => {
		try {

			let response;
			
			if (stateID != null) {
			
				response = await api.getCityOfState(stateID);
			} else {
				response = await api.getAllCities();
			}
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllCities(response.data));
			}
		} catch (e) {
			toast(`Error: respuesta errada del servidor, ${e}`);
		}
	};

export const GetAllCitiesVendors = (citiesVendors: CITYDTO[]): AppActions => ({
	type: GETALL_CITIES_VENDORS,
	citiesVendors,
});

export const startGetAllCitiesVendors =
	(stateID?: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			let response;
			if (stateID != null) {
				response = await api.getCityOfState(stateID);
			} else {
				response = await api.getAllCities();
			}
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllCitiesVendors(response.data));
			}
		} catch (e) {
			toast(`Error: respuesta errada del servidor, ${e}`);
		}
	};

export const GetAllExecutingUnits = (
	executingUnits: EXECUTINGUNITDTO[],
): AppActions => ({
	type: GETALL_EXECUTINGUNITS,
	executingUnits,
});

export const startGetAllExecutingUnits =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllEXECUTINGUNITSRequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllExecutingUnits(response.data));
		}
	};

export const GetAllTypeFiles = (
	typesFile: TYPESFILEDTO[],
): AppActions => ({
	type: GET_TYPESFILE,
	typesFile,
	
});

export const startGetAllTypeFiles=
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllTYPESFILERequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllTypeFiles(response.data));
		}
	};

export const GetAllStateFiles = (
	stateFile: STATEFILEDTO[],
): AppActions => ({
	type: GET_STATEFILE,
	stateFile,
});

export const startGetAllStateFiles=
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllSTATEFILERequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllStateFiles(response.data));
		}
	};

export const GetAllLivingPlaces = (
	livingPlaces: LIVINGPLACEDTO[],
): AppActions => ({
	type: GETALL_LIVINGPLACES,
	livingPlaces,
});

export const startGetAllLivingPlaces =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllLIVINGPLACESRequest();
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllLivingPlaces(response.data));
		}
	};

export const GetAllForces = (forces: FORCEDTO[]): AppActions => ({
	type: GETALL_FORCES,
	forces,
});

export const startGetAllForces =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllForces()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllForces(response.data));
		}
	};

export const GetAllCivilState = (
	civilstates: ESTADOCIVILDTO[],
): AppActions => ({
	type: GET_ALL_CIVIL_STATES,
	civilstates,
});

export const startGetAllCivilState =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getGetAllCivilStatesRequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllCivilState(response.data));
		}
	};

export const GetAllSexTypes = (sextypes: SEXODTO[]): AppActions => ({
	type: GET_ALL_SEX_TYPES,
	sextypes,
});

export const startGetAllSexTypes =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllGeneros()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllSexTypes(response.data));
		}
	};

export const GetAllDepartments = (
	departments: DEPARTAMENTODTO[],
): AppActions => ({
	type: GET_ALL_DEPARTMENTS,
	departments,
});

export const startGetAllDepartments =
	() => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllStates();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllDepartments(response.data));
			}
		} catch (e) {
	
			dispatch(SetError('server-error'));
		}
	};

export const GetAllCountries = (countries: PAISDTO[]): AppActions => ({
	type: GET_ALL_COUNTRIES,
	countries,
});

export const startGetAllCountries =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllCountriesRequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllCountries(response.data));
		}
	};

export const GetAllEconomicActivities = (
	economicActivities: ACTIVIDADECONOMICA[],
): AppActions => ({
	type: GET_ALL_ECONOMIC_ACTIVITIES,
	economicActivities,
});

export const startGetAllEconomicActivities =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllEconomicActivitiesRequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllEconomicActivities(response.data));
		}
	};

export const GetAllProfessions = (professions: PROFESION[]): AppActions => ({
	type: GET_ALL_PROFESSIONS,
	professions,
});

export const startGetAllProfessions =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllProfessionsRequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllProfessions(response.data));
		}
	};

export const GetAllTipoVivienda = (
	tipoVivienda: TIPOVIVIENDA[],
): AppActions => ({
	type: GET_ALL_TIPO_VIVIENDA,
	tipoVivienda,
});

export const startGetAllTipoVivienda =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getTipodeviviendas()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllTipoVivienda(response.data));
		}
	};

export const GetAllTiVivienda = (tiVivienda: TIVIVIENDA[]): AppActions => ({
	type: GET_ALL_TI_VIVIENDA,
	tiVivienda,
});

export const startGetAllTiVivienda =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.obtenerTiVivienda()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllTiVivienda(response.data));
		}
	};

export const GetAllEstadoVivienda = (
	estadoVivienda: ESTADOVIVIENDA[],
): AppActions => ({
	type: GET_ALL_ESTADO_VIVIENDA,
	estadoVivienda,
});

export const startGetAllEstadoVivienda =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllEstadoViviendaRequest()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllEstadoVivienda(response.data));
		}
	};

export const GetAllTipoCredito = (tipoCredito: TIPOCREDITO[]): AppActions => ({
	type: GET_ALL_TIPO_CREDITO,
	tipoCredito,
});

export const startGetAllTipoCredito =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getCreditType()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllTipoCredito(response.data));
		}
	};

export const GetActivitiesStatus = (): AppActions => ({
	type: GET_ACTIVITIES_STATUS,
});

export const GetActivitiesStatusSuccess = (
	activitiesStatus: ActivityStatusModel[],
): AppActions => ({
	type: GET_ACTIVITIES_STATUS_SUCCESS,
	activitiesStatus,
});

export const startGetActivitiesStatus =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetActivitiesStatus());
				const response = await api.getActivityStatus();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CatalogAdapter.adaptActivityStatusList(
						response.data,
					);
					dispatch(GetActivitiesStatusSuccess(dataAdapted));
				}
			} catch (e) {
			
				dispatch(SetError('server-error'));
			}
		};

export const GetCreditType = (): AppActions => ({
	type: GET_CREDIT_TYPES,
});

export const GetCreditTypeSuccess = (
	creditTypes: CreditTypeModel[],
): AppActions => ({
	type: GET_CREDIT_TYPES_SUCCESS,
	creditTypes,
});

export const startGetCreditType =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetCreditType());
				const response = await api.getCreditType();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CatalogAdapter.adaptCreditTypeList(
						response.data,
					);
					dispatch(GetCreditTypeSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetCitiesByDepartment = (): AppActions => ({
	type: GET_CITIES_BY_DEPARTMENT,
});

export const GetCitiesByDepartmentSuccess = (
	cities: CITYDTO[],
): AppActions => ({
	type: GET_CITIES_BY_DEPARTMENT_SUCCESS,
	cities,
});

export const startGetCitiesByDepartment = (
	idDepartment: number) =>
	async (dispatch: Dispatch<AppActions>): Promise<void> => {
		try {
			dispatch(GetCitiesByDepartment());
			const response = await api.getCitiesByDepartment(idDepartment);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetCitiesByDepartmentSuccess(response?.data));
			}
		} catch (e) {
		
			dispatch(SetError('server-error'));
		}
	};

export const GetPhoneTypes = (): AppActions => ({
	type: GET_PHONES_TYPES,
});

export const GetPhoneTypesSuccess = (phoneTypes: PHONETYPE[]): AppActions => ({
	type: GET_PHONES_TYPES_SUCCESS,
	phoneTypes,
});

export const startGetPhoneTypes =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetPhoneTypes());
				const response = await api.getPhoneTypes();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetPhoneTypesSuccess(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetMailTypes = (): AppActions => ({
	type: GET_MAIL_TYPES,
});

export const GetMailTypesSuccess = (MailTypes: MAILTYPE[]): AppActions => ({
	type: GET_MAIL_TYPES_SUCCESS,
	MailTypes,
});

export const startGetMailTypes =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetMailTypes());
				const response = await api.getMailTypes();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetMailTypesSuccess(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetCurrencyTypes = (): AppActions => ({
	type: GET_CURRENCY_TYPES,
});

export const GetCurrencyTypesSuccess = (
	CurrencyTypes: CURRENCYTYPE[],
): AppActions => ({
	type: GET_CURRENCY_TYPES_SUCCESS,
	CurrencyTypes,
});

export const startGetCurrencyTypes =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetCurrencyTypes());
				const response = await api.getCurrencyTypes();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetCurrencyTypesSuccess(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetCategory = (): AppActions => ({
	type: GET_CATEGORY,
});

export const GetCategorySuccess = (Categories: CATEGORIES[]): AppActions => ({
	type: GET_CATEGORY_SUCCESS,
	Categories,
});

export const startGetCategory =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetCategory());
				const response = await api.getCategories();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetCategorySuccess(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetPayrollCategoryConcept = (): AppActions => ({
	type: GET_PAYROLL_CATEGORY_CONCEPT,
});

export const GetPayrollCategoryConceptSuccess = (
	payrollCategoriesConcept: any[],
): AppActions => ({
	type: GET_PAYROLL_CATEGORY_CONCEPT_SUCCESS,
	payrollCategoriesConcept,
});

export const startGetPayrollCategoryConcept =
	(data: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetPayrollCategoryConcept());
				const response = await api.getPayrollCategoryConcept(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetPayrollCategoryConceptSuccess(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetEmailDomains = (): AppActions => ({
	type: GET_EMAIL_DOMAINS,
});

export const GetEmailDomainsSuccess = (emailDomains: any[]): AppActions => ({
	type: GET_EMAIL_DOMAINS_SUCCESS,
	emailDomains,
});


export const GetAddressTypes = (): AppActions => ({
	type: GET_ADDRESS_TYPE,
});

export const GetAddressTypesSuccess = (
	addressTypes: ADDRESSTYPE[],
): AppActions => ({
	type: GET_ADDRESS_TYPE_SUCCESS,
	addressTypes,
});

export const startGetAddressTypes =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetAddressTypes());
				const response = await api.getAddressTypes();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAddressTypesSuccess(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllTypeRequest = (typeRequest: TYPEREQUEST[]): AppActions => ({
	type: GETALL_TYPE_REQUEST,
	typeRequest,
});

export const startGetAllTypeRequest =
	(IdTipoActor: number) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllTypeRequest(IdTipoActor);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllTypeRequest(response.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetAllTypeModality = (typeModality: TYPEMODALITY[]): AppActions => ({
	type: GETALL_TYPE_MODALITY,
	typeModality,
});

export const startGetAllTypeModality =
	(idTipoSolicitud: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllTypeModality(idTipoSolicitud);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllTypeModality(response.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const startGetEmailDomains =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetEmailDomains());
				const response = await api.getEmailDomains();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetEmailDomainsSuccess(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllTypeSubModality = (typeSubModality: TYPESUBMODALITY[]): AppActions => ({
	type: GETALL_TYPE_SUBMODALITY,
	typeSubModality,
});

export const startGetAllTypeSubModality =
	(idTipoModalidad: string) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllTypeSubModality(idTipoModalidad);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllTypeSubModality(response.data));
			}
		}
		catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetAllHomeTypes = (
	typeHome: TYPEHOME[],
): AppActions => ({
	type: GET_ALL_TYPE_HOME,
	typeHome
});

export const startGetAllTypeHome =
	() =>
		async (dispatch: Dispatch<AppActions>) => {
			try {

				const response = await api.getTypeHome();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllHomeTypes(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllAbonoUnico = (
	abonoUnico: ABONOUNICO[],
): AppActions => ({
	type: GET_ALL_ABONO_UNICO,
	abonoUnico
});

export const startGetAllAbonoUnico =
	() =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getAbonoUnico();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllAbonoUnico(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllEstadoCartera = (
	estadoCartera: ESTADOCARTERA[],
): AppActions => ({
	type: GET_ALL_ESTADO_CARTERA,
	estadoCartera,
});
export const startGetAllEstadoCartera =
	(props: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getEstadoCartera()
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllEstadoCartera(response.data));
		}
	};

export const GetAllTipoAbono = (
	tipoAbono: TIPOABONO[],
): AppActions => ({
	type: GET_ALL_TIPO_ABONO,
	tipoAbono
});
export const startGetAllTipoAbono =
	() =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getTipoAbono();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllTipoAbono(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllTipoSubsidio = (
	tipoSubsidio: TIPOSUBSIDO[],
): AppActions => ({
	type: GET_ALL_TIPO_SUBSIDIO,
	tipoSubsidio
});
export const startGetAllTipoSubsidio =
	() =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getTipoSubsidio();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllTipoSubsidio(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllTipoCuenta = (
	tipoCuenta: TIPOCUENTA[],
): AppActions => ({
	type: GET_ALL_TIPO_CUENTA,
	tipoCuenta
});
export const startGetAllTipoCuenta =
	() =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getAllTipoCuenta();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllTipoCuenta(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllEntidadBancaria = (
	entidadBancaria: ENTIDADBANCARIA[],
): AppActions => ({
	type: GET_ALL_ENTIDAD_BANCARIA,
	entidadBancaria
});

export const startGetAllEntidadBancaria =
	() =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getAllEntidadBancaria();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllEntidadBancaria(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllCatalogoPorId = (
	valorCatalogo: VALORCATALOGO[],
): AppActions => ({
	type: GET_ALL_VALOR_CATALOGO_POR_ID,
	valorCatalogo
});

export const startGetAllCatalogoPorId =
	(catalogoId: number) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getAllCatalogoPorId(catalogoId);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllCatalogoPorId(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllValoresCatalogoPorNombre = (
	nombreCatalogo: string,
	valorCatalogo: VALORCATALOGO[],
): AppActions => ({
	type: GET_ALL_VALOR_CATALOGO_POR_NOMBRE,
	nombreCatalogo,
	valorCatalogo
});

export const startGetAllValoresCatalogoPorNombre =
	(nombreCatalogo: string) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getValoresCatalogoPorNombre(nombreCatalogo);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllValoresCatalogoPorNombre(nombreCatalogo, response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};


export const GetAllCatalogoParentesco = (
	parentescoCatalogo: VALORCATALOGO[],
): AppActions => ({
	type: GET_ALL_VALOR_CATALOGO_PARENTESCO,
	parentescoCatalogo
});

export const startGetAllCatalogoParentesco =
	(catalogoId: number) =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getAllCatalogoPorId(catalogoId);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllCatalogoParentesco(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetAllEntidadEducativa = (
	entidadEducativa: ENTIDADEDUCATIVA[],
): AppActions => ({
	type: GETALL_ENTIDAD_EDUCATIVA,
	entidadEducativa,
});


export const startGetAllEntidadEducativa =
	() => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllEntidadEducativa();
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllEntidadEducativa(response.data));
		}
	};

export const GetAllProgramaEducativo = (
	programaEducativo: PROGRAMAEDUCATIVO[],
): AppActions => ({
	type: GETALL_PROGRAMA_EDUCATIVO,
	programaEducativo,
});

export const startGetAllProgramaEducativo =
	(idEntidad: string) => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllProgramaEducativo(idEntidad);
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllProgramaEducativo(response.data));
		}
	};

export const GetAllProgramaEducativoPorEntidad = (
	programaPorEntidad: PROGRAMAPORENTIDADDTO[],
): AppActions => ({
	type: GETALL_PROGRAMA_EDUCATIVO_POR_ENTIDAD,
	programaPorEntidad,
});

export const GetAllNivelEducativo = (
	nivelEducativo: NIVELEDUCATIVO[],
): AppActions => ({
	type: GETALL_NIVEL_EDUCATIVO,
	nivelEducativo,
});

export const startGetAllNivelEducativo =
	() => async (dispatch: Dispatch<AppActions>) => {
		const response = await api.getAllNivelEducativo();
		if (response?.isSuccess == null) {
			dispatch(SetError(response));
		} else {
			dispatch(GetAllNivelEducativo(response.data));
		}
	};

export const GetAverageCost = (): AppActions => ({
	type: GET_AVERAGE_COST,
});

export const GetAverageCostSuccess = (getAverageCost: AVERAGECOSTDTO[]): AppActions => ({
	type: GET_AVERAGE_COST_SUCCESS,
	MediosDeEnvio: getAverageCost
})

export const GetAverageCostFailure = (getAverageCost: AVERAGECOSTDTO[]): AppActions => ({
	type: GET_AVERAGE_COST_FAILURE,
	MediosDeEnvio: getAverageCost
})

export const startGetAverageCost = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetAverageCost())
			const response = await api.getAverageCost();
			if (response?.isSuccess == null) {
				dispatch(GetAverageCostFailure(response.data));
			} else {
				dispatch(GetAverageCostSuccess(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetFormat = (): AppActions => ({
	type: GET_FORMAT
});

export const GetFormatSuccess = (getFormat: FORMATDTO[]): AppActions => ({
	type: GET_FORMAT_SUCCESS,
	Formato: getFormat
})

export const GetFormatFailure = (getFormat: FORMATDTO[]): AppActions => ({
	type: GET_FORMAT_FAILURE,
	Formato: getFormat
})

export const startGetFormat = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetFormat())
			const response = await api.getFormat();
			if (response?.isSuccess == null) {
				dispatch(GetFormatFailure(response.data));
			} else {
				dispatch(GetFormatSuccess(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetAllEntidadSeguroEducativo = (
	entidadSeguroEducativo: ENTIDADSEGUROEDUCATIVO[],
): AppActions => ({
	type: GET_ALL_ENTIDAD_SEGURO_EDUCATIVO,
	entidadSeguroEducativo
});

export const startGetAllEntidadSeguroEducativo =
	() =>
		async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getAllEntidaSeguroEducativo();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetAllEntidadSeguroEducativo(response?.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const GetInsertTypeIdSuccess = (
	IdentificacionType: IDENTIFICATIONTYPEDTO[],
): AppActions => ({
	type: GET_INSERT_TYPE_ID_SUCCESS,
	IdentificacionType: IdentificacionType
});

export const GetInsertTypeIdFailure = (
	error: any,
): AppActions => ({
	type: GET_INSERT_TYPE_ID_FAILURE,
	error: error
});

export const startPostInsertTypeId =
	(typeId: IDENTIFICATIONTYPEDTO) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.postInsertTypeId(typeId);
			if (response?.isSuccess == null) {
				dispatch(GetInsertTypeIdFailure(response));
			} else {
				dispatch(GetInsertTypeIdSuccess(response.data));
			}
		} catch (e) {
			
			dispatch(GetInsertTypeIdFailure('server-error'));
		}
	};

export const UpdateTypeIdSuccess = (
	typeId: IDENTIFICATIONTYPEDTO,
): AppActions => ({
	type: UPDATE_TYPE_ID_SUCCESS,
	typeId: typeId
});

export const startUpdateTypeId =
	(typeId: IDENTIFICATIONTYPEDTO) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.UpdateTypeId(typeId);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(UpdateTypeIdSuccess(response.data));
			}
		} catch (e) {
			
			dispatch(GetInsertTypeIdFailure('server-error'));
		}
	};

export const GetAllGeneros = (
	sextypes: SEXODTO[],
): AppActions => ({
	type: GET_ALL_SEX_TYPES,
	sextypes: sextypes
});

export const startGetAllGeneros = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllGeneros();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllGeneros(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const postInsertTypeGeneroSuccess = (
	generotype: any,
): AppActions => ({
	type: INSERT_TYPE_GENERO_SUCCESS,
	generotype: generotype
});

export const startPostInsertTypeGenero =
	(sextype: SEXODTO) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.postInsertTypeGenero(sextype);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(postInsertTypeGeneroSuccess(response.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const updateTypeGeneroSuccess = (
	sextype: SEXODTO,
): AppActions => ({
	type: UPDATE_TYPE_GENERO_SUCCESS,
	sextype: sextype
});

export const startUpdateTypeGenero =
	(sextype: SEXODTO) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.updateTypeGenero(sextype);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(updateTypeGeneroSuccess(response.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetAllTypesForces = (
	forces: FORCEDTO[],
): AppActions => ({
	type: GETALL_FORCES,
	forces: forces
});

export const startGetAllTypesForces = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllForces();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllTypesForces(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const postInsertForce = (
	forces: FORCEDTO[],
): AppActions => ({
	type: GETALL_FORCES,
	forces: forces
});

export const startPostInsertForce = (force: FORCEDTO) =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.postInsertForce(force);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(postInsertForce(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const UpdateTypeForceSuccess = (
	force: FORCEDTO,
): AppActions => ({
	type: UPDATE_TYPE_FORCE_SUCCESS,
	force: force
});

export const startUpdateForce =
	(force: FORCEDTO) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.updateForce(force);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(UpdateTypeForceSuccess(response.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetTypeAffiliate = (): AppActions => ({
	type: GET_TYPE_AFFILIATE,
});

export const GetTypeAffiliateSuccess = (tipoAfiliacion: TIPOAFILIACIONDTO[]): AppActions => ({
	type: GET_TYPE_AFFILIATE_SUCCESS,
	tipoAfiliacion: tipoAfiliacion,
});

export const GetTypeAffiliateFailure = (tipoAfiliacion: TIPOAFILIACIONDTO[] | null): AppActions => ({
	type: GET_TYPE_AFFILIATE_FAILURE,
	tipoAfiliacion: tipoAfiliacion,
});

export const startGetTypeAffiliate = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetTypeAffiliate());
			const response =
				await api.getTypeAffiliate();
			if (!response?.isSuccess) {
				dispatch(
					GetTypeAffiliateFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetTypeAffiliateSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró ningún tipo de afiliación.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetTypeAffiliateFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetAffiliateState = (): AppActions => ({
	type: GET_AFFILIATE_STATE,
});

export const GetAffiliateStateSuccess = (estadoAfiliacion: ESTADOAFILIACIONDTO[]): AppActions => ({
	type: GET_AFFILIATE_STATE_SUCCESS,
	estadoAfiliacion: estadoAfiliacion,
});

export const GetAffiliateStateFailure = (estadoAfiliacion: ESTADOAFILIACIONDTO[] | null): AppActions => ({
	type: GET_AFFILIATE_STATE_FAILURE,
	estadoAfiliacion: estadoAfiliacion,
});

export const startGetAffiliateState = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetAffiliateState());
			const response =
				await api.getAffiliateState();
			if (!response?.isSuccess) {
				dispatch(
					GetAffiliateStateFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetAffiliateStateSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró el estado de la afiliación.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetAffiliateStateFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetQualityField = (): AppActions => ({
	type: GET_QUALITY_FIELD,
});

export const GetQualityFieldSuccess = (campoCalidad: CAMPOCALIDADDTO[]): AppActions => ({
	type: GET_QUALITY_FIELD_SUCCESS,
	campoCalidad: campoCalidad,
});

export const GetQualityFieldFailure = (campoCalidad: CAMPOCALIDADDTO[] | null): AppActions => ({
	type: GET_QUALITY_FIELD_FAILURE,
	campoCalidad: campoCalidad,
});

export const startGetQualityField = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetQualityField());
			const response =
				await api.getQualityField();
			if (!response?.isSuccess) {
				dispatch(
					GetQualityFieldFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetQualityFieldSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró el campo de calidad.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetQualityFieldFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetAffiliateTypeProcedure = (): AppActions => ({
	type: GET_AFFILIATE_TYPE_PROCEDURE,
});

export const GetAffiliateTypeProcedureSuccess = (tipoAfiliacionProcedente: TIPOAFILIACIONPRODECENTEDTO[]): AppActions => ({
	type: GET_AFFILIATE_TYPE_PROCEDURE_SUCCESS,
	tipoAfiliacionProcedente: tipoAfiliacionProcedente,
});

export const GetAffiliateTypeProcedureFailure = (tipoAfiliacionProcedente: TIPOAFILIACIONPRODECENTEDTO[] | null): AppActions => ({
	type: GET_AFFILIATE_TYPE_PROCEDURE_FAILURE,
	tipoAfiliacionProcedente: tipoAfiliacionProcedente,
});

export const startGetAffiliateTypeProcedure = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetAffiliateTypeProcedure());
			const response =
				await api.getAffiliateTypeProcedure();
			if (!response?.isSuccess) {
				dispatch(
					GetAffiliateTypeProcedureFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetAffiliateTypeProcedureSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró el tipo de afiliación procedente.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetAffiliateTypeProcedureFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetAllCategories = (
	categories: CATEGORIES[],
): AppActions => ({
	type: GET_CATEGORY_SUCCESS,
	Categories: categories
});

export const startGetAllCategories = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllCategories();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllCategories(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const postInsertCategory = (
	category: CATEGORIES,
): AppActions => ({
	type: INSERT_TYPE_CATEGORY_SUCCESS,
	category: category
});

export const startPostInsertCategory = (category: CATEGORIES) =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.postInsertCategory(category);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(postInsertCategory(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const UpdateTypeCategorySuccess = (
	category: CATEGORIES,
): AppActions => ({
	type: UPDATE_TYPE_CATEGORY_SUCCESS,
	category: category
});

export const startUpdateCategory =
	(category: CATEGORIES) => async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.updateCategory(category);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(UpdateTypeCategorySuccess(response.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetDiscountPercentage = (): AppActions => ({
	type: GET_DISCOUNT_PERCENTAGE,
});

export const GetDiscountPercentageSuccess = (porcentajeDescuento: PORCENTAJEDESCUENTODTO[]): AppActions => ({
	type: GET_DISCOUNT_PERCENTAGE_SUCCESS,
	porcentajeDescuento: porcentajeDescuento,
});

export const GetDiscountPercentageFailure = (porcentajeDescuento: PORCENTAJEDESCUENTODTO[] | null): AppActions => ({
	type: GET_DISCOUNT_PERCENTAGE_FAILURE,
	porcentajeDescuento: porcentajeDescuento,
});

export const startGetDiscountPercentage = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetDiscountPercentage());
			const response =
				await api.getDiscountPercentage();
			if (!response?.isSuccess) {
				dispatch(
					GetDiscountPercentageFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetDiscountPercentageSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró el tipo de afiliación procedente.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetDiscountPercentageFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetAllGroupInconsistenciesSuccess = (
	gruposInconsistencias: GRUPOSINCONSITENCIASDTO[],
): AppActions => ({
	type: GET_ALL_GROUP_INCONSISTENCIES,
	gruposInconsistencias: gruposInconsistencias
});

export const startGetAllGroupsInconsistencies = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllGroupInconsistencies();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetAllGroupInconsistenciesSuccess(response?.data));
			}
		} catch (e) {
			dispatch(SetError('server-error'));
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetInconsistenciesByGroupSuccess = (
	inconsistencias: INCONSISTENCIADTO[],
): AppActions => ({
	type: GET_ALL_INCONSISTENCIES_BY_GROUP,
	inconsistencias: inconsistencias
});

export const startGetInconsistenciesByGroup = (grupo: number) =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getInconsistenciesByGroup(grupo);
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetInconsistenciesByGroupSuccess(response?.data));
			}
		} catch (e) {
			dispatch(SetError('server-error'));
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetAllToolsInconsistenciesSuccess = (
	herramientasInconsistencia: HERRAMIENTAINCONSISTENCIADTO[],
): AppActions => ({
	type: GET_ALL_TOOLS_INCONSISTENCIES,
	herramientasInconsistencia: herramientasInconsistencia
});

export const startGetAllToolsInconsistencies = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllToolsInconsistencies();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetAllToolsInconsistenciesSuccess(response?.data));
			}
		} catch (e) {
			dispatch(SetError('server-error'));
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetAllPointsAttentionSuccess = (
	puntosAtencion: PUNTOSATENCIONDTO[],
): AppActions => ({
	type: GET_ALL_POINTS_ATTENTION,
	puntosAtencion: puntosAtencion
});

export const startGetAllPointsAttention = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllPointsAttention();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(GetAllPointsAttentionSuccess(response?.data));
			}
		} catch (e) {
			dispatch(SetError('server-error'));
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetAllStatesRequestSuccess = (
	estadosTareas: ESTADOTAREADTO[],
): AppActions => ({
	type: GET_ALL_STATE_REQUEST,
	estadosTareas: estadosTareas
});

export const startGetAllStatesRequest = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			const response = await api.getAllRequestState();
			if (response?.isSuccess == null) {
				dispatch(SetError(response));
			} else {
				dispatch(GetAllStatesRequestSuccess(response?.data));
			}
		} catch (e) {
			
			dispatch(SetError('server-error'));
		}
	};

export const GetRegistrationOffice = (): AppActions => ({
	type: GET_REGISTRATION_OFFICE,
});

export const GetRegistrationOfficeSuccess = (oficinasRegistro: OFICINASREGISTRODTO[]): AppActions => ({
	type: GET_REGISTRATION_OFFICE_SUCCESS,
	oficinasRegistro: oficinasRegistro,
});

export const GetRegistrationOfficeFailure = (oficinasRegistro: OFICINASREGISTRODTO[] | null): AppActions => ({
	type: GET_REGISTRATION_OFFICE_FAILURE,
	oficinasRegistro: oficinasRegistro,
});

export const startGetRegistrationOffice = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetRegistrationOffice());
			const response =
				await api.getRegistrationOffice();
			if (!response?.isSuccess) {
				dispatch(
					GetRegistrationOfficeFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetRegistrationOfficeSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró ninguna oficina de registro.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetRegistrationOfficeFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetCausalBlockage = (): AppActions => ({
	type: GET_CAUSAL_BLOCKAGE,
});

export const GetCausalBlockageSuccess = (causalBloqueo: CAUSALBLOQUEODTO[]): AppActions => ({
	type: GET_CAUSAL_BLOCKAGE_SUCCESS,
	causalBloqueo: causalBloqueo,
});

export const GetCausalBlockageFailure = (causalBloqueo: CAUSALBLOQUEODTO[] | null): AppActions => ({
	type: GET_CAUSAL_BLOCKAGE_FAILURE,
	causalBloqueo: causalBloqueo,
});

export const startGetCausalBlockage = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetCausalBlockage());
			const response =
				await api.getCausalBlockage();
			if (!response?.isSuccess) {
				dispatch(
					GetCausalBlockageFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetCausalBlockageSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró ningún causal de bloqueo.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetCausalBlockageFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetRequestTypeRealStateRegistration = (): AppActions => ({
	type: GET_REQUEST_TYPE_REAL_STATE_REGISTRATION,
});

export const GetRequestTypeRealStateRegistrationSuccess =
	(tipoSolicitudMatriculaInmobiliaria: REQUESTTYPEREALSTATEREGISTRATIONDTO[]): AppActions => ({
		type: GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_SUCCESS,
		tipoSolicitudMatriculaInmobiliaria: tipoSolicitudMatriculaInmobiliaria,
	});

export const GetRequestTypeRealStateRegistrationFailure =
	(tipoSolicitudMatriculaInmobiliaria: REQUESTTYPEREALSTATEREGISTRATIONDTO[] | null): AppActions => ({
		type: GET_REQUEST_TYPE_REAL_STATE_REGISTRATION_FAILURE,
		tipoSolicitudMatriculaInmobiliaria: tipoSolicitudMatriculaInmobiliaria,
	});

export const startGetRequestTypeRealStateRegistration = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetRequestTypeRealStateRegistration());
			const response =
				await api.getRequestTypeRealStateRegistration();
			if (!response?.isSuccess) {
				dispatch(
					GetRequestTypeRealStateRegistrationFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetRequestTypeRealStateRegistrationSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró ningún tipo de solicitud.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetRequestTypeRealStateRegistrationFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

export const GetModalidadAfiliacionVeterano = (): AppActions => ({
	type: GET_MODALIDAD_AFILIACION_VETERANO,
});

export const GetModalidadAfiliacionVeteranoSuccess = (ModalidadAfiliacionVeterano: MODALIDADAFILIACIONVETERANODTO[]): AppActions => ({
	type: GET_MODALIDAD_AFILIACION_VETERANO_SUCCESS,
	ModalidadAfiliacionVeterano: ModalidadAfiliacionVeterano,
});

export const GetModalidadAfiliacionVeteranoFailure = (ModalidadAfiliacionVeterano: MODALIDADAFILIACIONVETERANODTO[] | null): AppActions => ({
	type: GET_MODALIDAD_AFILIACION_VETERANO_FAILURE,
	ModalidadAfiliacionVeterano: ModalidadAfiliacionVeterano,
});


export const startGetModalidadAfiliacionVeterano = () =>
	async (dispatch: Dispatch<AppActions>) => {
		try {
			dispatch(GetModalidadAfiliacionVeterano());
			const response =
				await api.GetModalidadAfiliacionVeterano();
			if (!response?.isSuccess) {
				dispatch(
					GetModalidadAfiliacionVeteranoFailure(response.data),
				);
				toast('Error: respuesta errada del servidor');
			} else {
				dispatch(
					GetModalidadAfiliacionVeteranoSuccess(
						response.data,
					),
				);
				if (response.data == null) {
					toast('No se encontró ninguna modalidad del veterano.');
				}
			}
		} catch (e) {
			
			dispatch(
				GetModalidadAfiliacionVeteranoFailure(null),
			);
			toast('Error: respuesta errada del servidor');
		}
	};

	export const GetBeneficiarioCartera = (
		beneficiarioCartera: any,
	): AppActions => ({
		type: GET_BENEFICIARIO_CARTERA,
		beneficiarioCartera,
	});
	
	export const startGetBeneficiarioCartera =
		() => async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getBeneficiarioCartera();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetBeneficiarioCartera(response.data));
				}
			} catch (e) {
		
				dispatch(SetError('server-error'));
			}
		};

	export const GetConceptosCartera = (
		conceptosCartera: any,
	): AppActions => ({
		type: GET_CONCEPTOS_CARTERA,
		conceptosCartera,
	});
	
	export const startGetConceptosCartera =
		() => async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getConceptosCartera();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetConceptosCartera(response.data));
				}
			} catch (e) {
		
				dispatch(SetError('server-error'));
			}
		};

	export const GetTipoComprobante = (
		tipoComprobante: any,
	): AppActions => ({
		type: GET_TIPOS_COMPROBANTE,
		tipoComprobante,
	});
	
	export const startGetTipoComprobante =
	
		() => async (dispatch: Dispatch<AppActions>) => {
			try {
				const response = await api.getTipoComprobante();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetTipoComprobante(response.data));
				}
			} catch (e) {
		
				dispatch(SetError('server-error'));
			}
		};
	

