import { EstadoReporte } from "./EstadoReporte";

export default interface ProcesoReporte {
    idProceso: string,
    nombreReporte: string,
    estado: EstadoReporte,
    progreso: number,
    detalleProceso?: string,
    tipoReporte: string
}