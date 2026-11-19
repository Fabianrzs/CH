using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class RespuestaConsultarSolicitudCesantias
    {
        public int SOL_NUMERO_SOLICITUD { get; set; }
        public string PCS_DESCRIPCION { get; set; }
        public int TID_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string CLI_PRIMER_NOMBRE { get; set; }
        public DateTime SOL_FECHA_SOLICITUD { get; set; }
        public decimal SOL_VALOR_A_RETIRAR { get; set; }
        public string TRA_NOMBRE { get; set; }
        public string CVL_DESCRIPCION { get; set; }
        public string CLI_ID { get; set; }
        public string SOL_CEDULA { get; set; }
        public string TIM_CODIGO { get; set; }
        public string TPS_SUB_ID { get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
        public string SOL_RADICADO_GD { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public Guid SLT_ID { get; set; }
        public int? NUMFOLIOS { get; set; }
        public Guid? IdUsuarioAsignado { get; set; }
        public string? UsuarioAsignado { get; set; }

        public string SOL_RECHAZADA { get; set; }
        public string TIM_DESCRIPCION { get; set; }
    }
}
