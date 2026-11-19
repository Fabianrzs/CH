import { CATEGORIES, FORCEDTO, IDENTIFICATIONTYPEDTO, 
         SEXODTO } from "../../../types/catalog.type";

export const TypeIdForm: IDENTIFICATIONTYPEDTO = {
    id: 0,
    codigo: '',
    descripcion: '',
    esEmpresarial: true,
    esActivo: true,
    ERP: 0,
    vigia: 0,
    embargo: 0
 }
 export const TypeTipoGeneroForm: SEXODTO = {
    id: 0,
    idCatalogo: 0,
    codigo: '',
    descripcion: '',
    estado: true
}

export const TypeForceForm: FORCEDTO = {
    id: 0,
    codigo: '',
    descripcion: '',
    digito: '',
    esSoldado: true
}

export const TypeCategoriesForm: CATEGORIES = {
    id: 0,
    nombre: '',
    descripcion: '',
    expresion: '',
    esEditable: true,
}
