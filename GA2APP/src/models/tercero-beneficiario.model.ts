import { TipoMatricula } from "./propietario-matricula.model";

export interface TerceroBeneficiarioModel {
    id: string,
    tipo?: string,
    tipoDoc: number,
    tipoDocObj: LlaveValor,
    numDoc: string,
    razonsocial: string,
    entidad: string | null,
    entidadObj: LlaveValor,
    tipoCuenta: number,
    tipoCuentaObj: LlaveValor,
    numeroCuenta: string,
    valorRetirar?: string,
    apoderado?: TipoApoderado[],
    valorRetirarDos?: string,
    esAutorizado?: boolean
}

interface LlaveValor {
    id: string,
    descripcion: string
}

export interface TipoApoderado {
    id: string,
    tipoDoc: number,
    numDoc: string,
    tipoDocObj: LlaveValorDos,
    razonsocial: string
}

interface LlaveValorDos {
    id: string,
    descripcion: string
}

export interface BeneficiarioModel {
    tipo: string,
    tipoDoc: number,
    numDoc: string,
    razonSocial: string,
    esAutorizado: boolean,
    parentesco?: string,
    primerNombre: string,
    segundoNombre: string,
    primerApellido: string,
    segundoApellido: string,
    matricula?: TipoMatricula[]
}
