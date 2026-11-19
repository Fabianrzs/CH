export interface CreditLegalCreateRequestModel {
    idSolicitudInformacionJuridica?: string;
    idSolicitudCredito: string;
    linderos: string;
    tipoParqueadero: string;
    edadJuridica: number;
    fechaPrimerActo: string;
    tradicionInmueble: string;
    analisisUlt20Años: string;
    analisisSitJuridica: string;
    analisisRegPropHorizontal: string;
    analisisPazySalvo: string;
    analisisVendedor: string;
    viabilidadJurNegocio: string;
    recomendaciones: string;
    conceptoJuridicoFin: string;
    conceptoEstJuridicio: string;
    direccion: string;
    codigoDepartamento: number;
    codigoCiudad: number;
    creadoPor: string;
    fechaCreacion: string;
    solicitudesCreditoInfoJurInmueble: SolicitudesCreditoInfoJurInmueble[];
    escritura: string,
    fechaEscritura: string,
    notaria:string,
    tipo_Matricula?: number,
    departamentoNotaria?: number,
    ciudadNotaria?: number,
    descripcionInmueble?:string
    folioMatriculas?: number,
    oficinaInstrumentosPublicos?:string
}

export interface SolicitudesCreditoInfoJurInmueble {
    idSolicitudInfJuridicaInmueble?: string;
    idSolicitudInformacionJuridica?: string;
    idSolicitudCredito: string;
    numeroMatricula: string;
    cedulaCatastral: string;
    areaPrivada: number;
    avaluoComercial: number;
    avaluoCatastral: number;
    creadoPor: string;
    fechaCreacion: string;
}
