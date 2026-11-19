import { CreditProductRequestModel } from './credit-product-request.model';
import { CreditLegalDataRequestModel } from './credit-legal-data-request.model';
import { CreditTechnicalDataRequestModel } from './credit-technical-data-request.model';
import { CreditConsultCreditRequestModel } from './credit-consult-credit-request.model';

export interface CreditFinancialAnalystRequestModel {
    requestProductData: CreditProductRequestModel;
    requestLegalData: CreditLegalDataRequestModel;
    requestTechnicalData: CreditTechnicalDataRequestModel;
    requestConsultCredit: CreditConsultCreditRequestModel;
    requestClientSimulation: string;
}
