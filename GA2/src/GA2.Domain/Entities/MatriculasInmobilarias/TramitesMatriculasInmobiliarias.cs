using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.MatriculasInmobilarias
{
    public class TramitesMatriculasInmobiliarias
    {
        public DateTime SOL_FECHA_SOLICITUD { get; set; }
        public decimal SOL_VALOR_A_RETIRAR {get; set; }
        public string SOL_RADICADO_GD { get; set; }
        public string PRP_NOMBRE_RAZON_SOCIAL {get; set; }
        public string PRP_NUMERO_IDENTIFICACION {get; set; }
        public string TID_DESCRIPCION {get; set; }
        public string ESS_DESCRIPCION {get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
        public string TIM_DESCRIPCION { get; set; }
        public int CLI_ID { get; set; }


    }
}
