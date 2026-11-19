export interface CreditFlowDocumentModel {
    tcrID: number;
    fluID: number;
    aflID: number;
    dcaID: number;
    tdcID: number;
    tdcDescription: string;
    dcaRequired: string;
    dcaOrder: number;
    existDocument: boolean;
}
