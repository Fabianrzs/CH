import { 
         EmbargoBeneficiariosDto, 
         EmbargoConceptoDto, 
         EmbargoDto,
         TipoEmbargoDto, 
         TipoProcesoEmbargoDto, 
         TipoRetencionDto } from "./embargo.model.type";

export const GET_ALL_TIPOS_EMBARGOS_SUCCESS = 'GET_ALL_TIPOS_EMBARGOS_SUCCESS';
export const GET_ALL_TIPOS_PROCESOS_EMBARGOS_SUCCESS = 'GET_ALL_TIPOS_PROCESOS_EMBARGOS_SUCCESS';
export const GET_ALL_EMBARGOS_SUCCESS = 'GET_ALL_EMBARGOS_SUCCESS';
export const POST_ALL_EMBARGOS_BY_DATE_SUCCESS = 'POST_ALL_EMBARGOS_BY_DATE_SUCCESS';
export const GET_ALL_UPDATE_EMBARGOS_SUCCESS = 'GET_ALL_UPDATE_EMBARGOS_SUCCESS';
export const POST_INSERT_EMBARGOS_SUCCESS = 'POST_INSERT_EMBARGOS_SUCCESS';
export const POST_UPDATE_EMBARGOS_SUCCESS = 'POST_UPDATE_EMBARGOS_SUCCESS';
export const DELETE_EMBARGOS_SUCCESS = 'DELETE_EMBARGOS_SUCCESS';

export const GET_ALL_TIPOS_RETENCION_SUCCESS = 'GET_ALL_TIPOS_RETENCION_SUCCESS';
export const GET_ALL_EMBAGOS_CUENTA_CONCEPTOS_SUCCESS = 'GET_ALL_EMBAGOS_CUENTA_CONCEPTOS_SUCCESS';
export const POST_INSERT_EMBARGOS_CONCEPTO_SUCCESS = 'POST_INSERT_EMBARGOS_CONCEPTO_SUCCESS';
export const POST_UPDATE_EMBARGOS_CONCEPTO_SUCCESS = 'POST_UPDATE_EMBARGOS_CONCEPTO_SUCCESS';
export const DELETE_EMBARGOS_CONCEPTO_SUCCESS = 'DELETE_EMBARGOS_CONCEPTO_SUCCESS';

export const GET_ALL_BENEFICIARIOS_PAGO_SUCCESS = 'GET_ALL_BENEFICIARIOS_PAGO_SUCCESS';
export const POST_INSERT_BENEFICIARIO_PAGO_SUCCESS = 'POST_INSERT_BENEFICIARIO_PAGO_SUCCESS';
export const POST_UPDATE_BENEFICIARIO_PAGO_SUCCESS = 'POST_UPDATE_BENEFICIARIO_PAGO_SUCCESS';
export const DELETE_BENEFICIARIO_PAGO_SUCCESS = 'DELETE_BENEFICIARIO_PAGO_SUCCESS';

export const POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS = 'POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS';
export const GET_ALL_DESEMBARGOS_CUENTA_SUCCESS = 'GET_ALL_DESEMBARGOS_CUENTA_SUCCESS';

export const GET_ALL_CONCEPTOS_SUCCESS = 'GET_ALL_CONCEPTOS_SUCCESS';

export const ON_ERROR = 'ON_ERROR';

export interface GetAllTiposEmbargosSuccess {
    type: 'GET_ALL_TIPOS_EMBARGOS_SUCCESS';
    tiposEmbargo: TipoEmbargoDto[];
}
export interface GetAllTiposProcesosEmbargosSuccess {
    type: 'GET_ALL_TIPOS_PROCESOS_EMBARGOS_SUCCESS';
    tiposProcesos: TipoProcesoEmbargoDto[];
}
export interface GetAllEmbargosSuccess {
    type: 'GET_ALL_EMBARGOS_SUCCESS';
    embargos: EmbargoDto[];
}
export interface PostAllEmbargosByDateSuccess {
    type: 'POST_ALL_EMBARGOS_BY_DATE_SUCCESS';
    embargos: EmbargoDto[];
}

export interface GetAllUpdateEmbargosSuccess {
    type: 'GET_ALL_UPDATE_EMBARGOS_SUCCESS';
    embargos: EmbargoDto[];
}
export interface PostInsertEmbargoSuccess {
    type: 'POST_INSERT_EMBARGOS_SUCCESS';
    embargo: EmbargoDto;
}
export interface PostUpdateEmbargoSuccess {
    type: 'POST_UPDATE_EMBARGOS_SUCCESS';
    embargo: EmbargoDto;
}
export interface DeleteEmbargoSuccess {
    type: 'DELETE_EMBARGOS_SUCCESS';
    embargo: EmbargoDto;
}

export interface GetAllTiposRetencionSuccess {
    type: 'GET_ALL_TIPOS_RETENCION_SUCCESS';
    tiposRetencion: TipoRetencionDto[];
}
export interface GetAllEmbargosConceptoSuccess {
    type: 'GET_ALL_EMBAGOS_CUENTA_CONCEPTOS_SUCCESS';
    embargosConcepto: EmbargoConceptoDto[];
}
export interface PostInsertEmbargoConceptoSuccess {
    type: 'POST_INSERT_EMBARGOS_CONCEPTO_SUCCESS';
    embargoConcepto: EmbargoConceptoDto;
}
export interface PostUpdateEmbargoConceptoSuccess {
    type: 'POST_UPDATE_EMBARGOS_CONCEPTO_SUCCESS';
    embargoConcepto: EmbargoConceptoDto;
}
export interface DeleteEmbargoConceptoSuccess {
    type: 'DELETE_EMBARGOS_CONCEPTO_SUCCESS';
    embargoConcepto: EmbargoConceptoDto;
}
export interface GetAllBeneficiariosDePagoSuccess {
    type: 'GET_ALL_BENEFICIARIOS_PAGO_SUCCESS';
    embargoBeneficiarios: EmbargoBeneficiariosDto[];
}
export interface PostInsertEmbargoBeneficiarioSuccess {
    type: 'POST_INSERT_BENEFICIARIO_PAGO_SUCCESS';
    embargoBeneficiario: EmbargoBeneficiariosDto;
}
export interface PostUpdateEmbargoBeneficiarioSuccess {
    type: 'POST_UPDATE_BENEFICIARIO_PAGO_SUCCESS';
    embargoBeneficiario: EmbargoBeneficiariosDto;
}
export interface DeleteEmbargoBeneficiarioSuccess {
    type: 'DELETE_BENEFICIARIO_PAGO_SUCCESS';
    embargoBeneficiario: EmbargoBeneficiariosDto;
}

export interface PostInsertDesembargoSuccess {
    type: 'POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS';
    embargoConcepto: EmbargoConceptoDto;
}

export interface GetAllDesembargosSuccess {
    type: 'GET_ALL_DESEMBARGOS_CUENTA_SUCCESS';
    embargosConcepto: EmbargoConceptoDto[];
}

export interface GetAllConceptosSuccess {
    type: 'GET_ALL_CONCEPTOS_SUCCESS';
    conceptos: any;
}
export interface OnError {
    type: 'ON_ERROR';
    error: any;
}

export type EmbargosType = 
 | GetAllEmbargosSuccess
 | GetAllUpdateEmbargosSuccess
 | GetAllTiposEmbargosSuccess
 | PostAllEmbargosByDateSuccess
 | GetAllTiposProcesosEmbargosSuccess
 | PostInsertEmbargoSuccess
 | PostUpdateEmbargoSuccess
 | DeleteEmbargoSuccess
 | GetAllTiposRetencionSuccess
 | GetAllEmbargosConceptoSuccess
 | PostInsertEmbargoConceptoSuccess
 | PostUpdateEmbargoConceptoSuccess
 | DeleteEmbargoConceptoSuccess
 | GetAllBeneficiariosDePagoSuccess
 | PostInsertEmbargoBeneficiarioSuccess
 | PostUpdateEmbargoBeneficiarioSuccess
 | DeleteEmbargoBeneficiarioSuccess
 | PostInsertDesembargoSuccess
 | GetAllDesembargosSuccess
 | GetAllConceptosSuccess
 | OnError;