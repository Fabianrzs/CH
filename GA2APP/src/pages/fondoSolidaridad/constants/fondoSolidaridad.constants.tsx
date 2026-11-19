export const CATALOG = {
    PARENTESCO: 3,
    NIVELES_ACADEMICOS: 65,
}

export const ESTADO = {
    APERTURA: 1,
    VERIFICACION: 2,
    PRERANKING: 3,
    RANKING: 4,
    CERRADA: 5
}

export const ESTADO_CONVOCATORIA = {
   APERTURA: 'Apertura',
   VERIFICACION: 'Verificación',
   CERRADA: 'Cerrada',
   ANULADO: 'Anulado',
}

export const ESTADO_POSTULACION = {
    PENDIENTE: 'PENDIENTE',
    APROBADO: 'APROBADO',
    RECHAZADO: 'RECHAZADO',
    ANULADO: 'ANULADO',
    INCONSISTENTE: 'INCONSISTENTE',
    BLOQUEADO: 'BLOQUEADO'
}

export const ACTIVIDAD_POSTULACION = {
    SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD: 'SOLICITUD POSTULACIÓN FONDO SOLIDARIDAD',
    CARGUE_DOCUMENTOS: 'CARGUE DE DOCUMENTOS FONDO SOLIDARIDAD',
    ENTREVISTA: 'ENTREVISTA',
    CALIFICACION: 'CALIFICACIÓN',
    VERIFICACION_DE_NOVEDADES: 'VERIFICACIÓN DE NOVEDADES',
    VERIFICACION_DE_EMBARGOS: 'VERIFICACIÓN DE EMBARGOS',
    VERIFICACION_CUENTA_INDIVIDUAL: 'VERIFICACIÓN DE CUENTA INDIVIDUAL'
}

export const ID_ACTIVIDAD_POSTULACION = {
    SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD: 1,
    CARGUE_DOCUMENTOS: 2,
    ENTREVISTA: 3,
    CALIFICACION: 4,
    VERIFICACION_DE_NOVEDADES: 5,
    VERIFICACION_DE_EMBARGOS: 6,
    VERIFICACION_CUENTA_INDIVIDUAL: 7
}

export const TIPO_SOLICITUD = {
    SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD: 1,
    AJUDICACION_VIVIENDA: 5,
    PERDIDA_BENEFICIO: 6,
}

export const TIPO_SOLICITUD_DESCRIPCION = {
    SOLICITUD_POSTULACION_FONDO_SOLIDARIDAD: 'SOLICITUD POSTULACIÓN FONDO SOLIDARIDAD',
    ADJUDICACION_VIVIENDA: 'ADJUDICACIÓN VIVIENDA'
}

// export const TIPO_MODALIDAD = {
//     RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA: 1,
//     BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA: 2,
//     MECANISMOS_ESPECIALES: 3
// }

export const TIPO_MODALIDAD = {
    HEROES: 1,
    MECANISMOS_ESPECIALES: 3
}

export const TIPO_SUBMODALIDAD = {
    RETIRO_O_DESVINCULACIÓN_POR_DISMINUCIÓN_DE_LA_CAPACIDAD_PSICOFÍSICA: 1,
    BENEFICIARIO_POR_FALLECIMIENTO_DEL_AFILIADO_PARA_SOLUCIÓN_DE_VIVIENDA: 2,
    MECANISMOS_ESPECIALES: 3
}

export const TIPO_MODALIDAD_GIRO_RECURSOS = {
    COMPRA_VIVIENDA_NUEVA: 1,
    COMPRA_VIVIENDA_USADA: 2,
    CONSTRUCCION_SOBRE_LOTE: 3,
    AMPLIACION_REPARACION_MEJORA_VIVIENDA: 4
}

export const TABS_INDEX = {
    RESUMEN: 0,
    CARGUE_ARCHIVO: 1,
    ENTREVISTA: 2,
    CALIFICACION: 3,
    TRAZABILIDAD: 4,
    INCONSITENCIAS: 5,
};

export const TABS_INDEX_CARGUE_ARCHIVO = {
   CARGAR_ARCHIVOS: 0,
   VER_ARCHIVOS: 1,
};

export const TABS_INDEX_ADJUDICACION_VIVIENDA = {
    RESUMEN: 0,
    ENTREVISTA: 2,
    CALIFICACION: 3,
    ASIGNACION_VIVIENDA: 4
};

export const TABS_INDEX_PERDIDA_BENEFICIO = {
    RESUMEN: 0,
    PERDIDA_BENEFICIO: 1,
};

export const TABS_INDEX_PROYECTOS = {
    CONSTRUCTORA_SUPERVISOR_Y_BENEFICIARIO_DE_PAGO: 0,
    PROYECTO_Y_CONTRATO: 1,
    POLIZA: 2,
    INMUEBLES: 3,
    SUPERVISOR_TECNICO: 4,
};

export const TIPO_IMPUTABILIDAD = {
    DISMINUCION: 'RETIRO O DESVINCULACIÓN POR DISMINUCIÓN DE LA CAPACIDAD PSICOFÍSICA',
    MUERTE: 'BENEFICIARIO(A) POR FALLECIMIENTO DEL AFILIADO PARA SOLUCIÓN DE VIVIENDA',
    PROYECTOS_MECANISMOS_ESPECIALES: 'PROYECTOS Y MECANISMOS ESPECIALES'
}

export interface tipoViviendaPostulacionDto {
    id: string,
    descripcion: string
}

export const tipo_vivenda_postulacion: tipoViviendaPostulacionDto[] = [
    {
        id:'FAMILIAR',
        descripcion: 'FAMILIAR',
    },
    {
        id:'ARRIENDO',
        descripcion: 'ARRIENDO',
    },
    {
        id:'PROPIA',
        descripcion: 'PROPIA',
    },
]

export interface viviendaDto {
    id: string,
    descripcion: string
}

export const posee_vivenda: viviendaDto[] = [
    {
        id:'SI',
        descripcion: 'SI',
    },
    {
        id:'NO',
        descripcion: 'NO',
    },
]

export interface unidadEjecutoraDto {
    id: string,
    descripcion: string
}

export const unidad_ejecutora: unidadEjecutoraDto[] = [
    {
        id:'FFMM',
        descripcion: 'FFMM',
    },
    {
        id:'PONAL',
        descripcion: 'PONAL',
    }
]

export interface catalogPostulacionDto {
    id: string | number,
    descripcion: string,
}

export const porcentajePostulacionLista: catalogPostulacionDto[] = [
    {
        id: 100,
        descripcion: 'NÚCLEO FAMILIAR QUE SE POSTULA POR EL 100% DEL BENEFICIO',
    },
    {
        id: 50,
        descripcion: 'NÚCLEO FAMILIAR QUE SE POSTULA POR MENOS DEL 100% DEL BENEFICIO',
    },
]

// export const imputabilidad: catalogPostulacionDto[] = [
//     {
//         id: 'MUERTE',
//         descripcion: 'MUERTE',
//     },
//     {
//         id:'DISMINUCIÓN DE LA CAPACIDAD LABORAL O MINUSVALÍA',
//         descripcion: 'DISMINUCIÓN DE LA CAPACIDAD LABORAL O MINUSVALÍA',
//     },
//     {
//         id:'DISMINUCIÓN DE LA CAPACIDAD LABORAL O MINUSVALÍA',
//         descripcion: 'DISMINUCIÓN DE LA CAPACIDAD LABORAL O MINUSVALÍA',
//     },
// ]

export const imputabilidad: catalogPostulacionDto[] = [
    {
        id: 'RETIRO O DESVINCULACIÓN POR DISMINUCIÓN DE LA CAPACIDAD PSICOFÍSICA',
        descripcion: 'RETIRO O DESVINCULACIÓN POR DISMINUCIÓN DE LA CAPACIDAD PSICOFÍSICA',
    },
    {
        id:'BENEFICIARIO(A) POR FALLECIMIENTO DEL AFILIADO PARA SOLUCIÓN DE VIVIENDA',
        descripcion: 'BENEFICIARIO(A) POR FALLECIMIENTO DEL AFILIADO PARA SOLUCIÓN DE VIVIENDA',
    },
    {
        id:'PROYECTOS Y MECANISMOS ESPECIALES',
        descripcion: 'PROYECTOS Y MECANISMOS ESPECIALES',
    },
]

export const imputabilidad_servicio_muerte: catalogPostulacionDto[] = [
    {
        id: 100,
        descripcion: 'EN EL SERVICIO POR CAUSA Y RAZÓN DEL MISMO',
    },
    {
        id: 80,
        descripcion: 'EN EL SERVICIO POR CAUSA Y RAZÓN DEL MISMO POR ENFERMEDAD CATASTRÓFICA O TERMINAL',
    },
    {
        id: 100,
        descripcion: 'EN EL SERVICIO COMO CONSECUENCIA DEL COMBATE O ACCIDENTE RELACIONADO CON EL MISMO, O POR ACCIÓN DIRECTA DEL ENEMIGO, EN TAREAS DE MANTENIMIENTO O RESTABLECIMIENTO DEL ORDEN PÚBLICO',
    },
    {
        id: 10,
        descripcion: 'EN EL SERVICIO, PERO NO POR CAUSA Y RAZÓN DEL MISMO, ES DECIR, ENFERMEDAD Y/O ACCIDENTE COMÚN, SIMPLE ACTIVIDAD',
    },
]

export const imputabilidad_servicio_disminucion_capacidad: catalogPostulacionDto[] = [
    {
        id: 100,
        descripcion: 'EN EL SERVICIO COMO CONSECUENCIA DEL COMBATE O EN ACCIDENTE RELACIONADO CON EL MISMO, \n O POR ACCIÓN DIRECTA DEL ENEMIGO, EN TAREAS DE MANTENIMIENTO \n O RESTABLECIMIENTO DEL ORDEN PÚBLICO O EN CONFLICTO INTERNACIONAL',
    },
    {
        id: 90,
        descripcion: 'EN EL SERVICIO POR CAUSA Y RAZÓN DEL MISMO, ES DECIR, ENFERMEDAD PROFESIONAL O ACCIDENTE DE TRABAJO',
    },
    {
        id: 10,
        descripcion: 'EN EL SERVICIO PERO NO POR CAUSA Y RAZÓN DEL MISMO, ES DECIR, ENFERMEDAD O ACCIDENTE COMÚN',
    }
]

export const hecho: catalogPostulacionDto[] = [
    {
        id: '100',
        descripcion: 'FALLECIDO',
    },
    {
        id: '0',
        descripcion: 'DISCAPACIDAD',
    },
]

export const numeroHabitantes = [
    1,2,3,4,5,6,7,8,9,10
]

export interface catalogInmuebleDto {
    id: string | number,
    descripcion: string,
}

export const tipoInmueble = [
    {
        descripcion: 'TIPO 1 - OFICIAL',
        tiposVivienda: [
            {
                id:1,
                descripcion: 'OFICIAL BÁSICA'
            },
            {
                id:2,
                descripcion: 'OFICIAL ADECUACIONES'
            }
        ]
    },
    {
        descripcion: 'TIPO 2 - SUBOFICIAL',
        tiposVivienda: [
            {
                id:1,
                descripcion: 'SUBOFICIAL BÁSICA'
            },
            {
                id:2,
                descripcion: 'SUBOFICIAL ADECUACIONES'
            }
        ]
    },
    {
        descripcion: 'TIPO 3 - SOLDADOS',
        tiposVivienda: [
            {
                id:1,
                descripcion: 'SOLDADOS BÁSICA'
            },
            {
                id:2,
                descripcion: 'SOLDADOS ADECUACIONES'
            }
        ]
    },
]

export const listaTiposSolicitud = [
    {
        id: 1,
        descripcion: 'SOLICITUD POSTULACIÓN FONDO SOLIDARIDAD'
    },
    {
        id: 2,
        descripcion: 'ADJUDICACIÓN DE VIVIENDA'
    },
    {
        id: 3,
        descripcion: 'GIRO DE RECURSOS'
    },
]

export const listaModalidadesGiroRecursos = [
    {
        id: 1,
        descripcion: 'COMPRA VIVIENDA NUEVA'
    },
    {
        id: 2,
        descripcion: 'COMPRA VIVIENDA USADA'
    },
    {
        id: 3,
        descripcion: 'CONSTRUCCIÓN SOBRE LOTE'
    },
    {
        id: 4,
        descripcion: 'AMPLIACIÓN REPARACIÓN O MEJORA DE LA VIVIENDA'
    },
]

export const listaEstadosConvocatoria = [
    {
        idEstado: 1,
        nombreEstado: 'Apertura'
    },
    {
        idEstado: 2,
        nombreEstado: 'Verificación'
    },
    {
        idEstado: 5,
        nombreEstado: 'Cerrada'
    },
    {
        idEstado: 6,
        nombreEstado: 'Anulado'
    },
]

export const ESTADO_PROYECTO = {
    EN_CREACION: 551,
    VIGENTE: 552,
    ANULADO: 554,
}

export const TIPO_CONTRATO_PROYECTO = {
    FIDUCIA: 542,
    CAJA_HONOR: 543
}

export const CLASE_INMUEBLE = {
    CASA: 100,
    APARTAMENTO: 109
}











