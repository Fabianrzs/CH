export interface AfiliacionDto {
    idCliente: number
    idAfiliacion: number
    idTipoAfiliacionExtraordinaria:	number
    numeroRadicado:	string
    fechaRadicado: Date
    fechaElaboracion: Date
    campoCalidad: number
    idTipoAfiliacionProcedente:	number
    tramiteAfiliacion: boolean
    conservaAntiguedad: boolean
    ratificacionCuotas:	number
    campoObservacionesJustificacion: string
    numeroActoAdministrativo: string
    fechaNotificacion:	Date
    campoCesantias:	boolean
    activacionDescuento: boolean
    campoObservacionesConceptualizacion: string
    idPorcentajeDescuento: number
    idModalidadAfiliacionVeterano: number
    ahorroConsignacionBancaria:	boolean
    solicitudTrasladoCesantias:	boolean
    idCiudad: number
    idDepartamento:	number
    oficioSolicitudCesantias: number
    oficioSolicitudTrasladoCesantias: number
    mesDeInicioActivacionDescuento: Date
    mesDeFinActivacionDescuento: Date
    ahorroConsignacionBancariaOpcion: number
    tipoIdentificacion: number
    cedula:	string
    primerNombre: string
    segundoNombre:string
    primerApellido: string
    segundoApellido: string
    fechaNacimiento: Date
    idCategoria: number
    idFuerza: number
    idUnidadEjecutora: number
    idGrado: number
    fechaAlta:	Date
    asignacionBasicaMensual: number
    regimen: string
}
export interface BeneficiariosAfilicionesDto {
    idDependiente: number
    idBeneficiarioAfiliacion: number
    idCliente: number
    idTipoIdentificacion: number
    tipoIdentificacion: string
    identificacion: string
    nombre: string
    participacion: number
    direccion: string
    correo: string
    telefono: string
    conservaAntiguedad: boolean
    numeroActoAdministrativoResolucionesReconocimientoPensional: number
    fechaNotificacionResolucionesReconocimientoPensional: string
    porcentajeReconocimientoPensionalDistribucionAportes: number
    numeroActoAdministrativoResolucionesReconocimientoPrestacionesSociales:	number
    fechaNotificacionResolucionesReconocimientoPrestacionesSociales: Date
    porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias: number
    derechoLiquidacionCuotasBeneficiarios:	boolean
    porcentajeReconocimientoPensionalDistribucionSubsidio: number
}
export interface AportesCategoriaAfiliacionesDto {
    clI_ID:	number
    ctG_ID:	number
    apC_CUOTAS_ACUMULADAS: number
    apC_FECHA_ULTIMA_COUTA:	Date
    apC_FECHA_PRIMERA_CUOTA: Date
    ctG_DESCRIPCION: string
    nullable: true
    periodoAportesDesde: Date
    periodoAportesHasta: Date
    ratificacionCuotas: number
    liquidacionCuotaFondoSolidaridad: boolean
    reintegroAportes: boolean
    derechoLiquidacionCuotas: boolean
    valorDerechoLiquidacionCuotas: number
}
export interface TramiteDesafiliacionDto {
    idTramiteDesafiliacion: number
    idAfiliacion: number
    idSolicitud: string
    idModalidad: string
    valor: number
}
export interface AfiliadoDto {
    idCliente:	number
    nombreCompleto: string
    nombreCompletoInvertido: string
    tipoDocumento: number
    documento: string
    sexo: string
    nullable: true
    departamento: number
    ciudad:	number
    
}

export interface AportesClienteDto {
    apC_ID:	number
    clI_ID:	number
    ctG_ID:	number
    apC_CUOTAS_ACUMULADAS: number
    apC_FECHA_ULTIMA_COUTA: Date
    apC_FECHA_PRIMERA_CUOTA: Date
    ctG_DESCRIPCION: string
}
export interface ArchivoAplicacionPagoCSVDto {
    fuerzaNombre: string
    tipoNomina:	string
    codConcepto: string
    concepto: string
    funcionario: string
    nroDocumento: string
    codigoMilitar: string
    valorCuota:	number
    fechaDesde:	Date
    fechaHasta:	Date
    
}
export interface BeneficiariosAfiliacionesDto {
    idDependiente: number
    idBeneficiarioAfiliacion: number
    idCliente: number
    idTipoIdentificacion: number
    tipoIdentificacion:	string
    identificacion:	string
    nombre:	string
    participacion: number
    direccion: string
    correo:	string
    telefono: string
    conservaAntiguedad:	boolean
    numeroActoAdministrativoResolucionesReconocimientoPensional: number
    fechaNotificacionResolucionesReconocimientoPensional: Date
    porcentajeReconocimientoPensionalDistribucionAportes: number
    numeroActoAdministrativoResolucionesReconocimientoPrestacionesSociales:	number
    fechaNotificacionResolucionesReconocimientoPrestacionesSociales: Date
    porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias: number
    derechoLiquidacionCuotasBeneficiarios: boolean
    porcentajeReconocimientoPensionalDistribucionSubsidio: number
    
}
export interface BloqueoCuentaConceptoDto {
    id:	string
    causalBloqueoId: number
    cuentaId: number
    conceptoId:	number
    tipoBloqueo: string
    nullable: true
    monto: boolean
    valor: number
    creadoPor: string
    estado:	boolean
}

export interface ClienteDto {
idCliente:	number
idTipoActor: number
idTipoIdentificacion: number
identificacion:	string
fechaExpedicionDocumento: Date
idPaisExpedicionDocumento: number
idDepartamentoExpedicionDocumento: number
idCiudadExpedicionDocumento: number
lugarExpedicionExtranjero: string
codigoMilitar: string
primerNombre: string
segundoNombre: string
primerApellido: string
segundoApellido: string
fechaNacimiento: Date
idPaisNacimiento: number
idDepartamentoNacimiento: number
idCiudadNacimiento: number
lugarNacimientoExtranjero: string
sexo: string
estadoCivil: string
idEstadoCivil: string
idProfesion: number
unidad: string
idCategoria: number
idFuerza: number
idGrado: number
idUnidadEjecutora: number
fechaInscripcion: Date
fechaAlta: Date
regimen: number
fechaAltaCalculada: Date
fechaReintegro:	Date
estadoCliente: string
idEstadoCliente: number
numeroValidacion: string
idPatrocinador: number
participacion: number
numeroResolucion: string
validacionBiometrica: boolean
estadoCargueNomina: number
cuentasHabilitadas: number
tasaAportes: number
tasaVoluntariaAportes: number
payload: string
idInformacionEconomica: number
idActividadEconomica: number
fechaCorte:	Date
ingresoMensual:	number
egresoMensual: number
totalActivos: number
totalPasivos: number
totalPatrimonio: number
ingresoAdicional: number
conceptoIngresosAdicionales: string
transaccionesMonedaExtranjera: boolean
idMoneda: number
montoTransaccionesExtranjero: number
idDireccion: number
consecutivoDireccion: number
idPaisResidencia: number
idDepartamentoResidencia: number
idCiudadResidencia: number
ciudadResidenciaExtranjero:	string
idTipoCalle: number
numeroCalle1: number
idLetra1: number
idBis1: number
idCardinal1: number
numeroCalle2: number
idLetra2: number
idBis2: number
idCardinal2: number
numeroCalle3: number
idLetra3: number
idCardinal3: number
complementoDireccion: string
direccion: string
barrio:	string
idTipoDireccion: number
idSistemaActualiza: number
idCorreo: number
correo:	string
idTipoCorreo: number
envioInformacionCorreo:	boolean
correoActivo: boolean
idTelefono: number
indicativoPaisTelefono: number
indicativoCiudadTelefono: number
telefono: string
idTipoTelefono: number
envioInformacionSMS: boolean
telefonoActivo:	boolean
idConyuge: number
idTipoIdentificacionConyuge: number
identificacionConyuge: string
primerNombreConyuge: string
segundoNombreConyuge: string
primerApellidoConyuge: string
segundoApellidoConyuge:	string
conyugeActivo: boolean
}


