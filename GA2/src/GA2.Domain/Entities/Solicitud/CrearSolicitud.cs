using System;

namespace GA2.Domain.Entities
{
    public class CrearSolicitud
    {
        public int? CTA_ID { get; set; }
        public Guid TPS_SUB_ID { get; set; }
        public int CLI_ID { get; set; }
        public Guid SOL_CREATEDOPOR { get; set; }
        public Guid TRA_ID { get; set; }
        public int SOL_ESTADO_SOLICITUD { get; set; }
        public int STL_ESTADO_SOLICITUD { get; set; }
        public DateTime SOL_FECHACREACION { get; set; }
        public string SOL_CEDULA { get; set; }
        public string SOL_NOMBRE { get; set; }
        public string SOL_RADICADO { get; set; }
    }
}
