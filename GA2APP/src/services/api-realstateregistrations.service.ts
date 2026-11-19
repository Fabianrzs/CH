import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { AgregarMatriculaDTO, HistorialNovedadesMatriculasInmobiliarias, HistorialPropietariosDTO, MatriculasInmobiliariasDTO, MatriculasInmobiliariasFilterDTO, NovedadesInsertarDTO } from '../types/realStateRegistrations.type';

const ApiRealStateRegistrations = () => {
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

    const GetRealStateRegistrations = (matriculainmobiliaria: MatriculasInmobiliariasFilterDTO) =>
        apiAxios
            .post('filterMatriculasInmobiliaria', matriculainmobiliaria, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetBasicInformation = (idInformacionBasica: string) =>
        apiAxios
            .get(`informacionBasicaMatriculaInmobiliariaById?request=${idInformacionBasica}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetPropietaryHistory = (idHistorialPropietarios: string) =>
        apiAxios
            .get(`historialPropietariosMatriculaInmobiliariaById?request=${idHistorialPropietarios}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);
 
    const GetBloqueos = (idMatricula: string) =>
        apiAxios
            .get(`historialNovedadsMatriculaInmobiliariaById?request=${idMatricula}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const GetOperacionesBloqueos = () =>
        apiAxios
            .get(`listarOperacionesMatriculas`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const DeletePropietaryHistory = (eliminarHistorialPropietarios: string) =>
    {
        apiAxios
        .delete(`EliminarHistorialDePropietarios?request=${eliminarHistorialPropietarios}`, {
            baseURL: routega2api
        })
        .then(res => res.data);
    }
        

    const DeleteNoveltyBlocks = (novedadesBloqueosEliminar: string) =>
        apiAxios
            .get(`historialNovedadsMatriculaInmobiliariaById?request=${novedadesBloqueosEliminar}`, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const AddNoveltyBlocks = (agregarNovedadOperacion: HistorialNovedadesMatriculasInmobiliarias) =>
        apiAxios
            .post(`insertNovedMatriculaInmobiliaria`,agregarNovedadOperacion, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const AddRealStateRegistrations = (agregarMatricula: AgregarMatriculaDTO) =>
        apiAxios
            .post('CrearMatriculaInmobiliaria', agregarMatricula, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const DeleteRealStateRegistrations = (eliminarMatricula: MatriculasInmobiliariasDTO) =>
        apiAxios
            .post('EliminarMatriculaInmobiliaria', eliminarMatricula, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const UpdateRealStateRegistrations = (actualizarMatricula: MatriculasInmobiliariasDTO) =>
        apiAxios
            .post('EditarInformacionBasicaMatricula', actualizarMatricula, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const UpdatePropietaryHistory = (actualizarHistorialPropietarios: HistorialPropietariosDTO) =>
        apiAxios
            .post('EditarHistorialPropietarios', actualizarHistorialPropietarios, {
                baseURL: routega2api,
            })
            .then(res => res.data);

    const UpdateNoveltyBlocks = (actualizarNovevadesBloqueos: NovedadesInsertarDTO) =>
        apiAxios
            .post('actualizarNovedadesBloqueos', actualizarNovevadesBloqueos, {
                baseURL: routega2api,
            })
            .then(res => res.data);
	    
    const GetCausalTypeById = (typeIdOperation: string) =>
    apiAxios
        .get(`listarCausalMatriculaTipoById?request=${typeIdOperation}`, {
            baseURL: routega2api,
        })
        .then(res => res.data);

    const AddPropietaryHistory = (propietario: HistorialPropietariosDTO) =>
    apiAxios
        .post('AgregarHistorialDePropietario', propietario, {
            baseURL: routega2api,
        })
        .then(res => res.data);

    const GetChangeHistory = (idEntidad: string) =>
    apiAxios
        .get(`Entidades/ObtenerHistorialCambios?idEntidad=${idEntidad}`, {
            baseURL: routega2api,
        })
        .then(res => res.data);

    return {
        GetRealStateRegistrations,
        GetBasicInformation,
        GetPropietaryHistory,
        GetBloqueos,
        GetOperacionesBloqueos,
        DeletePropietaryHistory,
        DeleteNoveltyBlocks,
        AddNoveltyBlocks,
        AddRealStateRegistrations,
        DeleteRealStateRegistrations,
        UpdateRealStateRegistrations,
        UpdatePropietaryHistory,
        UpdateNoveltyBlocks,
	    GetCausalTypeById,
        AddPropietaryHistory,
        GetChangeHistory
    };
};

export default ApiRealStateRegistrations;
