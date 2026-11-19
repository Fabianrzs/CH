import { JuzgadoDto } from "../../../types/court.model.type";


const convertToCourt = (juzgado: JuzgadoDto) => ({
    idJuzgado: juzgado.idJuzgado,
    codigoInterno: juzgado.codigoInterno,
    nombre: juzgado.nombre,
    idDepartamento: juzgado.idDepartamento,
    departamento: juzgado.departamento,
    idCiudad: juzgado.idCiudad,
    ciudad: juzgado.ciudad,
    codigoCiudad: juzgado.codigoCiudad,
    nroCuenta: juzgado.nroCuenta,
    codigoOficina: juzgado.codigoOficina,
    correoElectronico: juzgado.correoElectronico,
    celular: juzgado.celular,
    contacto: juzgado.contacto,
    estado: juzgado.estado
})

export {
    convertToCourt
}