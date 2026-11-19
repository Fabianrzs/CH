using System;

namespace GA2.Domain.Entities
{
    public class ObtenerTramiteSolicitud
    {
        public Guid SOL_ID { get; set; }
        public Guid SOL_ASIGNADO { get; set; }
        public Guid SLT_ID { get; set; }
        public Guid ULTIMO_SLT_ID { get; set; }
        public int CTA_ID { get; set; }
        public Guid TIM_ID { get; set; }
        public Guid TPS_SUB_ID { get; set; }
        public int SOL_ESTADO_SOLICITUD { get; set; }
        public int CLI_ID { get; set; }
        public bool? Posee_Avac { get; set; }
        public DateTime SOL_FECHA_SOLICITUD { get; set; }
        public bool SOL_ESTADO { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public string VOLVER_PANTALLA { get; set; }
        public string? SOL_GRUPO_ASIGNADO { get; set; }
        public string? SOL_NOMBREASIGNADO { get; set; }
        public string? EstadoCredito { get; set; }
        public string? INC_INCONSISTENCIA { get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
        public string TIM_DESCRIPCION { get; set; }
        public string PCS_DESCRIPCION { get; set; }
        public decimal SOL_VALOR_A_RETIRAR { get; set; }
        public string PESTANAS { get; set; }
        public string SECCIONES { get; set; }
        public int SOL_NUMERO_SOLICITUD { get; set; }
        public int NUMERO_FOLIO { get; set; }
        public Guid PCS_ID { get; set; }
        public string SOL_OBSERVACION { get; set; }
        public string TRA_NOMBRE { get; set; }
        public string SOL_RADICADO_GD { get; set; }
        public string TIM_CODIGO { get; set; }
        public string MPR_CONT { get; set; }
        public string? EMBARGO { get; set; } = "NO";
        public string FAC_NUME { get; set; }
        public string FAC_CONT { get; set; }
        public string SOL_RECHAZADA { get; set; }
        public DateTime SOL_FECHACREACION { get; set; }

        public string? SOL_CUMPLE_REQUISITO { get; set; }
        public string? SOL_CAUSAL { get; set; }
        public string? SOL_ACCION_REALIZAR { get; set; }
        public string? SOL_ACUERDO { get; set; }
        public string? SOL_CEDULA { get; set; }
    }

    public class SolicitudAcreditacionWorker
    {
        public int CLI_ID { get; set; }
        public DateTime FECHA_FIRMA_ESCRITURA { get; set; }
        public Guid SOL_ID { get; set; }
        public Guid TPS_SUB_ID { get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
        public DateTime SOL_FECHACREACION { get; set; }
        public string SOL_CEDULA { get; set; }
        public string SOL_CUMPLE_REQUISITO { get; set; }
        public string SOL_ACUERDO { get; set; }
    }
}
