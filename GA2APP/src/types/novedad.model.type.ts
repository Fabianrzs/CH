export interface NovedadesAfiliadoDto {
    idNovedad: number;
    idTipoIdentificacion: number;
    identificacion: string;
    idConceptoNovedad: number;
    fechaInicioNovedad: Date;
    fechaFinNovedad: Date;
    porcentaje: number;
    valor: number;
    id: number;
    idFuerza: number;
    idUnidadEjecutora: number;
    idUnidadFuerza: number;
    fuerzaDescripcion: string;
    nombreUnidadEjecutora: string;
    fechaReporte: Date;
    fechaInicio: Date;
    fechaFin: Date;
    periodo: string;
    idFormato: number;
    descripcionFormato: string;
    idMedioEnvio: number;
    descripcionMedioEnvio: string;
    codigoNovedadAlta: string;
    codigoNovedadBaja: string;
    novedadesConcepto?: any;
    

}

export interface BusquedaNovedades {
    fechaInicial: Date | null;
    fechaFinal: Date | null;
    clienteTipoIndentificacion: number;
    clienteIdentificacion: string;
  }