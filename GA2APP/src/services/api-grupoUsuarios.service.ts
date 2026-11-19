import axios from 'axios';
import StorageService from './storage.service';
import { routeidentityapi } from '../utils/baserouteapi.route';
import { GrupoUsersModel } from '../types/grupoUsers.model';

const GrupoUsers = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
        },
        baseURL: routeidentityapi,
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async config => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });



    const getAllGrupoUsuarios = () => 
    apiAxios             
        .get(`GetAllGrupoUsuarios`,{
            baseURL: routeidentityapi,
        })
        .then(res => res.data); 


    const getAllGrupoOpciones = () => 
    apiAxios             
        .get(`GetAllGrupoOpciones`,{
            baseURL: routeidentityapi,
        })
        .then(res => res.data); 

        const getAllOpciones = () => 
        apiAxios             
            .get(`getAllOpciones`,{
                baseURL: routeidentityapi,
            })
            .then(res => res.data); 

        const InserGrupoUsuario = async (grupo: GrupoUsersModel) => 
            apiAxios
                .post(`CreateGrupoUsuarios`,grupo,{
                    baseURL: routeidentityapi,
                })
                .then(res => res.data);

        const InserGrupoOpciones = async (grupo: GrupoUsersModel) => 
            apiAxios
                .post(`CrearGurpoOpciones`,grupo)
                .then(res => res.data);

        const InserOpciones = async (grupo: GrupoUsersModel) => 
            apiAxios
                .post(`CrearOpciones`,grupo, {
                    baseURL:routeidentityapi  
                })
                .then(res => res.data);

        const DeleteGrupoUsuario = async (grupo: GrupoUsersModel) => 
            apiAxios
                .post(`deletegrupousuario`,grupo, {
                    baseURL:routeidentityapi
                })
                .then(res => res.data);

        const DeleteGrupoOpciones = async (grupo: GrupoUsersModel) => 
            apiAxios
                .post(`eliminagrupoopciones`,grupo, {
                    baseURL:routeidentityapi
                })
                .then(res => res.data);


        const ActualizaGrupoOpciones = async (grupo: GrupoUsersModel) => 
            apiAxios
                .post(`ActualizaGrupoOpciones`,grupo, {
                    baseURL:routeidentityapi
                })
                .then(res => res.data);


        const ActualizaGrupoUsuarios = async (grupo: GrupoUsersModel) => 
            apiAxios
                .post(`UpdateGrupoUser`,grupo, {
                    baseURL:routeidentityapi  
                })
                .then(res => res.data);

        const ObtenerOpcionesTemporales = async (idUsuario: string) => 
            apiAxios
                .get(`ObtenerOpcionesTemporalesUsuario?request=${idUsuario}`, {
                    baseURL:routeidentityapi
                })
                .then(res => res.data);

        const InsertarOpcionesTemporales = async (data: any) => 
            apiAxios
                .post(`InsertarOpcionesTemporalesUsuario`, data,{
                    baseURL:routeidentityapi 
                })
                .then(res => res.data);

        const ActualizarOpcionesTemporales = async (data: any) => 
            apiAxios
                .post(`EditarOpcionesTemporalesUsuario`, data,{
                    baseURL:routeidentityapi
                })
                .then(res => res.data);

        const ObtenerGruposUsuarioPorId = async (idUsuario: string) => 
                apiAxios
                    .post(`ObtenerGruposUsuariosPorId?request=${idUsuario}`, {
                        baseURL:routeidentityapi
                    })
                    .then(res => res.data);
        
    return {
        getAllGrupoUsuarios,
        getAllGrupoOpciones,
        InserGrupoUsuario,
        DeleteGrupoUsuario,
        InserGrupoOpciones,
        DeleteGrupoOpciones,
        ActualizaGrupoOpciones,
        ActualizaGrupoUsuarios,
        getAllOpciones,
        InserOpciones,
        ObtenerOpcionesTemporales,
        ActualizarOpcionesTemporales,
        InsertarOpcionesTemporales,
        ObtenerGruposUsuarioPorId
    }


}

export default GrupoUsers;