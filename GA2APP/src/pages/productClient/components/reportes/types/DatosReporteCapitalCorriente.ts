export interface DatosReporteCapitalCorriente {
    numeroCredito: number,
    numeroIdentificacion: string,
    fechaApertura: Date,
    fechaTerminacion: Date,
    tipoCredito: string,
    periodos: CapitalCorriente[],
    [props: string]: number | string | Date | CapitalCorriente[];
}

export interface CapitalCorriente {
    mes: string,
    fechaPago: Date,
    capital: number,
}