export interface ProductoDto {
    idCredito: string;
    identificacion: string;   
    edad: number;
    // Categoria
    // Cargo/Grado
    valorVivienda: number;
    idEstadoVivienda: number;  // VIS, NO VIS
    idTipoVivienda: number; // Usada, Nueva
    claseInmueble: string; // apartamento, casa
    noMatricula: string;
    escritura: number;
    fechaEscritura: Date;
    notaria: string;
    linderos: string;
    direccionInmueble: string;
    ciudadInmueble: string;
    descripcionInmueble: string;
    
    cuotaCredito?: number;

    idTipoCredito: number;
    numeroCredito: number;
    fechaSolicitud: Date;
    valorDesembolso: number;
    fechaDesembolso: Date;
    valorCanonInicial: number;
    valorAbonoExtra: number; // canon extraordinario
    canonExtraordinario: number; 
    valorCuota: number;
    diaPago: number;
    fechaProximoPago: Date;
    fechaUltimoPago: Date;
    estadoCredito: string;  
    fechaEstadoCredito: Date;
    diasMora: number;
    numeroCuotasCanceladas: number;
    cuotasPendientes: number;
    valorOpcionCompra: number;  
    porcentajeOpcionCompra: number;
    valorInteresCorriente: number;
    ultimoPagoInteresCorriente: Date;
    valorDeudaRemanente: number; // causacion acumulada interes remanente
    valorSeguroVida: number;
    valorSeguroHogar: number;
    idEstadoCredito: number; // prueba
    tipoAbonoExtra: string;
    montoInicial: number;
    saldoCapital: number;
    saldoTotal: number;
    saldoCapitalMora: number;
    plazoActual: number;
    plazoTotal: number;
    tasaCredito: number;
    tasaCreditoEA: number;
    tasaCreditoPer: number;
    tasaFrech: number;
    alivioFrech: number;
    valorAlivioCuota: number;
    tasaSeguroHogar: number;
    tasaSeguroVida: number;
    acumuladoInteresCorriente: number;
    acumuladoInteresMora: number;
    ultimoPagoInteresMora: Date;
    acumuladoSeguroHogar: number;
    acumuladoMoraSeguroHogar: number;
    ultimoPagoSeguroHogar: Date;
    acumuladoSeguroVida: number;
    acumuladoMoraSeguroVida: number;
    ultimoPagoSeguroVida: Date;
    score: number;
    fechaFinPagos: Date;
    fuerza?: string;
    unidadEjecutor?:number;
    saldoCapitalTradicional?:number;
    saldoInicialRedicido?:number;
    valorCuotaTradicional?:number;
    notariaInmueble?:string
    porcentajeCanonInicial?: number;
    visOrNVis?: string;
   
    idTipoCalle?:string;
    idLetra1?:string;
    numeroCalle1?:string;
    direccionBis1?:string;
    idCardinal1?:string;
    numeroCalle2?:string;
    idLetra2?:string;
    idCardinal2?:string;
    numeroCalle3?:string;
    idLetra3?:string;
    idCardinal3?:string;
    direccionComplemento?:string;
    tipoIdentificacion?:number
    interesRemanente?:number;
    totalObligacion?:number;
    tipoCredito?:string;
    interesRemanenteFin?:number;
    saldoTotalFin?:number;
    capitalRemanenteFin?:number;
  
    interesesRemanenteCorte?:number;
    saldoTotalCorte?:number;
    capitalRemanenteCorte?:number;
    canonExtraordinarioCorte?:number;

    interesesRemanenteFinal?:number;
    saldoCapitalRemanenteFinal?:number;
    canonExtraordinarioFinal?:number;
    valorTotalAdeudadoFinal?:number;

    lineaCredito?:string;
    deudaActualTotal?:number;
    deudaActual?:number;
    interesMora?:number;


    valorGarantia?: number;
    calificacion?: string;

    valorPagado: number;
    porcentajeSeguroHogar: number;
    porcentajeSeguroVida: number;
    porcentaeTasaNominalMensual: number;
    
}

export interface AplicacionPago {
    idAplicacionPago: string;
    idCredito: string;
    fechaPago: Date | null;
    fechaAplicacion: Date;
    abonoSeguroHogar: number;
    abonoSeguroVida: number;
    abonoInteresCorriente: number;
    abonoInteresMora: number;
    abonoCapital: number;
    valorPago: number;
    idDct: number;
    tipoPago?:string;
    modalidadPago?:string;
    capitalRemanente?:number;
    interesRemanente?:number;
    totalDeudaRemanente?:number;
    valorCanonExtraordinario?:number;
    valorPagoCuota?:number;
    deudaRemanete?:number;
    aplicaPago?: boolean; 
    esPagoReversible?: boolean;
    fechaCreacion?: Date;
    fechaAplicacionInformacion?: Date;
}

export interface AplicacionMasivaPagos {
    idAplicacionPago: string;
    idCredito: string;
    fechaPago: Date | null;
    fechaAplicacion: Date;
    abonoSeguroHogar: number;
    abonoInteresCorriente: number;
    abonoInteresMora: number;
    abonoCapital: number;
    valorPago: number;
    idDct: number;
    creadoPor: string;
    fechaCreacion: Date;
    modificadoPor: string;
    fechaModificacion: Date;
    valor: number;
    seguro_hogar: number;
    interes_cte: number;
    interes_mora: number;
    capital: number;
    observaciones: string;
}
export interface CierreDiarioCartera {
    creditoId: string;
    fechaCierre: Date;
    numeroCredito: number;
    identificacion: string;
    valorDesembolso: number;
    saldoCapital: number;
    diasMora: number;
    saldoCapitalMora: number;
    causacionInteresesCorrientes: number;
    causacionInteresesMora: number;
}
export interface InsertAplicacionPagoBody  {
    numeroCredito: number,        
    valorPago: number,
    identificacion: string,
    fechaPago: Date | null
 } 

 export interface procesoAplicacionMasivaPago  {
    fechaAplicacion: Date | null;     
    valor: number | null;
    archivo: File | null;
    formatoAplicar: number | null;
    aplicanPagos: boolean | null;
    creadoPor: string | null;
 } 

 export interface procesoListarAplicacionMasivaPago  {
    fechaAplicacion?: Date; 
    fechaProceso?: Date;    
    unidadEjecutora?: number;
    tipoCredito?: number;
 } 

 export interface ErrorType {
     error: boolean,
     mensaje: string
 }
 export interface ErrorListType {
     diaPago: ErrorType,
     diasMora: ErrorType,
     valorVivienda: ErrorType,
     valorDesembolso: ErrorType,
     plazoTotal: ErrorType,
     saldoCapital: ErrorType,
     capitalMora: ErrorType,
     tasaSeguroVida: ErrorType,
     tasaSeguroHogar: ErrorType,
     alivioFrech: ErrorType,
     tasaCredito: ErrorType,
     tasaFrech: ErrorType
 }

