import { Dispatch } from 'redux';
import moment from 'moment';
import { toast } from 'react-toastify';
import CreditApi from '../services/api-credit.service';
import { AppActions } from '../types/appactions.type';
import {

	CLEAR_DETAIL_REQUEST_INFORMATION,
	GET_CREDIT_FINANCIAL_ANALYST_DATA,
	GET_CREDIT_FINANCIAL_ANALYST_DATA_SUCCESS,
	GET_DETAIL_COMMITTEE,
	GET_DETAIL_COMMITTEE_SUCCESS,
	ON_ERROR,
	POST_CONSULT_CREDIT,
	POST_CONSULT_CREDIT_SUCCESS,
	CLEAR_CREATEE_DESEMBOLSOO_REQUEST,
	POST_CREATEE_DESEMBOLSOO_REQUEST,
	POST_CREATEE_DESEMBOLSOO_REQUEST_SUCCESS,
	POST_CREATE_CREDIT_COMMITTEE_REQUEST,
	POST_CREATE_CREDIT_COMMITTEE_REQUEST_SUCCESS,
	POST_CREATE_SIMULATION,
	POST_CREATE_SIMULATION_SUCCESS,
	POST_ERROR_SIMULATION_SOBRANTE,
	POST_CREATE_UPDATE_COMMITTEE,
	POST_CREATE_UPDATE_COMMITTEE_SUCCESS,
	POST_CREDIT_BASIC_DATA,
	POST_CREDIT_BASIC_DATA_SUCCESS,
	POST_CREDIT_COMMITTEE,
	POST_CREDIT_COMMITTEE_APPROVE,
	POST_CREDIT_COMMITTEE_APPROVE_SUCCESS,
	POST_CREDIT_COMMITTEE_REQUEST,
	POST_CREDIT_COMMITTEE_REQUEST_SUCCESS,
	POST_CREDIT_COMMITTEE_SUCCESS,
	POST_CREDIT_CREATE_LEGAL_DATA,
	POST_CREDIT_CREATE_LEGAL_DATA_SUCCESS,
	POST_CREDIT_CREATE_TECHNICAL_DATA,
	POST_CREDIT_CREATE_TECHNICAL_DATA_SUCCESS,
	POST_CREDIT_FILES_DATA,
	POST_CREDIT_FILES_DATA_SUCCESS,
	POST_CREDIT_FINANCIAL_DATA,
	POST_CREDIT_FINANCIAL_DATA_SUCCESS,
	POST_CREDIT_FLOW_DOCUMENTS,
	POST_CREDIT_FLOW_DOCUMENTS_SUCCESS,
	POST_CREDIT_LEGAL_DATA,
	POST_CREDIT_LEGAL_DATA_SUCCESS,
	POST_CREDIT_OBSERVATIONS_DATA,
	POST_CREDIT_OBSERVATIONS_DATA_SUCCESS,
	POST_CREDIT_PRODUCT_DATA,
	POST_CREDIT_PRODUCT_DATA_SUCCESS,
	POST_CREDIT_TECHNICAL_DATA,
	POST_CREDIT_TECHNICAL_DATA_SUCCESS,
	POST_PROCEDURE_REQUEST,
	POST_PROCEDURE_REQUEST_SUCCESS,
	PUT_REQUEST_ACTIVITY_PROCESS,
	PUT_REQUEST_ACTIVITY_PROCESS_SUCCESS,
	RESET_CREDIT_STATE,
	GET_CREDIT_FLOW_DOCUMENTS_SUCCESS,
	GET_PRUEBA_DATA,
	GET_PRUEBA_DATA_SUCCESS,
	POST_LEGALIZACION_REQUEST,
	POST_LEGALIZACION_REQUEST_SUCCESS,
	POST_LEGALIZACION_FIRMAS_REQUEST,
	POST_LEGALIZACION_FIRMAS_REQUEST_SUCCESS,
	POST_VERIFICACION_TERCEROS_REQUEST,
	POST_VERIFICACION_TERCEROS_REQUEST_SUCCESS,
	GET_DATA_LEGALIZACION_REQUEST,
	GET_DATA_LEGALIZACION_REQUEST_SUCCESS,
	PATCH_DESEMBOLSO_REQUEST,
	PATCH_DESEMBOLSO_REQUEST_SUCCESS,
	GET_DESEMBOLSO_DESISTIMIENTO_REQUEST_SUCCESS,
	GET_DESEMBOLSO_DESISTIMIENTO_REQUEST,
	POST_DESISTIR_DESEMBOLSO_REQUEST,
	POST_DESISTIR_DESEMBOLSO_REQUEST_SUCCESS,
	GET_OBTENER_VALOR_DESEMBOLSO_REQUEST,
	GET_OBTENER_VALOR_DESEMBOLSO_REQUEST_SUCCESS,

	POST_OBTENER_VENDEDOR_REQUEST_SUCCESS,
    POST_OBTENER_VENDEDOR_REQUEST_FAILURE,
	POST_OBTENER_VENDEDOR_REQUEST,
    POST_ELIMINAR_VENDEDOR_REQUEST_SUCCESS,
    POST_ELIMINAR_VENDEDOR_REQUEST_FAILURE,
	POST_ELIMINAR_VENDEDOR_REQUEST,
    POST_CREAR_VENDEDOR_REQUEST_SUCCESS,
    POST_CREAR_VENDEDOR_REQUEST_FAILURE,
	POST_CREAR_VENDEDOR_REQUEST,
	POST_OBTENER_COMPROBANTE_CREDITO,
	POST_OBTENER_COMPROBANTE_CREDITO_SUCCESS,
	POST_CREDIT_COMMITTEE_SOLI_SUCCESS,
	POST_CREDIT_COMMITTEE_SOLI,
	POST_CREA_APROVACION_DESEMBOLSO_SUCCESS,
	POST_CREA_APROVACION_DESEMBOLSO,
	POST_GENERAR_BALANCE_SUCCESS,
	POST_GENERAR_BALANCE,
	POST_LIMPIAR_BALANCE,
	POST_GENERAR_AUX_BALANCE_SUCCESS,
	POST_GENERAR_AUX_BALANCE

} from '../types/credit.actions.type';
import { ProcedureRequestModel } from '../models/procedure-request.model';
import { ProcedureRequestRequestModel } from '../models/requests/procedure-request-request.model';
import CreditAdapter from '../adapters/credit.adapter';
import AuthService from '../services/auth.service';
import { RequestActivityRequestModel } from '../models/requests/request-activity-request.model';
import { REQUEST_ACTIVITY_PROCESS } from '../constants/constants';
import { CreditBasicDataModel } from '../models/credit-basic-data.model';
import { CreditBasicDataRequestModel } from '../models/requests/credit-basic-data-request.model';
import { CreditProductModel } from '../models/credit-product.model';
import { CreditProductRequestModel } from '../models/requests/credit-product-request.model';
import { CreditObservationModel } from '../models/credit-observation.model';
import { CreditObservationsRequestModel } from '../models/requests/credit-observations.request.model';
import { CreditFileModel } from '../models/credit-file.model';
import { CreditFilesRequestModel } from '../models/requests/credit-files-request.model';
import { CreditFinancialDataModel } from '../models/credit-financial-data.model';
import { CreditFinancialDataRequestModel } from '../models/requests/credit-financial-data-request.model';
import { CreditTechnicalCreateRequestModel } from '../models/requests/credit-technical-create-request.model';
import { CreditTechnicalDataRequestModel } from '../models/requests/credit-technical-data-request.model';
import { CreditTechnicalDataModel } from '../models/credit-technical-data.model';
import { CreditLegalDataRequestModel } from '../models/requests/credit-legal-data-request.model';
import { CreditLegalCreateRequestModel } from '../models/requests/credit-legal-create-request.model';
import { CreditLegalDataModel } from '../models/credit-legal-data.model';
import { CreditFinancialAnalystRequestModel } from '../models/requests/credit-financial-analyst-request.model';
import { CreditFlowDocumentModel } from '../models/credit-flow-document.model';
import { CreditSimulationDataModel } from '../models/credit-simulation-data.model';
import { CreditConsultCreditModel } from '../models/credit-consult-credit.model';
import { CreditConsultCreditRequestModel } from '../models/requests/credit-consult-credit-request.model';
import { CreditCommitteeModel } from '../models/credit-committee.model';
import { CreateUpdateCommittee } from '../models/requests/create-update-committee-request.model';
import { CreditDocumentModel } from '../models/credit-dpcument.model';
import { Prueba } from '../models/prueba-boton-model';
import NotificationService from '../services/notification.service';
import { CreditVendedorModel } from '../models/credit-vendedor.model';
import CarteraApi from '../services/api-cartera-service';

const api = CreditApi();
const apiCartera = CarteraApi();

const PostProcedureRequest = (): AppActions => ({
	type: POST_PROCEDURE_REQUEST,
});

const PostProcedureRequestSuccess = (
	procedureRequests: ProcedureRequestModel[],
): AppActions => ({
	type: POST_PROCEDURE_REQUEST_SUCCESS,
	procedureRequests,
});

export const startPostProcedureRequest =
	(data: ProcedureRequestRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				const user = AuthService.getUserInfo();
				data.usuario = user.USR_ID;
				dispatch(PostProcedureRequest());
				const response = await api.getProcessRequest(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptProcessRequestList(
						response.data,
					);
					dispatch(PostProcedureRequestSuccess(dataAdapted));
				}
			} catch (e) {
			
				dispatch(SetError('server-error'));
			}
		};

const SetError = (error: any): AppActions => ({
	type: ON_ERROR,
	error,
});

export const resetCreditState = (): AppActions => ({
	type: RESET_CREDIT_STATE,
});

export const clearDetailRequestInformation = (): AppActions => ({
	type: CLEAR_DETAIL_REQUEST_INFORMATION,
});

const PutRequestActivityProcess = (): AppActions => ({
	type: PUT_REQUEST_ACTIVITY_PROCESS,
});

const PutRequestActivityProcessSuccess = (
	result: RequestActivityRequestModel,
): AppActions => ({
	type: PUT_REQUEST_ACTIVITY_PROCESS_SUCCESS,
	result,
});

export const startRequestActivityProcess =
	(data: RequestActivityRequestModel, requestType: number) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			let response;
			try {
				const user = AuthService.getUserInfo();
				data.acT_MODIFICADO_POR = user.USR_ID;

				switch (requestType) {
					case REQUEST_ACTIVITY_PROCESS.APPROVE:
						response = await api.putRequestActivityApprove(data);
						break;
					case REQUEST_ACTIVITY_PROCESS.REJECTED:
						response = await api.putRequestActivityRejected(data);
						break;
					case REQUEST_ACTIVITY_PROCESS.REMEDY:
						response = await api.putRequestActivityRemedy(data);
						break;
					case REQUEST_ACTIVITY_PROCESS.UPDATE:
						response = await api.putRequestActivityUpdate(data);
						break;
					case REQUEST_ACTIVITY_PROCESS.BACK:
						response = await api.putRequestActivityBack(data);
						break;
					case REQUEST_ACTIVITY_PROCESS.DESISTIR:
						response = await api.putRequestActivityDisitir(data);
					
						break;
					default:
						response = null;
				}

				if (response != null) {
					if (response?.isSuccess == null) {
						dispatch(SetError(response.returnMessage));
					} else if (response.data == null) {
						dispatch(SetError(response.returnMessage));
					} else {
						dispatch(PutRequestActivityProcessSuccess(response.data));
					}
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditBasicData = (): AppActions => ({
	type: POST_CREDIT_BASIC_DATA,
});

const PostCreditBasicDataSuccess = (
	creditBasicData: CreditBasicDataModel,
): AppActions => ({
	type: POST_CREDIT_BASIC_DATA_SUCCESS,
	creditBasicData,
});

export const startPostCreditBasicData =
	(data: CreditBasicDataRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditBasicData());
				const response = await api.postCreditBasicData(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptCreditBasicData(
						response.data,
					);
					dispatch(PostCreditBasicDataSuccess(dataAdapted));
				}
			} catch (e) {
			
				dispatch(SetError('server-error'));
			}
		};

const PostCreditProductData = (): AppActions => ({
	type: POST_CREDIT_PRODUCT_DATA,
});

const PostCreditProductDataSuccess = (
	creditProductData: CreditProductModel,
): AppActions => ({
	type: POST_CREDIT_PRODUCT_DATA_SUCCESS,
	creditProductData,
});

export const startPostCreditProductData =
	(data: CreditProductRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				await dispatch(PostCreditProductData());
				const response = await api.postCreditProductData(data);
				
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptCreditProductData(
						response.data,
					);
					await dispatch(PostCreditProductDataSuccess(dataAdapted));
				}
			} catch (e) {
			
				dispatch(SetError('server-error'));
			}
		};

const PostCreditObservationsData = (): AppActions => ({
	type: POST_CREDIT_OBSERVATIONS_DATA,
});

const PostCreditObservationsDataSuccess = (
	observationsData: CreditObservationModel[],
): AppActions => ({
	type: POST_CREDIT_OBSERVATIONS_DATA_SUCCESS,
	observationsData,
});

export const startPostCreditObservationsData =
	(data: CreditObservationsRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditObservationsData());
				const response = await api.postCreditObservations(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptObservationsList(
						response.data,
					);
					dispatch(PostCreditObservationsDataSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditFilesData = (): AppActions => ({
	type: POST_CREDIT_FILES_DATA,
});

const PostCreditFilesDataSuccess = (
	filesData: CreditFileModel[],
): AppActions => ({
	type: POST_CREDIT_FILES_DATA_SUCCESS,
	filesData,
});

export const startPostCreditFilesData =
	(data: CreditFilesRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			
			try {
				dispatch(PostCreditFilesData());
				const response = await api.postCreditFiles(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptFilesList(response.data);
					dispatch(PostCreditFilesDataSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const GetPruebaData = (): AppActions => ({
	type: GET_PRUEBA_DATA
});

const GetPruebaDataSuccess = (
	prueba: Prueba,
): AppActions => ({
	type: GET_PRUEBA_DATA_SUCCESS,
	prueba,
});

export const startGetPruebaData =
	(data: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetPruebaData());
				const response = await api.getPrueba(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {

					dispatch(GetPruebaDataSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditFinancialData = (): AppActions => ({
	type: POST_CREDIT_FINANCIAL_DATA,
});

const PostCreditFinancialDataSuccess = (
	financialData: CreditFinancialDataModel,
): AppActions => ({
	type: POST_CREDIT_FINANCIAL_DATA_SUCCESS,
	financialData,
});

export const startPostCreditFinancialData =
	(data: CreditFinancialDataRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditFinancialData());
				const response = await api.postCreditFinancialData(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptFinancialData(
						response.data,
					);
					dispatch(PostCreditFinancialDataSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditCreateTechnicalData = (): AppActions => ({
	type: POST_CREDIT_CREATE_TECHNICAL_DATA,
});

const PostCreditCreateTechnicalDataSuccess = (
	creditTechnicalData: CreditTechnicalDataModel,
): AppActions => ({
	type: POST_CREDIT_CREATE_TECHNICAL_DATA_SUCCESS,
	creditTechnicalData,
});

export const startPostCreditCreateTechnicalData =
	(data: CreditTechnicalCreateRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditCreateTechnicalData());
				await api.postCreditCreateTechnicalData(data).then(response => {
					if (response?.isSuccess == null) {
						dispatch(SetError(response));
					} else {
						const dataAdapted = CreditAdapter.adaptCreditTechnicalData(
							response.data,
						);
						dispatch(PostCreditCreateTechnicalDataSuccess(dataAdapted));
					}
				});
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditTechnicalData = (): AppActions => ({
	type: POST_CREDIT_TECHNICAL_DATA,
});

const PostCreditTechnicalDataSuccess = (
	creditTechnicalData: CreditTechnicalDataModel,
): AppActions => ({
	type: POST_CREDIT_TECHNICAL_DATA_SUCCESS,
	creditTechnicalData,
});

export const startPostCreditTechnicalData =
	(data: CreditTechnicalDataRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
			await dispatch(PostCreditTechnicalData());
				await api.postCreditTechnicalData(data).then(response => {
					if (response?.isSuccess == null) {
						dispatch(SetError(response));
					} else {
						const dataAdapted = CreditAdapter.adaptCreditTechnicalData(
							response.data,
						);
					dispatch(PostCreditTechnicalDataSuccess(dataAdapted));
					}
				});
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditLegalData = (): AppActions => ({
	type: POST_CREDIT_LEGAL_DATA,
});

const PostCreditLegalDataSuccess = (
	creditLegalData: CreditLegalDataModel,
): AppActions => ({
	type: POST_CREDIT_LEGAL_DATA_SUCCESS,
	creditLegalData,
});

export const startPostCreditLegalData =
	(data: CreditLegalDataRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditLegalData());
				const response = await api.postCreditLegalData(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					
					const dataAdapted = CreditAdapter.adaptCreditLegalData(
						response.data,
					);
					dispatch(PostCreditLegalDataSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditCreateLegalData = (): AppActions => ({
	type: POST_CREDIT_CREATE_LEGAL_DATA,
});

const PostCreditCreateLegalDataSuccess = (
	creditLegalData: CreditLegalDataModel,
): AppActions => ({
	type: POST_CREDIT_CREATE_LEGAL_DATA_SUCCESS,
	creditLegalData,
});

export const startPostCreditCreateLegalData =
	(data: CreditLegalCreateRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditCreateLegalData());
				const response = await api.postCreditCreateLegalData(data);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptCreditLegalData(
						response.data,
					);
					dispatch(PostCreditCreateLegalDataSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const GetCreditFinancialAnalystData = (): AppActions => ({
	type: GET_CREDIT_FINANCIAL_ANALYST_DATA,
});

const GetCreditFinancialAnalystDataSuccess = (
	creditProductData: CreditProductModel,
	creditLegalData: CreditLegalDataModel,
	creditTechnicalData: CreditTechnicalDataModel,
	creditSimulationData: CreditSimulationDataModel,
	creditConsultCreditData: CreditConsultCreditModel,
): AppActions => ({
	type: GET_CREDIT_FINANCIAL_ANALYST_DATA_SUCCESS,
	creditProductData,
	creditLegalData,
	creditTechnicalData,
	creditSimulationData,
	creditConsultCreditData,
});

export const startGetCreditFinancialAnalystData =
	(request: CreditFinancialAnalystRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			dispatch(GetCreditFinancialAnalystData());
			const responseProduct = await api
				.postCreditProductData(request.requestProductData)
				.catch(e =>dispatch(SetError(`server-error, ${e}`)));
			const responseLegal = await api
				.postCreditLegalData(request.requestLegalData)
				.catch(e =>dispatch(SetError(`server-error, ${e}`)));
			const responseTechnical = await api
				.postCreditTechnicalData(request.requestTechnicalData)
				.catch(e =>dispatch(SetError(`server-error, ${e}`)));
			const responseSimulation = await api
				.postCreditClientSimulation(request.requestClientSimulation)
				.catch(e =>dispatch(SetError(`server-error, ${e}`)));
			const responseConsultCredit = await api
				.postConsultCreditData(request.requestConsultCredit)
				.catch(e =>dispatch(SetError(`server-error, ${e}`)));

			const dataAdaptedProduct = CreditAdapter.adaptCreditProductData(
				responseProduct?.data,
			);
			const dataAdaptedLegal = CreditAdapter.adaptCreditLegalData(
				responseLegal?.data,
			);
			const dataAdaptedTechnical = CreditAdapter.adaptCreditTechnicalData(
				responseTechnical?.data,
			);
			const dataAdaptedSimulation = CreditAdapter.adaptCreditSimulation(
				responseSimulation?.data,
			);

			const dataAdaptedConsultCredit = CreditAdapter.adaptConsultCreditData(
				responseConsultCredit?.data,
			);
			dispatch(
				GetCreditFinancialAnalystDataSuccess(
					dataAdaptedProduct,
					dataAdaptedLegal,
					dataAdaptedTechnical,
					dataAdaptedSimulation,
					dataAdaptedConsultCredit,
				),
			);
		};

const PostCreditFlowDocuments = (): AppActions => ({
	type: POST_CREDIT_FLOW_DOCUMENTS,
});

const PostCreditFlowDocumentsSuccess = (
	flowDocumentsData: CreditFlowDocumentModel[],
): AppActions => ({
	type: POST_CREDIT_FLOW_DOCUMENTS_SUCCESS,
	flowDocumentsData,
});

export const startPostCreditFlowDocuments =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditFlowDocuments());
				const response = await api.postCreditFlowDocuments(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptCreditFlowDocumentList(
						response.data,
					);
					dispatch(PostCreditFlowDocumentsSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const getCreditFlowDocumentsSuccess = (
	documentsData: CreditDocumentModel[],
): AppActions => ({
	type: GET_CREDIT_FLOW_DOCUMENTS_SUCCESS,
	documentsData,
});

export const startGetCreditFlowDocuments =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				const response = await api.getCreditFlowDocuments();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(getCreditFlowDocumentsSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostConsultCredit = (): AppActions => ({
	type: POST_CONSULT_CREDIT,
});

const PostConsultCreditSuccess = (
	consultCreditData: CreditConsultCreditModel,
	insurancesListData: any[],
): AppActions => ({
	type: POST_CONSULT_CREDIT_SUCCESS,
	consultCreditData,
	insurancesListData,
});

export const startPostConsultCredit =
	(request: CreditConsultCreditRequestModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostConsultCredit());
				const response = await api.postConsultCreditData(request);
				const response2 = await api.getInsurances();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					const dataAdapted = CreditAdapter.adaptConsultCreditData(
						response.data,
					);
					console.log('DATA ADAPTADA consultCreditData:', dataAdapted);

					dispatch(PostConsultCreditSuccess(dataAdapted, response2.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreateSimulation = (): AppActions => ({
	type: POST_CREATE_SIMULATION,
});
const SetErrorSobrante = (): AppActions => ({
	type: POST_ERROR_SIMULATION_SOBRANTE,
});

const PostCreateSimulationSuccess = (simulationData: any): AppActions => ({
	type: POST_CREATE_SIMULATION_SUCCESS,
	simulationData,
});

export const startPostCreateSimulation =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreateSimulation());
				const response = await api.postCreditCreateClientSimulation(
					request,
				);
				if(response?.data[0]?.sobrante !== null){
					// dispatch(SetErrorSobrante());
					const dataAdapted = CreditAdapter.adaptCreateSimulationListData(
						response.data,
					);
					dispatch(PostCreateSimulationSuccess(dataAdapted));

					await NotificationService.genericAlert(
						'Error en la Simulación',
						`${response.data[0]?.sobrante}`,
						'warning',
					);
				} else if(response?.isSuccess == null){
					dispatch(SetError(response));
					
				} else{
					const dataAdapted = CreditAdapter.adaptCreateSimulationListData(
						response.data,
					);
					await NotificationService.genericAlert(
						'Simulación',
						`Simulación creada`,
						'success',
					);
					dispatch(PostCreateSimulationSuccess(dataAdapted));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditCommittee = (): AppActions => ({
	type: POST_CREDIT_COMMITTEE,
});

const PostCreditCommitteeSuccess = (result: any): AppActions => ({
	type: POST_CREDIT_COMMITTEE_SUCCESS,
	result,
});

export const startPostCreditCommittee =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditCommittee());
				const response = await api.postCreditCommittee(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostCreditCommitteeSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const GetDetailCommittee = (): AppActions => ({
	type: GET_DETAIL_COMMITTEE,
});

const GetDetailCommitteeSuccess = (result: {
	topics: any[];
	members: any[];
	listMembers?: any[];
	listTopics?: any[];
}): AppActions => ({
	type: GET_DETAIL_COMMITTEE_SUCCESS,
	result,
});

export const startGetDetailCommittee =
	(creditCommittee?: CreditCommitteeModel) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetDetailCommittee());
				let responses;
				if (creditCommittee != null) {
					const request = {
						idComite: creditCommittee.codigoComite,
						indicador: 2,
					};
					responses = await Promise.all([
						api.getMembersCommittee(),
						api.getTopicsCommittee(),
						api.postGetMembersCommit(request),
						api.postGetTopicsCommit(request),
					]);
					if (responses[0].isSuccess == null) {
						dispatch(SetError(responses[0]));
					} else if (responses[1].isSuccess == null) {
						dispatch(SetError(responses[1]));
					} else if (responses[2].isSuccess == null) {
						dispatch(SetError(responses[2]));
					} else if (responses[3].isSuccess == null) {
						dispatch(SetError(responses[3]));
					} else {
						dispatch(
							GetDetailCommitteeSuccess({
								members: responses[0].data,
								topics: responses[1].data,
								listMembers: responses[2].data,
								listTopics: responses[3].data,
							}),
						);
					}
				} else {
					responses = await Promise.all([
						api.getMembersCommittee(),
						api.getTopicsCommittee(),
					]);
					if (responses[0].isSuccess == null) {
						dispatch(SetError(responses[0]));
					} else if (responses[1].isSuccess == null) {
						dispatch(SetError(responses[1]));
					} else {
						dispatch(
							GetDetailCommitteeSuccess({
								members: responses[0].data,
								topics: responses[1].data,
							}),
						);
					}
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreateUpdatedCommittee = (): AppActions => ({
	type: POST_CREATE_UPDATE_COMMITTEE,
});

const PostCreateUpdatedCommitteeSuccess = (result: any): AppActions => ({
	type: POST_CREATE_UPDATE_COMMITTEE_SUCCESS,
	result,
});

export const startPostCreateUpdatedCommittee =
	(request: CreateUpdateCommittee) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreateUpdatedCommittee());
				const user = AuthService.getUserInfo();
				const currentDate = moment().format();
				if (request.codigoComite != null) {
					request.actualizadoPor = user.USR_ID;
					request.fechaActualiza = currentDate;
				} else {
					request.creadoPor = user.USR_ID;
					request.fechaCreacion = currentDate;
				}
				const response = await api.postCreateCommittee(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostCreateUpdatedCommitteeSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditCommitteeRequest = (): AppActions => ({
	type: POST_CREDIT_COMMITTEE_REQUEST,
});

const PostCreditCommitteeRequestSuccess = (result: any): AppActions => ({
	type: POST_CREDIT_COMMITTEE_REQUEST_SUCCESS,
	result,
});

export const startPostCreditCommitteeRequestSuccess =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditCommitteeRequest());
				const response = await api.postCreditCommitteeRequest(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostCreditCommitteeRequestSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreditCommitteeApprove = (): AppActions => ({
	type: POST_CREDIT_COMMITTEE_APPROVE,
});

const PostCreditCommitteeApproveSuccess = (result: any): AppActions => ({
	type: POST_CREDIT_COMMITTEE_APPROVE_SUCCESS,
	result,
});

export const startPostCreditCommitteeApprove =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				const user = AuthService.getUserInfo();
				dispatch(PostCreditCommitteeApprove());
				request.fechaActualiza = moment().format();
				request.actualizadoPor = user.USR_ID;
				const response = await api.postCreditCommitteeApprove(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostCreditCommitteeApproveSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

		
const PostCreditCommitteeSoli = (): AppActions => ({
	type: POST_CREDIT_COMMITTEE_SOLI,
});

const PostCreditCommitteeSoliSuccess = (result: any): AppActions => ({
	type: POST_CREDIT_COMMITTEE_SOLI_SUCCESS,
	result,
});

export const startPostCreditCommitteeSoli =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreditCommitteeSoli());
				const response = await api.postCreditCommitteeSoli(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostCreditCommitteeSoliSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostCreateCreditCommitteeRequest = (): AppActions => ({
	type: POST_CREATE_CREDIT_COMMITTEE_REQUEST,
});

const PostCreateCreditCommitteeRequestSuccess = (): AppActions => ({
	type: POST_CREATE_CREDIT_COMMITTEE_REQUEST_SUCCESS
});

export const startPostCreateCreditCommitteeRequest =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				const user = AuthService.getUserInfo();
				dispatch(PostCreateCreditCommitteeRequest());
				request.fechaCreacion = moment().format();
				request.creadoPor = user.USR_ID;
				const response = await api.postCreateCreditCommitteeRequest(
					request,
				);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostCreateCreditCommitteeRequestSuccess());
					await NotificationService.genericAlert(
						'Guardado con éxito ',
						'Se realizó  con éxito el cambio de estado de la solicitud',
					);
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

export const ClearDesembolsoData = (): AppActions => ({
	type: CLEAR_CREATEE_DESEMBOLSOO_REQUEST,
});

const PostDesembolsoData = (): AppActions => ({
	type: POST_CREATEE_DESEMBOLSOO_REQUEST,
});
const PostDesembolsoDataSuccess = (desembolsoData: any): AppActions => ({
	type: POST_CREATEE_DESEMBOLSOO_REQUEST_SUCCESS,
	desembolsoData
});

export const startPostCreateDesembolsoRequestSuccess =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostDesembolsoData());
				const response = await api.postDesembolso(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostDesembolsoDataSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PatchDesembolsoData = (): AppActions => ({
	type: PATCH_DESEMBOLSO_REQUEST,
});
const PatchDesembolsoDataSuccess = (desembolsoAprovado: any): AppActions => ({
	type: PATCH_DESEMBOLSO_REQUEST_SUCCESS,
	desembolsoAprovado
});

export const startPostDesembolsoApprove =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PatchDesembolsoData());
				const response = await api.patchDesembolso(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PatchDesembolsoDataSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostLegalizacionData = (): AppActions => ({
	type: POST_LEGALIZACION_REQUEST,
});
const PostLegalizacionDataSuccess = (legalizacion: any): AppActions => ({
	type: POST_LEGALIZACION_REQUEST_SUCCESS,
	legalizacion
});

export const startConsultaLegalizacion =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostLegalizacionData());
				const response = await api.postLegalizacion(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostLegalizacionDataSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostLegalizacinFirmas = (): AppActions => ({
	type: POST_LEGALIZACION_FIRMAS_REQUEST,
});
const PostLegalizacinFirmasSuccess = (firmas: any): AppActions => ({
	type: POST_LEGALIZACION_FIRMAS_REQUEST_SUCCESS,
	firmas
});

export const startFirmas =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostLegalizacinFirmas());
				const response = await api.postLegalizacionFirmas(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostLegalizacinFirmasSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostVerificacionTerceros = (): AppActions => ({
	type: POST_VERIFICACION_TERCEROS_REQUEST,
});
const PostVerificacionTercerosSuccess = (terceros: any): AppActions => ({
	type: POST_VERIFICACION_TERCEROS_REQUEST_SUCCESS,
	terceros
});

export const startVerificacionTerceros =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostVerificacionTerceros());
				const response = await api.PostVerificacionTerc(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostVerificacionTercerosSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}

		};

const GetDataLegalizacion = (): AppActions => ({
	type: GET_DATA_LEGALIZACION_REQUEST,
});
const GetDataLegalizacionSuccess = (answerLegalizacion: any): AppActions => ({
	type: GET_DATA_LEGALIZACION_REQUEST_SUCCESS,
	answerLegalizacion
});

export const starDataLegalizacion =
	(request: string) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetDataLegalizacion());
				const response = await api.GetLegalizacionData(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetDataLegalizacionSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const GetDesembolsoDesistimiento = (): AppActions => ({
	type: GET_DESEMBOLSO_DESISTIMIENTO_REQUEST,
});

const GetDesembolsoDesistimientoSuccess = (causalDesistimiento: any): AppActions => ({
	type: GET_DESEMBOLSO_DESISTIMIENTO_REQUEST_SUCCESS,
	causalDesistimiento
});

export const startDesembolsoDesestimiento =
	() =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetDesembolsoDesistimiento());
				const response = await api.getCausalDesistimiento();
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetDesembolsoDesistimientoSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const PostDesistirDesembolo = (): AppActions => ({
	type: POST_DESISTIR_DESEMBOLSO_REQUEST,
});

const PostDesistirDesemboloSuccess = (desistirDesembolso: any): AppActions => ({
	type: POST_DESISTIR_DESEMBOLSO_REQUEST_SUCCESS,
	desistirDesembolso
});

export const starPostDesistirDesembolso =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostDesistirDesembolo());
				const response = await api.PostDesistminetoDesembolso(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostDesistirDesemboloSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

const GetObtenerValorDesembolsoCredito = (): AppActions => ({
	type: GET_OBTENER_VALOR_DESEMBOLSO_REQUEST,
});

const GetObtenerValorDesembolsoCreditoSuccess = (valorDesembolsoCreditoDto: any): AppActions => ({
	type: GET_OBTENER_VALOR_DESEMBOLSO_REQUEST_SUCCESS,
	valorDesembolsoCreditoDto
});

export const startObtenerValorDesembolsoCredito =
	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(GetObtenerValorDesembolsoCredito());
				const response = await api.getObtenerValorDesembolsoCredito(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(GetObtenerValorDesembolsoCreditoSuccess(response.data));
				}
			} catch (e) {
				
				dispatch(SetError('server-error'));
			}
		};

		
const PostObtenerVendedorSuccess = (formularioVendedor: any): AppActions => ({
	type: POST_OBTENER_VENDEDOR_REQUEST_SUCCESS,
	formularioVendedor
});

const PostObtenerVendedor = (): AppActions => ({
    type: POST_OBTENER_VENDEDOR_REQUEST,
});

export const startObtenerVendedorCredito =

	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostObtenerVendedor());
				const response = await api.postObtenerVendedor(request);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
					dispatch(PostObtenerVendedorSuccess(response.data));
				}
			} catch (e) {
				console.error(e);
				dispatch(SetError('server-error'));
			}
		};

const PostCrearVendedorSuccess = (dataVendedor: CreditVendedorModel): AppActions => ({
	type: POST_CREAR_VENDEDOR_REQUEST_SUCCESS,
	dataVendedor
});

const PostCrearVendedor = (): AppActions => ({
    type: POST_CREAR_VENDEDOR_REQUEST,
});

export const startCrearVendedorCredito =(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCrearVendedor());
				const response = await api.postCrearVendedor(request);
				if (response?.isSuccess === null || response?.isSuccess === false) {
					
					dispatch(SetError(response.returnMessage));
                    toast('Error: Cuentas Duplicadas', { type: toast.TYPE.ERROR })
				} else {
                    toast('Vendedor Creado Exitoso', { type: toast.TYPE.SUCCESS })
					dispatch(PostCrearVendedorSuccess(response.data));
				}
			} catch (e) {
				console.error(e);
                toast('Error del sistema', { type: toast.TYPE.ERROR })
				dispatch(SetError('server-error'));
			}
		};

const PostEliminarVendedorSuccess = (Id: any): AppActions => ({
	type: POST_ELIMINAR_VENDEDOR_REQUEST_SUCCESS,
	idVendedor: Id,
});

const PostEliminarVendedor = (): AppActions => ({
    type: POST_CREAR_VENDEDOR_REQUEST,
});

export const startEliminarVendedorCredito =

	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostEliminarVendedor());
				const response = await api.deleteEliminarVendedor(request.id);
				if (response?.isSuccess == null) {
					dispatch(SetError(response));
				} else {
                    toast('Vendedor eliminado con éxito', { type: toast.TYPE.SUCCESS })
					dispatch(PostEliminarVendedorSuccess(request.id));
				}
			} catch (e) {
                toast('Error del sistema', { type: toast.TYPE.ERROR })
				console.error(e);
				dispatch(SetError('server-error'));
			}
		};


const PostObtenerComprobantesCreditoSuccess = (response: any): AppActions => ({
	type: POST_OBTENER_COMPROBANTE_CREDITO_SUCCESS,
	comprobanteCredito: response,
});

const PostObtenerComprobantesCredito = (): AppActions => ({
	type: POST_OBTENER_COMPROBANTE_CREDITO,
});

export const startObtenerComprobantesCredito =

	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostObtenerComprobantesCredito());
				const response = await api.postObtenerComprobantesCredito(request);
				if (response?.isSuccess === null || response?.isSuccess === false) {
					toast('Error del sistema', { type: toast.TYPE.ERROR })
					dispatch(SetError(response.returnMessage));
				} else if (response.isSuccess === true && response.data.length === 0 ) {
					toast('Consulta sin datos disponibles', { type: toast.TYPE.WARNING })
					dispatch(PostObtenerComprobantesCreditoSuccess(response.data));
				} else {
                    toast('Consulta de comprobantes exitosa', { type: toast.TYPE.SUCCESS })
					dispatch(PostObtenerComprobantesCreditoSuccess(response.data));
				}
			} catch (e) {
                toast('Error del sistema', { type: toast.TYPE.ERROR })
				console.error(e);
				dispatch(SetError('server-error'));
			}
		};


const PostGenerarBalanceCarteraSuccess = (response: any): AppActions => ({
	type: POST_GENERAR_BALANCE_SUCCESS,
	balanceCartera: response,
});

const PostGenerarBalanceCartera = (): AppActions => ({
	type: POST_GENERAR_BALANCE,
});

const PostLimpiarStateBalanceCartera = (): AppActions => ({
    type: POST_LIMPIAR_BALANCE, 
});

export const startLimpiarStateBalanceCartera =
    () =>
    async (dispatch: Dispatch<AppActions>): Promise<void> => {
        dispatch(PostLimpiarStateBalanceCartera());
    };


export const startGenerarBalanceCartera =

	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostGenerarBalanceCartera());
				const response = await apiCartera.PostGenerarBalanceCartera(request);
				if (response?.isSuccess === null || response?.isSuccess === false) {
					toast('Error del sistema', { type: toast.TYPE.ERROR })
					dispatch(SetError(response.returnMessage));
				} else if (response.isSuccess === true && response.data.length === 0 ) {
					toast('Generacion de balance sin datos disponibles', { type: toast.TYPE.WARNING })
					dispatch(PostGenerarBalanceCarteraSuccess(response.data));
				} else {
					toast('Balance Generado con éxito', { type: toast.TYPE.SUCCESS })
					dispatch(PostGenerarBalanceCarteraSuccess(response.data));
				}
			} catch (e) {
				toast('Error del sistema', { type: toast.TYPE.ERROR })
				console.error(e);
				dispatch(SetError('server-error'));
			}
		};
	
		const PostDescargarBalanceCarteraSuccessAux = (): AppActions => ({
			type: POST_GENERAR_AUX_BALANCE_SUCCESS,
		});
		
		const PostDescargarBalanceCarteraAux = (): AppActions => ({
			type: POST_GENERAR_AUX_BALANCE,
		});
		
		export const startDescargarBalanceCarteraAux = (request: any) =>
			async (dispatch: Dispatch<AppActions>): Promise<void> => {
				try {
					dispatch(PostDescargarBalanceCarteraAux());
		

					const response = await apiCartera.postGenerarAuxiliarContableCartera(request);

					if (response) {
						
						dispatch(PostDescargarBalanceCarteraSuccessAux());
		
						const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
						const url = window.URL.createObjectURL(blob);
						const link = document.createElement('a');
						link.href = url;
						link.download = `Balance_Aux_.xlsx`;
						link.download = `Balance_Aux_${request.fechaCorte}.xlsx`;
						document.body.appendChild(link);
						link.click();
						
						window.URL.revokeObjectURL(url);
						document.body.removeChild(link);
		
						toast('Auxiliar de Balance descargado con éxito', { type: toast.TYPE.SUCCESS });
					} else {
						throw new Error('No se recibió un archivo válido.');
						dispatch(SetError('server-error'));

					}
				} catch (error: any) {
					// Aquí también aplicamos tu idea para capturar errores de tipo blob
					if (error.response && error.response.data) {
					  const completeResponse = error.response;
					  const data = JSON.parse(await completeResponse.data.text());
			  
					  toast(`Error: ${data.returnMessage}`, {
						type: toast.TYPE.ERROR,
					  });
			  
					  dispatch(SetError('server-error'));
					} else {
					  // Error general si no viene respuesta del servidor
					  const errorMessage = error.message || 'Error al descargar el Auxiliar';
					  toast(errorMessage, { type: toast.TYPE.ERROR });
					  dispatch(SetError('server-error'));
					}
				  }
				
			};
		

const PostCreaSCnMcontAprobacionCreditoSuccess = (response: any): AppActions => ({
	type: POST_CREA_APROVACION_DESEMBOLSO_SUCCESS,
	result: response,
});

const PostCreaSCnMcontAprobacionCredito = (): AppActions => ({
	type: POST_CREA_APROVACION_DESEMBOLSO,
});

export const startPostCreaSCnMcontAprobacionCredito =

	(request: any) =>
		async (dispatch: Dispatch<AppActions>): Promise<void> => {
			try {
				dispatch(PostCreaSCnMcontAprobacionCredito());
				const response = await api.postCreaSCnMcontAprobacionCredito(request);
				if (response?.isSuccess === null || response?.isSuccess === false) {
					toast('Error del sistema', { type: toast.TYPE.ERROR })
					dispatch(SetError(response.returnMessage));
				} else {
					const retornoExitoso = response.returnMessage.match(/<RETORNO>(.*?)<\/RETORNO>/);
					if(retornoExitoso[1] === "0"){
						toast('Comprobante generado exitosamente',{ type: toast.TYPE.SUCCESS });
						dispatch(PostCreaSCnMcontAprobacionCreditoSuccess(response))

					}else{
						const retornoError = response.returnMessage.match(/<TXTERROR>(.*?)<\/TXTERROR>/);
						toast( `Error: ${retornoError[1]}`, { type: toast.TYPE.ERROR });
						dispatch(PostCreaSCnMcontAprobacionCreditoSuccess(response))
					}
				}
			} catch (e) {
                toast('Error del sistema', { type: toast.TYPE.ERROR })
				dispatch(SetError('server-error'));
			}
		};

