using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Tabla de Solicitud Simulacion Credito por parte del Cliente
    /// </summary>
    /// <author>German Eduardo Guarnizo</author>
    /// <date>26/03/2021</date>
    public class SolicitudSimulacion
    {
        public Guid SLS_ID { get; set; }
        public DateTime SLS_FECHA_SOLICITUD { get; set; }
        public int TCR_ID { get; set; }
        public int SLS_NUMERO_PREAPROBADO { get; set; }
        public bool SLS_PROBLEMA_EMAIL { get; set; }
        public bool SLS_ENVIO_NOTIFICACION { get; set; }
        public string SLS_ESTADO { get; set; }
        public Guid SLS_USUARIO_ACTUALIZA { get; set; }
        public DateTime SLS_FECHA_ACTUALIZA { get; set; }
        public string SLS_RUTA_PDF_RESUMEN { get; set; }
        public double SLS_PORC_EXTRAPRIMA { get; set; }
        public string SLS_NUMERO_RADICADO { get; set; }
        public string SLS_RECORD_ID { get; set; }
        public int PA_ID { get; set; }
        public int SLS_SCORE_TU { get; set; }
        public int SLS_PORC_ENDEUDAMIENTO_SCH { get; set; }
        public int SLS_PORCENDEUDAMIENTO_CCH { get; set; }
        public Guid MEDIOS_CREDITO { get; set; }
        public int NUM_FOLIOS { get; set; }
        public int MEDIO_RECEPCION { get; set; }
        public string SMC_NUMERO_DOCUMENTO { get; set; }
        public string SLS_NUMERO_RADICADO_83 { get; set; }
        public string SLS_SIMULADOR_DESC { get; set; }
        public DateTime SLS_FECHA_VIGENCIA_361 { get; set; }
        public DateTime SLS_FECHA_VIGENCIA_83 { get; set; }
        public Guid SOC_ID { get; set; }

    }
}
