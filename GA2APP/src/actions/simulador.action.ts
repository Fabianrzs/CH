import { Dispatch } from 'redux';
import { toast } from 'react-toastify';
import { AppActions } from '../types/appactions.type';
import { SimuladorActorDto,
         SimuladorDto, 
         SimuladorUndEjDto, 
         TipoActorDto, 
         UnidadEjecutoraDto} from '../types/simulador.model.type';
import { ACTUALIZAR_SIMULADOR, 
         ACTUALIZAR_SIMULADOR_ACTOR, 
         ACTUALIZAR_SIMULADOR_UNIEJ, 
         CREAR_SIMULADOR, 
         CREAR_SIMULADOR_ACTOR, 
         CREAR_SIMULADOR_UNIEJ, 
         DELETE_SIMULADOR, 
         DELETE_SIMULADOR_UNIEJ, 
         ELIMINAR_SIMULADOR, 
         ELIMINAR_SIMULADOR_ACTOR, 
         ELIMINAR_SIMULADOR_UNIEJ, 
         ERROR_SIMULADOR, 
         GET_SIMULADOR, 
         GET_SIMULADOR_ACTOR, 
         GET_SIMULADOR_ACTOR_FAILURE, 
         GET_SIMULADOR_FAILURE, 
         GET_SIMULADOR_UNIEJ, 
         GET_SIMULADOR_UNIEJ_FAILURE, 
         GET_TIPO_ACTOR, 
         GET_TIPO_ACTOR_FAILURE, 
         GET_UNIDAD_EJECUTORA, 
         GET_UNIDAD_EJECUTORA_FAILURE, 
         INSERT_SIMULADOR, 
         INSERT_SIMULADOR_UNIEJ, 
         OBTENER_SIMULADOR, 
         OBTENER_SIMULADOR_ACTOR, 
         OBTENER_SIMULADOR_UNIEJ, 
         OBTENER_TIPO_ACTOR, 
         OBTENER_UNIDAD_EJECUTORA, 
         UPDATE_SIMULADOR,
         UPDATE_SIMULADOR_ACTOR,
         UPDATE_SIMULADOR_UNIEJ} from '../types/simulador.action.type';
import ApiSimuladorTasas from '../services/api-simulador.service';

const api = ApiSimuladorTasas ();


export const GetSimulador = (): AppActions => ({
    type: GET_SIMULADOR,
});

export const GetSimuladorFailure = (error: any): AppActions => ({
    type: GET_SIMULADOR_FAILURE,
    error: error,
});

export const UpdateSimulador = (): AppActions => ({
    type: UPDATE_SIMULADOR,
});

export const UpdatetSimuladorFailure = (error: any): AppActions => ({
    type: GET_SIMULADOR_FAILURE,
    error: error,
});

export const InsertSimulador = (): AppActions => ({
    type: INSERT_SIMULADOR,
});

export const DeleteSimulador = (): AppActions => ({
    type: DELETE_SIMULADOR,
});

export const GetSimuladorTasas = (
    simuladorGeneral: SimuladorDto[],    
): AppActions => ({
    type: OBTENER_SIMULADOR,
    simuladorGeneral: simuladorGeneral
})

export const PostInsertSimuladorTasas = (
    simulador: SimuladorDto,
): AppActions => ({
    type: CREAR_SIMULADOR,
    simulador: simulador,
})

export const PostUpdateSimuladorTasas = (
    simulador: SimuladorDto, 
): AppActions => ({
    type: ACTUALIZAR_SIMULADOR,
    simulador: simulador,
})

export const PostDeleteSimuladorTasas = (
    simulador: SimuladorDto,
): AppActions => ({
    type: ELIMINAR_SIMULADOR,
    simulador: simulador,
})

export const SetError = (error: any): AppActions => ({
    type: ERROR_SIMULADOR,
    error: error,
});



export const GetSimuladorActor = (): AppActions => ({
    type: GET_SIMULADOR_ACTOR,
});

export const GetSimuladorActorFailure = (error: any): AppActions => ({
    type: GET_SIMULADOR_ACTOR_FAILURE,
    error: error,
});

export const GetSimuladorActorTasas = (
    simuladorActores: SimuladorActorDto[],    
): AppActions => ({
    type: OBTENER_SIMULADOR_ACTOR,
    simuladorActores: simuladorActores
})

export const GetTipoActor = (): AppActions => ({
    type: GET_TIPO_ACTOR,
});

export const GetTipoActorFailure = (error: any): AppActions => ({
    type: GET_TIPO_ACTOR_FAILURE,
    error: error,
});

export const GetAllTipoActor = (
    simuladorTipoActor: TipoActorDto[],    
): AppActions => ({
    type: OBTENER_TIPO_ACTOR,
    simuladorTipoActor: simuladorTipoActor
})

export const UpdateSimuladorActor = (): AppActions => ({
    type: UPDATE_SIMULADOR_ACTOR,
});

export const UpdatetSimuladorActorFailure = (error: any): AppActions => ({
    type: GET_SIMULADOR_ACTOR_FAILURE,
    error: error,
});

export const InsertSimuladorTipoActor = (): AppActions => ({
    type: INSERT_SIMULADOR,
});

export const DeleteSimuladorTipoActor = (): AppActions => ({
    type: DELETE_SIMULADOR,
});

export const PostInsertSimuladorTipoActor = (
    simuladorActor: SimuladorActorDto,
): AppActions => ({
    type: CREAR_SIMULADOR_ACTOR,
    simuladorActor: simuladorActor
})

export const PostUpdateSimuladorTipoActor = (
    simuladorActor: SimuladorActorDto, 
): AppActions => ({
    type: ACTUALIZAR_SIMULADOR_ACTOR,
    simuladorActor: simuladorActor,
})

export const PostDeleteSimuladorTipoActor = (
    simuladorActor: SimuladorActorDto, 
): AppActions => ({
    type: ELIMINAR_SIMULADOR_ACTOR,
    simuladorActor: simuladorActor,
})


export const GetSimuladorUniej = (): AppActions => ({
    type: GET_SIMULADOR_UNIEJ
});

export const GetSimuladorUniejFailure = (error: any): AppActions => ({
    type: GET_SIMULADOR_UNIEJ_FAILURE,
    error: error,
});

export const GetAllUnidadEjecutora = (
    simuladorunidadEjecutora: UnidadEjecutoraDto[],    
): AppActions => ({
    type: OBTENER_UNIDAD_EJECUTORA,
    simuladorunidadEjecutora: simuladorunidadEjecutora
})

export const GetSimuladorUniEjTasas = (
    simuladorUniEjecutoras: SimuladorUndEjDto[],    
): AppActions => ({
    type: OBTENER_SIMULADOR_UNIEJ,
    simuladorUniEjecutoras: simuladorUniEjecutoras
})

export const GetUnidadEjecutora = (): AppActions => ({
    type: GET_UNIDAD_EJECUTORA,
});

export const GetUnidadEjecutoraFailure = (error: any): AppActions => ({
    type: GET_UNIDAD_EJECUTORA_FAILURE,
    error: error,
});

export const UpdateSimuladorUniej = (): AppActions => ({
    type: UPDATE_SIMULADOR_UNIEJ,
});

export const UpdatetSimuladorUniejFailure = (error: any): AppActions => ({
    type: GET_SIMULADOR_UNIEJ_FAILURE,
    error: error,
});

export const InsertSimuladorUnidadEjecutora = (): AppActions => ({
    type: INSERT_SIMULADOR_UNIEJ,
});

export const DeleteSimuladorUnidadEjecutora = (): AppActions => ({
    type: DELETE_SIMULADOR_UNIEJ,
});

export const PostInsertSimuladorUnidadEjecutora = (
    simuladorUniej: SimuladorUndEjDto,
): AppActions => ({
    type: CREAR_SIMULADOR_UNIEJ,
    simuladorUniej: simuladorUniej,
})

export const PostUpdateSimuladorUniej = (
    simuladorUniej: SimuladorUndEjDto, 
): AppActions => ({
    type: ACTUALIZAR_SIMULADOR_UNIEJ,
    simuladorUniej: simuladorUniej,
})

export const PostDeleteSimuladorUnidadEjecutora = (
    simuladorUniej: SimuladorUndEjDto, 
): AppActions => ({
    type: ELIMINAR_SIMULADOR_UNIEJ,
    simuladorUniej: simuladorUniej,
})



export const startGetAllSimuladorTasas = () => async (dispatch: Dispatch<AppActions>) => {
    dispatch(GetSimulador());
    const response = await api.getAllSimuladorTasas();
    if (response?.isSuccess == null) {
        dispatch(GetSimuladorFailure(response.data));
    } else {
        dispatch(GetSimuladorTasas(response.data));
        toast(response.returnMessage);
    }
};

export const startPostInsertSimuladorTasa = (simulador: any) =>
async (dispatch: Dispatch<AppActions>) => {        
    try {    
        dispatch(InsertSimulador());      
        const response = await api.postInsertSimuladorTasas(simulador);                           
        if (!response?.isSuccess) {
            dispatch(PostInsertSimuladorTasas(response.data));
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(PostInsertSimuladorTasas(response.data));
            if (response.data == null) {
                toast('Error al crear simulador tasas');
            }                
        }
    } catch (e) {
  
        dispatch(SetError(null));
        toast('Error: respuesta errada del servidor');
    }
};


export const startPostUpdateSimuladorTasas =
    (simulador: SimuladorDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateSimulador());
        const response = await api.postUpdateSimuladorTasas(simulador);
        if (response?.isSuccess == null) {
            dispatch(UpdatetSimuladorFailure(response.data));
        } else {
            dispatch(PostUpdateSimuladorTasas(response.data)); 
            toast(response.returnMessage);
        }
    };

export const startPostDeleteSimuladorTasas = (
    simulador: SimuladorDto
    ) =>
    async (dispatch: Dispatch<AppActions>) => {       
        try {  
            dispatch(DeleteSimulador()); 
            const response = await api.postDeleteSimuladorTasas(simulador);                           
            if (!response?.isSuccess) {
                dispatch(PostDeleteSimuladorTasas(response.data));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostDeleteSimuladorTasas(response.data));
                if (response.data == null) {
                    toast('Error al eliminar simulador tasas');
                }                
            }
        } catch (e) {
      
            dispatch(SetError(null));
            toast('Error: respuesta errada del servidor');
        }
};



export const startGetAllSimuladorActorTasas = 
    (idSimulacion: string) => async (dispatch: Dispatch<AppActions>) => {
    dispatch(GetSimuladorActor());
    const response = await api.postAllSimuladorActor(idSimulacion);
    if (response?.isSuccess == null) {
        dispatch(GetSimuladorActorFailure(response.data));
        toast('Error: respuesta errada del servidor');
    } else {
        dispatch(GetSimuladorActorTasas(response.data));
        toast(response.returnMessage);
    }
};

export const startGetAllTipoActor = 
    () => async (dispatch: Dispatch<AppActions>) => {
    dispatch(GetTipoActor());
    const response = await api.postAllTipoActor();
    if (response?.isSuccess == null) {
        dispatch(GetTipoActorFailure(response.data));
        toast('Error: respuesta errada del servidor');
    } else {
        dispatch(GetAllTipoActor(response.data));
        toast(response.returnMessage);
    }
};

export const startPostInsertSimuladorTipoActor = (idSimulador: string, simuladorActor: SimuladorActorDto) =>
async (dispatch: Dispatch<AppActions>) => {        
    try {    
        dispatch(InsertSimuladorTipoActor());      
        const response = await api.postInsertSimuladorTipoActor(idSimulador, simuladorActor);                           
        if (!response?.isSuccess) {
            dispatch(PostInsertSimuladorTipoActor(response.data));
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(PostInsertSimuladorTipoActor(response.data));
            if (response.data == null) {
                toast('Error al crear simulador tipo actor');
            }                
        }
    } catch (e) {
  
        dispatch(SetError(null));
        toast('Error: respuesta errada del servidor');
    }
};

export const startPostUpdateSimuladorTipoActor =
    (simuladorActor: SimuladorActorDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateSimuladorActor());
        const response = await api.postUpdateSimuladorTipoActor(simuladorActor);
        if (response?.isSuccess == null) {
            dispatch(UpdatetSimuladorActorFailure(response.data));
        } else {
            dispatch(PostUpdateSimuladorTipoActor(response.data)); 
            toast(response.returnMessage);
        }
    };

export const startPostDeleteSimuladorTipoActor = (
    simuladorActor: SimuladorActorDto
    ) =>
    async (dispatch: Dispatch<AppActions>) => {    
        try {  
            dispatch(DeleteSimuladorTipoActor()); 
            const response = await api.postDeleteSimuladorTipoActor(simuladorActor);                           
            if (!response?.isSuccess) {
                dispatch(PostDeleteSimuladorTipoActor(response.data));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostDeleteSimuladorTipoActor(response.data));
                if (response.data == null) {
                    toast('Error al eliminar simulador tipo actor');
                }                
            }
        } catch (e) {
      
            dispatch(SetError(null));
            toast('Error: respuesta errada del servidor');
        }
};



export const startPostAllSimuladorUniEjTasas = 
    (idSimulacion: number) => async (dispatch: Dispatch<AppActions>) => {
    dispatch(GetSimuladorUniej());
    const response = await api.postAllSimuladorUnidadEjecutora(idSimulacion);
    if (response?.isSuccess == null) {
        dispatch(GetSimuladorUniejFailure(response.data));
        toast('Error: respuesta errada del servidor');
    } else {
        dispatch(GetSimuladorUniEjTasas(response.data));
        toast(response.returnMessage);
    }
};

export const startPostInsertSimuladorUnidadEjecutora = (idSimulacion: number,
    simuladorUniej: SimuladorUndEjDto) =>
async (dispatch: Dispatch<AppActions>) => {        
    try {    
        dispatch(InsertSimuladorUnidadEjecutora());      
        const response = await api.postInsertSimuladorUnidadEjecutoraTasas(idSimulacion, simuladorUniej);                           
        if (!response?.isSuccess) {
            dispatch(PostInsertSimuladorUnidadEjecutora(response.data));
            toast('Error: respuesta errada del servidor');
        } else {
            dispatch(PostInsertSimuladorUnidadEjecutora(response.data));
            if (response.data == null) {
                toast('Error al crear simulador unidad ejecutora');
            }                
        }
    } catch (e) {
  
        dispatch(SetError(null));
        toast('Error: respuesta errada del servidor');
    }
};

export const startGetAllUnidadEjecutora = 
    () => async (dispatch: Dispatch<AppActions>) => {
    dispatch(GetUnidadEjecutora());
    const response = await api.getAllUnidadEjecutora();
    if (response?.isSuccess == null) {
        dispatch(GetUnidadEjecutoraFailure(response.data));
        toast('Error: respuesta errada del servidor');
    } else {
        dispatch(GetAllUnidadEjecutora(response.data));
        toast(response.returnMessage);
    }
};

export const startPostUpdateSimuladorUnidadEjecutora =
    (simuladorUniej: SimuladorUndEjDto) => async (dispatch: Dispatch<AppActions>) => {
        dispatch(UpdateSimuladorUniej());
        const response = await api.postUpdateSimuladorUnidadEjecutora(simuladorUniej);
        if (response?.isSuccess == null) {
            dispatch(UpdatetSimuladorUniejFailure(response.data));
        } else {
            dispatch(PostUpdateSimuladorUniej(response.data)); 
            toast(response.returnMessage);
        }
 };

export const startPostDeleteSimuladorUnidadEjecutora = (
    simuladorUniej: SimuladorUndEjDto
    ) =>
    async (dispatch: Dispatch<AppActions>) => {       
        try {  
            dispatch(DeleteSimuladorUnidadEjecutora()); 
            const response = await api.postDeleteSimuladorUnidadEjecutora(simuladorUniej);                           
            if (!response?.isSuccess) {
                dispatch(PostDeleteSimuladorUnidadEjecutora(response.data));
                toast('Error: respuesta errada del servidor');
            } else {
                dispatch(PostDeleteSimuladorUnidadEjecutora(response.data));
                if (response.data == null) {
                    toast('Error al eliminar simulador Unidad Ejecutora');
                }                
            }
        } catch (e) {
      
            dispatch(SetError(null));
            toast('Error: respuesta errada del servidor');
        }
};
