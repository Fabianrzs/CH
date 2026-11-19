export interface InconsistenciaSolicitudModel {
    listaInconsistenciaId: string;
    idTipoInconsistencia: string;
    herramientaId: string;
    puntoAtencionId: string;
    solicitudId: string;
    tecnicoId: string;
    objetoEstudio: string;
    analisis: string;
    observacion: string;
    estado: boolean;
    creadoPor: string;
}