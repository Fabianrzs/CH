export interface CreditTypeModel {
    id: number;
    description: string;
    parentID: number;
    level: number;
    state: number;
    createdFor: string;
    creationDate: string;
    modifiedFor: string;
    modifiedDate: string;
}
