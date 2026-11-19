export interface CreditLegalDataModel {
 
    requestInformationLegalID: string;
    requestCreditID: string;
    boundaries: string;
    parkingType: string;
    legalAge: number;
    dateFirstAct: string;
    propertyTradition: string;
    analysisUlt20Years: string;
    analysisSitLegal: string;
    analysisRegHorizontalProperty: string;
    analysisPeaceAndSave: string;
    analysisSealer: string;
    legalBusinessViability: string;
    recommendations: string;
    legalConceptEnd: string;
    legalConceptEst: string;
    createdFor: string;
    createDate: string;
    propertyAddress: string;
    propertyStateID: number;
    propertyCityID: number;
    createdName: string;
    propertyCity: string;
    propertyState: string;
    creditLegalPropertyData: CreditLegalPropertyDataModel[];
    escritura: string,
    fechaEscritura: string,
    notaria:string,
    tipo_Matricula?: number,
    ciudadNotaria?: number ,
    departamentoNotaria?: number ,
    descripcionInmueble?: string,
    folioMatriculas?:number,
    oficinaInstrumentosPublicos?:string,
   
}

export interface CreditLegalPropertyDataModel {
    requestLegalPropertyID: string;
    requestInformationLegalID: string;
    requestCreditID: string;
    realStateRegistrationNumber: string;
    cadastralID: string;
    privateArea: number;
    commercialValue: number;
    cadastralValue: number;
    createdFor: string;
    createdDate: string;
}
