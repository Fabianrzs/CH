export interface ConsultaTablaParametro {
    categoria: string | null;
    tabla: string | null;
}
export interface TablaParametroDto {
    datos:string[];
}
export interface TablaTitleParametroDto {
   nombreColumna:string;
   tipoDato:string;
   aceptaNulos:string;
}
export interface ModalTablaParametro {
    text1: string | null;
    date: string | null;
}
export interface DataEdit {
    data:any;
}
