using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class MovimientoContable
    {
        public Guid CRE_MOV_ID {  get; set; }
        public string NUM_COMPROBANTE { get; set; }
        public DateTime FECHA_MOVIMIENTO { get; set; }
        public string CUENTA_CONTABLE_SEVEN { get; set; }
        public Int32 NUM_CREDITO { get; set; }
        public int TIPO_IDENTIFICACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string CONCEPTO_APLICADO { get; set; }
        public decimal VALOR_MOVIMIENTO { get; set; }
        public string TIPO_MOVIMIENTO { get; set; }
    }
}
