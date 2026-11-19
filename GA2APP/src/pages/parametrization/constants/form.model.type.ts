import {
    PlazosDto,
    SeguroDto,
    TasaDto,
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
    LiquidacionDto,
    ParametroTransaccionDto,
    PlazoDto,
    PlazoTipoActorDto,
    PlazoUndejDto,
    SegurosDto,
    SeguroMunicipioDto,
    LiquidacionGeneralDto,
    SeguroRegionDto,
} from '../../../types/parametrization.model.type';

export const PlazosForm: PlazoDto = {
    plazoId: 0,
    plazoMinimoAnio: 0,
    plazoMinimoMes: 0,
    plazoMaximoAnio: 0,
    plazoMaximoMes: 0
};

export const PlazosTipoActorDtoForm: PlazoTipoActorDto = {
    plazoId: 0,
    plazoMinimoAnio: 0,
    plazoMinimoMes: 0,
    plazoMaximoAnio: 0,
    plazoMaximoMes: 0,
    plazoEstado: false,
    idTipoActor: 1,
    descripcionTipoActor: '',
    tipoCredito: 0
};

export const PlazosUniejDtoForm: PlazoUndejDto = {
    plazoId: 0,
    plazoMinimoAnio: 0,
    plazoMinimoMes: 0,
    plazoMaximoAnio: 0,
    plazoMaximoMes: 0,
    plazoEstado: false,
    unidadEjecutoraId: 1,
    nombreUnidadEjecutora: '',
    tipoCredito: 0
};

export const SeguroGeneralForm: SegurosDto = {
    idSeguro: 0,
    seguroVida: 0,
    seguroTodoRiesgo: 0,
    fechaModificacion: new Date,
    modificadoPor: 0,
    seguroEstado: false,
    seg_Iva:0,
};

export const SeguroExcepMunicipioForm: SeguroMunicipioDto = {
    idSeguro: 0,
    idExcepcionSeguro: 0, 
    seguroVida: 0,
    seguroTodoRiesgo: 0,
    seguroEstado: false,
    departamentoId: 91,
    departamentoCodigo: 91,
    departamentoNombre: '',
    ciudadId: 1086,
    ciudadCodigo: 1,
    ciudadNombre: '',
    fechaModificacion: new Date(),
    modificadoPor: 0,
}

export const SeguroRegionForm: SeguroRegionDto = {
    id: 0,
    nombre: '',
    seguro: 0,
}

export const LiquidacionGeneralForm: LiquidacionGeneralDto = {
    idLiquidacion: 0,
    fechaCorte: 0,
    fechaPago: 0,
    estado: 1,
    fechaModificacion: new Date (),
    modificadoPor: 0,
}

export const InsuranceForm: SeguroDto = {
    id: 0,
    seguroVida: '0',
    seguroTodoRiesgo: '0',
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};

export const RateForm: TasaDto = {
    id: 0,
    tasaUsuraEA: 0,
    tasaUsuraNM: 0,
    tasaCorrienteEA: 0,
    tasaCorrienteNM: 0,
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};
export const TermForm: PlazosDto = {
    id: 0,
    yearMin: 0,
    monthMin: 0,
    yearMax: 0,
    monthMax: 0,
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};

export const LegalReliefForm: LegalAlivioDto = {
    id: 0,
    alivioCuota: 0,
    vigenciaAlivioCuota: new Date(),
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};

export const LegalRateForm: LegalTasaDto = {
    id: 0,
    tasaFrech: 0,
    vigenciaTasaFrech: new Date(),
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};

export const LegalGmfForm: LegalGmfDto = {
    id: 0,
    gmf: 0,
    vigenciaGMF: new Date(),
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};

export const SettlementForm: LiquidacionDto = {
    id: 0,
    fechaCorte: 0,
    fechaPago: 0,
    fechaModificacion: new Date(),
    modificadoPor: 0,
    estado: false,
};

export const ParametroTransaccion: ParametroTransaccionDto = {
    paraM_TRANS_CONCEPTO: '',
    paraM_TRANS_CODIGO: 0,
    paraM_TRANS_PROCESO: '',
    paraM_TRANS_CREADO_POR: '00000000-0000-0000-0000-000000000000',
    paraM_TRANS_FECHA_CREACION: new Date(),
    paraM_TRANS_MODIFICADO_POR: '00000000-0000-0000-0000-000000000000',
    paraM_TRANS_FECHA_MODIFICACION: new Date(),
};
