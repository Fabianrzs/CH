export interface NotificacionDto {
    id: string;
    mensaje: string;
    receptor: string;
    tipo: string;
    visto: boolean;
    emisor: string;
    fechaCreacion: Date;
    estado: boolean;
}
