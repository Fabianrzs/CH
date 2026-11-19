export interface SimuladorDto {
    idSimulacion: string;
    descripcion: string;
    tasaEA: number;
    minimoPlazoMeses: number;
    maximoPlazoMeses: number;
    estado: string;  
    min_Canon_Inicial?: number,
    max_Canon_Inicial?: number,
    max_Opc_Compra?: number,
    seguroHogar: string,

}
export interface SimuladorActorDto {
    parametroId?: string;
    simuladorId?: string;
    tipoId?: string;
    parametroTasaEa: number;
    parametroEstado?: number;
  
}
export interface SimuladorUndEjDto {
    idParametro: number;
    idSimulacion: number;
    idParametroUnidadEjecutora: number;
    tasaEA: number;
    estado: number;
}
export interface TipoActorDto {
    idTipoActor: number;
    descripcionTipoActor: string;
}
export interface UnidadEjecutoraDto {
    idUnidadEjecutora: number;
    nombreUnidadEjecutora: string;
}