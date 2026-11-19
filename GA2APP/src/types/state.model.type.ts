export interface EstadoDto {
    id: number;
    codigo: number;
    concepto: string;
    diasMoraActivaEstado: string;
    saldoDeuda: string;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
