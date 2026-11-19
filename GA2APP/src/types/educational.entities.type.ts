export interface EntidadesEducativasFiltroRequest {
    tipoIdentificacion: number;
    nit: string;
    razonSocial: string;
    renovarArchivo: number;
}

export const EntidadesEducativasFiltroRequestInitial: EntidadesEducativasFiltroRequest = {
    tipoIdentificacion: 0,
    nit: '',
    razonSocial: '',
    renovarArchivo: 0,
};

export interface EntidadesEducativasFiltro {
	isValid: any;
    idEntidad: string;
    tipoIdentificacion: number;
    nit: string;
    razonSocial: string;
    correoElectronico: string;
    ciudad: number;
    direccion: string;
    telefono: string;
    contacto: string;
    estado: number | boolean;
    nombreCiudad: string;
    nombreDepartamento: string;
    departamento: number;
    programas: ProgramasEducativos[];
    sedes: SedeEntidadEducativa[];
    esAcreditada: number;
    tipoPersona: number;
    fechaCreacion: string;
    fechaCreacionArchivo: string;
    bloqueo: boolean;
    solicitudesProcesadas?: boolean;
    modificadoPor?: string;
}

export const EntidadesEducativasFiltroInitial: EntidadesEducativasFiltro = {
    idEntidad: '',
    tipoIdentificacion: 0,
    nit: '',
    razonSocial: '',
    correoElectronico: '',
    ciudad: 0,
    direccion: '',
    telefono: '',
    contacto: '',
    estado: 0,
    nombreCiudad: '',
    nombreDepartamento: '',
    departamento: 0,
    programas: [],
    sedes: [],
    esAcreditada: 0,
    tipoPersona: 0,
    fechaCreacion: '',
    fechaCreacionArchivo: '',
    bloqueo: false,
    solicitudesProcesadas: false,
    isValid: undefined
};

export interface SedeEntidadEducativa {
    idSedeEntidad: string;
    idEntidad: string;
    nombreSede: string;
    ciudadId: string;
    estado: boolean;
    codigoCiudad?: number | null;
    nombreCiudad?: string | null;
    creadoPor?: string | null;
    modificadoPor?: string | null;
}

export const SedeEntidadEducativaInitial: SedeEntidadEducativa = {
    idSedeEntidad: '',
    idEntidad: '',
    nombreSede: '',
    ciudadId: '',
    codigoCiudad: 0,
    nombreCiudad: '',
    estado: false,
    creadoPor: '',
    modificadoPor: '',
};

export interface ProgramasEducativos {
    id?: string | null;
    idEntidad: string;
    idTipoPrograma: number;
    razonSocialEntidad?: string | null;
    idPrograma?: string | null;
    programa: string;
    nivelEducativoId: string;
    nivelEducativo?: string | null;
    creadoPor?: string | null;
    modificacionPor?: string | null;
    estado?: number | null
}

export const ProgramasEducativosInitial: ProgramasEducativos = {
    id: '',
    idEntidad: '',
    razonSocialEntidad: '',
    idPrograma: '',
    programa: '',
    nivelEducativoId: '',
    nivelEducativo: '',
    idTipoPrograma: 0,
    creadoPor: '',
    modificacionPor: '',
};

export interface ProgramaEducativo {
    id: string;
    programa: string;
    estado?: number | null;
    idEntidad: string;
    nivelEducativoId: string;
    idTipoPrograma: number;
    creadoPor?: string | null;
    fechaCreacion?: string | null;
    modificacionPor?: string | null;
    FechaModificacion?: string | null;
}

export interface BloqueosEntidadesEducativas {
    idEntidad: string;
    idCausalBloqueo: number;
    funcionarioBloqueo: string;
    idTipoOperacion?: number | null;
    idBloqueo?: string | null;
    fechaBloqueo?: Date | null;
    tipoOperacionId?: number | null;
    nombreOperacion?: string | null;
    causalOperacionId?: number | null;
    nombreCausal?: string | null;
    userName?: string | null;
    observaciones?: string | null;
}

export const BloqueosEntidadesEducativasInitial: BloqueosEntidadesEducativas = {
    idBloqueo: '',
    idEntidad: '',
    idTipoOperacion: 0,
    idCausalBloqueo: 0,
    fechaBloqueo: new Date,
    funcionarioBloqueo: '',
    observaciones: '',
};


export interface CuentaEntidadesEducativas {
    idCuentaBancaria?: string
    idEntidad?: string;
    idEntidadBancaria: string;
    razonSocialEntidad?: string;
    idTipoCuenta: number;
    nombreTipoCuenta?: string;
    numeroCuenta: string;
    estado?: boolean;
    tipoDocumento?: string;
    enE_FIDEICOMISO_ID?: string;
}
export const CuentaEntidadesEducativasInitial: CuentaEntidadesEducativas = {
    idEntidad: '',
    idEntidadBancaria: '',
    idTipoCuenta: 0,
    numeroCuenta: '',
    estado: false,
    enE_FIDEICOMISO_ID: '',
}
export interface Fidecomiso {
    idTipoIdentificacion?: number
    nroIdentificacion?: string
    nombreRazonSocial?: string
    fecCreacion?: Date
    entidad_id?: string,
    ENE_FIDEICOMISO_ID?: string | null,
    usr_id?: string  | null

}
export const FidecomisoInitial: Fidecomiso = {
    idTipoIdentificacion: 0,
    nroIdentificacion: '',
    nombreRazonSocial: '',
    fecCreacion: new Date,
    ENE_FIDEICOMISO_ID: ''
}

export interface EntidadesEducativasAgregar {
    tipoIdentificacion: number;
    nit: string;
    razonSocial: string;
    correoElectronico: string;
    ciudad: number;
    departamento: number;
    direccion: string;
    telefono: string;
    contacto: string;
    esAcreditada: number;
    tipoPersona?: number;
    estado?: boolean;
}

export const EntidadesEducativasAgregarInitial: EntidadesEducativasAgregar = {
    tipoIdentificacion: 0,
    nit: '',
    razonSocial: '',
    correoElectronico: '',
    departamento: 0,
    ciudad: 0,
    direccion: '',
    telefono: '',
    contacto: '',
    esAcreditada: 0,
    tipoPersona: 0,
    estado: true,
};

export interface ArchivosEntidadesEducativas {
    idArchivoParametrizado: string;
    nombreArchivo: string;
    descripcionArchivo: string;
    esRequerido: boolean;
    orden: number;
    esMultiple: boolean;
    nuevoCargue: boolean;
    formato: string;
    estadoParametrizacion: boolean;
    tipoPersonaId: number;
    tipoPersona: string;
    idArchivoCargado: string;
    nombreArchivoCargado: string | null;
    rutaStorage: string;
    extension: string;
    numeroFolio: string | null;
    fechaCargue: string | null;
    numeroArchivos: string | null;
    archivosCargados: FileList | null;
}

export const ArchivosEntidadesEducativasInitial: ArchivosEntidadesEducativas = {
    idArchivoParametrizado: '',
    nombreArchivo: '',
    descripcionArchivo: '',
    esRequerido: true,
    orden: 0,
    esMultiple: false,
    formato: '',
    estadoParametrizacion: true,
    tipoPersonaId: 0,
    tipoPersona: '',
    idArchivoCargado: '',
    nombreArchivoCargado: null,
    rutaStorage: '',
    extension: '',
    numeroFolio: '',
    fechaCargue: null,
    numeroArchivos: '',
    archivosCargados: null,
    nuevoCargue: false
};
export interface TramiteEntity {
    tipoDocumento: string
    numeroDocumento: string
    nombre: string,
    fechaCreacion: Date,
    programa: string,
    nivel: string,
    subModalidad: string,
    modalidad: string,
    radicado: string
    cedula: string;
    estado: string;
}
export const TramiteEntity: TramiteEntity = {
    tipoDocumento: '',
    numeroDocumento: '',
    nombre: '',
    fechaCreacion: new Date,
    programa: '',
    nivel: '',
    subModalidad: '',
    modalidad: '',
    radicado: '',
    cedula: '',
    estado: '',
}

export interface HistorialEntity {

    idEntidadCambio: any,
    idEntidad: string,
    pestaña: string,
    campo: string,
    vlrAnterior: string,
    vlrNuevo: string,
    idUsuario: string,
    fechaModificacion: Date
}

export const HistorialEntityInitial: HistorialEntity = {
    idEntidadCambio: '',
    idEntidad: '',
    pestaña: '',
    campo: '',
    vlrAnterior: '',
    vlrNuevo: '',
    idUsuario: '',
    fechaModificacion: new Date,
}

export interface CreateAnotacion {

    idEntidad: string,
    anotacion: string,
    creadoPor: string,
    fechaCreacion: Date

}

export const CreateAnotacion: CreateAnotacion = {
    idEntidad: '',
    anotacion: '',
    creadoPor: '',
    fechaCreacion: new Date
}

export interface HistorialCambios {

    pestania: string,
    campo: string,
    valorAnterior: string,
    valorActualizacion: string,
    usuario: string,
    fechaModificacion: string

}

export const HistorialCambios: HistorialCambios = {
    pestania: '',
    campo:  '',
    valorAnterior:  '',
    valorActualizacion:  '',
    usuario:  '',
    fechaModificacion:  ''
}


