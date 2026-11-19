
export interface CarteraComprobanrtesDataModel {
    aprobado: boolean;
    comprobante_Id: string;
    comprobante_Seven_Id: string;
    detalle_Movimiento_Id: string;
    estado: string;
    fecha_Comprobante: string;
    fecha_Transaccion: string;
    lstMovimientoComprobante: lstMovimientoComprobante[];
    tipo_Comprobante: string;
    tipo_Operacion: string;
    total_Comprobante_Credito: string;
    total_Comprobante_Debito: string;
}

export interface lstMovimientoComprobante {

    centro_Costo: string;
    comprobante_CarteraId: string;
    credito: string;
    cuenta_Contable: string;
    debito: string;
    descripcion_Cuenta: string;
    detalle: string;
    movimiento_Comprobante_CarteraId: string;
    total_Registros: number;

}

