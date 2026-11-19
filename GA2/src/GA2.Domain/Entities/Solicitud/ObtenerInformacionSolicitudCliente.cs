using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ObtenerInformacionSolicitudCliente
    {
        public Guid SOL_ID { get; set; }
        public Guid TPS_SUB_ID { get; set; }
        public int SOL_ESTADO_SOLICITUD { get; set; }
        public int SOL_ESTADO { get; set; }
        public decimal SOL_VALOR_A_RETIRAR { get; set; }
        public int CLI_ID { get; set; }
        public DateTime SOL_FECHA_SOLICITUD { get; set; }
        public Guid SLT_ID { get; set; }
        public Guid TRA_ID { get; set; }
        public bool SLT_ESTADO { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public string TRA_NOMBRE { get; set; }
        public string TRA_PERFIL { get; set; }
        public Guid PCS_ID { get; set; }
        public string PCS_NOMBRE { get; set; }
        public Guid MAI_ID { get; set; }
        public string MAI_NUMERO_MATRICULA { get; set; }
        public string MAI_NUM_MATRICULA { get; set; }
        public DateTime MAI_FECHA_REGISTRO { get; set; }
        public bool MAI_BLOQUEO { get; set; }
        public string MAI_TIPO_BLOQUEO { get; set; }        
        public int DPD_ID { get; set; }
        public string DPD_NOMBRE { get; set; }
        public string OFC_DESCRIPCION { get; set; }
        public string TIM_DESCRIPCION { get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
    }
}
