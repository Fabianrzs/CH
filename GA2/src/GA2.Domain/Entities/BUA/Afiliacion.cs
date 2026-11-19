using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class Afiliacion
    {
        public int CLI_ID { get; set; }
        public int AFL_ID { get; set; }
        public int TPF_ID { get; set; }
        //public int DPT_ID { get; set; }
        public string AFL_NUMERO_RADICADO { get; set; }
        public DateTime AFL_FECHA_RADICADO { get; set; }
        public DateTime AFL_FECHA_ELABORACION { get; set; }
        public int CLD_ID { get; set; }
        public int TPP_ID { get; set; }
        public bool AFL_TRAMITE_AFILIACION { get; set; }
        public bool AFL_CONSERVA_ANTIGUEDAD { get; set; }
        public int AFL_RATIFICACION_CUOTAS { get; set; }
        public string AFL_OBSERVACIONES_JUSTIFICACION { get; set; }
        public string AFL_NUMERO_ACTO_ADMINISTRATIVO { get; set; }
        public DateTime AFL_FECHA_NOTIFICACION { get; set; }
        public bool AFL_CAMPO_CESANTIAS { get; set; }
        public bool AFL_ACTIVACION_DESCUENTO { get; set; }
        //public bool AFL_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD { get; set; }
        //public bool AFL_REINTEGRO_APORTES { get; set; }
        //public DateTime AFL_PERIODO_APORTES_DESDE { get; set; }
        //public DateTime AFL_PERIODO_APORTES_HASTA { get; set; }
        //public bool AFL_DERECHO_LIQUIDACION_CUOTAS { get; set; }
        public string AFL_OBSERVACIONES_CONCEPTUALIZACION { get; set; }
        public int PRC_ID { get; set; }
        public int MDL_ID { get; set; }
        public bool AFL_AHORRO_CONSIGNACION_BANCARIA { get; set; }
        public bool AFL_SOLICITUD_TRASLADO_CESANTIAS { get; set; }
        //public bool AFL_CONSERVA_ANTIGUEDAD_DEPENDIENTE { get; set; }
        //public int AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL { get; set; }
        //public DateTime AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PENSIONAL { get; set; }
        //public int AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_APORTES { get; set; }
        //public int AFL_PORCENTAJE_RECONOCIMIENTO_PENSIONAL_DISTRIBUCION_SUBSIDIO { get; set; }
        //public int AFL_NUMERO_ACTO_ADMINISTRATIVO_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES { get; set; }
        //public DateTime AFL_FECHA_NOTIFICACION_RESOLUCIONES_RECONOCIMIENTO_PRESTACIONES_SOCIALES { get; set; }
        //public int AFL_PORCENTAJE_RECONOCIMIENTO_PRESTACIONES_SOCIALES_DISTRIBUCION_CESANTIAS { get; set; }
        //public bool AFL_DERECHO_LIQUIDACION_CUOTAS_DEPENDIENTE{ get; set; }
        public int DPC_ID { get; set; }
        public int DPD_ID { get; set; }


        // InformacionCliente
        public int TID_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; } 
        public string CLI_PRIMER_NOMBRE { get; set; }
        public string CLI_SEGUNDO_NOMBRE { get; set; }
        public string CLI_PRIMER_APELLIDO { get; set; }
        public string CLI_SEGUNDO_APELLIDO { get; set; }
        public DateTime CLI_FECHA_NACIMIENTO { get; set; }
        public string CLI_REGIMEN { get; set; }
        public int CTG_ID { get; set; }
        public int FRZ_ID { get; set; }
        public int UEJ_ID { get; set; }
        public int GRD_ID { get; set; }
        public decimal CIE_INGRESO_MENSUAL { get; set; }
        public DateTime CLI_FECHA_ALTA { get; set; }
        public int AFL_OFICIO_SOLICITUD_CESANTIAS { get; set; }
        public DateTime AFL_MES_INICIO_ACTIVACION_DESCUENTO { get; set; }
        public DateTime AFL_MES_FIN_ACTIVACION_DESCUENTO { get; set; }
        public int AFL_AHORRO_CONSIGNACION_BANCARIA_OPCION { get; set; }

    }
}
