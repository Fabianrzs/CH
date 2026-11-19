import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { UnidadEjecutoraCronogramaDTO, UnidadEjecutoraCronogramaEditarDTO } from '../types/cronograma.type';

const ApiSchedule = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: `${routega2api}`,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getExecutingUnitsSchedule = () =>
        apiAxios
            .get('cronograma/obtenerUnidadesEjecutorasCronograma', {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const editExecutingUnitSchedule = (unidadEjecutoraCronogramaFechaDeReporte: UnidadEjecutoraCronogramaEditarDTO) =>
        apiAxios
            .patch('cronograma/editarFechaDeReporte', unidadEjecutoraCronogramaFechaDeReporte, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const deleteExecutingUnitSchedule = (eliminarUnidadEjecutora: UnidadEjecutoraCronogramaDTO | undefined) =>
        apiAxios
            .delete(`cronograma/eliminarUnidadEjecutoraCronograma?idCronograma=${eliminarUnidadEjecutora?.id}`, {
                baseURL: routega2api
            })
            .then(res => res.data);

    const addExecutingUnitSchedule = (agregarUnidadEjecutora: UnidadEjecutoraCronogramaDTO) =>
        apiAxios
            .post('cronograma/AgregarUnidadEjecutoraCronograma', agregarUnidadEjecutora, {
                baseURL: routega2api,
            })
            .then(res => res.data);


    const deleteNovelty = (idNovedad: number) =>
        apiAxios
            .delete(`cronograma/EliminarNovedades?idNovedad=${idNovedad}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const ObtenerNovedadesUnidadEjecutora = (idUnidadEjecutora: number) =>
        apiAxios
            .get(`cronograma/ObtenerNovedadesUnidadEjecutora?idUnidadEjecutora=${idUnidadEjecutora}`)
            .then((res: any) => res.data);

    const InsertarNovedadUsuario = (data: any) =>
        apiAxios
            .post(`cronograma/InsertarNovedadAfiliado`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const ObtenerNovedadesPorUnidadEjecutora = (data: any) =>
        apiAxios
            .post(`cronograma/ObtenerNovedadesPorUnidadEjecutora`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const EliminarNovedades = (data: any) =>
        apiAxios
            .delete(`cronograma/EliminarNovedadAfiliado?idNovedad=${data}`, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const GenerarArchivo = (data: any) =>
        apiAxios
            .post(`cronograma/GeneracionArchivoNovedades`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const ObtenerNovedadAfiliadoPorId = (idNovedad: number) =>
        apiAxios
            .get(`cronograma/ObtenerNovedadAfiliadoPorId?idNovedad=${idNovedad}`, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const EditarNovedadAfiliado = (data: any) =>
        apiAxios
            .post(`cronograma/EditarNovedadAfiliado`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const InsertarAudoriaNovedades = (data: any) =>
        apiAxios
            .post(`cronograma/InsertarAudoriaNovedades`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const ValidaNovedadesFondoSolidaridad = (data: any): Promise<any> =>
        apiAxios
            .post(`cronograma/ValidaNovedadesFondoSolidaridad`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const InsertarDescargaDefinitiva = (data: any) =>
        apiAxios
            .post(`cronograma/InsertarDescargaDefinitiva`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const ObtenerArchivosDescargaDefinitiva = (data: any) =>
        apiAxios
            .post(`cronograma/ObtenerArchivosDescargaDefinitiva`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    const ActualizarEstadoDescargaDefinitiva = (data: any) =>
        apiAxios
            .post(`cronograma/ActualizarEstadoDescargaDefinitiva`, data, {
                baseURL: routega2api,
            })
            .then((res: any) => res.data);

    return {
        getExecutingUnitsSchedule,
        editExecutingUnitSchedule,
        deleteExecutingUnitSchedule,
        addExecutingUnitSchedule,
        deleteNovelty,
        ObtenerNovedadesUnidadEjecutora,
        InsertarNovedadUsuario,
        ObtenerNovedadesPorUnidadEjecutora,
        EliminarNovedades,
        GenerarArchivo,
        ObtenerNovedadAfiliadoPorId,
        EditarNovedadAfiliado,
        InsertarAudoriaNovedades,
        ValidaNovedadesFondoSolidaridad,
        InsertarDescargaDefinitiva,
        ObtenerArchivosDescargaDefinitiva,
        ActualizarEstadoDescargaDefinitiva
    };
};

export default ApiSchedule;
