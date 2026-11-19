export interface CargueNominaDto {
    ejecutoraTMPID: number;
    id: number;
    fechaInicial: Date;
    periodoAportes: string;
    categoria: string;
    tipo: string;
    unidadEjecutora: number;
    file: string;
    estado: boolean;
}
export interface EjecutorDto {
    FILE_UNIDAD_EJECUTORA: number;
    FILE_TIPO: string;
    FILE_CATEGORIA: string;
    FILE_PERIODOS_APORTES: number;
    DCT_ID: string;
    DCT_FECHA_INCIAL: Date;
}

export const EjecutorInicializado: EjecutorDto = {
    DCT_FECHA_INCIAL: new Date(),
    DCT_ID: '',
    FILE_CATEGORIA: '',
    FILE_PERIODOS_APORTES: 0,
    FILE_TIPO: '',
    FILE_UNIDAD_EJECUTORA: 0,
};

export interface DocumentoCarga {
    documentoId?: string;
    tipoDocumentoId?: number;
    nombre?: string;
    fechaInicialDocumento?: Date;
    estadoDocumento?: number;
    causalEstadoDocumento?: number;
    unidadEjecutora?: number;
    fechaFinalDocumento?: Date;
    anulaId?: number;
    alerta?: string;
    fechaCreacion?: Date;
    creadoPor?: string;
    modificadoPor?: string;
    fechaModificacion?: Date;
    usuarioID?: string;
    causalRechazoID?: string;
    noSolicitud?: number;
}

export const DocumentoCargaInitial: DocumentoCarga = {
    documentoId: '',
    tipoDocumentoId: 0,
    nombre: '',
    fechaInicialDocumento: new Date(),
    estadoDocumento: 0,
    causalEstadoDocumento: 0,
    unidadEjecutora: 0,
    fechaFinalDocumento: new Date(),
    anulaId: 0,
    alerta: '',
    fechaCreacion: new Date(),
    creadoPor: '',
    modificadoPor: '',
    fechaModificacion: new Date(),
    noSolicitud: 0,
};

export interface ArchivosCargados {
    idDocumento: number;
    idTipoArchivo: number;
    tipoArchivo: string;
    clasificacionArchivo: string;
    monto: string;
    periodo: number;
    fechaCarga: number;
    documentoNombre: string;
    estado: string;
    usuario: string;
    errores: string;
    idUnidadEjecutora?: number;
    registros?: number;
    esdId?: number;
    noSolicitud: number;
} 
