export interface CreateUpdateCommittee {
    codigoComite?: number;
    fecha: string;
    lugar: string;
    horaInicio: string;
    horaFinalizacion: string;
    numeroComite?: number;
    desarrollo: string;
    temasAprobacion: string;
    anotacion: string;
    cargoAnalista: string;
    codigoEstado?: string;
    creadoPor?: string;
    fechaCreacion?: string;
    actualizadoPor?: string;
    fechaActualiza?: string;
    estado?: string;
    comiteIntegrantesDto: CreateUpdateMemberRequest[];
    temasComiteCreditoDto: CreateUpdateTopicRequest[];
}

export interface CreateUpdateMemberRequest {
    idComiteCredito?: number;
    idComiteIntegrante?: string;
    idAsistente: number;
    orden: number;
    creadoPor?: string;
    fechaCreacion?: string;
}

export interface CreateUpdateTopicRequest {
    idComiteCredito?: number;
    idTema?: string;
    codigoTema: number;
    orden: number;
    observacion: string;
    creadoPor?: string;
    fechaCreacion?: string;
    descripcionTema?: string;
}
