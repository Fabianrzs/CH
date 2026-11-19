import {
    LegalAlivioDto,
    LegalTasaDto,
    LegalGmfDto,
} from '../../../types/parametrizationLegal.model.type';

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
