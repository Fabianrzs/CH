import { GrupoUsersModel } from "./grupoUsers.model";

export interface User {
    id: string;
    nombre: string;
    segundoNombre:string;
    apellido: string;
    segundoApellido:string;
    identificacion:string;
    correo:string;
    nombreUsuario: string;
    contrasena: string;
    estado:boolean;
    tipoIdentificationCode:string;
    roles:GrupoUsersModel[];
    tipoIdentificationId:number;
    repitaContrasena:string;
    token:string;
    directory:boolean;
    inhabilitado:boolean;
}


export interface UseRecuperaContrasena {
    token:string;
    contrasena:string;   
}
