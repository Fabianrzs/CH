export interface ReporteEstadoCuentaPlanPago {
    numeroCuota: number, 
    fechaPago: Date, 
    capital: number, 
    capitalPorPagar: number, 
    intereses: number, 
    interesesPorPagar: number, 
    seguroHogar: number, 
    seguroVida: number, 
    valorAPagar: number, 
}