import { Dispatch } from "react";
import { toast } from "react-toastify";
import ApiNovedades from "../services/api-novedades.service"
import { AppActions } from "../types/appactions.type";
import { NovedadesAfiliadoDto } from "../types/novedad.model.type";
import { GET_ALL_CONCEPTOS_SUCCESS, NovedadesType, NOVEDADES_GET_NOVEDADES_AFILIADO_SUCCESS, NOVEDADES_POST_INSERTAR_NOVEDAD_AFILIADO_SUCCESS, ON_ERROR } from "../types/novedades.actions.type";


const api = ApiNovedades();

export const GetNovedadesAfiliadoSuccess = (novedadesConcepto: NovedadesAfiliadoDto,): NovedadesType => ({
    type: NOVEDADES_GET_NOVEDADES_AFILIADO_SUCCESS,
    novedadesConcepto: novedadesConcepto
});

export const PostInsertarNovedadAfiliadoSuccess = (novedadesConcepto: NovedadesAfiliadoDto,): NovedadesType => ({
    type: NOVEDADES_POST_INSERTAR_NOVEDAD_AFILIADO_SUCCESS,
    novedadesConcepto: novedadesConcepto
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

const novedad: NovedadesAfiliadoDto = {
    idNovedad: 0,
    idTipoIdentificacion: 0,
    identificacion: "",
    idConceptoNovedad: 0,
    fechaInicioNovedad: new Date(),
    fechaFinNovedad: new Date(),
    porcentaje: 0,
    valor: 0,
    id: 0,
    idFuerza: 0,
    idUnidadEjecutora: 0,
    idUnidadFuerza: 0,
    fuerzaDescripcion: "",
    nombreUnidadEjecutora: "",
    fechaReporte: new Date(),
    fechaInicio: new Date(),
    fechaFin: new Date(),
    periodo: "",
    idFormato: 0,
    descripcionFormato: "",
    idMedioEnvio: 0,
    descripcionMedioEnvio: "",
    codigoNovedadAlta: "",
    codigoNovedadBaja: ""
};

export const startGetNovedadesAfiliado = (idType: number, idNumber: string) => async (dispatch: Dispatch<AppActions>) => {
    
    try {
      const responseGetNovedades = await api.getObtenerNovedadesAfiliado(
        idType,
        idNumber,
      );
    
      if (responseGetNovedades?.isSuccess == null) {
        dispatch(GetNovedadesAfiliadoSuccess(responseGetNovedades?.data || []));
      } else {
        dispatch(GetNovedadesAfiliadoSuccess(responseGetNovedades.data));
        toast(responseGetNovedades.returnMessage);
      }
    } catch (error) {
      toast('Error al obtener los datos de novedades');
    }
};

export const startPostNovedadAfiliado = (novedad: NovedadesAfiliadoDto) => async (dispatch: Dispatch<AppActions>) => {
    try {
      const responseInsertNovedad = await api.postInsertarNovedadAfiliado(novedad);
    
      if (responseInsertNovedad?.isSuccess == null) {
        dispatch(PostInsertarNovedadAfiliadoSuccess(responseInsertNovedad?.data || []));
      } else {
        dispatch(PostInsertarNovedadAfiliadoSuccess(responseInsertNovedad.data));
        toast(responseInsertNovedad.returnMessage);
      }
    } catch (error) {
      toast('Error al insertar la novedad del afiliado');
    }
};


