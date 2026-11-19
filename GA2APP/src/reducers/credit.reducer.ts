import { ProcedureRequestModel } from '../models/procedure-request.model';
import {
    CLEAR_CREATEE_DESEMBOLSOO_REQUEST,
    CLEAR_DETAIL_REQUEST_INFORMATION,
    CreditActionsType,
    GET_CREDIT_FINANCIAL_ANALYST_DATA,
    GET_CREDIT_FINANCIAL_ANALYST_DATA_SUCCESS,
    GET_DETAIL_COMMITTEE,
    GET_DETAIL_COMMITTEE_SUCCESS,
    ON_ERROR,
    POST_CONSULT_CREDIT,
    POST_CONSULT_CREDIT_SUCCESS,
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
    GET_CREDIT_FLOW_DOCUMENTS_SUCCESS,
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
    GET_PRUEBA_DATA_SUCCESS,
    GET_PRUEBA_DATA,
    POST_LEGALIZACION_REQUEST,
    POST_LEGALIZACION_REQUEST_SUCCESS,
    POST_LEGALIZACION_FIRMAS_REQUEST,
    POST_LEGALIZACION_FIRMAS_REQUEST_SUCCESS,
    POST_VERIFICACION_TERCEROS_REQUEST_SUCCESS,
    POST_VERIFICACION_TERCEROS_REQUEST,
    GET_DATA_LEGALIZACION_REQUEST_SUCCESS,
    GET_DATA_LEGALIZACION_REQUEST,
    PATCH_DESEMBOLSO_REQUEST,
    PATCH_DESEMBOLSO_REQUEST_SUCCESS,
    GET_DESEMBOLSO_DESISTIMIENTO_REQUEST_SUCCESS,
    GET_DESEMBOLSO_DESISTIMIENTO_REQUEST,
    POST_DESISTIR_DESEMBOLSO_REQUEST,
    POST_DESISTIR_DESEMBOLSO_REQUEST_SUCCESS,
    GET_OBTENER_VALOR_DESEMBOLSO_REQUEST_SUCCESS,
    POST_OBTENER_VENDEDOR_REQUEST_SUCCESS,
    POST_OBTENER_VENDEDOR_REQUEST_FAILURE,
	POST_OBTENER_VENDEDOR_REQUEST,
    POST_CREAR_VENDEDOR_REQUEST_SUCCESS,
    POST_CREAR_VENDEDOR_REQUEST_FAILURE,
    POST_CREAR_VENDEDOR_REQUEST,
    POST_ELIMINAR_VENDEDOR_REQUEST_SUCCESS,
    POST_ELIMINAR_VENDEDOR_REQUEST_FAILURE,
    POST_ELIMINAR_VENDEDOR_REQUEST,
    POST_OBTENER_COMPROBANTE_CREDITO_SUCCESS,
    POST_OBTENER_COMPROBANTE_CREDITO,
    POST_CREDIT_COMMITTEE_SOLI_SUCCESS,
    POST_CREDIT_COMMITTEE_SOLI,
    POST_CREA_APROVACION_DESEMBOLSO_SUCCESS,
    POST_CREA_APROVACION_DESEMBOLSO,
    POST_GENERAR_BALANCE,
    POST_GENERAR_BALANCE_SUCCESS,
    POST_LIMPIAR_BALANCE,
    POST_GENERAR_AUX_BALANCE,
    POST_GENERAR_AUX_BALANCE_SUCCESS,
    
    
} from '../types/credit.actions.type';
import { RequestActivityRequestModel } from '../models/requests/request-activity-request.model';
import { CreditBasicDataModel } from '../models/credit-basic-data.model';
import CreditAdapter from '../adapters/credit.adapter';
import { CreditProductModel } from '../models/credit-product.model';
import { CreditObservationModel } from '../models/credit-observation.model';
import { CreditFileModel } from '../models/credit-file.model';
import { CreditFinancialDataModel } from '../models/credit-financial-data.model';
import { CreditTechnicalDataModel } from '../models/credit-technical-data.model';
import { CreditLegalDataModel } from '../models/credit-legal-data.model';
import { CreditFlowDocumentModel } from '../models/credit-flow-document.model';
import { CreditSimulationDataModel } from '../models/credit-simulation-data.model';
import { CreditConsultCreditModel } from '../models/credit-consult-credit.model';
import { CreditCreateSimulationDataModel } from '../models/credit-create-simulation-data.model';
import { CreditCommitteeModel } from '../models/credit-committee.model';
import { CreditDocumentModel } from '../models/credit-dpcument.model';

export interface CreditState {
    loading: boolean;
    error: any;
    procedureRequests: ProcedureRequestModel[];
    responseRequestActivity?: RequestActivityRequestModel;
    creditBasicData: CreditBasicDataModel;
    creditProductData: CreditProductModel;
    creditObservationsData: CreditObservationModel[];
    creditFilesData: CreditFileModel[];
    creditFinancialData: CreditFinancialDataModel;
    creditTechnicalData: CreditTechnicalDataModel;
    creditLegalData: CreditLegalDataModel;
    createIsSuccessfully: boolean;
    creditSimulationData: CreditSimulationDataModel;
    creditFlowDocumentsData: CreditFlowDocumentModel[];
    creditConsultCreditData: CreditConsultCreditModel;
    creditSimulationResultData: CreditCreateSimulationDataModel[];
    desembolsoData: any[];
    creditCommitteeList: any;
    listTopics: any[];
    listMembers: any[];
    currentListTopics: any[];
    currentListMembers: any[];
    redirectTo: string | null;
    creditCommitteeRequestList: any[];
    approveCreditCommitteeSuccessfully?: boolean;
    createdCommittee: any;
    insurancesList: any[];
    documentsData: CreditDocumentModel[];
    prueba:any;
    legalizacion: any;
    PDF: any;
    firmas: any;
    terceros: any;
    answerLegalizacion: any;
    desembolsoAprovado: any;
    causalDesistimiento: any;
    desistirDesembolso: any;
    valorDesembolsoCreditoDto: any;
    formularioVendedor: any;
    comprobanteCredito: any;
    creditCommitteeSoli: any;
    balanceCartera: any;

}

const DetailInitialState = {
    creditBasicData: CreditAdapter.adaptCreditBasicData({}),
    creditProductData: CreditAdapter.adaptCreditProductData({}),
    creditObservationsData: [],
    creditFilesData: [],
    creditFinancialData: CreditAdapter.adaptFinancialData({}),
    creditTechnicalData: CreditAdapter.adaptCreditTechnicalData({}),
    creditLegalData: CreditAdapter.adaptCreditLegalData({}),
    createIsSuccessfully: false,
    creditSimulationData: CreditAdapter.adaptCreditSimulation({}),
    creditFlowDocumentsData: CreditAdapter.adaptCreditFlowDocumentList([]),
    creditConsultCreditData: CreditAdapter.adaptConsultCreditData({}),
    creditSimulationResultData: [],
    desembolsoData: [],
    error: null,
    responseRequestActivity: undefined,
    approveCreditCommitteeSuccessfully: undefined,
    listTopics: [],
    listMembers: [],
    currentListTopics: [],
    currentListMembers: [],
    redirectTo: null,
    creditCommitteeRequestList: [],
    createdCommittee: null,
    insurancesList: [],
    documentsData: [],
    prueba: [],
    legalizacion: [],
    PDF:[],
    firmas:[],
    terceros:[],
    answerLegalizacion:[],
    desembolsoAprovado:[],
    causalDesistimiento:[],
    desistirDesembolso:[],
    valorDesembolsoCreditoDto:undefined,
    formularioVendedor: [],
    comprobanteCredito: [],
    creditCommitteeSoli: [],
    balanceCartera: [],

};

const InitialState: CreditState = {
    loading: false,
    procedureRequests: [],
    creditCommitteeList: [],
    ...DetailInitialState,
};

const CreditReducer = (
    state: CreditState = InitialState,
    action: CreditActionsType,
): CreditState => {
    switch (action.type) {
        case POST_CREDIT_OBSERVATIONS_DATA:
        case POST_CREDIT_FILES_DATA:
        case POST_CREDIT_PRODUCT_DATA:
        case POST_CREDIT_BASIC_DATA:
        case PUT_REQUEST_ACTIVITY_PROCESS:
        case POST_PROCEDURE_REQUEST:
        case POST_CREDIT_FINANCIAL_DATA:
        case POST_CREDIT_CREATE_TECHNICAL_DATA:
        case POST_CREDIT_CREATE_LEGAL_DATA:
        case POST_CREDIT_TECHNICAL_DATA:
        case POST_CREDIT_LEGAL_DATA:
        case GET_CREDIT_FINANCIAL_ANALYST_DATA:
        case POST_CREDIT_FLOW_DOCUMENTS:
        case POST_CONSULT_CREDIT:
        case POST_CREATE_SIMULATION:
        case POST_CREDIT_COMMITTEE:
        case POST_CREDIT_COMMITTEE_SOLI:
        case GET_DETAIL_COMMITTEE:
        case POST_CREATE_UPDATE_COMMITTEE:
        case POST_CREDIT_COMMITTEE_REQUEST:
        case POST_CREDIT_COMMITTEE_APPROVE:
        case POST_CREATE_CREDIT_COMMITTEE_REQUEST:
        case POST_CREATEE_DESEMBOLSOO_REQUEST:
        case GET_PRUEBA_DATA:
        case POST_LEGALIZACION_REQUEST:
        case POST_LEGALIZACION_FIRMAS_REQUEST:
        case POST_VERIFICACION_TERCEROS_REQUEST:
        case GET_DATA_LEGALIZACION_REQUEST:
        case PATCH_DESEMBOLSO_REQUEST:
        case GET_DESEMBOLSO_DESISTIMIENTO_REQUEST:
        case POST_DESISTIR_DESEMBOLSO_REQUEST:
        case POST_CREA_APROVACION_DESEMBOLSO:
            


            return {
                ...state,
                loading: true,
                error: null,
                approveCreditCommitteeSuccessfully: undefined,
                createIsSuccessfully: false,
            };
        case ON_ERROR:
            return {
                ...state,
                loading: false,
                error: action.error,
            };
        case RESET_CREDIT_STATE:
            return InitialState;
        case CLEAR_DETAIL_REQUEST_INFORMATION:
            return {
                ...state,
                ...DetailInitialState,
            };
        case POST_PROCEDURE_REQUEST_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                procedureRequests: action.procedureRequests,
            };
        case PUT_REQUEST_ACTIVITY_PROCESS_SUCCESS:
            return {
                ...state,
                loading: false,
                responseRequestActivity: action.result,
                error: null,
            };
        case POST_CREDIT_BASIC_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditBasicData: action.creditBasicData,
            };
        case POST_CREDIT_PRODUCT_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditProductData: action.creditProductData,
            };
        case POST_CREDIT_OBSERVATIONS_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditObservationsData: action.observationsData,
            };
        case POST_CREDIT_FILES_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditFilesData: action.filesData,
            };
        case POST_CREDIT_FINANCIAL_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditFinancialData: action.financialData,
            };
        case POST_CREDIT_CREATE_TECHNICAL_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                createIsSuccessfully: true,
                creditTechnicalData: action.creditTechnicalData,
            };
        case POST_CREDIT_TECHNICAL_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditTechnicalData: action.creditTechnicalData,
            };
        case POST_CREDIT_LEGAL_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditLegalData: action.creditLegalData,
            };
        case POST_CREDIT_CREATE_LEGAL_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                createIsSuccessfully: true,
                creditLegalData: action.creditLegalData,
            };
        case GET_CREDIT_FINANCIAL_ANALYST_DATA_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditProductData: action.creditProductData,
                creditLegalData: action.creditLegalData,
                creditTechnicalData: action.creditTechnicalData,
                creditSimulationData: action.creditSimulationData,
                creditConsultCreditData: action.creditConsultCreditData,
            };
        case POST_CREDIT_FLOW_DOCUMENTS_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditFlowDocumentsData: action.flowDocumentsData,
            };
        case GET_CREDIT_FLOW_DOCUMENTS_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                documentsData: action.documentsData,
            };
        case POST_CONSULT_CREDIT_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditConsultCreditData: action.consultCreditData,
                insurancesList: action.insurancesListData,
            };
        case POST_CREATE_SIMULATION_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditSimulationResultData: action.simulationData,
            };
        case POST_ERROR_SIMULATION_SOBRANTE:
            return {
                ...state,
                loading: false,
                error: null,
           };
        case POST_CREDIT_COMMITTEE_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditCommitteeList: action.result,
            };
        case POST_CREDIT_COMMITTEE_SOLI_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                creditCommitteeSoli: action.result,
            };
        case POST_CREA_APROVACION_DESEMBOLSO_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                approveCreditCommitteeSuccessfully: false,
            };
        case GET_DETAIL_COMMITTEE_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                listMembers: action.result.members,
                listTopics: action.result.topics,
                currentListMembers:
                    action.result.listMembers != null
                        ? action.result.listMembers
                        : [],
                currentListTopics:
                    action.result.listTopics != null
                        ? action.result.listTopics
                        : [],
            };
        case POST_CREATE_UPDATE_COMMITTEE_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                redirectTo: 'request',
                createdCommittee: action.result,
            };
        case POST_CREDIT_COMMITTEE_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                creditCommitteeRequestList: action.result,
            };
        case POST_CREDIT_COMMITTEE_APPROVE_SUCCESS:
            const creditCommitteIndex = state.creditCommitteeList.findIndex(
                (item: CreditCommitteeModel) =>
                    item.codigoComite === action.result.codigoComite,
            );
            const { creditCommitteeList } = state;
            creditCommitteeList[creditCommitteIndex].codigoEstado = 'A';
            creditCommitteeList[creditCommitteIndex].estado = 'Aprobado';
            return {
                ...state,
                error: null,
                creditCommitteeList,
                loading: false,
                approveCreditCommitteeSuccessfully: true,
            };
        case POST_CREATE_CREDIT_COMMITTEE_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
            };
        case POST_CREATEE_DESEMBOLSOO_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                desembolsoData: action.desembolsoData
            };
        case CLEAR_CREATEE_DESEMBOLSOO_REQUEST:
            return {
                ...state,
                ...DetailInitialState,
                desembolsoData: []
            };

        case POST_LEGALIZACION_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                legalizacion: action.legalizacion
            }

        case GET_PRUEBA_DATA_SUCCESS:
                return {
                    ...state,
                    error: null,
                    loading: false,
                    prueba: action.prueba
                };

        case POST_LEGALIZACION_FIRMAS_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                firmas: action.firmas
            };


        case POST_VERIFICACION_TERCEROS_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                terceros: action.terceros
            };

        case GET_DATA_LEGALIZACION_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: true,
                answerLegalizacion: action.answerLegalizacion
            };

        case PATCH_DESEMBOLSO_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                desembolsoAprovado: action.desembolsoAprovado
            };

        case GET_DESEMBOLSO_DESISTIMIENTO_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                causalDesistimiento: action.causalDesistimiento
            };

        case POST_DESISTIR_DESEMBOLSO_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                desistirDesembolso: action.desistirDesembolso
            };

        case GET_OBTENER_VALOR_DESEMBOLSO_REQUEST_SUCCESS:
            return {
                ...state,
                error: null,
                loading: false,
                valorDesembolsoCreditoDto: action.valorDesembolsoCreditoDto
            };
        
        case POST_OBTENER_VENDEDOR_REQUEST:
            return {
                ...state,
                loading: true,
                error: null   
            }; 

        case POST_OBTENER_VENDEDOR_REQUEST_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                formularioVendedor: action.formularioVendedor
                
            };

            case POST_OBTENER_VENDEDOR_REQUEST_FAILURE:
            return {
                ...state,
                error: action.error,
                loading: false,
            };

        case POST_CREAR_VENDEDOR_REQUEST:
            return {
                ...state,
                loading: true,
                error: null   
            }; 

        case POST_CREAR_VENDEDOR_REQUEST_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                formularioVendedor: [...state.formularioVendedor.map((item : any) =>({...item})), action.dataVendedor]
                
            };

        case POST_ELIMINAR_VENDEDOR_REQUEST:
            return {
                ...state,
                loading: true,
                error: null   
            }; 

        case POST_ELIMINAR_VENDEDOR_REQUEST_SUCCESS:
                return {
                    ...state,
                    loading: false,
                    error: null,
                    formularioVendedor: state.formularioVendedor.filter((c:any) => c.id !== action.idVendedor),
                   
                };
        case POST_OBTENER_COMPROBANTE_CREDITO:
            return {
                ...state,
                loading: true,
                error: null   
            }; 

        case POST_OBTENER_COMPROBANTE_CREDITO_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                comprobanteCredito: action.comprobanteCredito
            };

        case POST_GENERAR_BALANCE:
            return {
                ...state,
                loading: true,
                error: null,   
                balanceCartera: []

            }; 
        case POST_LIMPIAR_BALANCE:
            return {
                ...state,
                error: null,   
                balanceCartera: []

            }; 

        case POST_GENERAR_BALANCE_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
                balanceCartera: action.balanceCartera
            };
    
        case POST_GENERAR_AUX_BALANCE:
            return {
                ...state,
                loading: true,
                error: null   
            }; 

        case POST_GENERAR_AUX_BALANCE_SUCCESS:
            return {
                ...state,
                loading: false,
                error: null,
            };

        default:
            return state;
    }
};

export default CreditReducer;
