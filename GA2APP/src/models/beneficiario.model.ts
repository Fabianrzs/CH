export interface BeneficiarioModel {
    id: string;
    tipoDoc: number;
    tipoDocObj: LlaveValor;
    numDoc: string;
    parentesco: number | null;
    parentescoObj: LlaveValor | null;
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
    ValorRetirarDos: number;
}

interface LlaveValor {
    id: number | null,
    descripcion: string
}