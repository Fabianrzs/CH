export interface CreditTechnicalCreateRequestModel {
    idSolicitudInformacionTecnica?: string;
    idSolicitudCredito: string;
    valorAvaluoComercial: number;
    // valorVentaInmueble: number;
    consideracionesFinales: string;
    declaraCumplimiento: string;
    condicionesSalvedades: string;
    conceptoEstTecnico: string;
    estrato: string;
    creadoPor: string;
    fechaCreacion: string;
    solicitudesCreditoInfTecnicaInmueble: CreditTechnicalPropertyInfo[];
    valorAvaluoCatastral?:number
}

interface CreditTechnicalPropertyInfo {
    idSolicitudInfoTecnicaInmueble?: string;
    idSolicitudtecnica?: string;
    idSolicitudCredito: string;
    numeroMatricula: string;
    fechaMatricula: string;
    numeroChip: string;
    cedulaCatastral: string;
    creadoPor: string;
    fechaCreacion: string;
}
