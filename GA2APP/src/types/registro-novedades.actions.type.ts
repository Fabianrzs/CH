import { type } from "os";
import { ConsultarAuditoriaNovedadesDto, InsertarAuditoriaNovedadesDto } from "./registro-novedades.model.type";

export const GET_AUDITORIA_NOVEDADES = 'GET_AUDITORIA_NOVEDADES';
export const POST_AUDITORIA_NOVEDADES = 'POST_AUDITORIA_NOVEDADES';

export interface GetAuditoriaNovedades {
    type: 'GET_AUDITORIA_NOVEDADES';
    consultarAuditoriaNovedad: ConsultarAuditoriaNovedadesDto;
}

export interface PostAuditoriaNovedades {
    type: 'POST_AUDITORIA_NOVEDADES';
    insertarAuditoriaNovedad: InsertarAuditoriaNovedadesDto;
}

export type ConsultaAuditoriaNovedades =
    | GetAuditoriaNovedades
    | PostAuditoriaNovedades;
