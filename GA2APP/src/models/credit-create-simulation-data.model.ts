export interface CreditCreateSimulationDataModel {
    smdID: string;
    smcID: string;
    quota: number;
    cutDate: string;
    paymentDate: string;
    balance: number;
    interests: number;
    capital: number;
    lifeInsurance: number;
    quakeInsurance: number;
    feeTotal: number;
    extraPayment: number;
    monthlyPayment: number;
    payerUnitQuota: number;
    reductionInterest: number;
    reductionQuota: number;
    fonViviendaPayment: number;
    smlv: number;
    inflation: number;
    annualIncrement: number;
    visFinancial: number;
    minSalaryVIS: number;
    noVISFinancial: number;
    quakeInsurancePercentage: number;
    outlayDays: number;
    monthsLawFrech: number;
    monthLawAlive: number;
    allowanceMonthSol: number;
    allowanceMonthOther: number;
    rateEA: number;
    preApprovedValidDays: number;
    minMonthTerms: number;
    maxMonthTerms: number;
    state: string;
    createdFor: string;
    createdDate: string;
    modifiedFor: string;
    modifiedDate: string;
    canonIniLSG: number;
    canonFinLSG: number;
    purchaseOptionLSG: number;
    savingFactor: string;
    severanceFactor: string;
    loanValue: number;
    rateValue: number;
    visLiving: string;
    rightFee: string;
    rightRate: string;
    simulationID: string;
    minMonths: number;
    maxMonths: number;
    nominalRateValue: number;
    nominalRateFValue: number;
    abonoUnico: number;
    valorAbonoExtra: number;
    totalIntereses:number;
    capitalReducido:number;
    capitalPorPagar: number;
    interesesPorPagar: number;
    afectacionCuentaIndividual: number;
    saldoCapitalReducido: number;
    valorDeudaRemanente: number;
    saldoLeasing: number;
    capitalLeasing: number;
    capitalpagar: number;
    capitalSaldo: number;
    capitalInteresporPagar: number;
    capitalSaldoTotal: number;
    interesLeasing: number;
    interesPorPagar: number;
    seguroVidaLeasing: number;
    saldoCapital: number;
    saldoTotal: number;
}
export interface DatosSimulacionParaEstudioModel {
    cuotas: number;
    tipoActorId: number;
    tipoCreditoId: number;
}

export const DatosSimulacionParaEstudioForm: DatosSimulacionParaEstudioModel  = {
    cuotas:0,
    tipoActorId: 0,
    tipoCreditoId:0,
}
