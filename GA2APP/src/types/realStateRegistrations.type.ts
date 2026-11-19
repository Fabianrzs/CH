import { StringifyOptions } from "querystring";

export interface MatriculasInmobiliariasDTO {
    maiId: string;
    maiNumeroMatricula: string;
    maiDireccion: string;
    maiFechaRegistro: Date;
    prpTipoIdentificacion: string;
    prpNumeroIdentificacion: string;
    prpNombreRazonSocial: string;
    estado: string;
    dcpId: number;
    dpdId: number; 
    idOficinaRegistro: number; 
    propietarios:HistorialPropietariosDTO[];
    novedades: HistorialNovedadesMatriculasInmobiliarias [];
    causales : CausalMatriculaInmobiliariaDto[] | null;
    tramites: HistorialTramites [];
    cambios: HistorialCambios [];
    prpPrimerNombre: string;
    prpSegundoNombre: string;
    prpPrimerApellido: string;
    prpSegundoApellido: string;
    nroCelular: string;
    tipoIdentificacion: number;
    mail: string;
    codigoOficina: string;
    nombreUsuario: string;
    celular: string;
    correo: string;
    idUsuario?: string;
} 

export const MatriculasInmobiliariasInitial: MatriculasInmobiliariasDTO = {
    maiId: '',
    maiNumeroMatricula: '',
    maiDireccion: '',
    maiFechaRegistro: new Date(),
    prpTipoIdentificacion: '',
    prpNumeroIdentificacion: '',
    prpNombreRazonSocial: '',
    dcpId: 0,
    dpdId: 0,
    idOficinaRegistro: 0,
    propietarios: [],
    novedades: [],
    tramites:[],
    causales:[],
    estado: '',
    prpPrimerNombre: '',
    prpSegundoNombre: '',
    prpPrimerApellido: '',
    prpSegundoApellido:'',
    nroCelular:'',
    tipoIdentificacion:0,
    mail:'',
    codigoOficina: '',
    cambios: [],
    nombreUsuario: '',
    celular: '',
    correo: ''
};
export interface requestExcel{
    columnDataList: columnDataList[];

}
export interface columnDataList{
    ColumnName: string;
    Data:string[];
}
export interface HistorialCambios {
    
    matcId: string;
    maiId:string;
    matcPestaña:string;
    matcCampo:string;
    matcVlrAnterior:string;
    matcVlrNuevo:string;
    match_user:string;
    matc_fecmood:string;
    usuarioBloqueado: string;
}
export interface MatriculasInmobiliariasFilterDTO {
    numeroMatricula: string;
    numeroIdentificacion: string;
    tipoIdentificacion: number;
    fechaCreacion: null;
    fechaFin: null;
    bloqueo:number;
    nomOfc:string;
}

export interface HistorialPropietariosDTO  {
    maiId: string;
    prpId: string;
    prpTipoIdentificacion: string;
    prpIdTipoIdentificacion: number;
    prpNumeroIdentificacion: string;
    prpNombreRazonSocial: string;
    prpPrimerNombre: string;
    prSegundoNombre: string;
    prpPrimerApellido: string;
    prpSegundoApellido: string;
    numeroMatricula: string;
    correo: string;
    telefono: string;
    nombreUsuario: string;
    pmaFecCre : string;
}
export interface HistorialTramites {
    tipoDoc: string;
    nroDoc: number;
    nombreRazonSocial: string;
    nroRadicado: number;
    tipoSolicitud: string;
    modalidad:string;
    fecSolicitud: Date;
    valorSolicitud: number;
    estado: string;

}
export const HistorialPropietariosDTOsInitial: HistorialPropietariosDTO = {
    maiId: '',
    prpId: '',
    prpIdTipoIdentificacion: 0,
    prpNumeroIdentificacion: '',
    prpNombreRazonSocial: '',
    prpPrimerNombre: '',
    prSegundoNombre: '',
    prpPrimerApellido:'',
    prpSegundoApellido:'',
    prpTipoIdentificacion: '',
    numeroMatricula: '',
    correo: '',
    telefono: '',
    nombreUsuario: '',
    pmaFecCre: ''
};

export interface HistorialNovedadesMatriculasInmobiliarias {
    idTipoOperacion: string;
    tipoOperacion: string;
    numeroMatricula: string;
    fechaNovedad: Date;
    tipoSolicitud: string;
    tipoSolicitudId: string;
    causalId: number;
    causal: string;
    observaciones: string;
    fechaRegistro: Date;
    createPor: string;
    createPorId: string;
    descripcionTipoSolicitud: string;
    idNovedad: string;
    idMatricula: string;
    IdTipoSolicitud: number;
    ultimoRegistro: string;
    nombreUsuario: string;
    TipoOperacionId: number;
}

export interface HistorialPropietariosDeleteDTO {
    maiId: string;
}

export interface NovedadesBloqueosDeleteDTO {
    idTipoOperacion: string;
}

export interface NovedadesInsertarDTO {
    id: string;
    tipoOperacionId: string;
    matriculaId: string;
    fechaNovedad: Date;
    tipoSolicitudId: number;
    causalId: number;
    solicitudId: string;
    fechaOperacion: Date;
    observaciones: string;
    userCreadorId: string;
}

export interface CausalMatriculaInmobiliariaDto {
    idCausal: number;
    nombreCausal: string;
    tipoCausal: number;
}
export const CausalMatriculaInmobiliariaDTOInitial: CausalMatriculaInmobiliariaDto = {
    idCausal:0,
    nombreCausal:'',
    tipoCausal:0
}
export interface OperacionesMatriculasDTO {
    id: string;
    nombre: string;
    descripcion: string;
    estado: boolean;
}

export const OperacionesMatriculasInitial: OperacionesMatriculasDTO = {
    id: '',
    nombre: '',
    descripcion: '',
    estado: false,
};

export interface AgregarMatriculaDTO {
    numeroMatricula: string,
    fechaRegistro: Date,
    departamento: number,
    ciudad: number,
    direccion: string,
    tipoIdentificacion: number,
    numeroIdentificacion: string,
    razonSocial: string,
    primerNombre: string,
    segundoNombre: string,
    primerApellido: string,
    segundoApellido: string,
    correo: string,
    celular: string,
    idOficinaRegistro: number
    codigoOficina: string
}

export const AgregarMatriculaInitial: AgregarMatriculaDTO = {
    fechaRegistro: new Date(),
    departamento: 0,
    cuidad: 0,
    direccion: '',
    tipoIdentificacion: 0,
    numeroIdentificacion: '',
    razonSocial: '',
    correo: '',
    celular: '',
    idOficinaRegistro: 0,
    numeroMatricula: '',
    primerNombre: "",
    segundoNombre: "",
    primerApellido: "",
    segundoApellido: "",
    codigoOficina: ""
};

export interface CausalesByidTypeDTO {
    id: string;
    nombre: string;
    descripcion: string;
    estado: boolean;
}

export const CausalesByIdTypeInitial: CausalesByidTypeDTO = {
    id: '',
    nombre: '',
    descripcion: '',
    estado: false,
};

export interface NovedadMatriculaDto {
    matriculaId: string;
    causalId: string;
    tipoOperacionId: string;
}

export const NovedadMatriculaForm: NovedadMatriculaDto = {
    matriculaId: '',
    causalId: '',
    tipoOperacionId: '',
};