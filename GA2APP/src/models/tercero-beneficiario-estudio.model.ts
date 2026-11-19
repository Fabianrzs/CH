export interface TerceroBeneficiarioEstudioModel {
    id: string;
    tipoDocEstudio: string;
    tipoDocEstudioObj: LlaveValor;
    numDocEstudio: string;
    razonsocialEstudio: string;
    parentescoEstudio: string;
    parentescoEstudioObj: LlaveValor;
    tipoBeneficiario: string;
}

interface LlaveValor {
    id: string,
    descripcion: string
}