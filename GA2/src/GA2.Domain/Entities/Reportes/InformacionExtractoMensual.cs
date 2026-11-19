using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Reportes
{
    public class InformacionExtractoMensual
    {
        public Guid CRE_ID { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public int TIPO_CREDITO_ID { get; set; }
        public string TIPO_CREDITO { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public int PLAZO_TOTAL { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public string TIPO_LIQUIDACION { get; set; }
        public int CUOTAS_PAGADAS { get; set; }
        public int CUOTAS_PENDIENTES { get; set; }
        public int CUOTAS_MORA { get; set; }
        public decimal INTERES_CORRIENTE_EA { get; set; }
        public decimal INTERES_MORA_EA { get; set; }
        public decimal SALDO { get; set; }
        public DateTime FECHA_CORTE { get; set; }

        public decimal CAPITAL { get; set; }
        public decimal INTERES_MORA { get; set; }
        public decimal INTERES_CORRIENTE { get; set; }
        public decimal SEGURO_VIDA { get; set; }
        public decimal SEGURO_TODO_RIESGO { get; set; }
        public decimal SEGUROS { get; set; }
        public decimal ANTICIPOS { get; set; }
        public decimal OTROS_CARGOS { get; set; }
        public decimal COBERTURA_FRECH { get; set; }
        public decimal OTROS_GASTOS { get; set; }
        public decimal VALOR_PAGO { get; set; }
        public decimal TOTAL_APLICADO { get; set; }

        public decimal PLN_PAGO_SEGURO_VIDA { get; set; }
        public decimal PLN_PAGO_SEGURO_TODO_RIESGO { get; set; }
        public decimal PLN_PAGO_INTERES_CORRIENTE { get; set; }
        public decimal PLN_PAGO_CAPITAL { get; set; }
        public decimal PLN_PAGO_A_PAGAR { get; set; }
    }
}
