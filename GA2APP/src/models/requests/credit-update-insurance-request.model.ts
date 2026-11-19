export interface CreditUpdateInsuranceRequestModel {
    idSolicitudCredito: string;
    convenioAseguradora: string;
    convenioAseguradoraHogar: string;
    idAseguradora: number;
    porcentajeExtraprima: number;
    actualizadoPor: string;
    fechaActualiza: string;
    idAseguradoraHogar: number;
    talla?: string;
    peso?: string;
    edad?: number;
}
