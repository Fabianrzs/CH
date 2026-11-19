export interface ConsultaReporteTransUnions {
    numeroIdentificacion: string | null;
    fechaDesde: Date | string | null;
    fechaHasta: Date | string | null;
    estado: number | null;
}

export interface ReporteTransUnionDto {
    rtU_ID: string;
    rtU_FECHA_CONSULTA: string;
    rtU_NUMERO_IDENTIFICACION: string;
    rtU_NOMBRE: string;
    rtU_DEPARTAMENTO: string;
    rtU_CIUDAD: string;
    rtU_TIPO_CREDITO: string;
    rtU_VALOR_INMUEBLE: number;
    rtU_ESTADO: number;
    rtU_MOTIVO: string;
    rtU_CREADO_POR: string | null;
}