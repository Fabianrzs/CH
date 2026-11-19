import { ConsultaTablaParametro,ModalTablaParametro,DataEdit } from "../../../types/tablaParametro.model.type";

export const ConsultaTablaParametroForm: ConsultaTablaParametro = {
    categoria: null,
    tabla:null,
}

export const ModalTablaParametroForm: ModalTablaParametro = {
    text1: null,
    date:null,
}

export const CATALOG = {
    ESTADO: 85,
};

export const DataEditForm:DataEdit = {
    data:null,
};

export const TABS_INDEX_UE = {
    INFORMACION: 0,
    CONTACTOS: 1,
    DIRECCIONES: 2,
    TIPOS_ARCHIVOS: 3,
    ANOTACIONES: 4,
    PRESUPUESTO: 5,
    USUARIOS: 6,
    GENERACION_ARCHIVOS: 7,
    MOVIMIENTOS: 8
};

export const TABLA_PARAMETRO = {
    FUERZAS:'FRZ_FUERZA',
    BANCOS: 'ENT_ENTIDAD_BANCARIA',
    UNIDADES_EJECUTORAS: 'UEJ_UNIDADES_EJECUTORAS'
}