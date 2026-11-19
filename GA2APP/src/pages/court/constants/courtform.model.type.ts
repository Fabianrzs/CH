import { DEPARTAMENTODTO } from "../../../types/catalog.type";
import { JuzgadoDto } from "../../../types/court.model.type";

export const juzgadoForm: JuzgadoDto = {
    idJuzgado: '',
    codigoInterno: '',
    nombre: '',
    idDepartamento: null,
    departamento: '',
    idCiudad: null,
    ciudad: '',
    nroCuenta: '',
    codigoCiudad: 0,
    codigoOficina: '',
    correoElectronico: '',
    celular: '',
    contacto: '',
    estado: true,
    tipoActor: '',
    otroTipoActor: ""
}

export const departamentForm: DEPARTAMENTODTO = {
    id: 0,
    idPais: 0,
    codigo: 91,
    nombre: '',
    idOficina: ""
}