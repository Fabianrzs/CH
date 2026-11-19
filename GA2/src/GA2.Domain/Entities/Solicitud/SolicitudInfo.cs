using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class SolicitudInfo
    {
        public string TIM_DESCRIPCION { get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
        public string SOL_VALOR_A_RETIRAR { get; set; }
        public int CLI_ID { get; set; }
        public string SOL_RADICADO_GD { get; set; }
        public string PCS_NOMBRE { get; set; }
        public string PCS_NOMBRE_DODO { get; set; }
        public string SOLICITUD { get; set; }
        public string TIPO_SUBMODALIDAD { get; set; }
        public string TRAMITE_PAGO { get; set; }
        public string MODALIDAD_PAGO { get; set; }
        public string TIPO_VIVIENDA { get; set; }
        public Guid TPS_SUB_ID { get; set; }
    }
}
