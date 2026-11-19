export interface ObtenerDesembargoDto {
    id: string;
    radicadoWorkManager: string;
    fechaRadicadoWorkManager: Date;
    radicadoJuzgado: string;
    fechaRadicadoJuzgado: Date;
    emailRespuesta: string;
    nombreDemandado: string;
    expedienteBancoAgrario: string;
    remanente: boolean;
    observaciones: string;
    respuesta: string;
    valor: number;
    cliente: string;
    clienteId: number;
    juzgadoId: string;
    procesoId: string;
    tipoEmbargoId: string;
    fechaRegistro: Date;
    fechaActualizacion: Date;
    creadoPor: string;
    actualizadoPor: string;
    juzgado: string;
    tipoEmbargo: string;
    tipoProceso: string;
    primerNombreCliente: string;
    segundoNombreCliente: string;
    primerApellidoCliente: string;
    segundoApellidoCliente: string;
    identificacionCliente: string;
    nombreCompletoCliente: string;
    tipoIdentificacionId: number;
    tipoIdentificacion: string;
    clienteTipoIdentificacion: number;
    estado: boolean;
    tipoActor: string;
    estadoEmbargo: string;
}

export interface DesembargoModelDto {
    id: string;
    radicadoWorkManager: string;
    fechaRadicadoWorkManager: Date;
    radicadoJuzgado: string;
    fechaRadicadoJuzgado: Date;
    emailRespuesta: string;
    direccionRespuesta: string;
    expedienteBancoAgrario: string;
    remanente: boolean;
    observaciones: string;
    respuesta: string;
    clienteTipoIdentificacion: number;
    clienteIdentificacion: string;
    juzgadoId: string;
    procesoId: string;
    proceso: string;
    beneficiariosPagos: BeneficiarioDto[];
    conceptosEmbargos: ConceptoDesembargosDto[];
  }

export interface DesembargoDto {
    id: string;
    desembargoCuentaConceptoId: string;
    nombreDemandado: string;
    fechaRegistro: Date;
    estado: boolean;
    radicadoWorkManager: string;
}

export interface TipoProcesoDesembargoDto {
    id: string;
    nombre: string;
    descripcion: string;
    estado: boolean;
}

export interface ConceptoDesembargosDto {
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
}

export interface CuentaModelDto {
    cuentaId: number;
    cuentaIdIntegracion: number;
    numeroCuenta: number;
    clinteId: number;
    tipoCuentaId: number;
    estadoCuentaId: number;
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
    conceptos: ConceptoModelDto[];
    nombreCuenta: string;
}

export interface ConceptoModelDto {
    id: number;
    cuentaId: number;
    conceptoId: number;
    conceptoDescripcion: string;
    saldo: number;
    fechaUltimoAporte: Date;
    ingresos: number;
    sva: number;
}

export interface BusquedaDesembargos {
    fechaInicial: Date;
    fechaFinal: Date;
    clienteTipoIndentificacion: number;
    identificacionCliente: string;
    radicadoWorkManager: string;
  }

export interface ConsultaIdentificacion {
    clienteTipoIndentificacion: number;
    identificacionCliente: string;
    tipoIdentificacionId: number;
    tipoIdentificacion: string;
}

  export interface DesembargoInsertDto{
    radicadoWorkManager: string;
    fechaRadicadoWorkManager: Date;
    radicadoJuzgado: string;
    fechaRadicadoJuzgado: Date;
    emailRespuesta: string;
    direccionRespuesta: string;
    nombreDemandante: string;
    nombreDemandado: string;
    expedienteBancoAgrario: string;
    remanente: boolean;
    observaciones: string;
    respuesta: string;
    valor: number;
    clienteIdentificacion: string;
    clienteTipoIdentificacion: number;
    juzgadoId: string;
    procesoId: string;
    tipoEmbargoId: string
  }

  export interface DesembargoUpdateDto{
    id: string;
    radicadoWorkManager: string;
    fechaRadicadoWorkManager: Date;
    radicadoJuzgado: string;
    fechaRadicadoJuzgado: Date;
    emailRespuesta: string;
    direccionRespuesta: string;
    nombreDemandante: string;
    nombreDemandado: string;
    expedienteBancoAgrario: string;
    remanente: boolean;
    observaciones: string;
    respuesta: string;
    valor: 0;
    clienteTipoIdentificacion: number;
    clienteIdentificacion: string;
    juzgadoId: string;
    procesoId: string;
    tipoEmbargoId: string
  }

  export interface BeneficiarioDto {
    id: string;
    tipoDocumentoId: number,
    numeroIdentificacion: string;
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
    demandante: boolean;
    beneficiarioPago: boolean;
    insert: boolean;
  }

export interface TipoDesembargoDto {
    id: string;
    nombre: string;
    descripcion: string;
    estado: boolean;
}