using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class EquivalenciaCuentasContablesSeven
    {
        public Guid CTA_EQUI_ID { get; set; }
        public int TCR_ID { get; set; }
        public string CUENTA_CONTABLE_SEVEN { get; set; }
        public string DESCRIPCION_CUENTA { get; set; }
        public string TIPO_OPERACION { get; set; }
        public Int32 COD_OPERCION_SEVEN { get; set; }
        public string REF_COLUMN_APLICA_PAGO { get; set; }
        public string CUENTAS_NIF { get; set; }
    }
}
