export interface ActualizarEstadoEmbargoDto{
  id: string;
  Inconsistencia: boolean;
  aclaracion: string;
}
export interface EmbargoModelDto {
  embargo: [{
    id: string;
    radicadoWorkManager: string;
    fechaRadicadoWorkManager: Date;
    radicadoJuzgado: string;
    fechaRadicadoJuzgado: Date;
    emailRespuesta: string;
    nombreCliente: string;
    nombreDemandante: string;
    nombreDemandado: string;
    expedienteBancoAgrario: string;
    remanente: boolean;
    Inconsistencia: boolean;
    aclaracion: string;
    observaciones: string;
    valor: number;
    clienteId: number;
    identificacionCliente: string;
    clienteTipoIdentificacion: number | null;
    clienteNombre: string;
    idDepartamento: string;
    idCiudad: string;
    juzgadoId: string;
    codigoInterno: string | null;
    codigoOficina: number | null;
    numeroCuenta: number | null;
    procesoId: string;
    tipoEmbargoId: string;
    juzgado: string;
    tipoEmbargo: string;
    tipoProceso: string;
    tipoIdentificacion: string;
    proceso: string;
    direccion: string;
    numeroSolicitud: number;
    tipoIdentificacionId: number;
    estadoEmbargo: string;
  }];
  beneficiarios: [{
    id: string;
    tipoDocumentoId: number,
    numeroIdentificacion: string;
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
    embargoId: string;
    demandante: boolean;
    beneficiarioPago: boolean;
    insert: boolean;
    bpE_BENEFICIARIO: boolean;
  }];
  conceptos: [{
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
    tipoRetencion: string | null;
    tipoEmbargo: string | null;
    estado: string | null;
  }];
}
export interface radicadoEmbargoDto {
  RadicadoWorkManager: string;
  FechaRadicadoWorkManager: Date;
  RadicadoJuzgado: string;
  FechaRadicadoJuzgado: Date;
  EmailRespuesta: string;
  NombreDemandado: string;
  TipoIdentificacionId: number;
  ClienteId: number;
  JuzgadoId: string;
  IdentificacionCliente: string;
  CreadoPor: string;
}

export interface EmbargoDto {
  id: string;
  radicadoWorkManager: string;
  fechaRadicadoWorkManager: Date;
  radicadoJuzgado: string;
  fechaRadicadoJuzgado: Date;
  emailRespuesta: string;
  nombreCliente: string;
  nombreDemandante: string;
  nombreDemandado: string;
  expedienteBancoAgrario: string;
  remanente: boolean;
  Inconsistencia: boolean;
  aclaracion: string;
  observaciones: string;
  valor: number;
  clienteId: number;
  identificacionCliente: string;
  clienteTipoIdentificacion: number | null;
  clienteNombre: string;
  idDepartamento: string;
  idCiudad: string;
  juzgadoId: string;
  codigoInterno: string | null;
  codigoOficina: number | null;
  numeroCuenta: number | null;
  procesoId: string;
  tipoEmbargoId: string;
  juzgado: string;
  tipoEmbargo: string;
  tipoProceso: string;
  tipoIdentificacion: string;
  proceso: string;
  direccion: string;
  numeroSolicitud: number;
  tipoIdentificacionId: number;
  estadoEmbargo: string;
  creadopornombre: string;
  creadoporapellido: string;
  CreadoPor: string;
  NombreCompletoCliente: string;
  tipoSolicitud: number;
  estado: boolean;

}

export interface EmbargoInsertDto {
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
  Inconsistencia: boolean;
  aclaracion: string;
  observaciones: string;
  respuesta: string;
  valor: number;
  clienteIdentificacion: string;
  clienteTipoIdentificacion: number;
  juzgadoId: string;
  procesoId: string;
  tipoEmbargoId: string;
  numeroSolicitud: number;
}

export interface EmbargoUpdateDto {
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
  Inconsistencia: boolean;
  aclaracion: string;
  observaciones: string;
  respuesta: string;
  valor: 0;
  clienteTipoIdentificacion: number;
  clienteIdentificacion: string;
  juzgadoId: string;
  procesoId: string;
  tipoEmbargoId: string;
  numeroSolicitud: number;
}

export interface TipoEmbargoDto {
  id: string;
  nombre: string;
  descripcion: string;
  estado: boolean;
}

export interface TipoProcesoEmbargoDto {
  id: string;
  nombre: string;
  descripcion: string;
  estado: boolean;
}

export interface TipoRetencionDto {
  idReal: any;
  id: string;
  nombre: string;
  descripcion: string;
  estado: boolean;
}


export interface EmbargoConceptoDto {
  id: string;
  valor: number;
  fechaInicio: Date;
  fechaFin: Date;
  indicadorCesantias: boolean;
  tipoRetencionId: string;
  tipoEmbargoId: string;
  radicadoWorkManager: string;
  numeroCuenta: number;
  cuentaId: number;
  conceptoId: number;
}

export interface ConceptoEmbargosDto {
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
  tipoRetencion: string | null;
  tipoEmbargo: string | null;
}


export interface EmbargoConceptoInsertDto {
  valor: number;
  fechaInicio: Date;
  fechaFin: Date;
  indicadorCesantias: boolean;
  tipoRetencionId: string;
  tipoEmbargoId: string;
  radicadoWorkManager: string;
  numeroCuenta: number;
}

export interface BeneficiarioDto {
  id: string;
  tipoDocumentoId: number,
  numeroIdentificacion: string;
  primerNombre: string;
  segundoNombre: string;
  primerApellido: string;
  segundoApellido: string;
  embargoId: string;
  demandante: boolean;
  beneficiarioPago: boolean;
  entidad: string;
  insert: boolean;
  bpE_BENEFICIARIO: boolean;
}
export interface EmbargoBeneficiariosDto {
  id: string;
  radicadoWorkManager: string;
  tipoDocumentoId: number;
  tipoIdentificacion: string;
  numeroIdentificacion: string;
  primerNombre: string;
  segundoNombre: string;
  primerApellido: string;
  segundoApellido: string;
}
export interface EmbargoBeneficiariosInsertDto {
  radicadoWorkManager: string;
  tipoDocumentoId: number;
  numeroIdentificacion: string;
  primerNombre: string;
  segundoNombre: string;
  primerApellido: string;
  segundoApellido: string;
}
export interface CuentaModelDto {
  cuentaId: number;
  cuentaIdIntegracion: number;
  numeroCuenta: number;
  clinteId: number;
  tipoCuentaId: number;
  estadoCuentaId: number;
  estadoCuenta: string;
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

export interface BusquedaEmbargos {
  fechaInicial: Date | null;
  fechaFinal: Date | null;
  clienteTipoIndentificacion: number;
  clienteIdentificacion: string;
  radicadoWorkManager: string;
  Estado: number;
  Estados: number | null;
}

export interface BusquedaDesembargos {
  fechaInicial: Date | null;
  clienteTipoIndentificacion: number;
  fechaFinal: Date | null;
  clienteIdentificacion: string;
  radicadoWorkManager: string;
  Estado: number;
  Estados: number | null;
}


export interface InsertarPagoEmbargo {
  IdEmargo: string,
  Fecha: Date | string,
  ClienteTipoIdentificacion: number;
  ClienteIdentificacion: number
  NombreRazonSocial: string,
  Valor: number,
  TipoRetencionId: string,
}

export interface PagoEmbargoDto {
  IdEmargo: string,
  Fecha: Date | string,
  ClienteTipoIdentificacion: number;
  Observacion: string;
  NumeroCuenta: string;
  ClienteIdentificacion: string;
  TipoEmbargo: string;
  NombreRazonSocial: string;
  Valor: number;
  Aprobado: boolean;
  Entidad: string;
  CreadoPor: string;
  TipoRetencionId: any;
}