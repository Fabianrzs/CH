export interface DocumentosPorSubModalidadModel {
    idArchivo: string;
    nombreArchivo: string;
    descripcionArchivo: string;
    esRequerido: boolean;
    ordenArchivo: number;
    esMultipleArchivo: boolean;
    formatoArchivo: string;
    numeroArchivos?: string;
    archivosCargados: FileList;
    esArchivoCargado: boolean;
    numeroFolio: string;
    errorArchivoCargado: boolean;
}
