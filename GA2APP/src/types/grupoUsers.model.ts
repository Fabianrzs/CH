import { GrupoOpcionesModel } from "./grupo-opciones.model.type";

export interface GrupoUsersModel {
    id: number;
    codigo: number;
    nombre: string;
    descripcion: string;
    estado: string;
    opciones:GrupoOpcionesModel[];
  
}

export interface GruposOfUserModel{
    grupo:string;
    estado:string;
}