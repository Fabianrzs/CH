import NumberFormat from "react-number-format";

export interface Welcome {
    isSuccess: boolean;
    returnMessage: string;
    data: ParametrosVerificacionDerechoDto[];
}

export interface ParametrosVerificacionDerechoDto {
    id: number;
    anio: number;
    mes: number;
    fechaOperacion: Date;
    fechaAprobacion: Date;
    totalCuentasVerificadas: number;
    totalCambiosEstado: number;
    usuario: string;
    aprobado: number;
    numeroRadicado: string | null;
}

export interface InfoVerificacionDerechoDto {
    vddId: number;
    paramVdId: number;
    estadoInicial: number;
    estadoFinal: number;
    motivoCambio: string;
    cuentasAfectadas: number;
}

export interface ResultVerificacionDerechoDto {
    id: number;
    ctaId: number;
    ctaNumero: number;
    cliId: number;
    fechaUltimoMovimientoCredito: Date;
    ctaFechaPrimerAporte: Date;
    ctaCuotas: number;
    cuotasPendientes: number;
    ectIdInicial: number;
    estadoInicial: string;
    ectIdFinal: number;
    estadoFinal: string;
    cambiarEstado: string;
    periodoAnio: number;
    periodoMes: number;
    ctaIdIntegracion: number;
    tctId: number;
    ctaFechaCreacion: Date;
    cantidadMovimientoCredito: number;
    ultimoAnioPeriodo: number;
    ultimoMesPeriodo: number;
    ahorrosObligarotios: number;
    flag: string;
    identificacion: string;
    numeroSolicitud: 0;
}

export interface FiltroVerificacionDerechoDto {
    DesdeAnio: number;
    DesdeMes: number;
    HastaAnio: number;
    HastaMes: number;
}

export interface VerificacionDerechoPorPeriodoDto {
    anio: number;
    mes: number;
    UsuarioId: string | null;
    UsuarioNombre: string | null;

    
}

export interface InterecesNoCausados {
    
    identificacion: string;
    anio: number;
    conceptos: string;
    totalInteresesNoCausados: number;
    saldoCuentaInteresNoCausado: number;

    
}


interface Datos {
    tipoCuenta: string;
    concepto: string;
    totalBase: number;
    abonadoPeriodo: number;
    noAbonadoPeriodo: number;
    totalReconocidoPeriodo: number;
    noAbonadosSaldoInsuficiente: number;
  }


  export interface ParametrosAbonosInteresDto {
    
    
    anio: number;
    mes: number;
    fechaRecepcion: Date;
    fechaAprobacion: Date;
    valorIpc: number;
    porcentajeIpc: number;
    totalInteresesAbonados : number;
    comprobanteSolucionViviendaAportes: string;
    comprobanteSolucionViviendaCesantias: string;
    comprobanteSolucionAdministracionCesantias: string;
    comprobanteAVAC: string;
    comprobanteAVAV: string;
    comprobanteAVC: string;
    numeroRadicado: number | null;

    estado: number;
    nombreEstado: string | null;
    
   
}


export interface InteresNoCausadoDto {   
    
    anio: number;
    usuarioId: number;
    concepto: number | null;
    ctaId: number | null;

    tipoCuentaId: number | null;
    saldo: number | null;

    interesesNoCausados: number | null;
    saldoTotalInteresNoCausado: number | null;       
   
}

