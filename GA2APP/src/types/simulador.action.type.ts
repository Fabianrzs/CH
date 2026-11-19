import { SimuladorDto, 
         SimuladorActorDto, 
         SimuladorUndEjDto, 
         TipoActorDto, 
         UnidadEjecutoraDto } 
        from "./simulador.model.type";

export const GET_SIMULADOR = 'GET_SIMULADOR'; 
export const GET_SIMULADOR_FAILURE = 'GET_SIMULADOR_FAILURE';
export const UPDATE_SIMULADOR = 'UPDATE_SIMULADOR';  
export const UPDATE_SIMULADOR_FAILURE = 'UPDATE_SIMULADOR_FAILURE'; 
export const INSERT_SIMULADOR = 'INSERT_SIMULADOR';  
export const DELETE_SIMULADOR = 'DELETE_SIMULADOR';  
export const CREAR_SIMULADOR = 'CREAR_SIMULADOR'; 
export const ACTUALIZAR_SIMULADOR = 'ACTUALIZAR_SIMULADOR'; 
export const OBTENER_SIMULADOR = 'OBTENER_SIMULADOR'; 
export const ELIMINAR_SIMULADOR = 'ELIMINAR_SIMULADOR'; 

export const GET_SIMULADOR_ACTOR = 'GET_SIMULADOR_ACTOR'; 
export const GET_SIMULADOR_ACTOR_FAILURE = 'GET_SIMULADOR_ACTOR_FAILURE';
export const UPDATE_SIMULADOR_ACTOR = 'UPDATE_SIMULADOR_ACTOR';  
export const UPDATE_SIMULADOR_ACTOR_FAILURE = 'UPDATE_SIMULADOR_ACTOR_FAILURE'; 
export const CREAR_SIMULADOR_ACTOR = 'CREAR_SIMULADOR_ACTOR'; 
export const INSERT_SIMULADOR_ACTOR = 'INSERT_SIMULADOR_ACTOR'; 
export const DELETE_SIMULADOR_ACTOR = 'DELETE_SIMULADOR_ACTOR'; 
export const ACTUALIZAR_SIMULADOR_ACTOR = 'ACTUALIZAR_SIMULADOR_ACTOR'; 
export const OBTENER_SIMULADOR_ACTOR = 'OBTENER_SIMULADOR_ACTOR'; 
export const ELIMINAR_SIMULADOR_ACTOR = 'ELIMINAR_SIMULADOR_ACTOR'; 

export const GET_SIMULADOR_UNIEJ = 'GET_SIMULADOR'; 
export const GET_SIMULADOR_UNIEJ_FAILURE = 'GET_SIMULADOR_FAILURE';
export const INSERT_SIMULADOR_UNIEJ = 'INSERT_SIMULADOR_UNIEJ';  
export const UPDATE_SIMULADOR_UNIEJ = 'UPDATE_SIMULADOR_UNIEJ';  
export const UPDATE_SIMULADOR_UNIEJ_FAILURE = 'UPDATE_SIMULADOR_UNIEJ_FAILURE'; 
export const DELETE_SIMULADOR_UNIEJ = 'DELETE_SIMULADOR_UNIEJ'; 
export const CREAR_SIMULADOR_UNIEJ = 'CREAR_SIMULADOR_UNIEJ';
export const ACTUALIZAR_SIMULADOR_UNIEJ = 'ACTUALIZAR_SIMULADOR_UNIEJ'; 
export const OBTENER_SIMULADOR_UNIEJ = 'OBTENER_SIMULADOR_UNIEJ'; 
export const ELIMINAR_SIMULADOR_UNIEJ = 'ELIMINAR_SIMULADOR_UNIEJ'; 

export const GET_TIPO_ACTOR = 'GET_TIPO_ACTOR'; 
export const OBTENER_TIPO_ACTOR = 'OBTENER_TIPO_ACTOR';
export const GET_TIPO_ACTOR_FAILURE = 'GET_TIPO_ACTOR_FAILURE';

export const GET_UNIDAD_EJECUTORA = 'GET_UNIDAD_EJECUTORA'; 
export const OBTENER_UNIDAD_EJECUTORA = 'OBTENER_UNIDAD_EJECUTORA';
export const GET_UNIDAD_EJECUTORA_FAILURE = 'GET_UNIDAD_EJECUTORA_FAILURE';

export const ERROR_SIMULADOR = 'ERROR_SIMULADOR';


export interface GetSimulador {
    type: 'GET_SIMULADOR';    
}
export interface GetSimuladorFailure {
    type: 'GET_SIMULADOR_FAILURE';    
    error: any;
}
export interface UpdateSimulador {
    type: 'UPDATE_SIMULADOR';    
}
export interface UpdateSimuladorFailure {
    type: 'UPDATE_SIMULADOR_FAILURE';  
    error: any;  
}
export interface InsertSimulador {
    type: 'INSERT_SIMULADOR';    
}
export interface DeleteSimulador {
    type: 'DELETE_SIMULADOR';    
}
export interface CrearSimulador {
    type: 'CREAR_SIMULADOR';
    simulador: SimuladorDto;
}
export interface ActualizarSimulador {
    type: 'ACTUALIZAR_SIMULADOR';
    simulador: SimuladorDto;
}

export interface ObtenerSimulador {
    type: 'OBTENER_SIMULADOR';
    simuladorGeneral: SimuladorDto [];
}
export interface EliminarSimulador {
    type: 'ELIMINAR_SIMULADOR';
    simulador: SimuladorDto;
}



export interface GetSimuladorActor {
    type: 'GET_SIMULADOR_ACTOR';    
}

export interface GetSimuladorActorFailure {
    type: 'GET_SIMULADOR_ACTOR_FAILURE';    
    error: any;
}

export interface UpdateSimuladorActor {
    type: 'UPDATE_SIMULADOR_ACTOR';    
}
export interface UpdateSimuladorActorFailure {
    type: 'UPDATE_SIMULADOR_ACTOR_FAILURE';  
    error: any;  
}
export interface InsertSimuladorActor {
    type: 'INSERT_SIMULADOR_ACTOR';    
}

export interface DeleteSimuladorActor {
    type: 'DELETE_SIMULADOR_ACTOR';    
}
export interface CrearSimuladorActor {
    type: 'CREAR_SIMULADOR_ACTOR';
    simuladorActor: SimuladorActorDto;
}

export interface ActualizarSimuladorActor {
    type: 'ACTUALIZAR_SIMULADOR_ACTOR';
    simuladorActor: SimuladorActorDto;
}

export interface ObtenerSimuladorActor {
    type: 'OBTENER_SIMULADOR_ACTOR';
    simuladorActores: SimuladorActorDto [];
}

export interface EliminarSimuladorActor {
    type: 'ELIMINAR_SIMULADOR_ACTOR';
    simuladorActor: SimuladorActorDto;
}



export interface GetSimuladorUniej {
    type: 'GET_SIMULADOR_UNIEJ';    
}

export interface GetSimuladorUniejFailure {
    type: 'GET_SIMULADOR_UNIEJ_FAILURE';    
    error: any;
}
export interface UpdateSimuladorUniej {
    type: 'UPDATE_SIMULADOR_UNIEJ';    
}
export interface UpdateSimuladorUniejFailure {
    type: 'UPDATE_SIMULADOR_UNIEJ_FAILURE';  
    error: any;  
}

export interface InsertSimuladorUniej {
    type: 'INSERT_SIMULADOR_UNIEJ';
}

export interface DeleteSimuladorUniej {
    type: 'DELETE_SIMULADOR_UNIEJ';    
}
export interface CrearSimuladorUniej {
    type: 'CREAR_SIMULADOR_UNIEJ';
    simuladorUniej: SimuladorUndEjDto;
}
export interface ActualizarSimuladorUniej {
    type: 'ACTUALIZAR_SIMULADOR_UNIEJ';
    simuladorUniej: SimuladorUndEjDto;
}

export interface ObtenerSimuladorUniej {
    type: 'OBTENER_SIMULADOR_UNIEJ';
    simuladorUniEjecutoras: SimuladorUndEjDto [];
}

export interface EliminarSimuladorUniej {
    type: 'ELIMINAR_SIMULADOR_UNIEJ';
    simuladorUniej: SimuladorUndEjDto;
}


export interface GetTipoActor {
    type: 'GET_TIPO_ACTOR';
}
export interface ObtenerTipoActor {
    type: 'OBTENER_TIPO_ACTOR';
    simuladorTipoActor: TipoActorDto [];
}
export interface GetTipoActorFailure {
    type: 'GET_TIPO_ACTOR_FAILURE';    
    error: any;
}



export interface GetUnidadEjecutora {
    type: 'GET_UNIDAD_EJECUTORA';
}
export interface ObtenerUnidadEjecutora {
    type: 'OBTENER_UNIDAD_EJECUTORA';
    simuladorunidadEjecutora: UnidadEjecutoraDto [];
}

export interface GetUnidadEjecutoraFailure {
    type: 'GET_UNIDAD_EJECUTORA_FAILURE';    
    error: any;
}


export interface ErrorSimulador {
    type: 'ERROR_SIMULADOR';
    error: any;
}

export type SimuladorActionTypes =

    | GetSimulador
    | GetSimuladorFailure
    | UpdateSimulador
    | UpdateSimuladorFailure
    | InsertSimulador
    | DeleteSimulador
    | CrearSimulador
    | ActualizarSimulador
    | ObtenerSimulador
    | EliminarSimulador
    | GetSimuladorActor
    | GetSimuladorActorFailure
    | UpdateSimuladorActor
    | UpdateSimuladorActorFailure
    | InsertSimuladorActor
    | DeleteSimuladorActor
    | CrearSimuladorActor
    | ActualizarSimuladorActor
    | ObtenerSimuladorActor
    | EliminarSimuladorActor
    | GetSimuladorUniej
    | GetSimuladorUniejFailure
    | UpdateSimuladorUniej
    | UpdateSimuladorUniejFailure
    | DeleteSimuladorUniej
    | CrearSimuladorUniej
    | InsertSimuladorUniej
    | ActualizarSimuladorUniej 
    | ObtenerSimuladorUniej
    | EliminarSimuladorUniej
    | GetTipoActor
    | ObtenerTipoActor
    | GetTipoActorFailure
    | GetUnidadEjecutora
    | ObtenerUnidadEjecutora
    | GetUnidadEjecutoraFailure
    | ErrorSimulador;

