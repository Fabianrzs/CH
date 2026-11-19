import { ProcedureRequestModel } from '../models/procedure-request.model';
import { RequestActivityRequestModel } from '../models/requests/request-activity-request.model';
import { CreditBasicDataModel } from '../models/credit-basic-data.model';
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
import { CreditDocumentModel } from '../models/credit-dpcument.model';
import { Prueba } from '../models/prueba-boton-model';
import { CreditVendedorModel } from '../models/credit-vendedor.model';



export const RESET_CREDIT_STATE = 'RESET_CREDIT_STATE';
export const CLEAR_DETAIL_REQUEST_INFORMATION =  'CLEAR_DETAIL_REQUEST_INFORMATION';
export const ON_ERROR = 'ON_ERROR';

export const POST_PROCEDURE_REQUEST = 'POST_PROCEDURE_REQUEST';
export const POST_PROCEDURE_REQUEST_SUCCESS = 'POST_PROCEDURE_REQUEST_SUCCESS';

export const PUT_REQUEST_ACTIVITY_PROCESS = 'PUT_REQUEST_ACTIVITY_PROCESS';
export const PUT_REQUEST_ACTIVITY_PROCESS_SUCCESS = 'PUT_REQUEST_ACTIVITY_PROCESS_SUCCESS';

export const POST_CREDIT_BASIC_DATA = 'POST_CREDIT_BASIC_DATA';
export const POST_CREDIT_BASIC_DATA_SUCCESS = 'POST_CREDIT_BASIC_DATA_SUCCESS';

export const POST_CREDIT_PRODUCT_DATA = 'POST_CREDIT_PRODUCT_DATA';
export const POST_CREDIT_PRODUCT_DATA_SUCCESS = 'POST_CREDIT_PRODUCT_DATA_SUCCESS';

export const POST_CREDIT_OBSERVATIONS_DATA = 'POST_CREDIT_OBSERVATIONS_DATA';
export const POST_CREDIT_OBSERVATIONS_DATA_SUCCESS = 'POST_CREDIT_OBSERVATIONS_DATA_SUCCESS';

export const POST_CREDIT_FILES_DATA = 'POST_CREDIT_FILES_DATA';
export const POST_CREDIT_FILES_DATA_SUCCESS = 'POST_CREDIT_FILES_DATA_SUCCESS';


/// esto hay que borrar porque es una prueba

export const GET_PRUEBA_DATA = 'GET_PRUEBA_DATA' ;
export const GET_PRUEBA_DATA_SUCCESS = 'GET_PRUEBA_DATA_SUCCESS' ;



export const POST_CREDIT_FINANCIAL_DATA = 'POST_CREDIT_FINANCIAL_DATA';
export const POST_CREDIT_FINANCIAL_DATA_SUCCESS = 'POST_CREDIT_FINANCIAL_DATA_SUCCESS';

export const POST_CREDIT_CREATE_TECHNICAL_DATA = 'POST_CREDIT_CREATE_TECHNICAL_DATA';
export const POST_CREDIT_CREATE_TECHNICAL_DATA_SUCCESS = 'POST_CREDIT_CREATE_TECHNICAL_DATA_SUCCESS';

export const POST_CREDIT_TECHNICAL_DATA = 'POST_CREDIT_TECHNICAL_DATA';
export const POST_CREDIT_TECHNICAL_DATA_SUCCESS = 'POST_CREDIT_TECHNICAL_DATA_SUCCESS';

export const POST_CREDIT_LEGAL_DATA = 'POST_CREDIT_LEGAL_DATA';
export const POST_CREDIT_LEGAL_DATA_SUCCESS = 'POST_CREDIT_LEGAL_DATA_SUCCESS';

export const POST_CREDIT_CREATE_LEGAL_DATA = 'POST_CREDIT_CREATE_LEGAL_DATA';
export const POST_CREDIT_CREATE_LEGAL_DATA_SUCCESS = 'POST_CREDIT_CREATE_LEGAL_DATA_SUCCESS';

export const GET_CREDIT_FINANCIAL_ANALYST_DATA = 'GET_CREDIT_FINANCIAL_ANALYST_DATA';
export const GET_CREDIT_FINANCIAL_ANALYST_DATA_SUCCESS = 'GET_CREDIT_FINANCIAL_ANALYST_DATA_SUCCESS';

export const POST_CREDIT_FLOW_DOCUMENTS = 'POST_CREDIT_FLOW_DOCUMENTS';
export const POST_CREDIT_FLOW_DOCUMENTS_SUCCESS = 'POST_CREDIT_FLOW_DOCUMENTS_SUCCESS';

export const GET_CREDIT_FLOW_DOCUMENTS_SUCCESS = 'GET_CREDIT_FLOW_DOCUMENTS_SUCCESS';


export const POST_CONSULT_CREDIT = 'POST_CONSULT_CREDIT';
export const POST_CONSULT_CREDIT_SUCCESS = 'POST_CONSULT_CREDIT_SUCCESS';

export const POST_CREATE_SIMULATION = 'POST_CREATE_SIMULATION';
export const POST_ERROR_SIMULATION_SOBRANTE = 'POST_ERROR_SIMULATION_SOBRANTE';
export const POST_CREATE_SIMULATION_SUCCESS = 'POST_CREATE_SIMULATION_SUCCESS';

export const POST_CREDIT_COMMITTEE = 'POST_CREDIT_COMMITTEE';
export const POST_CREDIT_COMMITTEE_SUCCESS = 'POST_CREDIT_COMMITTEE_SUCCESS';

export const GET_DETAIL_COMMITTEE = 'GET_DETAIL_COMMITTEE';
export const GET_DETAIL_COMMITTEE_SUCCESS = 'GET_DETAIL_COMMITTEE_SUCCESS';

export const POST_CREATE_UPDATE_COMMITTEE = 'POST_CREATE_UPDATE_COMMITTEE';
export const POST_CREATE_UPDATE_COMMITTEE_SUCCESS = 'POST_CREATE_UPDATE_COMMITTEE_SUCCESS';

export const POST_CREDIT_COMMITTEE_REQUEST = 'POST_CREDIT_COMMITTEE_REQUEST';
export const POST_CREDIT_COMMITTEE_REQUEST_SUCCESS = 'POST_CREDIT_COMMITTEE_REQUEST_SUCCESS';

export const POST_CREDIT_COMMITTEE_APPROVE = 'POST_CREDIT_COMMITTEE_APPROVE';
export const POST_CREDIT_COMMITTEE_APPROVE_SUCCESS = 'POST_CREDIT_COMMITTEE_APPROVE_SUCCESS';

export const POST_CREDIT_COMMITTEE_SOLI = 'POST_CREDIT_COMMITTEE_SOLI';
export const POST_CREDIT_COMMITTEE_SOLI_SUCCESS = 'POST_CREDIT_COMMITTEE_SOLI_SUCCESS';

export const POST_CREA_APROVACION_DESEMBOLSO = 'POST_CREA_APROVACION_DESEMBOLSO';
export const POST_CREA_APROVACION_DESEMBOLSO_SUCCESS = 'POST_CREA_APROVACION_DESEMBOLSO_SUCCESS';

export const POST_CREATE_CREDIT_COMMITTEE_REQUEST = 'POST_CREATE_CREDIT_COMMITTEE_REQUEST';
export const POST_CREATE_CREDIT_COMMITTEE_REQUEST_SUCCESS = 'POST_CREATE_CREDIT_COMMITTEE_REQUEST_SUCCESS';

// LEGALIZACION

export const POST_LEGALIZACION_REQUEST=  'POST_LEGALIZACION_REQUEST';
export const POST_LEGALIZACION_REQUEST_SUCCESS = 'POST_LEGALIZACION_REQUEST_SUCCESS';

export const  POST_LEGALIZACION_FIRMAS_REQUEST=  'POST_LEGALIZACION_FIRMAS_REQUEST';
export const  POST_LEGALIZACION_FIRMAS_REQUEST_SUCCESS = 'POST_LEGALIZACION_FIRMAS_REQUEST_SUCCESS';

export const  POST_VERIFICACION_TERCEROS_REQUEST=  'POST_VERIFICACION_TERCEROS_REQUEST';
export const  POST_VERIFICACION_TERCEROS_REQUEST_SUCCESS = 'POST_VERIFICACION_TERCEROS_REQUEST_SUCCESS';

export const  GET_DATA_LEGALIZACION_REQUEST=  'GET_DATA_LEGALIZACION_REQUEST';
export const  GET_DATA_LEGALIZACION_REQUEST_SUCCESS = 'GET_DATA_LEGALIZACION_REQUEST_SUCCESS';

export const POST_CREATEE_DESEMBOLSOO_REQUEST ='POST_CREATEE_DESEMBOLSOO_REQUEST';
export const POST_CREATEE_DESEMBOLSOO_REQUEST_SUCCESS ='POST_CREATEE_DESEMBOLSOO_REQUEST_SUCCESS';
export const CLEAR_CREATEE_DESEMBOLSOO_REQUEST ='CLEAR_CREATEE_DESEMBOLSOO_REQUEST';

export const PATCH_DESEMBOLSO_REQUEST ='PATCH_DESEMBOLSO_REQUEST';
export const PATCH_DESEMBOLSO_REQUEST_SUCCESS ='PATCH_DESEMBOLSO_REQUEST_SUCCESS';

export const GET_DESEMBOLSO_DESISTIMIENTO_REQUEST = 'GET_DESEMBOLSO_DESISTIMIENTO_REQUEST';
export const GET_DESEMBOLSO_DESISTIMIENTO_REQUEST_SUCCESS ='GET_DESEMBOLSO_DESISTIMIENTO_REQUEST_SUCCESS';

export const POST_DESISTIR_DESEMBOLSO_REQUEST = 'POST_DESISTIR_DESEMBOLSO_REQUEST';
export const POST_DESISTIR_DESEMBOLSO_REQUEST_SUCCESS ='POST_DESISTIR_DESEMBOLSO_REQUEST_SUCCESS';

export const GET_OBTENER_VALOR_DESEMBOLSO_REQUEST = 'GET_OBTENER_VALOR_DESEMBOLSO_REQUEST';
export const GET_OBTENER_VALOR_DESEMBOLSO_REQUEST_SUCCESS ='GET_OBTENER_VALOR_DESEMBOLSO_REQUEST_SUCCESS';

export const POST_OBTENER_VENDEDOR_REQUEST_SUCCESS= 'POST_OBTENER_VENDEDOR_REQUEST_SUCCESS';
export const POST_OBTENER_VENDEDOR_REQUEST_FAILURE= 'POST_OBTENER_VENDEDOR_REQUEST_FAILURE';
export const POST_OBTENER_VENDEDOR_REQUEST= 'POST_OBTENER_VENDEDOR_REQUEST';

export const POST_CREAR_VENDEDOR_REQUEST_SUCCESS= 'POST_CREAR_VENDEDOR_REQUEST_SUCCESS';
export const POST_CREAR_VENDEDOR_REQUEST_FAILURE= 'POST_CREAR_VENDEDOR_REQUEST_FAILURE';
export const POST_CREAR_VENDEDOR_REQUEST= 'POST_CREAR_VENDEDOR_REQUEST';

export const POST_ELIMINAR_VENDEDOR_REQUEST_SUCCESS= 'POST_ELIMINAR_VENDEDOR_REQUEST_SUCCESS';
export const POST_ELIMINAR_VENDEDOR_REQUEST_FAILURE= 'POST_ELIMINAR_VENDEDOR_REQUEST_FAILURE';
export const POST_ELIMINAR_VENDEDOR_REQUEST= 'POST_ELIMINAR_VENDEDOR_REQUEST';

export const POST_OBTENER_COMPROBANTE_CREDITO_SUCCESS= 'POST_OBTENER_COMPROBANTE_CREDITO_SUCCESS';
export const POST_OBTENER_COMPROBANTE_CREDITO= 'POST_OBTENER_COMPROBANTE_CREDITO';

export const POST_GENERAR_BALANCE_SUCCESS= 'POST_GENERAR_BALANCE_SUCCESS';
export const POST_GENERAR_BALANCE= 'POST_GENERAR_BALANCE';
export const POST_LIMPIAR_BALANCE= 'POST_LIMPIAR_BALANCE';


export const POST_GENERAR_AUX_BALANCE_SUCCESS= 'POST_GENERAR_AUX_BALANCE_SUCCESS';
export const POST_GENERAR_AUX_BALANCE= 'POST_GENERAR_AUX_BALANCE';


export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export interface ClearDesembolsoData {
    type: 'CLEAR_CREATEE_DESEMBOLSOO_REQUEST';
}

export interface PostDesembolsoData {
    type: 'POST_CREATEE_DESEMBOLSOO_REQUEST';
}
export interface PostDesembolsoDataSuccess {
    type: 'POST_CREATEE_DESEMBOLSOO_REQUEST_SUCCESS';
    desembolsoData: any[];
}



export interface ResetCreditState {
    type: 'RESET_CREDIT_STATE';
}

export interface ClearDetailRequestInformation {
    type: 'CLEAR_DETAIL_REQUEST_INFORMATION';
}

export interface GetCreditTypes {
    type: 'POST_PROCEDURE_REQUEST';
}

export interface GetCreditTypesSuccess {
    type: 'POST_PROCEDURE_REQUEST_SUCCESS';
    procedureRequests: ProcedureRequestModel[];
}

export interface PutRequestActivityProcess {
    type: 'PUT_REQUEST_ACTIVITY_PROCESS';
}

export interface PutRequestActivityProcessSuccess {
    type: 'PUT_REQUEST_ACTIVITY_PROCESS_SUCCESS';
    result: RequestActivityRequestModel;
}

export interface PostCreditBasicData {
    type: 'POST_CREDIT_BASIC_DATA';
}

export interface PostCreditBasicDataSuccess {
    type: 'POST_CREDIT_BASIC_DATA_SUCCESS';
    creditBasicData: CreditBasicDataModel;
}



interface PostCreditProductData {
    type: 'POST_CREDIT_PRODUCT_DATA';
}

interface PostCreditProductDataSuccess {
    type: 'POST_CREDIT_PRODUCT_DATA_SUCCESS';
    creditProductData: CreditProductModel;
}

interface PostCreditObservationsData {
    type: 'POST_CREDIT_OBSERVATIONS_DATA';
}

interface PostCreditObservationsDataSuccess {
    type: 'POST_CREDIT_OBSERVATIONS_DATA_SUCCESS';
    observationsData: CreditObservationModel[];
}

interface PostCreditFilesData {
    type: 'POST_CREDIT_FILES_DATA';
}

interface PostCreditFilesDataSuccess {
    type: 'POST_CREDIT_FILES_DATA_SUCCESS';
    filesData: CreditFileModel[];
}


/// estdo es una prueba y se debe borrar

interface GetPruebaData{
type: 'GET_PRUEBA_DATA';
}

interface GetPruebaDataSuccess{
  type: 'GET_PRUEBA_DATA_SUCCESS';
  prueba: Prueba  
}


interface PostCreditFinancialData {
    type: 'POST_CREDIT_FINANCIAL_DATA';
}

interface PostCreditFinancialDataSuccess {
    type: 'POST_CREDIT_FINANCIAL_DATA_SUCCESS';
    financialData: CreditFinancialDataModel;
}

interface PostCreditCreateTechnicalData {
    type: 'POST_CREDIT_CREATE_TECHNICAL_DATA';
}

interface PostCreditCreateTechnicalDataSuccess {
    type: 'POST_CREDIT_CREATE_TECHNICAL_DATA_SUCCESS';
    creditTechnicalData: CreditTechnicalDataModel;
}

interface PostCreditTechnicalData {
    type: 'POST_CREDIT_TECHNICAL_DATA';
}

interface PostCreditTechnicalDataSuccess {
    type: 'POST_CREDIT_TECHNICAL_DATA_SUCCESS';
    creditTechnicalData: CreditTechnicalDataModel;
}

interface PostCreditLegalData {
    type: 'POST_CREDIT_LEGAL_DATA';
}

interface PostCreditLegalDataSuccess {
    type: 'POST_CREDIT_LEGAL_DATA_SUCCESS';
    creditLegalData: CreditLegalDataModel;
}

interface PostCreditCreateLegalData {
    type: 'POST_CREDIT_CREATE_LEGAL_DATA';
}

interface PostCreditCreateLegalDataSuccess {
    type: 'POST_CREDIT_CREATE_LEGAL_DATA_SUCCESS';
    creditLegalData: CreditLegalDataModel;
}

interface GetCreditFinancialAnalystData {
    type: 'GET_CREDIT_FINANCIAL_ANALYST_DATA';
}

interface GetCreditFinancialAnalystDataSuccess {
    type: 'GET_CREDIT_FINANCIAL_ANALYST_DATA_SUCCESS';
    creditProductData: CreditProductModel;
    creditTechnicalData: CreditTechnicalDataModel;
    creditLegalData: CreditLegalDataModel;
    creditSimulationData: CreditSimulationDataModel;
    creditConsultCreditData: CreditConsultCreditModel;
}

interface PostCreditFlowDocuments {
    type: 'POST_CREDIT_FLOW_DOCUMENTS';
}

interface PostCreditFlowDocumentsSuccess {
    type: 'POST_CREDIT_FLOW_DOCUMENTS_SUCCESS';
    flowDocumentsData: CreditFlowDocumentModel[];
}

interface GetCreditFlowDocumentsSuccess {
    type: 'GET_CREDIT_FLOW_DOCUMENTS_SUCCESS';
    documentsData: CreditDocumentModel[];
}

interface PostConsultCredit {
    type: 'POST_CONSULT_CREDIT';
}

interface PostConsultCreditSuccess {
    type: 'POST_CONSULT_CREDIT_SUCCESS';
    consultCreditData: CreditConsultCreditModel;
    insurancesListData: any[];
}

interface PostCreateSimulation {
    type: 'POST_CREATE_SIMULATION';
}
interface SetErrorSobrante {
    type: 'POST_ERROR_SIMULATION_SOBRANTE';
}

interface PostCreateSimulationSuccess {
    type: 'POST_CREATE_SIMULATION_SUCCESS';
    simulationData: CreditCreateSimulationDataModel[];
}

interface PostCreditCommittee {
    type: 'POST_CREDIT_COMMITTEE';
}

interface PostCreditCommitteeSuccess {
    type: 'POST_CREDIT_COMMITTEE_SUCCESS';
    result: any;
}

interface GetDetailCommittee {
    type: 'GET_DETAIL_COMMITTEE';
}

interface GetDetailCommitteeSuccess {
    type: 'GET_DETAIL_COMMITTEE_SUCCESS';
    result: {
        topics: any[];
        members: any[];
        listMembers?: any[];
        listTopics?: any[];
    };
}

interface PostCreateUpdateCommittee {
    type: 'POST_CREATE_UPDATE_COMMITTEE';
}

interface PostCreateUpdateCommitteeSuccess {
    type: 'POST_CREATE_UPDATE_COMMITTEE_SUCCESS';
    result: any;
}

interface PostCreditCommitteeRequest {
    type: 'POST_CREDIT_COMMITTEE_REQUEST';
}

interface PostCreditCommitteeRequestSuccess {
    type: 'POST_CREDIT_COMMITTEE_REQUEST_SUCCESS';
    result: any;
}

interface PostCreditCommitteeApprove {
    type: 'POST_CREDIT_COMMITTEE_APPROVE';
}

interface PostCreditCommitteeApproveSuccess {
    type: 'POST_CREDIT_COMMITTEE_APPROVE_SUCCESS';
    result: any;
}

interface PostCreditCommitteeSoli {
    type: 'POST_CREDIT_COMMITTEE_SOLI';
}

interface PostCreditCommitteeSoliSuccess {
    type: 'POST_CREDIT_COMMITTEE_SOLI_SUCCESS';
    result: any;
}
interface PostCreaSCnMcontAprobacionCredito {
    type: 'POST_CREA_APROVACION_DESEMBOLSO';
}

interface PostCreaSCnMcontAprobacionCreditoSuccess {
    type: 'POST_CREA_APROVACION_DESEMBOLSO_SUCCESS';
    result: any;
}

interface PostCreateCreditCommitteeRequest {
    type: 'POST_CREATE_CREDIT_COMMITTEE_REQUEST';
}

interface PostCreateCreditCommitteeRequestSuccess {
    type: 'POST_CREATE_CREDIT_COMMITTEE_REQUEST_SUCCESS';
}


// LEGALIZACION

interface PostLegalizacionRequest {
    type: 'POST_LEGALIZACION_REQUEST';
}

interface PostLegalizacionRequestSuccess {
    type: 'POST_LEGALIZACION_REQUEST_SUCCESS';
    legalizacion: any;
}

interface PostLegalizacionFirmasRequest {
    type: 'POST_LEGALIZACION_FIRMAS_REQUEST';
}

interface PostLegalizacionFirmasRequestSuccess {
    type: 'POST_LEGALIZACION_FIRMAS_REQUEST_SUCCESS';
    firmas: any;
}

interface PostVerificacionTercerosRequest  {
    type: 'POST_VERIFICACION_TERCEROS_REQUEST';
}

interface PostVerificacionTercerosRequestSuccess  {
    type: 'POST_VERIFICACION_TERCEROS_REQUEST_SUCCESS';
    terceros: any;
}

interface GetDataLegalizacion   {
    type: 'GET_DATA_LEGALIZACION_REQUEST';
}

interface GetDataLegalizacionSuccess   {
    type: 'GET_DATA_LEGALIZACION_REQUEST_SUCCESS';
    answerLegalizacion: any;
}


interface PatchDesembolsoData    {
    type: 'PATCH_DESEMBOLSO_REQUEST';
}

interface PatchDesembolsoDataSuccess    {
    type: 'PATCH_DESEMBOLSO_REQUEST_SUCCESS';
    desembolsoAprovado: any;
}

interface GetDesembolsoDesistimiento    {
    type: 'GET_DESEMBOLSO_DESISTIMIENTO_REQUEST';
}


interface GetDesembolsoDesistimientoSuccess    {
    type: 'GET_DESEMBOLSO_DESISTIMIENTO_REQUEST_SUCCESS';
    causalDesistimiento: any;
}

interface PostDesistirDesembolo    {
    type: 'POST_DESISTIR_DESEMBOLSO_REQUEST';
}


interface PostDesistirDesemboloSuccess    {
    type: 'POST_DESISTIR_DESEMBOLSO_REQUEST_SUCCESS';
    desistirDesembolso: any;
}

interface GetObtenerValorDesembolsoCredito    {
    type: 'GET_OBTENER_VALOR_DESEMBOLSO_REQUEST';
}

interface GetObtenerValorDesembolsoCreditoSuccess    {
    type: 'GET_OBTENER_VALOR_DESEMBOLSO_REQUEST_SUCCESS';
    valorDesembolsoCreditoDto: any;
}


// VENDEDOR O BENEFICIARIO SOLICITUD

export interface PostObtenerVendedor {
    type: 'POST_OBTENER_VENDEDOR_REQUEST';
}
export interface PostObtenerVendedorSuccess {
    type: 'POST_OBTENER_VENDEDOR_REQUEST_SUCCESS';
    formularioVendedor: CreditVendedorModel;
}
export interface PostObtenerVendedorFailure {
    type: 'POST_OBTENER_VENDEDOR_REQUEST_FAILURE';
    error: any;
}

export interface PostCrearVendedor {
    type: 'POST_CREAR_VENDEDOR_REQUEST';
}
export interface PostCrearVendedorSuccess {
    type: 'POST_CREAR_VENDEDOR_REQUEST_SUCCESS';
    dataVendedor: CreditVendedorModel;

}
export interface PostCrearVendedorFailure {
    type: 'POST_CREAR_VENDEDOR_REQUEST_FAILURE';
    error: any;
}

export interface DeleteEliminarVendedor {
    type: 'POST_ELIMINAR_VENDEDOR_REQUEST';
}
export interface DeleteEliminarVendedorSuccess {
    type: 'POST_ELIMINAR_VENDEDOR_REQUEST_SUCCESS';
    idVendedor: string;
    // formularioVendedor: CreditVendedorModel;
}
export interface DeleteEliminarVendedorFailure {
    type: 'POST_ELIMINAR_VENDEDOR_REQUEST_FAILURE';
    error: any;
}
export interface PostObtenerComprobantesCredito {
    type: 'POST_OBTENER_COMPROBANTE_CREDITO';
}
export interface PostObtenerComprobantesCreditoSuccess {
    type: 'POST_OBTENER_COMPROBANTE_CREDITO_SUCCESS';
    comprobanteCredito: string;
}

export interface PostGenerarBalanceCartera {
    type: 'POST_GENERAR_BALANCE';
}
export interface PostLimpiarStateBalanceCartera {
    type: 'POST_LIMPIAR_BALANCE';
}
export interface PostGenerarBalanceCarteraSuccess {
    type: 'POST_GENERAR_BALANCE_SUCCESS';
    balanceCartera: string;
}

export interface PostDescargarBalanceCarteraAux {
    type: 'POST_GENERAR_AUX_BALANCE';
}
export interface PostDescargarBalanceCarteraSuccessAux {
    type: 'POST_GENERAR_AUX_BALANCE_SUCCESS';
}


export type CreditActionsType =
    | GetCreditTypes
    | GetCreditTypesSuccess
    | PutRequestActivityProcess
    | PutRequestActivityProcessSuccess
    | PostCreditBasicData
    | PostCreditBasicDataSuccess
    | PostCreditProductData
    | PostCreditProductDataSuccess
    | PostCreditObservationsData
    | PostCreditObservationsDataSuccess
    | PostCreditFilesData
    | PostCreditFilesDataSuccess
    | PostCreditFinancialData
    | PostCreditFinancialDataSuccess
    | PostCreditCreateTechnicalData
    | PostCreditCreateTechnicalDataSuccess
    | PostCreditTechnicalData
    | PostCreditTechnicalDataSuccess
    | PostCreditLegalData
    | PostCreditLegalDataSuccess
    | PostCreditCreateLegalData
    | PostCreditCreateLegalDataSuccess
    | GetCreditFinancialAnalystData
    | GetCreditFinancialAnalystDataSuccess
    | PostCreditFlowDocuments
    | PostCreditFlowDocumentsSuccess
    | PostConsultCreditSuccess
    | PostConsultCredit
    | PostCreateSimulation
    | SetErrorSobrante
    | PostCreateSimulationSuccess
    | PostCreditCommittee
    | PostCreditCommitteeSuccess
    | GetDetailCommittee
    | GetDetailCommitteeSuccess
    | PostCreateUpdateCommittee
    | PostCreateUpdateCommitteeSuccess
    | PostCreditCommitteeRequest
    | PostCreditCommitteeRequestSuccess
    | PostCreditCommitteeApprove
    | PostCreditCommitteeApproveSuccess
    | PostCreateCreditCommitteeRequest
    | PostCreateCreditCommitteeRequestSuccess
    | OnError
    | ClearDetailRequestInformation
    | PostDesembolsoData
    | PostDesembolsoDataSuccess
    | ClearDesembolsoData
    | GetCreditFlowDocumentsSuccess
    | GetPruebaData
    | GetPruebaDataSuccess
    | PostLegalizacionRequest
    | PostLegalizacionRequestSuccess
    | PostLegalizacionFirmasRequest
    | PostLegalizacionFirmasRequestSuccess
    | PostVerificacionTercerosRequest
    | PostVerificacionTercerosRequestSuccess
    | GetDataLegalizacion
    | GetDataLegalizacionSuccess
    | PatchDesembolsoData
    | PatchDesembolsoDataSuccess
    | GetDesembolsoDesistimiento
    | GetDesembolsoDesistimientoSuccess
    | PostDesistirDesembolo
    | PostDesistirDesemboloSuccess
    | ResetCreditState
    | GetObtenerValorDesembolsoCredito
    | GetObtenerValorDesembolsoCreditoSuccess
    | PostObtenerVendedor
    | PostObtenerVendedorSuccess
    | PostObtenerVendedorFailure
    | PostCrearVendedor
    | PostCrearVendedorSuccess
    | PostCrearVendedorFailure
    | DeleteEliminarVendedor
    | DeleteEliminarVendedorSuccess
    | DeleteEliminarVendedorFailure
    | PostObtenerComprobantesCredito
    | PostObtenerComprobantesCreditoSuccess
    | PostCreditCommitteeSoli
    | PostCreditCommitteeSoliSuccess
    | PostCreaSCnMcontAprobacionCredito
    | PostCreaSCnMcontAprobacionCreditoSuccess
    | PostGenerarBalanceCartera
    | PostLimpiarStateBalanceCartera
    | PostGenerarBalanceCarteraSuccess
    | PostDescargarBalanceCarteraAux
    | PostDescargarBalanceCarteraSuccessAux;
