import axios from "axios";
import { Recuperarcontrasena } from "../types/login.type";
import { ROLDTO } from "../types/rol.type";
import { User, UseRecuperaContrasena } from "../types/user.type";
import { routeidentityapi } from "../utils/baserouteapi.route";
import StorageService from "./storage.service";


const LoginUsers = () => {

    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'X-Frame-Options': 'sameorigin'
            
        },
    });

    apiAxios.interceptors.request.use(async config => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const ValidaTokenContrasena = (token: string) => 
    apiAxios             
        .get(`consultatoken?token=${token}`,{
            baseURL: routeidentityapi,
        })
        .then(res => res.data); 
    
    const createUserRequest = async (data: User) => 
        apiAxios
            .post('CreateUser', data, {
                baseURL: routeidentityapi
            })
            .then((res: any) => res.data);

    const getAllUsersRequest = () =>
        apiAxios
            .get('GetAllUsers', {
                baseURL: routeidentityapi
            })
            .then(res => res.data);

    const UpdateUserRequest = async (data: User) => 
        apiAxios
            .patch('UpdateUser', data, {
                baseURL: routeidentityapi
            })
            .then((res: any) => res.data);

    const DeleteUserRequest = async (data: User) => 
        apiAxios
            .post('DeleteUser', data, {
                baseURL: routeidentityapi
            })
            .then((res: any) => res.data);

    const RecupertaContrasena = async (data: Recuperarcontrasena) => 
        apiAxios
            .post('recuperacontrasena', data, {
                baseURL: routeidentityapi
            })
            .then((res: any) => res.data);  
    
    const ActualizaPassword = async (data: UseRecuperaContrasena) => 
        apiAxios
            .post('UpdatePassword', data, {
                baseURL: routeidentityapi
            })
            .then((res: any) => res.data);

    const getAllROLDTOsRequest = async () => 
        apiAxios
            .get('roles', {
                baseURL: routeidentityapi
            })
            .then((res: any) => res.data);
                
    const createROLDTORequest = async (rol: ROLDTO) => 
        apiAxios
            .post('roles', rol,{
                baseURL: routeidentityapi
            })
            .then((res: any) => res.data);
    return {
        ValidaTokenContrasena,
        createUserRequest,
        getAllUsersRequest,
        UpdateUserRequest,
        DeleteUserRequest,
        RecupertaContrasena,
        ActualizaPassword,
        getAllROLDTOsRequest,
        createROLDTORequest
    };
}

export default LoginUsers;