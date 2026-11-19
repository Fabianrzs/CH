export interface JuzgadoDto {
    idJuzgado: string;
    codigoInterno: string;
    nombre: string;
    idDepartamento: number| null;
    departamento: string;
    idCiudad: number | null;
    ciudad: string;
    codigoCiudad: number;
    nroCuenta: string;
    codigoOficina: string;
    correoElectronico: string;
    celular: string;
    contacto: string;
    Estado: boolean;
    tipoActor: string;
    otroTipoActor: string;
    otroTipoActorComf: string;
}

export interface DespachosJudiciales {
    idDepartamento: number | null;
    idCiudad: number | null;
    codigo: string | '';
    CuentaDeposito: string | '';
    Estado: number;
}

export interface CuentaDepositoDto {
    codigoInterno: string | null;
    nCuenta: string | null;
}
