using System;

namespace GA2.Domain.Entities
{
    public class ObtenerSolicitud
    {
        public Guid SOL_ID { get; set; }
        public int CTA_ID { get; set; }
        public Guid TPS_SUB_ID { get; set; }
        public int CLI_ID { get; set; }
        public int SOL_ESTADO_SOLICITUD { get; set; }
        public bool SOL_ESTADO { get; set; }
        public string SOL_OBSERVACION { get; set; }
        public decimal SOL_VALOR_A_RETIRAR { get; set; }
        public int SOL_NUMERO_SOLICITUD { get; set; }
        public string SOL_RADICADO_GD { get; set; }
        public string MPR_CONT { get; set; }
        public string FAC_CONT { get; set; }
        public string FAC_NUME { get; set; }
    }
}
