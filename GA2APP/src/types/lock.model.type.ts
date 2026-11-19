export interface BloqueoDto {
    id: number;
    codigo: number;
    concepto: string;
    diasMora: string;
    reversible: boolean;
    aceleracionDeuda: boolean;
    fechaModificacion: Date;
    modificadoPor: number;
    estado: boolean;
}
