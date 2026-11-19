import { SimuladorDto, 
         SimuladorActorDto, 
         SimuladorUndEjDto, 
         TipoActorDto, 
         UnidadEjecutoraDto } from "../../../types/simulador.model.type";

export const SimuladorForm: SimuladorDto = {
    idSimulacion: '',
    descripcion: '',
    tasaEA: 0,
    minimoPlazoMeses: 0,
    maximoPlazoMeses: 0,
    estado: '1', 
};

export const SimuladorActorForm: SimuladorActorDto = {
    parametroId: '',
    simuladorId: '',
    tipoId: '',
    parametroTasaEa: 0,
    parametroEstado: 1
}

export const SimuladorUndEjForm: SimuladorUndEjDto = {
    idParametro: 0,
    idSimulacion: 0,
    idParametroUnidadEjecutora: 0,
    tasaEA: 0,
    estado: 1,
}

export const TipoActorForm: TipoActorDto = {
    idTipoActor: 0,
    descripcionTipoActor: '',
}

export const UnidadEjecutoraForm: UnidadEjecutoraDto = {
    idUnidadEjecutora: 0,
    nombreUnidadEjecutora: '',
}
