export interface NovedadesCronogramaDto {
    id:number;
    idFuerza: number;
    idUnidadEjecutora: number;
    fuerzaDescripcion: string;
    nombreUnidadEjecutora: string;
    fechaReporte: Date;
    fechaInicio: Date;
    fechaFin: Date;
    periodo: string;
    idFormato: number;
    descripcionFormato: string;
    idMedioEnvio: number;
    descripcionMedioEnvio: string;
}

export interface novedades {
    idNovedad: number;
    idConcepto: number;
    codigoNovedadAlta: string;
    codigoNovedadBaja: string;
    idUnidadEjecutora: number;
}