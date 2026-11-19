import axios from 'axios';
import StorageService from './storage.service';
import { routega2api } from '../utils/baserouteapi.route';
import { SimuladorActorDto, SimuladorDto, SimuladorUndEjDto } from '../types/simulador.model.type';

    const ApiSimuladorTasas = () => {
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

    const getAllSimuladorTasas = () => 
        apiAxios             
            .get(`cartera/obtenerTasasSimulador`,{
                baseURL: routega2api,
            })
            .then(res => res.data);  

    const postInsertSimuladorTasas = (simulador: SimuladorDto) => 
        apiAxios             
            .post(`credito/CrearTasasSimulacion`, simulador,  {

                baseURL: routega2api,
            })
            .then(res => res.data);           

    const postUpdateSimuladorTasas = (simulador: SimuladorDto) => 
    apiAxios             
        .post(`cartera/ActualizarTasasSimulacion`,simulador, {
            // IdSimulacion: simulador.idSimulacion,
            // Descripcion: simulador.descripcion, 
            // TasaEA: simulador.tasaEA,
            // MinimoPlazoMeses: simulador.minimoPlazoMeses,
            // MaximoPlazoMeses: simulador.maximoPlazoMeses,
            // Estado: simulador.estado
            // min_Canon_Inicial: simulador.min_Canon_Inicial,
            // max_Canon_Inicial: simulador.max_Canon_Inicial,
            // max_Opc_Compra: simulador.max_Opc_Compra,
        // },{
            baseURL: routega2api,
        })
        .then(res => res.data); 
    
    const postDeleteSimuladorTasas = (simulador: SimuladorDto) =>
        apiAxios
            .post(`credito/EliminarTasasSimulacion`,{
                IdSimulacion: simulador.idSimulacion
            }, {
                baseURL: routega2api,
            })
            .then(res => res.data); 

    const postAllSimuladorActor = (idSimulacion: string) => 
        apiAxios             
            .post(`credito/ConsultaTipoActorById`, {
                idSimulacion: idSimulacion
            }, {
                baseURL: routega2api,
            })
            .then(res => res.data);
   
    const postAllTipoActor = () => 
        apiAxios             
            .post(`cartera/obtenerTipoActor`,{                
            },{
                baseURL: routega2api,
            })
            .then(res => res.data);  

    const postInsertSimuladorTipoActor = (idSimulador: string, simuladorActor: SimuladorActorDto) => 
    apiAxios             
        .post(`credito/CrearActorSimulacion`, {               
            simuladorId: idSimulador, 
            tipoId:simuladorActor.tipoId,
            parametroTasaEa: simuladorActor.parametroTasaEa
        }, {
            baseURL: routega2api,
        })
        .then(res => res.data);

    const postUpdateSimuladorTipoActor = (simuladorActor: SimuladorActorDto) => 
    apiAxios             
        .post(`credito/ActualizarTasaSimuladorActor`, simuladorActor,{
            baseURL: routega2api,
        })
        .then(res => res.data); 

    const postDeleteSimuladorTipoActor = (simuladorActor: SimuladorActorDto) =>
    apiAxios
        .post(`credito/EliminarActorSimulacion`,simuladorActor, {
            baseURL: routega2api,
        })
        .then(res => res.data); 

    const postAllSimuladorUnidadEjecutora = (idSimulacion: number) => 
    apiAxios             
        .post(`cartera/obtenerUnidadesEjecutorasPorId`, {
            idSimulacion: idSimulacion
        }, {
            baseURL: routega2api,
        })
        .then(res => res.data);
    
    const postInsertSimuladorUnidadEjecutoraTasas = (idSimulacion: number, simuladorUniEj: SimuladorUndEjDto) => 
    apiAxios             
        .post(`credito/CrearUnidadEjecutoraSimulador`, {               
                idSimulacion: idSimulacion,
                idUnidadEjecutora: simuladorUniEj.idParametroUnidadEjecutora,
                tasaEa: simuladorUniEj.tasaEA
        }, {
            baseURL: routega2api,
        })
        .then(res => res.data);    
    
    const getAllUnidadEjecutora = () => 
    apiAxios             
        .get(`cartera/consultarUnidadEjecutora`,{
            baseURL: routega2api,
        })
        .then(res => res.data); 
        
    const postUpdateSimuladorUnidadEjecutora = (simuladorUniEj: SimuladorUndEjDto) => 
    apiAxios             
        .post(`credito/ActualizarUnidadEjecutoraSimulador`, {
            idParametro: simuladorUniEj.idParametro,
            idUnidadEjecutora: simuladorUniEj.idParametroUnidadEjecutora,
            tasaEa: simuladorUniEj.tasaEA,
            estado: simuladorUniEj.estado 
        },{
            baseURL: routega2api,
        })
        .then(res => res.data);   
        
    const postDeleteSimuladorUnidadEjecutora = (simuladorUniej: SimuladorUndEjDto) =>
    apiAxios
        .post(`credito/EliminarUnidadEjecutoraSimulacion`,{
            idParametro: simuladorUniej.idParametro
        }, {
            baseURL: routega2api,
        })
        .then(res => res.data); 

    return {
        getAllSimuladorTasas,
        postInsertSimuladorTasas,
        postUpdateSimuladorTasas,
        postDeleteSimuladorTasas,
        postAllSimuladorActor,
        postAllTipoActor,
        postInsertSimuladorTipoActor,
        postUpdateSimuladorTipoActor,
        postDeleteSimuladorTipoActor,
        postAllSimuladorUnidadEjecutora,
        postInsertSimuladorUnidadEjecutoraTasas,
        getAllUnidadEjecutora,
        postUpdateSimuladorUnidadEjecutora,
        postDeleteSimuladorUnidadEjecutora 
    };
};

export default ApiSimuladorTasas;