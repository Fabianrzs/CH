export interface ConsultarAuditoriaNovedadesDto {
    tipoDocumento: number,
    nDocumento: number,
    detalleModificacion: string;
    fechaModificacion: Date;
    fechaInicio: Date;
    fechaFin: Date;
    usuarioModificacion: string;
    idModulo: number;
    tipoAccion: string;
    modulo: string;
    idUnidadEjecutora: number;
    nombreUnidadEjecutora: string;
}

export interface InsertarAuditoriaNovedadesDto {
    tipoDocumento: number,
    nDocumento: number,
    detalleModificacion: string;
    fechaModificacion: Date;
    fechaInicio: Date;
    fechaFin: Date;
    usuarioModificacion: string;
    idModulo: number;
    tipoAccion: string;
    idUnidadEjecutora: number;
    nombreUnidadEjecutora: string;
}