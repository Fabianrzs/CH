import { GrupoOpcionesModel } from "./grupo-opciones.model.type";

export interface Login {
    userName: string;
    credential: string;
    isActiveDirectory: boolean;
}

export const LoginInitial: Login = {
    userName: '',
    credential: '',
    isActiveDirectory: false
};


export interface Recuperarcontrasena {
    tipo: string;
    numero: string;
    url:string
}


export interface loginResponse {
    token: string;
    opciones: GrupoOpcionesModel[];

}