using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Postulacion.Enum
{
    public class EstadoPostulacionEnums
    {
        public static string Pendiente = "PENDIENTE";
        public static string Rechazado = "RECHAZADO";
        public static string Anulado = "ANULADO";
        public static string Aprobado = "APROBADO";
        public static string Inconsistente = "INCONSISTENTE";
        public static string Bloqueado = "BLOQUEADO";
    }
    public enum EstadoPostulacionEnum
    {
        Pendiente = 1,
        Aprobado = 2,
        Rechazado = 3,
        Inconsistente = 4,
        Anulado = 5,
        Bloqueado = 6
    }
    public enum SubModalidadPostulacionEnum
    { 
        ProyectoMecanismoEspeciales = 3,
        Heroes = 1
    }
    public class ImputabilidadServicioDisminucionCapacidad
    {
        public static string ConsecuenciaCombate = "EN EL SERVICIO COMO CONSECUENCIA DEL COMBATE O EN ACCIDENTE RELACIONADO CON EL MISMO, \n O POR ACCIÓN DIRECTA DEL ENEMIGO, EN TAREAS DE MANTENIMIENTO \n O RESTABLECIMIENTO DEL ORDEN PÚBLICO O EN CONFLICTO INTERNACIONAL";
        public static string ServicioCausaORazon = "EN EL SERVICIO POR CAUSA Y RAZÓN DEL MISMO, ES DECIR, ENFERMEDAD PROFESIONAL O ACCIDENTE DE TRABAJO";
        public static string ServicioNoCusa = "EN EL SERVICIO PERO NO POR CAUSA Y RAZÓN DEL MISMO, ES DECIR, ENFERMEDAD O ACCIDENTE COMÚN";
    }
    public class PorcentajePostulacion
    {
        public static string Nuclueox100 = "NÚCLEO FAMILIAR QUE SE POSTULA POR EL 100% DEL BENEFICIO";
        public static string Nucleo = "NÚCLEO FAMILIAR QUE SE POSTULA POR MENOS DEL 100% DEL BENEFICIO";
    }
    public class CatalogPostulacionDto
    {
        public static string MuerteAfiliado = "MUERTE DEL AFILIADO";
        public static string DisminucionPsicofisica = "DISMINUCIÓN DE LA CAPACIDAD PSICOFÍSICA";
    }
    public class ModalidadFondoSolidaridad
    {
        public static string ProyectoMecanimos = "PROYECTOS Y MECANISMOS ESPECIALES";
        public static string FallecimientoAfiliado = "BENEFICIARIO(A) POR FALLECIMIENTO DEL AFILIADO PARA SOLUCIÓN DE VIVIENDA";
        public static string DisminucionCapacidad = "RETIRO O DESVINCULACIÓN POR DISMINUCIÓN DE LA CAPACIDAD PSICOFÍSICA";
    }
}
