using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera
{
    public class ObtenerGarantiaActualizada
    {

        public string CLI_IDENTIFICACION { get; set; } // CLI_IDENTIFICACION
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; } // CRE_FECHA_DESEMBOLSO
        public DateTime CRE_FECHA_FINALIZA_PAGOS { get; set; } // CRE_FECHA_FINALIZA_PAGOS
        public decimal VALOR_AJUSTADO_ANTERIOR { get; set; } // VALOR_AJUSTADO_ANTERIOR
        public decimal CRE_AVA_PORCENTAJE { get; set; } // CRE_AVA_PORCENTAJE
        public decimal VALOR_AJUSTE { get; set; } // VALOR_AJUSTE
        public decimal NUEVO_VALOR_AJUSTADO { get; set; } // NUEVO_VALOR_AJUSTADO

    }
}
