using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Comprobantes.MovimientoComprobante
{
    public class MovimientoComprobanteEntity
    {
        public int MCC_ID { get; set; }
        public int CC_ID { get; set; }
        public int MCC_CUENTA_CONTABLE { get; set; }
        public string MCC_DESCRIPCION_CUENTA { get; set; }
        public decimal MCC_CENTRO_COSTO { get; set; }
        public decimal MCC_DETALLE { get; set; }
        public string MCC_TIPO { get; set; }
        public decimal MCC_MONTO { get; set; }
    }
}
