export interface ClienteDto {
    tiposVinculacion: any;
    idNovedad: number;
    idCliente: number;
    idTipoActor: number;
    idTipoIdentificacion: number;
    identificacion: string;
    fechaExpedicionDocumento: Date;
    idPaisExpedicionDocumento: number | undefined;
    idCiudadExpedicionDocumento: number | undefined;
    idDepartamentoExpedicionDocumento: number | undefined;
    lugarExpedicionExtranjero: string;
    codigoMilitar: string;
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
    fechaNacimiento: Date;
    idPaisNacimiento: number | undefined;
    idDepartamentoNacimiento: number | undefined;
    idCiudadNacimiento: number | undefined;
    lugarNacimientoExtranjero: string;
    sexo: string;
    estadoCivil: string;
    idProfesion: number;
    unidad: string;
    idCategoria: number;
    idFuerza: number;
    idGrado: number | undefined;
    idUnidadEjecutora: number | undefined;
    fechaInscripcion: Date;
    fechaAlta: Date;
    regimen: number;
    fechaAltaCalculada: Date | null;
    fechaReintegro: Date;
    estadoCliente: any;
    numeroValidacion: number;
    idPatrocinador: number;
    participacion: number;
    numeroResolucion: number;
    validacionBiometrica: boolean;
    esBeneficiario:boolean;
    estadoCargueNomina: number;
    idInformacionEconomica: number;
    idActividadEconomica: number | undefined;
    fechaCorte: Date;
    ingresoMensual: number;
    egresoMensual: number;
    totalActivos: number;
    totalPasivos: number;
    totalPatrimonio: number;
    ingresoAdicional: number;
    conceptoIngresosAdicionales: string;
    transaccionesMonedaExtranjera: boolean | undefined;
    idMoneda: number | undefined;
    montoTransaccionesExtranjero: number;
    idDireccion: number;
    consecutivoDireccion: number;
    idPaisResidencia: number | undefined;
    idDepartamentoResidencia: number | undefined;
    idCiudadResidencia: number | undefined;
    ciudadResidenciaExtranjero: string;
    idTipoCalle: number;
    numeroCalle1: number;
    idLetra1: number;
    idBis1: number;
    idCardinal1: number;
    numeroCalle2: number;
    idLetra2: number;
    idBis2: number;
    idCardinal2: number;
    numeroCalle3: number;
    idLetra3: number;
    idCardinal3: number;
    complementoDireccion: string;
    direccion: string;
    barrio: string;
    idTipoDireccion: number;
    idSistemaActualiza: number;
    idCorreo: number;
    correo: string;
    correo2: string;
    idTipoCorreo: number;
    envioInformacionCorreo: boolean;
    correoActivo: boolean;
    idTelefono: number;
    indicativoPaisTelefono: number;
    indicativoCiudadTelefono: number;
    telefono: number;
    telefono2:number;
    idTipoTelefono: number;
    envioInformacionSMS: boolean;
    telefonoActivo: boolean;
    idConyuge: number;
    idTipoIdentificacionConyuge: number | null;
    identificacionConyuge: string;
    primerNombreConyuge: string;
    segundoNombreConyuge: string;
    primerApellidoConyuge: string;
    segundoApellidoConyuge: string;
    tasaAportes: number;
    tasaVoluntariaAportes: number;
    cuentas: any[];
    aportesCategoria: AporteCategoria[];
    dependientes: Dependiente[];
    solicitudes: Solicitud[];   
    fechaInicial: Date;
    fechaInicioNovedad: Date;
    fechaFinNovedad: Date;
    anotaciones: Anotaciones[];
    porcentaje: number;
    saldoDisponible: number | bigint;
    fechaActualizaDatos?:string;
    estadosCliente:any[];
    causalesEstadoCliente:any[];
    estadosCausaleslCliente:any[];
    embargos:any[];  
    idCiudadExpedicion:number;
    fechaPrimeraCuota:Date|null;  
    fechaUltimaCuota:Date|null;
    numeroCuotas:number;
    situacionActual:string;
    numeroValidacionVigia:string;
    tasaSalarioBase:number;
    valorEstimadoSubsidio:string;
    valorSubsidio:number;
    fechaActualizaDatosFinancieros?:string;
    fechaActualizaDatosContacto?:string;
    tiposSolicitud:any[];
    nombreTipoIdentificacion: string;
    embargosAfiliado: any[];
}

export interface RestaurarIntegracionDto {
    idCliente: number | null;
    idInformacionFinanciera: number| null;
    idDireccion: number | null;
    idTipoIdentificacion: number | null;
    numeroIdentificacion: string;
}

export interface CuentaDto {
    idCuenta: number;
    numeroCuenta: number;
    subcuenta: number;
    idTct: number;
    saldo: number;
    fechaCreacion: Date;
    idEct: number;
    idCcn: number;
    idDocumento: number;
    idTpa: number;
    cuotas: number;
    fechaCierre: Date;
    idCuentaHabiente: number;
    cuotasPendientes: number;
    fechaPrimerAporte: Date;
    saldoCanje: number;
    saldoDisponible: number;
    principal: number;
    movimientos: Movimiento[];
    conceptosSaldosBloqueos: ConceptosSaldosBloqueos[];
    estadoCuenta:string;
    embargos: any[];
    conceptos: any[];
}



export interface AporteCategoria {
    cLI_ID: number;
    ctG_ID: number;
    apC_CUOTAS_ACUMULADAS: number;
    apC_FECHA_ULTIMA_COUTA: Date;
    apC_FECHA_PRIMERA_CUOTA: Date;
}

export interface Anotaciones {
    anotacioN_ID: number;
    AnotacionClienteId: any;
    afiliado: string;
    anotacion: string;
    clI_ID: number;
    coD_USUARIO: string;
    fechA_ANOTACION: Date;
    identificacion:string;
    usuario:string;
}

export interface DependienteCategoria {
    idDependiente: number;
    idCliente: number;
    idTipoIdentificacion: number;
    tipoIdentificacion: string;
    identificacion: string;
    nombre: string;
    participacion: number;
    direccion: string;
    correo: string;
    telefono: string;
    conservaAntiguedad: boolean;
    numeroActoAdministrativoResolucionesReconocimientoPensional: number;
    fechaNotificacionResolucionesReconocimientoPensional: Date;
    porcentajeReconocimientoPensionalDistribucionAportes: number;
    numeroActoAdministrativoResolucionesReconocimientoPrestacionesSociales: number;
    fechaNotificacionResolucionesReconocimientoPrestacionesSociales: Date
    porcentajeReconocimientoPrestacionesSocialesDistribucionCesantias: number;
    derechoLiquidacionCuotasBeneficiarios: boolean;
    porcentajeReconocimientoPensionalDistribucionSubsidio: number;
    periodoAportesDesdeBeneficiarios: Date,
    periodoAportesHastaBeneficiarios: Date, 
    valorDerechoLiquidacionCuotasBeneficiarios: number,
    numeroCuotasderechoLiquidacionCuotasBeneficiarios: number,
    index: number
}

export interface Dependiente {
    idDependiente: number;
    idCliente: number;
    tipoIdentificacion: string;
    identificacion: string;
    nombre: string;
    participacion: number;
    resolucion:string;
    fechaAdquisicion:Date;
}

export interface Movimiento {
    mvT_ID: string;
    ctA_ID: number;
    cnC_ID: number;
    mvT_VALOR: number;
    caT_TIPO_MOVIMIENTO: string;
    mvT_FECHA_PROCESO: Date;
    dcT_ID: number;
    mvT_ANO_PERIODO: number;
    mvT_MES_PERIODO: number;
    mvT_FECHA_MOVIMIENTO: Date;
    cnC_CONCEPTO:string;
}

export interface ConceptosSaldosBloqueos {
    cantidadIngresos: number;
    concepto: string;
    documento: number
    estadoBloqueo: string;
    fechaBloqueo: Date;
    fechaUltimoAporteConcepto: Date;
    idConcepto: number;
    idCuenta: number;
    idExpd: number;
    nroExpd: string;
    origen: string;
    saldoConceptoCuenta: number;
    saldoInicialAno: number;
    valorAcumulado: number;
    valorBloqueo: number;
}

export interface CuentasMovimientoAfiliado {
    numeroCuenta: number;
    idConcepto: number;
    saldo: number;
    movimientos: number;
    valorSolicitado?: number;
}

export interface AdminCuentasAfiliado {

    clienteId: number;
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
    identificacion: string;
    tipoIdentificacionId: number;
    categoriaId: number;
    fuerzaId: number;
    gradoId: number;
    unidadEjecutoraId: number;
    siglaUnidadEjecutora:string;
    categoria: string;
    fuerza: string;
    grado: string;
    unidadEjecutora: string;
    tipoIdentificacion: string;
    fechaInscripcion: Date;
    tipoAfiliacion: string;
    tipoAfiliacionId: number;
    fechaAlta: Date;
    causalEstadoCuenta: [{
        causalEstadoId: number;
        estadoCuentaId: number;
        causalCuentaDescripcion: string;
        causalActiva: boolean;
    }];
    tipoCuentaC: [{
        idTipoCuentaC: number;
        descripcionTipoCuentaC: string;
        prefijoCuentaC: string;
        tipoGeneralCuentaC: string;
        aplicanInteresesCuentaC: boolean;
        aplicanRendimientosCuentaC: boolean;
        aplicanCuotasCuentaC: boolean;
        activaCuentaC: boolean;
        centroCostoCuenta: string;
    }];
    estadosCuenta: [{
        idEstado: number;
        estado: string;
    }];
    modalidadesAfiliacion: [{
        idTipoAfiliacion: number;
        descripcionTipoAfiliacion: string;
    }];
    porcentajeDescuentos: [{
        idPorcentajeDescuento: number;
        descripcionPorcentajeDescuento: string;
        productoDescuento: string;
    }];
    aportes: [{
        clI_ID: number;
        ctG_ID: number;
        apC_CUOTAS_ACUMULADAS: number;
        apC_FECHA_ULTIMA_COUTA: Date;
        apC_FECHA_PRIMERA_CUOTA: Date;
        ctG_DESCRIPCION: string;
    }];
    cuentas: [
        {
            cuentaId: number;
            cuentaIdIntegracion: number;
            numeroCuenta: number;
            clinteId: number;
            tipoCuentaId: number;
            estadoCuentaId: number;
            estadoCuenta:string;
            causalEstadoId: number;
            documentoId: number;
            fechaCreacion: Date;
            fechaCierre: Date;
            fechaPrimerAporte: Date;
            saldo: number;
            saldoCanje: number;
            saldoDisponible: number;
            CuentaBloqueo: boolean;
            nombreCuenta:string;
            embargos: number;            
            movimientos: [{
                conceptoId: number;
                cuentaid: number;
                documentoId: number;
                fechaMovimiento: Date;
                fechaProceso: Date;
                valor: number;
                tipoMovimiento: string;
                periodoAno: string;
                movimientoId: number
            }];
            conceptos: [
                {
                    id: number;
                    cuentaId: number;
                    conceptoId: string;
                    saldo: number;
                    fechaUltimoAporte: Date;
                    ingresos: number;
                    sva: number;
                    descripcionConcepto: string;
                    valorEmbargo: number;
                    valorBloqueoEmbargo: number;
                    valorDevolver: number;
                    movimientos:
                    [
                        {
                            conceptoId: number;
                            cuentaid: number;
                            documentoId: number;
                            fechaMovimiento: Date;
                            fechaProceso: Date;
                            valor: number;
                            tipoMovimiento: string;
                            periodoAno: string;
                            movimientoId: number
                        }
                    ]
                }
            ] | undefined;
            bloqueos: [{
                id: string,
                causalBloqueoId: string,
                cuentaId: number,
                conceptoId: number,
                tipoBloqueo: string,
                monto: boolean,
                valor: number,
                estado: boolean,
            }]
            cueotas: number;
            inclusionSaldos: [{
                numeroCuenta: string,
                observaciones: string,
                generaOrdenRecaudo: boolean
                opciones: [{
                    periodoDesde: Date,
                    periodoHasta: Date,
                    idConcepto: number,
                    valor: number,
                    cuotas: number
                }]
            }],
            listaEmbargos: [{
                id: string,
                valor: number
                fechaInicio: Date,
                fechaFin: Date,
                indicadorCesantias: boolean,
                embardoId: string,
                tipoRetencionId: string,
                tipoEmbargoId: string,
                cuentaId: number,
                conceptoId: number,
                fechaRegistro: Date,
                fechaActualizacion: Date,
                creadoPorId: string,
                actualizadoPorId: string,
                estado: number,
                radicadoWorkManager: string,
                numeroCuenta: number,
                tipoRetencion: string,
                tipoembargo: string,
                idTipoProceso: string,
                tipoProceso: string,
                tipoEmbargo: string,
                numeroSolicitud: number
            }]
        }
    ]
}

export interface PermisoActualizacion
{
    paC_PERFIL: string;
    pac_FECHAADICION: string;
    pac_USUARIOADICION: string;
}

export interface AportesCategoriaAfiliaciones
{
    clI_ID: number;
    ctG_ID: number;
    apC_CUOTAS_ACUMULADAS: number;
    apC_FECHA_ULTIMA_COUTA: Date;
    apC_FECHA_PRIMERA_CUOTA: Date;
    ctG_DESCRIPCION: string;
    idAportesCategoriaAfiliaciones: number;
    periodoAportesDesde: Date;
    periodoAportesHasta: Date;
    ratificacionCuotas: number,
    liquidacionCuotaFondoSolidaridad: boolean,
    reintegroAportes: boolean,
    derechoLiquidacionCuotas: boolean,
    valorDerechoLiquidacionCuotas: number,
    numeroCuotasLiquidacion: number,
    regimen: string,
    observaciones: string,

}

export interface InclusionSaldoDto {
    periodoDesde: Date,
    periodoHasta: Date,
    idConcepto: number,
    valor: number,
    cuotas: number,
    cuotasAntiguas: number,
    idInclusionSaldoOpciones: number
}

export interface InclusionSaldoRequestDto {
    idInclusionSaldoEncabezado: number,
    idCliente: number,
    opciones: InclusionSaldoDto[],
    observaciones: string,
    generaOrdenRecaudo: boolean,
    fechaInicialRecaudo: Date,
    fechaFinalRecaudo: Date,
    aprobacion: boolean
}

export interface Solicitud {
    idSolicitud: number;
    documentoCausante: string;
    numeroSolicitud: string;
    radicado: string;
    tipoSolicitud: string;
    modalidad: string;
    estadoSolicitud: string;
    fechaInicioSolicitud: Date;
    fechaFinSolicitud: Date;
    valor: number;
    valorPagado: number;
    actividad: string;
}

export interface InteresesNoCausadosDto {
    anoProceso: number,
    concepto: string,
    totalInteresesCausados: number,
    saldoCuentaIntereses: number,
   
}

export interface InteresesNoCausadosRequestDto {
    idSolicitud: number,
    idTipoSolicitud: number,
    opciones: string,
    concepto: string,
    valor: number,
    periodo: number,
    solicitudPago: number,
    causado: number,
    fechaSolicitud: Date,
}



