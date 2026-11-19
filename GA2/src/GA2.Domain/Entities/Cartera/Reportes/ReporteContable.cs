using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReporteContable
    {
        public int TIPO_OPERACION { get; set; }
        public int TCR_ID { get; set; }
        public string DESCRIPCION_OPERACION { get; set; }
        public string DESCRIPCION_CUENTA { get; set; }
        public DateTime FECHA_OPERACION { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public string CUENTA_SEVEN { get; set; }
        public string CUENTAS_NIF { get; set; }
        public decimal DEBITO { get; set; }
        public decimal CREDITO { get; set; }
        public long TOTAL_REGISTROS { get; set; }
        public string CENTRO_COSTO { get; set; }
        public string CENTRO_COSTO_CREDITO { get; set; }
        public string CC_ID_COMPROBANTE { get; set; }

    }

}
