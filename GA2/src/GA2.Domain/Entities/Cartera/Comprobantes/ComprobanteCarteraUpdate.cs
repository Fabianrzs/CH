using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Comprobantes
{
    public class ComprobanteCarteraUpdate
    {
        public int CC_ID { get; set; }
        public int? CC_COMPROBANTE_SEVEN_ID { get; set; }
        public int TIPO_OPERACION { get; set; }
        //public DateTime CC_FECHA_APROBACION { get; set; }
        public Guid CC_APROBADO_POR { get; set; }
        public bool? ES_EXITOSO { get; set; }
        public string RESPUESTA_SEVEN { get; set; }
        public string ESTADO { get; set; }
    }    
    public class DetallesComprobanteCarteraUpdate
    {
        public int CC_ID { get; set; }
        public string RESPUESTA_SEVEN { get; set; }
    }
}
