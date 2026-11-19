export interface TerceroApoderadoModel {
    id: string;
    tipoDoc: number;
    tipoDocObj: LlaveValor;
    numDoc: string;
    razonSocial: string;
    parentesco: number | null;
    parentescoObj: LlaveValor | null;
    esAutorizado: boolean;
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
    encontrado?: string;
}

interface LlaveValor {
    id: number | null,
    descripcion: string
}

