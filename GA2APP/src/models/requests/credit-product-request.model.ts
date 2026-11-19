export interface CreditProductRequestModel {
    soB_ID?: string;
    soC_ID: string;
    ind: number;
}

export interface Matricula {
    idMatricula?: string;
    numeroMatricula?: string;
    fechaMatricula?: Date;
    chip?: string;
    cedulaCatastral?: string;
    areaPrivadaConstruida?: number;
    areaAvaluoComercial?: number;
    areaAvaluoCatastral?: number;
    socId: string;
    tipoInmueble?: string;
    commercialValue?: string;
    tipo_Matricula?: number;

  }

  export const matriculaForm: Matricula = {
    idMatricula: "",
    numeroMatricula: "",
    fechaMatricula: new Date(),
    chip: "",
    cedulaCatastral: "",
    areaPrivadaConstruida: 0,
    areaAvaluoComercial: 0,
    areaAvaluoCatastral: 0,
    socId: "",
    tipoInmueble: "",
    commercialValue: "",
    tipo_Matricula: 0,
}
