export interface UnidadEjecutoraCronogramaDTO {
    id: number;
    idUnidadEjecutora: number;
    idFuerza: number;
    idUnidadFuerza: number;
    fuerzaDescripcion: string;
    nombreUnidadEjecutora: string;
    fechaReporte: Date;
    fechaInicio: Date;
    fechaFin: Date;
    periodo: Date;
    idFormato: number;
    descripcionFormato: string;
    idMedioEnvio: number;
    idFormato2: number;
    descripcionMedioEnvio: string;
    novedades: NovedadesCronograma[];
    tipoDocumento: any;
    nDocumento: any;
}

export interface NovedadesCronograma{
    idNovedad: number;
    idConceptoNovedad: number;
    codigoNovedadAlta: string;
    codigoNovedadBaja: string;
    idUnidadEjecutora: string;
    editing: boolean;
}

export interface UnidadEjecutoraCronogramaEditarDTO {
    id: number;
    fechaReporte: Date;
}

export interface UnidadEjecutoraCronogramaDeleteDTO {
    id: number;
} 


