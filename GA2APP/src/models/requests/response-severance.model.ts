import { NovedadMatriculaDto } from "../../types/realStateRegistrations.type";
import { MatriculaInmobiliariaModel } from "../matricula-inmobiliaria.model";
import { PropietarioMatriculaModel } from "../propietario-matricula.model";
import { ConyugueModel } from "../conyuge-solicitudes.model";
import { TerceroApoderadoModel } from "../tercero-apoderado.model";
import { TerceroBeneficiarioEstudioEntidadEducativaModel } from "../tercero-beneficiario-estudio-entidad-educativa.model";
import { TerceroBeneficiarioEstudioModel } from "../tercero-beneficiario-estudio.model";
import { BeneficiarioModel, TerceroBeneficiarioModel } from "../tercero-beneficiario.model";
import { TerceroEntidadEducativaModel } from "../tercero-entidad-educativa.model";
import { TerceroEntidadSeguroEducativaModel } from "../tercero-entidad-seguro-educativa.model";
import { TerceroTenedorAccionesModel } from "../tercero-tenedor-acciones.model";
import { TerceroVendedorModel } from "../tercero-vendedor.model";
import { InformacionTurnosModel } from "../Informacion-turno.model";
import { InformacionAdjudicacionModel } from "../Informacion-adjudicacion.model";
import { InformacionEscrituracionModel } from "../Informacion-escrituracion.model";
import { SolicitudMisCuentasDto } from "./solicitud-tarea.model";

export interface ResponseSeveranceModel {
    solicitudId: string;
    solicitudTareaId: string;
    ultimoSolicitudTareaId: string;
    cuentaId: number;
    procesoId: string;
    tipoSolicitudId: number;
    clienteId: number;
    numeroSolicitud: any;
    fechaSolicitud: Date;
    direccionCliente: string;
    telefonoCliente: string;
    celularCliente: string;
    correoCliente: string;
    observacionSolicitud: string;
    ObservacionesSolicitudUltimo?: string;
    estadoSolicitud: boolean;
    descripcionModalidad: string;
    descripcionSubModalidad: string;
    descripcionProceso: string;
    nombresCliente: string;
    apellidosCliente: string;
    identificacionCliente: string;
    tipoIdentificacion: string;
    valorRetirar: string;
    valorTotal: string;
    valorAhorro: string;
    pestanas: string;
    tipoModalidadId: string;
    tipoSubModalidadId: string;
    nombreTarea: string;
    timCodigo:string | undefined;
    saldoCuenta: number | undefined;
    numeroRadicadoSG: string | undefined;
    solicitudEncontrada: boolean;
    idConyuge: number | undefined;
    idTipoIdentificacionConyuge: number | undefined;
    identificacionConyuge: string | undefined;
    primerNombreConyuge: string | undefined;
    segundoNombreConyuge: string | undefined;
    primerApellidoConyuge: string | undefined;
    segundoApellidoConyuge: string | undefined;
    conyugeActivo: boolean | undefined;
    solicitudTareaEstadoId: number | undefined;
    conyugueDto?: ConyugueModel[];
    solicitudMatriculaDto: MatriculaInmobiliariaModel[];
    novedadMatriculaDto: NovedadMatriculaDto[];
    solicitudPropietarioDto: PropietarioMatriculaModel[];
    solicitudTerceroVendedorDto: TerceroVendedorModel[];
    solicitudTerceroBeneficiarioDto: TerceroBeneficiarioModel[];
    solicitudTerceroApoderadoDto: TerceroApoderadoModel[];
    solicitudBeneficiarioDto?: BeneficiarioModel[];
    solicitudTerceroEntidadEducativaDto: TerceroEntidadEducativaModel;
    solicitudTerceroBeneficiarioEstudioDto: TerceroBeneficiarioEstudioModel[];
    solicitudTerceroBeneficiarioEstudioEntidadEducativaDto: TerceroBeneficiarioEstudioEntidadEducativaModel[];
    solicitudTerceroEntidadSeguroEducativoDto: TerceroEntidadSeguroEducativaModel;
    solicitudTerceroTenedorAccionesDto: TerceroTenedorAccionesModel;
    solicitudMiscuentasDto?: SolicitudMisCuentasDto; 
    informacionTurnoDto: InformacionTurnosModel;
    informacionAdjudicacionDto: InformacionAdjudicacionModel;
    informacionEscrituracionDto: InformacionEscrituracionModel;
    numfolio:any;
    embargo?:string;
    idProcesoSolicitud?:string;
    posee_Avac?: boolean;
    secciones?:string;
    numero_Folio?:number;
    soL_RECHAZADA?:string;
    fechaFirmaEscritura?:Date;
    observacion: string;
    cumpleRequisito: string;
    observaciones: string;

}

export const TerceroEntidadEducativaModelForm: TerceroEntidadEducativaModel = {
    nit: '',
    razonSocial: '',
    idEntidad: '',
    idProgramaEntidad: '',
    idNivel: '',
    reciboPago: '',
    fechalimite: '',
    razonsocial: '',
    valor_recibo: 0,
};

export const TerceroEntidadSeguroEducativaModelForm: TerceroEntidadSeguroEducativaModel = {
    tipoDocId: '',
    numDoc: '',
    razonSocialAseguradora: '',
    entidadSeguroEducativoId: '',
    numeroPoliza: '',
};

export const TerceroTenedorAccionesModelForm: TerceroTenedorAccionesModel = {
    tipoDoc: '',
    numDoc: '',
    razonSocial: '',
    direccion: '',
    departamento: '',
    ciudad: '',
    correo: '',
    telefono: '',
    emisor: '',
};

export const InformacionTurnoDtoForm: InformacionTurnosModel = {
    tipoRecepcion: '',
    radPreTramite: '',
    numeroComision: '',
    tipoTurno: '',
    turno: '',
    tipoSucursal: '',
    fechaComision: new Date(),
};

export const InformacionAdjudicacionDtoForm: InformacionAdjudicacionModel = {
    tipoVivienda: '',
    direccion: '',
    SIA_CIUDAD: '',
    matriculaInmobiliaria: '',
    valorSubsidio: '',
};

export const InformacionEscrituracionDtoForm: InformacionEscrituracionModel = {
    numeroEscritura: '',
    notaria: '',
    fechaEscrituracion: new Date(),
};

export const ResponseSeveranceModelForm: ResponseSeveranceModel = {
    solicitudId: '',
    solicitudTareaId: '',
    ultimoSolicitudTareaId: '',
    cuentaId: 0,
    procesoId: '',
    numeroSolicitud: 0,
    tipoSolicitudId: 0,
    clienteId: 0,
    fechaSolicitud: new Date(),
    direccionCliente: '',
    telefonoCliente: '',
    celularCliente: '',
    correoCliente: '',
    observacionSolicitud: '',
    estadoSolicitud: false,
    descripcionModalidad: '',
    descripcionSubModalidad: '',
    descripcionProceso: '',
    nombresCliente: '',
    apellidosCliente: '',
    identificacionCliente: '',
    tipoIdentificacion: '',
    valorRetirar: '',
    valorTotal: '',
    valorAhorro: '',
    pestanas: '',
    tipoModalidadId: '',
    tipoSubModalidadId: '',
    nombreTarea: '',
    timCodigo: '',
    saldoCuenta: 0,
    numeroRadicadoSG: '',
    solicitudEncontrada: false,
    idConyuge: 0,
    idTipoIdentificacionConyuge: 0,
    solicitudTareaEstadoId: 0,
    identificacionConyuge: '',
    primerNombreConyuge: '',
    segundoNombreConyuge: '',
    primerApellidoConyuge: '',
    segundoApellidoConyuge: '',
    conyugeActivo: false,
    conyugueDto: [],
    solicitudMatriculaDto: [],
    novedadMatriculaDto: [],
    solicitudPropietarioDto: [],
    solicitudTerceroVendedorDto: [],
    solicitudTerceroBeneficiarioDto: [],
    solicitudTerceroApoderadoDto: [],
    solicitudTerceroEntidadEducativaDto: TerceroEntidadEducativaModelForm,
    solicitudTerceroBeneficiarioEstudioDto: [],
    solicitudTerceroBeneficiarioEstudioEntidadEducativaDto: [],
    solicitudTerceroEntidadSeguroEducativoDto: TerceroEntidadSeguroEducativaModelForm,
    solicitudTerceroTenedorAccionesDto: TerceroTenedorAccionesModelForm,
    informacionTurnoDto: InformacionTurnoDtoForm,
    informacionAdjudicacionDto: InformacionAdjudicacionDtoForm,
    informacionEscrituracionDto: InformacionEscrituracionDtoForm,
    numfolio: 0,
    fechaFirmaEscritura: new Date(),
    observacion: '',
    cumpleRequisito: ""
};




