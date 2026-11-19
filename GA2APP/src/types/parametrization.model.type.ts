export interface PlazoDto {
    plazoId: number;
    plazoMinimoAnio: number;
    plazoMinimoMes: number;
    plazoMaximoAnio: number;
    plazoMaximoMes: number;
}
export interface PlazoTipoActorDto {
    plazoId: number;
    plazoMinimoAnio: number;
    plazoMinimoMes: number;
    plazoMaximoAnio: number;
    plazoMaximoMes: number;
    plazoEstado: boolean;
    idTipoActor: number;
    descripcionTipoActor: string;
    tipoCredito: number;
}
export interface PlazoUndejDto {
    plazoId: number;
    plazoMinimoAnio: number;
    plazoMinimoMes: number;
    plazoMaximoAnio: number;
    plazoMaximoMes: number;
    plazoEstado: boolean;
    unidadEjecutoraId: number;
    nombreUnidadEjecutora: string;
    tipoCredito: number;
}
export interface SegurosDto {
    idSeguro: number;
    seguroVida: number;
    fechaModificacion: Date;
    modificadoPor: number;
    seguroEstado: boolean;
    seguroTodoRiesgo:number;
    seg_Iva:number;
}

export interface SeguroMunicipioDto {
    idSeguro: number;
    idExcepcionSeguro: number
    seguroVida: number;
    seguroTodoRiesgo: number;
    seguroEstado: boolean;
    departamentoId: number;
    departamentoCodigo: number;
    departamentoNombre: string;
    ciudadId: number;
    ciudadCodigo: number;
    ciudadNombre: string;
    fechaModificacion: Date;
    modificadoPor: number;
}

export interface SeguroRegionDto {
    id: number;
    nombre: string;
    seguro: number;
}

export interface LiquidacionGeneralDto {
    idLiquidacion: number;
    fechaCorte: number;
    fechaPago: number;
    estado: number;
    fechaModificacion: Date;
    modificadoPor: number;
}

export interface SeguroDto {
    id: number;
    seguroVida: string;
    seguroTodoRiesgo: string;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
export interface TasaDto {
    id: number;
    tasaUsuraEA: number;
    tasaUsuraNM: number;
    tasaCorrienteEA: number;
    tasaCorrienteNM: number;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
export interface PlazosDto {
    id: number;
    yearMin: number;
    monthMin: number;
    yearMax: number;
    monthMax: number;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}

export interface LegalAlivioDto {
    id: number;
    alivioCuota: number;
    vigenciaAlivioCuota: Date;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
export interface LegalTasaDto {
    id: number;
    tasaFrech: number;
    vigenciaTasaFrech: Date;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
export interface LegalGmfDto {
    id: number;
    gmf: number;
    vigenciaGMF: Date;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
export interface ParametroTransaccionDto {
    paraM_TRANS_CONCEPTO: string;
    paraM_TRANS_CODIGO: number;
    paraM_TRANS_PROCESO: string;
    paraM_TRANS_CREADO_POR: string;
    paraM_TRANS_FECHA_CREACION: Date;
    paraM_TRANS_MODIFICADO_POR: string;
    paraM_TRANS_FECHA_MODIFICACION: Date;
}
export interface LiquidacionDto {
    id: number;
    fechaCorte: number;
    fechaPago: number;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
export interface IPCDto {
    id: number;
    anio: number;
    mes: number;
    FechaActualizacion: Date;
    ValorIPC: number;
    PorcentajeIPC: number;
    Desde_Anio: number;
    Desde_Mes: number;
    Hasta_Anio: number;
    Hasta_Mes: number;
    UsuarioId: string | null;
    UsuarioNombre: any;
}



