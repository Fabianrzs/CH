export interface GrupoOpcionesModel {
    id: number;
    codigo: string;
    nombre: string;
    descripcion: string;
    estado: string;
    checked: boolean;
    subOpciones: GrupoSubOpcionesModel[];
}

export interface GrupoSubOpcionesModel {
    id: number;
    codigo: string;
    nombre: string;
    descripcion: string;
    estado: string;
    checked: boolean;
}