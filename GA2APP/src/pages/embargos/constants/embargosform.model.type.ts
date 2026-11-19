import { DespachosJudiciales } from "../../../types/court.model.type";
import {
  BusquedaDesembargos,
  BusquedaEmbargos,
  ConceptoEmbargosDto,
  EmbargoBeneficiariosDto,
  EmbargoBeneficiariosInsertDto,
  EmbargoConceptoDto,
  EmbargoConceptoInsertDto,
  EmbargoDto,
  EmbargoInsertDto,
  EmbargoUpdateDto
} from "../../../types/embargo.model.type";

export const embargoForm: EmbargoDto = {
  id: '',
  radicadoWorkManager: '',
  fechaRadicadoWorkManager: new Date(),
  radicadoJuzgado: '',
  fechaRadicadoJuzgado: new Date(),
  emailRespuesta: '',
  nombreCliente: '',
  nombreDemandante: '',
  nombreDemandado: '',
  expedienteBancoAgrario: '',
  remanente: false,
  Inconsistencia: false,
  observaciones: '',
  aclaracion: '',
  valor: 0,
  clienteId: 0,
  identificacionCliente: '',
  clienteTipoIdentificacion: 1,
  clienteNombre: '',
  idDepartamento: '',
  idCiudad: '',
  juzgadoId: '',
  codigoInterno: null,
  codigoOficina: null,
  numeroCuenta: null,
  procesoId: '',
  tipoEmbargoId: '',
  juzgado: '',
  tipoEmbargo: '',
  tipoProceso: '',
  tipoIdentificacion: '',
  proceso: '',
  direccion: '',
  numeroSolicitud: 0
}

export const embargoInsertForm: EmbargoInsertDto = {
  radicadoWorkManager: '',
  fechaRadicadoWorkManager: new Date(),
  radicadoJuzgado: '',
  fechaRadicadoJuzgado: new Date(),
  emailRespuesta: '',
  direccionRespuesta: '',
  nombreDemandante: '',
  nombreDemandado: '',
  expedienteBancoAgrario: '',
  remanente: false,
  Inconsistencia: false,
  observaciones: '',
  respuesta: '',
  valor: 0,
  clienteIdentificacion: '',
  clienteTipoIdentificacion: 1,
  juzgadoId: '',
  procesoId: '',
  tipoEmbargoId: '',
  numeroSolicitud: 0
}

export const embargoUpdateForm: EmbargoUpdateDto = {
  id: '',
  radicadoWorkManager: '',
  fechaRadicadoWorkManager: new Date(),
  radicadoJuzgado: '',
  fechaRadicadoJuzgado: new Date(),
  emailRespuesta: '',
  direccionRespuesta: '',
  nombreDemandante: '',
  nombreDemandado: ' ',
  expedienteBancoAgrario: '',
  remanente: false,
  Inconsistencia: false,
  observaciones: '',
  respuesta: '',
  valor: 0,
  clienteTipoIdentificacion: 1,
  clienteIdentificacion: '',
  juzgadoId: '',
  procesoId: '',
  tipoEmbargoId: '',
  numeroSolicitud: 0
}

export const embargoConceptoForm: EmbargoConceptoDto = {
  id: '',
  valor: 0,
  fechaInicio: new Date(),
  fechaFin: new Date(),
  indicadorCesantias: false,
  tipoRetencionId: '',
  tipoEmbargoId: '',
  radicadoWorkManager: '',
  numeroCuenta: 0,
  cuentaId: 0,
  conceptoId: 0
}

export const embargoConcetoInsertDto: EmbargoConceptoInsertDto = {
  valor: 0,
  fechaInicio: new Date(),
  fechaFin: new Date(),
  indicadorCesantias: false,
  tipoRetencionId: '',
  tipoEmbargoId: '',
  radicadoWorkManager: '',
  numeroCuenta: 0
}

export const conceptoEmbargoForm: ConceptoEmbargosDto = {
  id: '',
  conceptoDescripcion: '',
  valor: null,
  fechaInicio: null,
  fechaFin: null,
  indicadorCesantias: false,
  tipoRetencionId: '',
  tipoEmbargoId: '',
  cuentaId: 0,
  conceptoId: 0,
  numeroCuenta: 0,
  saldo: 0,
  insert: true,
  tipoRetencion: null,
  tipoEmbargo: null
}

export const beneficiarioForm: EmbargoBeneficiariosDto = {
  id: '',
  radicadoWorkManager: '',
  tipoDocumentoId: 1,
  tipoIdentificacion: '',
  numeroIdentificacion: '',
  primerNombre: '',
  segundoNombre: '',
  primerApellido: '',
  segundoApellido: ''
}

export const beneficiarioInsertForm: EmbargoBeneficiariosInsertDto = {
  radicadoWorkManager: '',
  tipoDocumentoId: 1,
  numeroIdentificacion: '',
  primerNombre: '',
  segundoNombre: '',
  primerApellido: '',
  segundoApellido: ''
}

export const busquedaEmbargosForm: BusquedaEmbargos = {
  fechaInicial: null,
  fechaFinal: null,
  clienteTipoIndentificacion: 0,
  clienteIdentificacion: '',
  radicadoWorkManager: '',
  Estado: 1,
  Estados: 395,
}

export const busquedaDesembargoForm: BusquedaDesembargos = {
  fechaInicial: null,
  fechaFinal: null,
  clienteTipoIndentificacion: 0,
  clienteIdentificacion: '',
  radicadoWorkManager: '',
  Estado: 1,
  Estados: 396,

}

export const despachosJudicialesForm: DespachosJudiciales = {
  idDepartamento: null,
  idCiudad: null,
  codigo: '',
  CuentaDeposito: '',
  Estado: 1,
}


