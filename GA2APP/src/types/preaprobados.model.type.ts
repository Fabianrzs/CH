export interface ConsultaPreaprobado {
    numeroDocumento: string | null;
    fechaSimulacion: Date | null;
    fechaSimulacionFin: Date | null;
    estado: string | null;
}

export interface PreaprobadoDto {
    idPreaprobado: string;
    tipoCredito: string;
    fechaSimulacion: Date;
    fechaAprobacion: Date;
    numeroDcocumento: string;
    valorPrestamo: number;
    numeroPreaprobado: number;
    estado: string;
    descripcion: string;
    primerNombre: string;
    segundoNombre: string;
    primerApellido: string;
    segundoApellido: string;
}

