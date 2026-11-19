using System;

namespace GA2.Domain.Entities
{
    public class DetalleSimulacionCliente
    {
        public Guid SMD_ID { get; set; }
        public Guid SMC_ID { get; set; }
        public int SMD_CUOTA { get; set; }
        public DateTime SMD_FECHA_CORTE { get; set; }
        public DateTime SMD_FECHA_PAGO { get; set; }
        public decimal SMD_SALDO { get; set; }
        public decimal SMD_INTERESES { get; set; }
        public decimal SMD_CAPITAL { get; set; }
        public decimal SMD_SEGURO_VIDA { get; set; }
        public decimal SMD_SEGURO_TERREMOTO { get; set; }
        public decimal SMD_CUOTA_TOTAL { get; set; }
        public decimal SMD_ABONO_EXTRA { get; set; }
        public decimal SMD_ABONO_MENSUAL { get; set; }
        public decimal SMD_CUOTA_UNIDAD_PAGADORA { get; set; }
        public decimal SMD_INTERESES_REDUCCION { get; set; }
        public decimal SMD_CUOTA_REDUCCION { get; set; }
        public decimal SMD_COBRO_FONVIVIENDA { get; set; }
        public decimal SMD_SALDO_LEASING { get; set; }
        public decimal SMD_INTERES_LEASING { get; set; }
        public decimal SMD_CAPITAL_LEASING { get; set; }
        public decimal SMD_CUOTA_LEASING { get; set; }
        public decimal SMD_CUOTA_LEASING_SUBSIDIO { get; set; }
        public decimal SMD_INTERES_TRADICIONAL_SUBSIDIO { get; set; }
        public decimal SMD_INTERES_LEASING_SUBSIDIO { get; set; }
        public decimal SMD_SEGURO_VIDA_LEASING { get; set; }
        public decimal SMD_ABONO_UNICO { get; set; }
        public decimal SMD_SALDO_REDUCIDO_LEASING { get; set; }
        public decimal SMD_INTERESES_PAGADOS_LEASING { get; set; }
        public decimal SMD_CAPITAL_PAGADO_LEASING { get; set; }
        public decimal SMD_CAPITAL_A_PAGAR_LEASING { get; set; }
        public decimal SMD_SALDO_CAPITAL_LEASING { get; set; }
        public decimal SMD_INTERES_A_PAGAR_LEASING { get; set; }
        public decimal SMD_SALDO_TOTAL_LEASING { get; set; }
        public decimal SMD_AFECTACION_INDIVIDUAL_LEASING { get; set; }
        public decimal SMD_VALOR_FINANCIAR { get; set; }
        public decimal SMD_SEGUROS_TOTALES { get; set; }
        public int SMD_PLAZO { get; set; }
        public string SMD_NUMERO_RADICADO { get; set; }
        public int SMD_NUMERO_SOLICITUD { get; set; }
        public int SMD_SCORE { get; set; }
        public DateTime SID_FECHA_DESEMBOLSO { get; set; }


    }
}
