import { User } from './user.type';

export interface Proceso {
    PCS_ID: string;
    PCS_NOMBRE: string;
    PCS_DESCRIPCION: string;
    PCS_ESTADO: boolean;
    PSC_CREATEDOPOR: string;
    PSC_FECHACREACION: Date;
    PSC_MODIFICADOPOR: string;
    PSC_FECHAMODIFICACION: Date;
    PSC_VERSION: string;
}

export interface Tarea {
    TRA_ID: string;
    PCS_ID: string;
    TRA_NOMBRE: string;
    TRA_FECHAINICIO: Date;
    TRA_FECHAFIN: Date;
    TRA_CIERRE_ESTIMADO: Date;
    TRA_ACTIVA: boolean;
    RL_ID_RESPONSABLE: string;
    TRA_ACTORES: User[];
    TRA_CREATEDOPOR: string;
    TRA_FECHACREACION: Date;
    TRA_MODIFICADOPOR: string;
    TRA_FECHAMODIFICACION: Date;
    TRA_ID_ENTRADA: string;
    TRA_ID_SALIDA: string;
}

export interface Anadido {
    TRA_ID: string;
    AND_ID: string;
    AND_NOMBREANADIDO: string;
    AND_FILE: string;
    AND_TIPO: number;
    AND_CREATEDOPOR: string;
    AND_FECHACREACION: Date;
    AND_MODIFICADOPOR: string;
    AND_FECHAMODIFICACION: Date;
}

export interface Regla {
    RGL_ID: string;
    TRA_ID: string;
    RGL_NOMBRE: string;
    RGL_CUMPLEREGLA: boolean;
    RGL_CREATEDOPOR: string;
    RGL_FECHACREACION: Date;
    RGL_MODIFICADOPOR: string;
    RGL_FECHAMODIFICACION: Date;
}

export interface Restricciones {
    TRA_ID: string;
    RTC_ID: string;
    RTC_NOMBRE: string;
    TRA_ID_RESTRICCION: string;
    RTC_CREATEDOPOR: string;
    RTC_FECHACREACION: Date;
    RTC_MODIFICADOPOR: string;
    RTC_FECHAMODIFICACION: Date;
}
