

import { List } from "lodash";

export interface SolicitudTareaModel {
	solicitudId: string;
	solicitudTareaId: string;
	creadoPor: string;
	valorRetirar: string;
	observacion?: string | null;
	conyugueDto: ConyugueDto[];
	solicitudMatriculaDto: SolicitudMatriculaDto[];
	solicitudPropietarioDto: SolicitudPropietarioDto[];
	solicitudTerceroApoderadoDto: SolicitudTerceroDto[];
	solicitudTerceroBeneficiarioDto: SolicitudTerceroBeneficiarioDto[];
	solicitudTerceroVendedorDto: SolicitudTerceroVendedorDto[];
	solicitudTerceroConstructorDto: SolicitudTerceroDto | null;
	solicitudTerceroAutorizadoDto: SolicitudTerceroDto | null;
	solicitudTerceroEntidadEducativaDto: SolicitudTerceroEntidadEducativaDto[];
	solicitudTerceroEntidadSeguroEducativoDto: SolicitudTerceroEntidadSeguroEducativoDto | null;
	solicitudTerceroBeneficiarioEstudioDto: SolicitudTerceroBeneficiarioEstudioDto[] | any[];
	// solicitudBeneficiariosDto?: SolicitudBeneficiarioDto[];
	solicitudTerceroTenedorAccionesDto: SolicitudTerceroTenedorAccionesDto | null;
	solicitudMiscuentasDto?: SolicitudMisCuentasDto | null;
	informacionTurnoDto: InformacionTurnoDto | null;
	informacionAdjudicacionDto?: informacionAdjudicacionDto | null;
	informacionEscrituracionDto?: informacionEscrituracionDto | null;
	posee_Avac?:boolean;
	tipoDocObj?:any
	
}

export interface SolicitudBeneficiarioDto {
	tipoDoc: string;
	numDoc: string;
	nombres: string;
	apellidos: string;
	porcentaje: number;
	parentesco: string; // string o number
}

export interface informacionEscrituracionDto {   
	numeroEscritura: string; 
	notaria: string; 
	fechaEscrituracion: Date;
	}

export interface informacionAdjudicacionDto {
	tipoVivienda: string;
	direccion: string;
	SIA_CIUDAD: string;
	matriculaInmobiliaria: string;
	valorSubsidio: string;
  }

export interface InformacionTurnoDto {
	tipoRecepcion: string; 
	radPreTramite: string; 
	numeroComision: string; 
	tipoTurno: string; 
	turno: string;
	tipoSucursal: string;
	fechaComision: Date;
  }

export interface ConyugueDto {
	ClientId: number;
	TipoDoc: number;
	Identificacion: string;
	PrimerNombre: string;
	SegundoNombre: string;
	PrimerApellido: string;
	SegundoApellido: string;
}

export interface SolicitudMatriculaDto {
	numeroMatricula: string;
	direccion: string;
	ciudadId: number;
	esPrincipal: boolean;
	oficinaCodigo?: string;
	
}

export interface SolicitudPropietarioDto {
	matricula: Matricula[];
	numeroIdentificacion: string;
	tipoIdentificacion: TipoIdentificacion;
	razonSocial: string;
}

export interface Matricula {
	id: string;
	numeroMatricula: string;
	estado: string;
}

export interface TipoIdentificacion {
	id: number;
	descripcion: string;
}

export interface SolicitudTerceroDto {
	tipoDoc: number;
	numDoc: string;
	razonSocial: string;
	numeroLicencia?: string;
	tipoDocObj?:any
	primerNombre?:any
}

export interface SolicitudTerceroBeneficiarioDto {
	tipoDoc: number;
	numDoc: string;
	razonsocial: string;
	entidad: string | null;
	tipoCuenta: number;
	numeroCuenta: string;
	valorRetirarDos?: string;
}

export interface SolicitudTerceroVendedorDto {
	tipoDoc: number;
	numDoc: string;
	razonSocial: string;
	direccion: string;
	ciudad: number;
	correo: string;
	telefono: string;
}

export interface SolicitudTerceroEntidadEducativaDto {
	nit: string;
	razonSocial: string;
	idEntidad: string;
	idNivel: string;
	reciboPago: string;
	fechalimite: string;
}

export interface SolicitudTerceroEntidadSeguroEducativoDto {
	tipoDocId: string;
	numDoc: string;
	razonSocialAseguradora: string;
	entidadSeguroEducativoId: string;
	numeroPoliza: string;
}

// export interface SolicitudTerceroBeneficiarioEstudioDto {
// 	numDocEstudio: string;
// 	parentescoEstudio: string;
// 	razonsocialEstudio: string;
// 	tipoDocEstudio: string;
// }

export interface SolicitudTerceroBeneficiarioEstudioDto {
	numDocEstudio: string;
	// parentescoEstudio: string;
	razonsocialEstudio: string;
	tipoDocEstudio: string; 
	tipoDocEstudioObj: {
	  id: string;
	  descripcion: string;
	};
	
  }

  
export interface SolicitudTerceroTenedorAccionesDto {
	tipoDoc: string;
	numDoc: string;
	razonSocial: string;
	direccion: string;
	ciudad: string;
	correo: string;
	telefono: string;
	emisor: string;
	razonsocial?: string;
}

export interface SolicitudDto {
	numeroSolicitud: number;
	procesoDescripcion: string;
	idTipoIdentificacion: number;
	cedulaCliente: string;
	nombreCliente: string;
	fechaSolicitud: Date;
	valorRetirar: number;
	tareaNombre: string;
	estadoSolicitud: string;
	idCliente: number;
	timCodigo: string;
	tpsSubId: string;
	tpsSubDescripcion: string;
	radicadoGestionDocumental: string;
	solicitudTareaId: string;
	fechaModalidadInicio?: Date;
	fechaModalidadFin?: Date;
    modalidad?: string ;
    estadoTarea?: number;
    tipoSolicitud?: string;
    numRadicado?:string;
	tipoSolicitudNombre?: string;
	tipoModalidadNombre?: string;
	tipoSubModalidadNombre?: string;
	numeroDocumeto?: number;
	fechaCreacion?: Date;
	actividad?: string;
	estadoProceso?: string;
	nombreFirma?: string;
	apellidoFirma?: string;
	nombreCompleto?: string;
	tipoModalidad?: number;
	tipoSubModalidad?: number;
	idTipoSolicitud?: number;
	idModalidad?: number;
	idSubModalidad?: number;
	tipoDocumentoId?: number;
	nombreCompletoAfiliado?: string;
	userName?: string;
	userId?: string;
	noRadicado?: string;
	usuarioAsignado?: string;
	idUsuarioAsignado?: string;
	numeroSolictud?:number;
}

export interface ParametrosBusqueda {
	identificacion: string;
	estadoTarea: number;
	tipoSolicitud: string | null;
	numRadicado:string|null;
	fechaIni?: string | null;
	fechaFin?: string | null;
    modalidad?: string | null;
	submodalidad?: string | null;
	idUsuario?:string;
	numeroSolictud?:number;
 
}

export interface FormularioInconsistenciasDto {
    grupoInconsistencia: number;

    novedad: string;

    listaInconsistenciaId: string;
    causalInterrupcionId: string | null;

    herramientaId: string | null;
    puntoAtencionId: string | null;

    solicitudId: string;
    solicitudTareaId?: string;

    tecnicoId: string;

    objetoEstudio: string | null;
    analisis: string | null;
    observacion: string | null;

    index?: number;

    inconsistenciaDescripcion: string | null;
    inconsistenciaId: string | null;

    formularioInconsistenciaId?: string;
    id?: string;

    grupoVinculoNovedadId?: string;
    idGrupoNovedad?: string;
    novedadVinculoId?: string;

    subsanado: boolean;


    fecha_Novedad?: string;
    fechaNovedad?: string;
    fechaCreacionNovedad?: string;
    fecha_Creacion?: string;
    fechaCreacion?: string;
    fecha_Subsanacion?: string;
    fechaRegistro?: string;

    informe?: string;
    pagoSucesivo?: string;

    selected?: boolean;
    disabled?: boolean;
    otroCampo?: any;

    tpO_NOV_DESCRIPCION?: string;
    caU_DESCRIPCION?: string;
}

export interface ComunicacionSolicitud {
    idComunicacion: string | null;
    idSolicitud: string | null;
    idCausalComunicacion: number;
    textoIncluir: string;
    firmaDigital: boolean;
	radicado: string;
	radicadoRelacionado?: string;
	idComunicacion2?: string | null;
	idCausalComunicacion2?: number;
	textoIncluir2?: string;
	radicado2?: string;
	radicadoRelacionado2?: string;
}

export interface FormulariosSolicitudDto {
	id: number;
	titulo: string;
	subtitulo: string;
	icono: string;
	botonera: boolean;
	formularios: List<string>;
}

export interface SolicitudMisCuentasDto {
	saldoCuenta: number;
	cuenta: string;
	trasladoAutorizado: boolean;
}