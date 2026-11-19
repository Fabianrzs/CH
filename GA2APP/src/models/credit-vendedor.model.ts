export interface CreditVendedorModel {
    id?: string;
    soc_id?: string;
    tipo_Doc: number;
    numero_Doc: string;
    nombre_Vendedor: string;
    direccion: string;
    departamento: string;
    ciudad: string;
    correo_Electronico: string;
    telefono: string;
    fecha_Entrega: any;
    
    tipo_Cuenta: string;
    banco: string;    
    cuenta: string;
    codProveeSeven: number;

}