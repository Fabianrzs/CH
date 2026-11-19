export interface TerceroVendedorModel {
    id: string;
    tipoDoc: number;
    tipoDocObj: LlaveValor;
    numDoc: string;
    razonSocial: string;
    direccion: string;
    departamentoVendedor: LlaveValor;
    ciudad: number;
    ciudadObj: LlaveValor;
    correo: string;
    telefono: string;
}

interface LlaveValor {
    id: number,
    descripcion: string
}

