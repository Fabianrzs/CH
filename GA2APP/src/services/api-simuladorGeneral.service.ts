import axios from "axios";
import StorageService from "./storage.service";
import { routega2api } from '../utils/baserouteapi.route';
import { SimuladorGeneralDto } from "../types/simuladorGeneral.model.type";

const ApiSimuladorGeneral = () => {
    
    const apiAxios = axios.create({
        headers: {
            'Cache-Control': 'no-cache',
            'Content-Type': 'application/json',
            'X-Frame-Options': 'sameorigin'
        },
        timeout: 15000,
    });

    apiAxios.interceptors.request.use(async (config: any) => {
        const token = StorageService.getSessionToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    const getParametrosGenerales = () => 
        apiAxios             
            .get(`cartera/obtenerParamGeneralesCreditoYCartera`,{
                baseURL: routega2api,
            })
            .then(res => res.data);  
    
    const postUpdateParametrosGenerales = async (simulador: SimuladorGeneralDto) => 
     apiAxios      
        .post(`credito/actualizarParamGeneralesCreditoYCartera`, 
            simulador,{
            baseURL: routega2api,
        })
        .then(res => res.data); 

    return {
        getParametrosGenerales,
        postUpdateParametrosGenerales        
    };

};

export default ApiSimuladorGeneral;
