import { type } from "os";
import { AfiliacionDto, ClienteDto } from "./clienteUnidadEjecutora.type";

export const POST_CLIENTS_UNIDAD_EJECUTORA = 'POST_CLIENTS_UNIDAD_EJECUTORA';
export const POST_CLIENTS_UNIDAD_EJECUTORA_SUCCESS = 'POST_CLIENTS_UNIDAD_EJECUTORA_SUCCESS'
export const POST_CLIENTS_UNIDAD_EJECUTORA_FAILURE = 'POST_CLIENTS_UNIDAD_EJECUTORA_FAILURE'

export interface PostClientsUnidadEjecutora {
    type: 'POST_CLIENTS_UNIDAD_EJECUTORA';
    ClientUEJ: ClienteDto;
}

export interface PostClientsUnidadEjecutoraSuccess {
    type: 'POST_CLIENTS_UNIDAD_EJECUTORA_SUCCESS';
    ClientUEJ: ClienteDto;
}

export interface PostClientsUnidadEjecutoraFailure {
    type: 'POST_CLIENTS_UNIDAD_EJECUTORA_FAILURE';
    ClientUEJ: ClienteDto;
}

export type ClienteUnidadEjecutoraTypes =
    | PostClientsUnidadEjecutora
    | PostClientsUnidadEjecutoraSuccess
    | PostClientsUnidadEjecutoraFailure;
    