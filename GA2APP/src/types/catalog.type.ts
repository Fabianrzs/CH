export interface IDENTIFICATIONTYPEDTO {
    id: number;
    codigo: string;
    descripcion: string;
    esEmpresarial: boolean;
    esActivo: boolean;
    ERP: number;
    vigia: number;
    embargo: number;
}

export interface GRADEDTO {
    id: number;
    descripcion: string;
    esEspecial: string;
    codigo: string;
    sigla: string;
    esCivil: boolean;
    esActivo: boolean;
    categoria: number;
    fuerza: number
}
export interface LINEDTO {
    idLineaCredito: number;
    descripcion: string;
    tipoCredito: number;
    tpa: number;
    cuotaMinima: number;
    cuotaMaxima: number;
    fechaCreacion: Date;
}

export interface PAYDTO {

    id: number;
    descripcion: string;
    nombre: string;
    codigo: string;
    estado: boolean;

}

export interface FUERZADTO {
    id: number;
    descripcion: string;
    codigo: string;
}

export interface CATEGORIADTO {
    id: number;
    descripcion: string;
}

export interface CITYDTO {
    id: number;
    idDepartamento: number;
    codigo: number;
    nombre: string;
    codigoCirRegistral: string;
    vis_NoVis: boolean;

}

export interface MOVIMIENTOS {

    movimiento_Comprobante_CarteraId: number,
    comprobante_CarteraId: number,
    cuenta_Contable: number,
    descripcion_Cuenta: string,
    centro_Costo: number,
    detalle: number,
    tipo: string,
    monto: number

}

export interface CAUSALBLOQUEOCUENTA {
    id: number;
    nombre: string;
    descripcion: string;
    estado: boolean;
}

export interface CAUSALESTADOCUENTA {
    causalEstadoId: number;
    estadoCuentaId: number;
    causalCuentaDescripcion: string;
    causalActiva: boolean;
}


export interface TIPODECUENTAC {
    idTipoCuentaC: number;
    descripcionTipoCuentaC: string;
    prefijoCuentaC: string;
    tipoGeneralCuentaC: string;
    aplicanInteresesCuentaC: boolean;
    aplicanRendimientosCuentaC: boolean;
    aplicanCuotasCuentaC: boolean;
    activaCuentaC: boolean;
    centroCostoCuenta: string;
}

export interface ESTADOCUENTA {
    idEstado: number;
    estado: string;
}

export interface MODALIDADAFILIACION {
    idTipoAfiliacion: number;
    descripcionTipoAfiliacion: string;
}

export interface PORCENTAJEDESCUENTO {
    idPorcentajeDescuento: number;
    descripcionPorcentajeDescuento: string;
    productoDescuento: string;
}



export interface EXECUTINGUNITDTO {
    id: number;
    identificacion: string;
    nombre: string;
    sigla: string;
    fecha: string;
    codigo: string;
    tasaAporte: string;
    areaNegocio: string;
    esAval: boolean;
    fuerza: number;
    idUnidadPrincipal?: number | null;
}

export interface TYPESFILEDTO {
    id: number;
    tipo: string;
    clasificacion: string;
    clasificacion2: string;
    descripcion: string;
}

export interface STATEFILEDTO {
    id: string;
    descripcion: string;
    tipo: string;
    estado: string;
    concepto: string;
}

export interface LIVINGPLACEDTO {
    id: string;
    descripcion: string;
    esPromocionada: boolean;
    esActiva: boolean;
}

export interface FORCEDTO {
    id: number;
    codigo: string;
    descripcion: string;
    digito?: string;
    esSoldado: boolean;
}

export interface SEXODTO {
    id: number;
    idCatalogo: number;
    codigo: string;
    descripcion: string;
    estado: boolean;
}

export interface ESTADOCIVILDTO {
    id: string;
    descripcion: string;
}
export interface DEPARTAMENTODTO {
    id: number;
    idPais: number;
    codigo: number;
    nombre: string;
    idOficina: string;
}
export interface PAISDTO {
    id: number;
    codigo: number;
    nombre: string;
}

export interface ACTIVIDADECONOMICA {
    id: number;
    descripcion: string;
    codigoCiiu: number;
    estado: number;
    creadoPor: string;
    fechaCreacion: Date;
    actualizadoPor: string;
    fechaActualizado: Date;
}

export interface PROFESION {
    id: number;
    descripcion: string;
    estado: number;
    creadoPor: string;
    fechaCreacion: Date;
    actualizadoPor: string;
    fechaActualizacion: Date;
}

export interface TIPOCREDITO {
    tcR_ID: number;
    tcR_DESCRIPCION: string;
    tcR_ID_PADRE: number;
    tcR_NIVEL: number;
    tcR_ESTADO: number;
    tcR_CREADO_POR: string;
    tcR_FECHA_CREACION: Date;
    tcR_MODIFICADO_POR: string;
    tcR_FECHA_MODIFICACION: Date;
}

export interface TIVIVIENDA {
    tiV_VIVIENDA_ID: number;
    tiV_VIVIENDA_DESCRIPCION: string;
}

export interface TYPEHOME {
    id: number,
    descripcion: string,
    esPromocionada: true,
    esActiva: true
}

// export interface TIPOVIVIENDA {
//     tipoV_VIVIENDA_ID: number;
//     tipoV_VIVIENDA_DESCRIPCION: string;
// }
export interface TIPOVIVIENDA {
    id: number;
    descripcion: string;
}

export interface ESTADOVIVIENDA {
    esV_ID: number;
    esV_DESCRIPCION: string;
}

export interface PHONETYPE {
    idTipoTelefono: number;
    descripcionTipoTelefono: string;
    activo: string;
}

export interface MAILTYPE {
    idTipoCorreo: number;
    descripcionTipoCorreo: string;
    activo: string;
}

export interface CURRENCYTYPE {
    idMoneda: number;
    descripcionMoneda: string;
    codigoMoneda: string;
}

export interface CATEGORIES {
    id: number;
    nombre: string;
    descripcion: string;
    expresion: string;
    esEditable: boolean;
}

export interface ADDRESSTYPE {
    tipoDireccionId: number;
    descripcion: string;
}

export interface TYPEREQUEST {
    idTipoSolicitud: string;
    descripcionTipoSolicitud: string;
    activo: boolean;
}

export interface TYPEMODALITY {
    idTipoModalidad: string;
    descripcionTipoModalidad: string;
    activo: boolean;
    codigoModalidad: string;
}

export interface TYPESUBMODALITY {
    idTipoSubModalidad: string;
    descripcionTipoSubModalidad: string;
    activo: boolean;
    timCodigo: string;
}
export interface ABONOUNICO {
    idAbonoUnico: number,
    descripcionAbono: string,
    activo: string
}

export interface TIPOABONO {
    idTipoAbono: number,
    descripcionTipoAbono: string,
    activo: true
}

export interface TIPOSUBSIDO {
    idTipoSubsudio: number,
    descripcionTipoSubsidio: string,
    activo: true
}
export interface ESTADOCARTERA {
    idEstado: number,
    descripcion: string,
}

export interface TIPOCUENTA {
    idTipoCuenta: number,
    descripcionTipoCuenta: string,
    prefijoTipoCuenta: string,
    categoriaTipoCuenta: string,
    aplicaInteres: boolean,
    aplicaRendimientos: boolean,
    aplicaCuotas: boolean,
    activo: boolean,
    centroCostoTipoCuenta: string
}

export interface ENTIDADBANCARIA {
    idEntidadBancaria: string;
    numeroIdentificacion: string;
    nombreRazonSocial: string;
    codigoBanco: number;
    estado: boolean;
    creadoPor: string;
    fechaCreacion: string;
    modificadoPor: string;
    fechaModificacion: string;
}

export interface CatalogData {
    isSuccess: boolean;
    returnMessage: string | null;
    data: InfoTurnoDto;
}

export interface turnosValor {
    tiT_ID: string;
    tiT_IDITEM: string;
    tiT_IDLISTA: string;
    tiT_CODIGO: string;
    tiT_NOMBRE: string;
    tiT_ID_ITEM_PADRE: string;
    tiT_ACTIVO: string;
    tiT_USUARIO: string;
    tiT_FECHA: string;
    tiT_HOST: string;
    tiT_TOOL_TIP_TEXT: string | null;
}

export interface InfoTurnoDto {
    Turnos: turnosValor[];
    sucursales: sucursalesValor[];
    recepciones: recepcionesValor[];
}

export interface sucursalesValor {
    siT_ID: string,
    siT_DESCRICION: string,
    siT_CODIGO: string
}

export interface recepcionesValor {
    riT_ID: string,
    riT_DESCRICION: string,
    riT_CODIGO: string
}

export interface VALORCATALOGO {
    id: number;
    codigo: string;
    descripcion: string;
    esActivo: boolean;
    idValor: number;
}

export interface VALORCATEGORIA {
    idCategoria: number;
    categoria: string;
}

export interface VALORTABLAPARAMETRO {
    idTablaParametro: number;
    tablaParametro: string;
    descripcion: string;
}

export interface TIPOINMUEBLE {
    idValor: number;
    descripcion: string;
}

export interface ENTIDADEDUCATIVA {
    archivos?: any;
    ciudad?: number;
    contacto?: string;
    correoElectronico?: string;
    creadoPor?: string;
    cuentas?: string;
    departamento?: string;
    direccion?: string;
    estado?: boolean;
    fechaCreacion?: Date;
    fechaModificacion?: Date;
    idEntidad: string;
    modificadoPor?: string;
    nit: string;
    nombreCiudad?: string;
    nombreDepartamento?: string;
    programas?: string;
    razonSocial: string;
    sedes?: string;
    telefono?: string;
    tipoIdentificacion?: number;
    esAcreditada?: number;
}

export interface PROGRAMAEDUCATIVO {
    id: string;
    descripcion: string;
}

export interface NIVELEDUCATIVO {
    id: string;
    descripcion: string;
}

export interface FORMATDTO {
    id: number;
    descripcion: string;
}

export interface AVERAGECOSTDTO {
    id: number;
    descripcion: string;
}

export interface ENTIDADSEGUROEDUCATIVO {
    idEntidadSeguroEducativo: string;
    tipoIdentificacion: string;
    numeroIdentificacion: string;
    nombreRazonSocial: string;
    razonSocial?: string;
    nit?: string;
}

export interface TIPOAFILIACIONDTO {
    idTipoAfiliacion: number;
    descripcionTipoAfiliacion: string;
}

export interface TIPOVINCULACIONDTO {
    IdTipoActor: number;
    DescripcionTipoActor: string;
}

export interface ESTADOAFILIACIONDTO {
    idEstadoAfiliacion: number;
    descripcionEstadoAfiliacion: string;
}

export interface CAMPOCALIDADDTO {
    idCampoCalidad: number;
    descripcionCampoCalidad: string;
}

export interface TIPOAFILIACIONPRODECENTEDTO {
    idTipoAfiliacionProcedente: number;
    descripcionTipoAfiliacionProcedente: string;
}

export interface PORCENTAJEDESCUENTODTO {
    idPorcentajeDescuento: number,
    descripcionPorcentajeDescuento: string,
}
export interface PORCENTAJEDESCAFILIACIONESDTO {
    descripcionPorcentajeDescuento: string,
    productoDescuento: string
}
export interface GRUPOSINCONSITENCIASDTO {
    idGrupo: string;
    grupo: number;
    estado: boolean;
}
export interface INCONSISTENCIADTO {
    grupoInconsistencia: number;
    idTipoInconsistencia: string;
    descripcionInconsistencia: string;
}

export interface HERRAMIENTAINCONSISTENCIADTO {
    idHerramienta: string;
    herramientaDescripcion: string;
}

export interface PUNTOSATENCIONDTO {
    idPuntosAtencion: string;
    puntosAtencion: string;
}

export interface IPC {
    id: number,
    anio: number,
    mes: number,
    valorIPC: number,
    fechaActualizacion: Date
}

export interface ESTADOTAREADTO {
    id: number;
    codigo: string;
    descripcion: string;
    estado: number;
}

export interface OFICINASREGISTRODTO {
    idOficina: number;
    descripcionOficina: string;
    codigoOficina: string;
    codigoDepartamento: number;
}

export interface CAUSALBLOQUEODTO {
    idCausalBloqueo: number;
    descripcionCausalBloqueo: string;
}

export interface REQUESTTYPEREALSTATEREGISTRATIONDTO {
    idTipoSolicitud: number;
    descripcionTipoSolicitud: string;
}
export interface PROGRAMAPORENTIDADDTO {
    idPrograma: string;
    idEntidad: string;
    razonSocialEntidad: string;
    programa: string;
    nivelEducativoId: string;
    nivelEducativo: string;
    estado: number;
    catalogoValorId: string;
    catalogoValorDescripcion: string;
    creadoPor: string;
    fechaCreacion: string;
    modificadoPor: string;
    fechaModificacion: string;
    idProgramaEntidad: string;
}

export interface NIVELEDUCATIVOPORPROGRAMADTO {
    nivelId: string;
    nivelDescripcion: string;
}

export interface MODALIDADAFILIACIONVETERANODTO {
    idModalidad: number;
    descripcionModalidad: string;
}

export interface PARENTESCO {
    id: number;
    codigo: string;
    descripcion: string;
    esActivo: boolean;
    idValor: number;
}

export interface CONCEPTOTYPEDTO {
    id: number;
    descripcion: string;
    concepto: string;
}

export interface LISTABENEFICIARIOS {
    id: any;
    listaBeneficiarios: any;
    tipoBeneficiario: any;
    descripcion: any;
    tipoDocEstudioObj: any;
    numDocEstudio: any;
    razonsocialEstudio: any;
    tipoDocCliente: any;
    tipoDocHijo: any;
    numeroDocHijo: any;
    nombreHijo: any;
    parentescoEstudio: any;
    parentescoEstudioObj: any;
    tipoDeBeneficiario: string;
    primerNombre?: string;
    segundoNombre?: string;
    primerApellido?: string;
    segundoApellido?: string;
}

export interface BeneficiarioHijo {
    id: number;
    tipoBeneficiario: 'HIJO/HIJA';
    descripcion: string;
    tipoDocEstudioObj: string | null;
    numDocEstudio: string;
    razonsocialEstudio: string;
    tipoDocHijo: string;
    numeroDocHijo: string;
    nombreHijo: string;
}
