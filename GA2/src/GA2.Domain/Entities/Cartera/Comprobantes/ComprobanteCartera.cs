using GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Comprobantes
{
    public class ComprobanteCartera
    {
        public int CC_ID { get; set; }
        public int? CC_COMPROBANTE_SEVEN_ID { get; set; }
        public DateTime CC_FECHA_COMPROBANTE { get; set; }
        public int TCC_TIPO_COMPROBANTE_ID { get; set; }
        public int DPD_DEPARTAMENTO_ID { get; set; }
        public int DPC_CIUDAD_ID { get; set; }
        public int TCC_TIPO_CONCEPTO_ID { get; set; }
        public int TBC_TIPO_BENEFICIARIO_ID { get; set; }
        public int TIPO_IDENTIFICACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public DateTime CC_FECHA_APROBACION { get; set; }
        public Guid? CC_APROBADO_POR { get; set; }
        public bool? CC_APROBADO { get; set; }
        public decimal TOTAL_COMPROMBANTE_CREDITO { get; set; }
        public decimal TOTAL_COMPROMBANTE_DEBITO { get; set; }
        public int ESTADO_ID { get; set; }

        public IEnumerable<MovimientoComprobanteEntity> MOVCOMPROBANTE { get; set; } = new List<MovimientoComprobanteEntity>();
    }
}
