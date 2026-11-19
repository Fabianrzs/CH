import { Dispatch } from "react";
import ApiCourts from "../services/api-courts.service";
import NotificationService from "../services/notification.service";
import { AppActions } from "../types/appactions.type";
import { JuzgadoDto } from "../types/court.model.type";
import {
    DELETE_COURT_SUCCESS, ON_ERROR,
    POST_ALL_COURTS_BY_CITY_SUCCESS,
    POST_ALL_COURTS_BY_DEPARTAMENT_SUCCESS,
    POST_INSERT_COURT_SUCCESS,
    POST_UPDATE_COURT_SUCCESS
} from "../types/courts.actions.type";


const api = ApiCourts();

export const PostAllCourtsByDepartamentSuccess = (
    courts: JuzgadoDto[],
): AppActions => ({
    type: POST_ALL_COURTS_BY_DEPARTAMENT_SUCCESS,
    courts: courts
});

export const PostAllCourtsByCitySuccess = (
    courts: JuzgadoDto[],
): AppActions => ({
    type: POST_ALL_COURTS_BY_CITY_SUCCESS,
    courts: courts
});

export const PostInsertCourtSuccess = (
    court: JuzgadoDto,
): AppActions => ({
    type: POST_INSERT_COURT_SUCCESS,
    court: court
});

export const PostUpdateCourtSuccess = (
    court: JuzgadoDto,
): AppActions => ({
    type: POST_UPDATE_COURT_SUCCESS,
    court: court
});

export const deleteCourtSuccess = (
    court: JuzgadoDto,
): AppActions => ({
    type: DELETE_COURT_SUCCESS,
    court: court
});

export const SetError = (error: any): AppActions => ({
    type: ON_ERROR,
    error: error,
});

export const startPostAllCourtsByDepartament =
    (departament: number) => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.postAllCourtsByDepartament(departament);
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(PostAllCourtsByDepartamentSuccess(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };

export const startPostAllCourtsByCity =
    (departament: number, city: number) => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.postAllCourtsByCity(departament, city);
            if (response?.isSuccess == null) {
                dispatch(SetError(response));
            } else {
                dispatch(PostAllCourtsByCitySuccess(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };

// export const startPostInsertCourt =
//     (juzgado: JuzgadoDto) => async (dispatch: Dispatch<AppActions>) => {
//         try {
//             const response = await api.postInsertCourts(juzgado);

//             if (response?.isSuccess == null || !response.isSuccess) {
//                 dispatch(SetError(response));
//                 await NotificationService.genericAlertError("ERROR", response?.returnMessage || "No se pudo guardar la información.");
//             } else {
//                 dispatch(PostInsertCourtSuccess(response.data));
//                 await NotificationService.genericAlert(
//                     'Guardado',
//                     'Se ha guardado correctamente la información',
//                 );
//             }
//         } catch (e: any) {
//             if (e.response && e.response.status === 409) {
//                 const serverResponse = e.response.data;
//                 await NotificationService.genericAlertError("ERROR", `${serverResponse.returnMessage} Puede estar inactivo.`);
//             } else {
//                 dispatch(SetError('server-error'));
//             }
//         }
//     };

// export const startPostInsertCourt = 
//     (juzgado: JuzgadoDto) => async (dispatch: Dispatch<AppActions>) => {
//         try {
//             const response = await api.postInsertCourts(juzgado);

//             if (response?.isSuccess == null || !response.isSuccess) {
//                 dispatch(SetError(response));
//                 await NotificationService.genericAlertError("ERROR", response?.returnMessage || "No se pudo guardar la información.");
//                 return response;  // Devolver la respuesta aunque sea con error
//             } 
//                 dispatch(PostInsertCourtSuccess(response.data));
//                 await NotificationService.genericAlert(
//                     'Guardado',
//                     'Se ha guardado correctamente la información',
//                 );
//                 return response;  // Devolver la respuesta exitosa
            
//         } catch (e: any) {
//             if (e.response && e.response.status === 409) {
//                 const serverResponse = e.response.data;
//                 await NotificationService.genericAlertError("ERROR", `${serverResponse.returnMessage} Puede estar inactivo.`);
//             } else {
//                 dispatch(SetError('server-error'));
//             }
//             return null;  // Devuelve null en caso de error
//         }
//     };


// export const startPostUpdateCourt =
//     (juzgado: JuzgadoDto) => async (dispatch: Dispatch<AppActions>) => {
//         try {
//             const response = await api.postUpdateCourts(juzgado);

//             if (response?.isSuccess == null || !response.isSuccess) {
//                 dispatch(SetError(response));
//                 await NotificationService.genericAlertError("ERROR", `${response?.returnMessage || "No se pudo actualizar la información."} Puede estar inactivo.`);
//             } else {
//                 dispatch(PostUpdateCourtSuccess(response.data));
//                 await NotificationService.genericAlert(
//                     'Actualizado con éxito',
//                     'Se realizó con éxito la actualización',
//                 );
//             }
//         } catch (e: any) {
//             if (e.response && e.response.status === 409) {
//                 const serverResponse = e.response.data;
//                 await NotificationService.genericAlertError("ERROR", `${serverResponse.returnMessage} Puede estar inactivo.`);
//             } else {
//                 dispatch(SetError('server-error'));
//             }
//         }
//     };

export const startPostInsertCourt =
    (juzgado: JuzgadoDto) => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.postInsertCourts(juzgado);

            if (!response?.isSuccess) {
                dispatch(SetError(response));
                await NotificationService.genericAlertError("ERROR", response?.returnMessage || "No se pudo guardar la información.");
                return response;  // Devolver respuesta con error, controlada
            } 

            dispatch(PostInsertCourtSuccess(response.data));
            await NotificationService.genericAlert(
                'Guardado',
                'Se ha guardado correctamente la información',
            );
            return response;  // Devolver respuesta exitosa

        } catch (e: any) {
            if (e.response && e.response.status === 409) {
                const serverResponse = e.response.data;
                await NotificationService.genericAlertError("ERROR", `${serverResponse.returnMessage} Puede estar inactivo.`);
            } else {
                dispatch(SetError('server-error'));
                await NotificationService.genericAlertError("ERROR", "Error de servidor al guardar el juzgado.");
            }
            return { isSuccess: false };  // Devuelve un objeto consistente
        }
    };

export const startPostUpdateCourt =
    (juzgado: JuzgadoDto) => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.postUpdateCourts(juzgado);

            if (!response?.isSuccess) {
                dispatch(SetError(response));
                await NotificationService.genericAlertError("ERROR", `${response?.returnMessage || "No se pudo actualizar la información."} Puede estar inactivo.`);
                return response;  // Devolver respuesta con error, controlada
            }

            dispatch(PostUpdateCourtSuccess(response.data));
            await NotificationService.genericAlert(
                'Actualizado con éxito',
                'Se realizó con éxito la actualización',
            );
            return response;  // Devolver respuesta exitosa

        } catch (e: any) {
            if (e.response && e.response.status === 409) {
                const serverResponse = e.response.data;
                await NotificationService.genericAlertError("ERROR", `${serverResponse.returnMessage} Puede estar inactivo.`);
            } else {
                dispatch(SetError('server-error'));
                await NotificationService.genericAlertError("ERROR", "Error de servidor al actualizar el juzgado.");
            }
            return { isSuccess: false };  // Devuelve un objeto consistente
        }
    };




export const startDeleteCourt =
    (juzgado: JuzgadoDto) => async (dispatch: Dispatch<AppActions>) => {
        try {
            const response = await api.deleteCourts(juzgado);
            if (response?.isSuccess === null) {
                dispatch(SetError(response));
            } else {
                dispatch(deleteCourtSuccess(response.data));
            }
        } catch (e) {
            dispatch(SetError('server-error'));
        }
    };
