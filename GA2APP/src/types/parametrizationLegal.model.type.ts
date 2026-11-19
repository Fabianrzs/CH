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
