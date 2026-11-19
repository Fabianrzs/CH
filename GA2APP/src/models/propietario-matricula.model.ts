export interface PropietarioMatriculaModel {
    numeroIdentificacion: string;
    razonSocial: string;
    tipoIdentificacion: TipoIdentificacion;
    matricula: TipoMatricula[];
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
    matriculas?: TipoMatricula[];
}

interface TipoIdentificacion {
    id: number,
    descripcion: string
}

export interface TipoMatricula {
    id: string,
    numeroMatricula: string;
    estado: string
}
