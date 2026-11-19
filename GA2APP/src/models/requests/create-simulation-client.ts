export interface CreateSimulationClient {
    documento: string;
    tipoVivienda: string;
    valorInmueble: string;
    tipoAbono: number;
    subsidio: string;
    abonoUnico: boolean;
    valorPrestamo:number;
    valorCuota:number;
    plazo:number;
    canonInicial:number;
    opcionCompra:number;
    ValorViviendaSeguro?:number;
    TasaVoluntariaAportes?: boolean;
    fechaDesembolso?:string;
}

export interface CreditProductRequestModel {
    soB_ID?: string;
    soC_ID: string;
    ind: number;
    fechaDesembolso?:string;
}

export interface AdministracionParametroModel {
    tipoDocumentoId: any;
    numeroDocumento: string;
}

