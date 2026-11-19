export default interface ResumenReporteContable {
    totalValorDebito: number;
    totalValorCredito: number,
    totalCreditos: number,
    totalPorTipoCredito: ResumenReporteContablePorTipoCredito[]
}

export interface ResumenReporteContablePorTipoCredito {
    idTipoCredito: number;
    codigoTipoCredito: string;
    tipoCredito: string;
    totalValorDebito: number;
    totalValorCredito: number,
    totalCreditos: number,
}