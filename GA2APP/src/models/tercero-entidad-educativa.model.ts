export interface TerceroEntidadEducativaModel {
    nit:string;
    razonSocial:string;
    idEntidad:string;
    idProgramaEntidad: string;
    idNivel:string;
    reciboPago:string;
    fechalimite:string;
    razonsocial?:string;
    departamento?: number;
    ciudad?: number;
    modalidad_estudio?: string;
    intensidadHoraria?: '',
    horas?:string,
    fecha_de_inicio?: string;
    fecha_fin?: string;
    acreditada?: string;
    valor_recibo?: number;
}
