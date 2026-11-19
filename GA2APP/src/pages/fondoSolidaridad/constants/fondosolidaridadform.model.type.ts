export interface Postulante {
  id?: string;
  nombre: string;
  apellido: string;
  tipoDocumentoId: number;
  numeroDocumento: number;
  departamentoExpedicion?: number | null;
  ciudadExpedicion?: number | null;
  parentesco: string;
  sexo?: string | null;
  fechaNacimiento?: string | null;
  estadoCivil?: string | null;
  ocupacion?: number | null;
  nivelEducativo?: number | null;
  afectionFisica?: string | null;
  afectacionSensorial?: string | null;
  enfermedadEspecifica?: string | null;
  tratamientoMedico?: string | null;
  dificultadEscaleras?: string | null;
  medicoPermanente?: string | null;
  ayudaTecnica?: string | null;
  tipAyudaTecnicaId?: string | null;
  condicionSalud?: string | null;
  porcentaje?: number;
  tipoIntegranteId?: string | null;
  tipoParentescoId?: string | null;
  tipoDiscapacidadId?: string | null;
  porcentajePostulacion?: number;
  porcentajeAportes?: number;
  porcentajeCesantias?: number;
  tieneCuenta?: string;
}

export interface DocumentosPostulacionModel {
  id: number;
  idModalidad: number;
  nombre: string;
  requerido: number;
  numeroFolio: string;
  esMultipleArchivo: boolean;
  orden: number;
  formatoArchivo: string;
  numeroArchivos?: string;
  archivoCargado: number;
  archivosCargados: FileList;
  errorArchivoCargado: boolean;
}

export interface DocumentosPorSolicitudPostulacionModel {
  id: string;
  nombreArchivoCargado: string;
  rutaArchivo: string;
  extension: string;
  nombreArchivo: string;
  orden: number;
}

export interface Constructora {
id?: string;
id_Constructuora?: string;
nombreConstructora: string;
nit?: number;
direccion: string;
telefono?: number;
idDepartamento: number;
departamento?: string;
idCiudad: number;
ciudad?: string;
email: string;
}

export interface Supervisor {
codigo?: number,
tipoDocumento: number,
numeroDocumento:number,
nombresApellidos: string,
cargo: string,
email: string,
telefono: number,
}

export interface BeneficiarioPago {
codigo?: number,
tipoDocumento: number,
numeroDocumento: number,
digitoVerificacion: number,
nombreRazonSocial: string,
entidad: string,
tipoCuenta: string,
numeroCuenta: number,
direccion: string,
actividadEconomica: string,
}

export interface Poliza {
concepto: string,
radicado: number,
fechaInicioPoliza: string,
fechaVigencia: string,
numeroPoliza: number,
aseguradora: string,
valorPoliza: number,
porcentajePoliza: number,
ajustador: string,
descripcionPoliza: string,
}

export interface Inmueble {
id: string,
nombreTipoInmueble?: string,
tipoInmueble: string,
valor: number,
areaPrivada: number,
areaConstruida: number,
nombreVivienda?: string, 
tipoVivienda: string,
claseVivienda: string,
cantidad: number,
descripcion: string,
}

export interface Supervision {
  id?: string,
  personaAtendioVisita: string,
  cargo: string,
  supervisorTecnico: number,
  nombreSupervisorTecnico: string,
  fechaVisita: string,
  desarrolloProyecto: string, 
  observacionesGenerales: string,
}

export interface ClaseInmueble {
id: string,
claseInmueble: string,
numeroInmueble: number,
direccionInmueble: string,
}

export interface ItemAvance {
id: string,
itemAvance: string,
porcentaje: number,
}

export interface TerceroApoderadoPostulacion {
id: string,
tipoDocumentoId: number,
numeroDocumento: string,
parentesco: number,
esAutorizado: boolean,
primerNombre: string,
segundoNombre: string,
primerApellido: string,
segundoApellido: string,
}

export interface TerceroBeneficiarioPostulacion {
id: string,
beneficiario: string,
numeroDocumento: string,
nombre: string,
tipoCuenta: number,
entidad: string,
numeroCuenta: number,
confirmacionNumeroCuenta: number,
valorRetirar: number,
}