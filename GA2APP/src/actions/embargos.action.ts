import { Dispatch } from "react";
import ApiEmbargo from "../services/api-embargos.service";
import { AppActions } from "../types/appactions.type";
import { 
         EmbargoBeneficiariosDto,
         EmbargoConceptoDto, 
         EmbargoDto, 
         EmbargoModelDto, 
         TipoEmbargoDto, 
         TipoProcesoEmbargoDto, 
         TipoRetencionDto} from "../types/embargo.model.type";
import { DELETE_BENEFICIARIO_PAGO_SUCCESS, DELETE_EMBARGOS_CONCEPTO_SUCCESS, DELETE_EMBARGOS_SUCCESS, GET_ALL_BENEFICIARIOS_PAGO_SUCCESS, GET_ALL_CONCEPTOS_SUCCESS, GET_ALL_DESEMBARGOS_CUENTA_SUCCESS, GET_ALL_EMBAGOS_CUENTA_CONCEPTOS_SUCCESS, GET_ALL_EMBARGOS_SUCCESS, 
         GET_ALL_TIPOS_EMBARGOS_SUCCESS, 
         GET_ALL_TIPOS_PROCESOS_EMBARGOS_SUCCESS, 
         GET_ALL_TIPOS_RETENCION_SUCCESS, 
         GET_ALL_UPDATE_EMBARGOS_SUCCESS, 
         ON_ERROR,
         POST_ALL_EMBARGOS_BY_DATE_SUCCESS,
         POST_INSERT_BENEFICIARIO_PAGO_SUCCESS,
         POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS,
         POST_INSERT_EMBARGOS_CONCEPTO_SUCCESS,
         POST_INSERT_EMBARGOS_SUCCESS,
         POST_UPDATE_BENEFICIARIO_PAGO_SUCCESS,
         POST_UPDATE_EMBARGOS_CONCEPTO_SUCCESS,
         POST_UPDATE_EMBARGOS_SUCCESS,
         } from "../types/embargos.actions.type";

const api = ApiEmbargo();

export const GetAllTiposEmbargosSuccess = (
    tiposEmbargo: TipoEmbargoDto[],
): AppActions => ({
    type: GET_ALL_TIPOS_EMBARGOS_SUCCESS,
    tiposEmbargo: tiposEmbargo
});

export const GetAllTiposProcesosEmbargosSuccess = (
    tiposProcesos: TipoProcesoEmbargoDto[],
): AppActions => ({
    type: GET_ALL_TIPOS_PROCESOS_EMBARGOS_SUCCESS,
    tiposProcesos: tiposProcesos
});

export const GetAllEmbargosSuccess = (
    embargos: EmbargoDto[],
): AppActions => ({
    type: GET_ALL_EMBARGOS_SUCCESS,
    embargos: embargos
});

export const PostAllEmbargosByDateSuccess = (
    embargos: EmbargoDto[],
): AppActions => ({
    type: POST_ALL_EMBARGOS_BY_DATE_SUCCESS,
    embargos: embargos
});

export const GetAllUpdateEmbargosSuccess = (
    embargos: EmbargoDto[],
): AppActions => ({
    type: GET_ALL_UPDATE_EMBARGOS_SUCCESS,
    embargos: embargos
});

export const PostInsertEmbargoSuccess = (
    embargo: EmbargoDto,
): AppActions => ({
    type: POST_INSERT_EMBARGOS_SUCCESS,
    embargo: embargo
});

export const PostUpdateEmbargoSuccess = (
    embargo: EmbargoDto,
): AppActions => ({
    type: POST_UPDATE_EMBARGOS_SUCCESS,
    embargo: embargo
});

export const DeleteEmbargoSuccess = (
    embargo: EmbargoDto,
): AppActions => ({
    type: DELETE_EMBARGOS_SUCCESS,
    embargo: embargo
});

export const GetAllTiposRetencionSuccess = (
    tiposRetencion: TipoRetencionDto[],
): AppActions => ({
    type: GET_ALL_TIPOS_RETENCION_SUCCESS,
    tiposRetencion: tiposRetencion
});


export const GetAllEmbargosConceptoSuccess = (
    embargosConcepto: EmbargoConceptoDto[],
): AppActions => ({
    type: GET_ALL_EMBAGOS_CUENTA_CONCEPTOS_SUCCESS,
    embargosConcepto: embargosConcepto
});

export const PostInsertEmbargoConceptoSuccess = (
    embargoConcepto: EmbargoConceptoDto,
): AppActions => ({
    type: POST_INSERT_EMBARGOS_CONCEPTO_SUCCESS,
    embargoConcepto: embargoConcepto
});

export const PostUpdateEmbargoConceptoSuccess = (
    embargoConcepto: EmbargoConceptoDto,
): AppActions => ({
    type: POST_UPDATE_EMBARGOS_CONCEPTO_SUCCESS,
    embargoConcepto: embargoConcepto
});

export const DeleteEmbargoConceptoSuccess = (
    embargoConcepto: EmbargoConceptoDto
): AppActions => ({
    type: DELETE_EMBARGOS_CONCEPTO_SUCCESS,
    embargoConcepto: embargoConcepto
});

export const GetAllBeneficiariosEmbargo = (
    embargoBeneficiarios: EmbargoBeneficiariosDto[],
): AppActions => ({
    type: GET_ALL_BENEFICIARIOS_PAGO_SUCCESS,
    embargoBeneficiarios: embargoBeneficiarios
});

export const PostInsertEmbargoBeneficiarioSuccess = (
    embargoBeneficiario: EmbargoBeneficiariosDto,
): AppActions => ({
    type: POST_INSERT_BENEFICIARIO_PAGO_SUCCESS,
    embargoBeneficiario: embargoBeneficiario
});

export const PostUpdateEmbargoBeneficiarioSuccess = (
    embargoBeneficiario: EmbargoBeneficiariosDto,
): AppActions => ({
    type: POST_UPDATE_BENEFICIARIO_PAGO_SUCCESS,
    embargoBeneficiario: embargoBeneficiario
});

export const DeleteEmbargoBeneficiarioSuccess = (
    embargoBeneficiario: EmbargoBeneficiariosDto,
): AppActions => ({
    type: DELETE_BENEFICIARIO_PAGO_SUCCESS,
    embargoBeneficiario: embargoBeneficiario
});

export const PostInsertDesembargoSuccess = (
    embargoConcepto: EmbargoConceptoDto,
): AppActions => ({
    type: POST_INSERT_DESEMBARGO_EMBARGO_CONCEPTO_SUCCESS,
    embargoConcepto: embargoConcepto
});

export const GetAllDesembargosCuenta = (
    embargosConcepto: EmbargoConceptoDto[],
): AppActions => ({
    type: GET_ALL_DESEMBARGOS_CUENTA_SUCCESS,
    embargosConcepto: embargosConcepto
});

export const GetAllConceptosSuccess = (
    conceptos: any,
): AppActions => ({
    type: GET_ALL_CONCEPTOS_SUCCESS,
    conceptos: conceptos
});

export const SetError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startGetTiposEmbargo =
    () => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getTiposEmbargos();
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllTiposEmbargosSuccess(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };

export const startGetTiposProcesoEmbargo =
() => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.getTiposProcesosEmbargos();
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(GetAllTiposProcesosEmbargosSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startGetEmbargos =
() => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.getAllEmbargos();
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(GetAllEmbargosSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startGetEmbargosByDate =
(fechaInicial: Date, fechaFinal: Date) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postAllEmbargosByDate(fechaInicial,fechaFinal);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(PostAllEmbargosByDateSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startUpdateAllEmbargos =
 (embargos: EmbargoDto[]) => async (dispatch: Dispatch<AppActions>) =>{
  dispatch(GetAllUpdateEmbargosSuccess(embargos));
};

export const startPostInsertEmbargo =
(embargo: EmbargoModelDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postInsertEmbargo(embargo);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(PostInsertEmbargoSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startPostUpdateEmbargo =
(embargo: EmbargoModelDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postUpdateEmbargo(embargo);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(PostUpdateEmbargoSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

// export const startDeleteEmbargo =
// (embargo: EmbargoDto) => async (dispatch: Dispatch<AppActions>) => {
//     try {
//         const response = await api.deleteEmbargo(embargo);
//         if (response?.isSuccess == null) {
//             dispatch(SetError(response));
//         } else {
//             dispatch(DeleteEmbargoSuccess(response.data));
//         }
//     } catch (e) {
//         dispatch(SetError('server-error'));
//     }
// };

export const startGetTiposRetencion =
    () => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getTiposRetencion();
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllTiposRetencionSuccess(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };

export const startGetAllEmbargosCuentaConcepto =
() => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.getEmbargosCuentaConcepto();
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(GetAllEmbargosConceptoSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startPostInsertEmbargoConcepto =
(embargo: EmbargoConceptoDto, conceptoId: number) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postInsertEmbargoCuentaConcepto(embargo, conceptoId);
        if (response?.isSuccess === null) {
            dispatch(SetError(response));
        } else if(response?.isSuccess === false){
            dispatch(SetError(response.returnMessage));
        }  else if(response?.isSuccess === true){
            dispatch(PostInsertEmbargoConceptoSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startPostUpdateEmbargoConcepto =
(embargo: EmbargoConceptoDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postUpdateEmbargoCuentaConcepto(embargo);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else if(response?.isSuccess === false){
            dispatch(SetError(response.respuesta));
        }  else if(response?.isSuccess === true){
            dispatch(PostUpdateEmbargoConceptoSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startDeleteEmbargoConcepto =
(embargo: EmbargoConceptoDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.deleteEmbargoConcepto(embargo);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(DeleteEmbargoConceptoSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startGetAllBeneficiariosEmbargo =
    () => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getAllBeneficiariosEmbago();
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllBeneficiariosEmbargo(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };

export const startPostInsertBeneficiarioPago =
(embargoBeneficiario: EmbargoBeneficiariosDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postInsertEmbargoBeneficiario(embargoBeneficiario);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(PostInsertEmbargoBeneficiarioSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startPostUpdateEmbargoBeneficiario =
(embargoBeneficiario: EmbargoBeneficiariosDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postUpdateEmbargoBeneficiario(embargoBeneficiario);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(PostUpdateEmbargoBeneficiarioSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startDeleteEmbargoBeneficiario =
(embargoBeneficiario: EmbargoBeneficiariosDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.deleteEmbargoBeneficiario(embargoBeneficiario);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(DeleteEmbargoBeneficiarioSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startPostInsertDesembargo =
(embargoConcepto: EmbargoConceptoDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
        const response = await api.postInsertDesembargo(embargoConcepto);
        if (response?.isSuccess == null) {
            dispatch(SetError(response));
        } else {
            dispatch(PostInsertDesembargoSuccess(response.data));
        }
    } catch (e) {
        dispatch(SetError('server-error'));
    }
};

export const startGetAllDesembargos=
    () => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getAllDesembargos();
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllDesembargosCuenta(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };

    export const startGetAllConceptosEmbargosById=
    (idEmbargo: string) => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.getObtenerConceptosEmbargoById(idEmbargo);
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(GetAllConceptosSuccess(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };