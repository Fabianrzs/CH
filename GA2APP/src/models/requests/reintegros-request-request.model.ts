export interface ReintegrosRequestRequestModel {
    radicado?: string;
    unidades?: number;
    tipos?: number;
}

export interface AfiliadoModel {
    tipoDocumentoId: string;
    numeroDocumento: string;
    nombres: string;
    apellidos: string;
    idReintegro?: number;
    id_Tipo_Reintegro: number;
}

export interface CuentaModelDto {
    cuenta: any;
    cuentaId: number;
    cuentaIdIntegracion: number;
    numeroCuenta: number;
    clinteId: number;
    tipoCuentaId: number;
    estadoCuentaId: number;
    estadoCuenta?: string;
    causalEstadoId: number;
    documentoId: number;
    fechaCreacion: Date;
    fechaCierre: Date;
    fechaPrimerAporte: Date;
    saldo: number;
    saldoCanje: number;
    saldoDisponible: number;
    cueotas: number;
    cuentaBloqueo: boolean;
    conceptos: ConceptoDto[];
    nombreCuenta: string;
  }

export interface ConceptoDto {
    id: string;
    conceptoDescripcion: string;
    valor: number | null;
    fechaInicio: Date | null;
    fechaFin: Date | null;
    indicadorCesantias: boolean;
    tipoRetencionId: string;
    tipoEmbargoId: string;
    cuentaId: number;
    conceptoId: number;
    numeroCuenta: number;
    saldo: number;
    insert: boolean;
    valorEmbargo: number;
    valorDevolver: number;
    valorBloqueoEmbargo: number;
    valorNewSaldo: number;
}

