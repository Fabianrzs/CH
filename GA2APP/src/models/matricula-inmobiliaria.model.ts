export interface MatriculaInmobiliariaModel {
    id: string;
    numeroMatricula: string;
    esPrincipal: boolean;
    direccion: string;
    ciudadId: number;
    departamentoId: number;
    departamento: LlaveValor;
    ciudad: LlaveValor;
    bloqueo?: boolean;
    causalId?: string;
    tipoOperacionId?: string;
    oficinaRegistroId?: number;
    esBloqueada?: boolean;
    oficinaCodigo?: string;
    registros?: number;
    idOficina?:string;
    causal?:string;
    tipo_bloqueo?:string;
    ciudadInmueble?:LlaveValor;
    maiNumeroMatricula: string;
    maiId: any;
    bloquearEdicion: any;
}

interface LlaveValor {
    id: number;
    descripcion: string;
}

