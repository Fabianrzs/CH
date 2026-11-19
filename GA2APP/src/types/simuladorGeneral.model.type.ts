export interface SimuladorGeneralDto {
    parametroSalarioMinimo: number;
    porcentajeInflacion: number;
    porcentajeViviendaVis: number;
    porcentajeViviendaNoVis: number;
    limiteSalariosVis: number;
    diasPreaprobado: number;
    diasAprobado?: number;
    puntajeMinimoCentrales: number;
    porcentajeMinimoEndeudamiento: number;
    
    tarifaUnicaAvaluos: number;
    tarifaEstudioTitulos: number;
    maximoMesesAlivioTasa: number;
    numeroMesesAlivio: number;
    factorAhorro: number,
    factorCesantias: number,
    porcentajeIncrementoAnual: number,
    vip?: string,
    vipCien?: string,
    vis?: string,
    visDos?: string,
    smlmv?: number,
    smlmvVis?: number,
    smlmvVisDos?: number,
    smlmvCien?: number,
    valorMinimoCuota?: number,
    valorTasaUsura?: number,
    valorTasaMora?: number,
    fechaTasaUsura?: Date,
    fechaTasaMora?: Date,
    vigenciaAprovacion?: number,
    vigenciaPreaprovado?:number,
    porcentajeMinimoCanonInicial?: number;
    porcentajeMaximoCanonInicial?: number;
    porcentajeMaximoOpcionCompra?: number;
    vigenciaDiasConsultaCentrales?: number;
    activeTu?:boolean;
    factorSubsidio?: number;
    score_Min_TU?: number;
    valor_SMMLV: number;
    mesesDiferir: number;
    activeBlob?: boolean,
    valorIpvAnual?: number,
    valorIpvAnualOtrasCiudades?: number,
    tasaUsuraPeriodica?: number,
    ahorroIntereses?:boolean,
    cesantiasIntereses?:boolean,
    rangoAscenso?: boolean
    incluirObligacionesExternasCapacidadPago?: boolean
}


