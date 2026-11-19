export interface CreditTechnicalDataModel {
    requestTechnicalInformationID: string;
    creditRequestID: string;
    commercialValue: string;
    propertySaleValue: string;
    finalConsiderations: string;
    complianceDeclaration: string;
    conditionsSavings: string;
    conceptValue: string;
    stratum: string;
    creditTechnicalPropertyInformationData: CreditTechnicalPropertyInformationData[];
    createdFor: string;
    createdDate: string;
    createdName: string;
    catastralValue: string;
}

export interface CreditTechnicalPropertyInformationData {
    requestPropertyTechnicalInformationID: string;
    requestTechnicalInformationID: string;
    creditRequestID: string;
    realStateRegistrationNumber: string;
    realStateRegistrationDate: string;
    propertyChip: string;
    cadastralID: string;
    createdFor: string;
    createdDate: string;
}
