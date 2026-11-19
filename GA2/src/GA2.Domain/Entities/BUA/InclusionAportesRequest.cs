using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class InclusionAportesRequest
    {
        public int IAP_ID { get; set; }
        public int CLI_ID { get; set; }
        public int CTA_ID { get; set; }
        public string IAP_NUMERO_REFERENCIA { get; set; }
        public string IAP_OBSERVACIONES { get; set; }
        public bool IAP_GENERAR_ORDEN_RECAUDO { get; set; } 
        public DateTime IAP_FECHA_INICIAL_RECAUDO { get; set; }
        public DateTime IAP_FECHA_FINAL_RECAUDO { get; set; }
        public bool IAP_APROBADA { get; set; }
    }
}
