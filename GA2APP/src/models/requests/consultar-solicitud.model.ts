export interface ConsultarSolicitudModel {
    tipoSubModalidadId: string | null;
    solicitudTareaId: string | null;
    identificacion: string;
    tipoIdentificacion: string | number;
    fechaIni?: Date;
	fechaFin?: Date;
    modalidad?: string;
    estadoTarea?: number;
    tipoSolicitud?: string;
    numRadicado?:string;
    cuentas?:string;
    tipoCuenta?:string;
    tareaNombre?:string;
    tipoVinculacion?:string;
    numeroSolicitud?:string;
    usuarioIngreso?:string;
    solicitante?:string;
    nroPlanilla?:string;
    comprobante?:string;
}
