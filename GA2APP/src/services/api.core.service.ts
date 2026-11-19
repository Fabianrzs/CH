import axios from 'axios';
import { Response } from '../types/response.type';
import { Login } from '../types/login.type';
import StorageService from './storage.service';
import { routeidentityapi } from '../utils/baserouteapi.route';

const Api = () => {
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'mode':'no-cors',
            'X-Frame-Options': 'sameorigin'
        },
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async config => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const login =  (login: Login) =>         
        apiAxios
            .post(`login`, login, {
                baseURL: routeidentityapi,
            })
            .then(res => res.data)
            .catch(error => error);

    const UpdateSession = (user: string) => 
        apiAxios
            .get(`UpdateSession?idUsuario=${user}`, {
                baseURL: routeidentityapi
            })
            .then(res => res.data)
            .catch(error => error);

    const Logout = (user: string) => 
        apiAxios
            .get(`Logout?idUsuario=${user}`, {
                baseURL: routeidentityapi
            })
            .then(res => res.data)
            .catch(error => error);
    return {
        login,
        UpdateSession,
        Logout
    };
};

export default Api;
