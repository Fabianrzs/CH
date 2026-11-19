export interface CierreMensualModel{
    tipoProceso:number,
    generaActualizacion: boolean,

}

export interface ListaCierreMensual{
    id: string,
    fechaCargue: Date,
    fechaProcesamiento: Date,
    usuarioProcesamiento: string,
    generaActualizacion: boolean,
    tipoProceso: number,
    cuentaAbonoAhorro: number,
    conceptosAhorro: string,
    cuentaAbonoCesantias: number,
    conceptosCesantias: string,
    tipoProcesoDesc:string,
    estadoDesc:string,
    ipc: number,
    anio: number,
    mes: number,
    estado: number
}
