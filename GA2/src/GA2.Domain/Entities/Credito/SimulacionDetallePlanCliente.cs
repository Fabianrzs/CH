using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class SimulacionDetallePlanCliente
    {
        public Guid SPD_ID { get; set; }
        public Guid SPC_ID { get; set; }
        public int SPD_CUOTA { get; set; }
        public DateTime SPD_FECHA_CORTE { get; set; }
        public DateTime SPD_FECHA_PAGO { get; set; }
        public double SPD_SALDO { get; set; }
        public double SPD_INTERESES { get; set; }
        public double SPD_CAPITAL { get; set; }
        public double SPD_SEGURO_VIDA { get; set; }
        public double SPD_SEGURO_TERREMOTO { get; set; }
        public double SPD_CUOTA_TOTAL { get; set; }
        public double SPD_ABONO_EXTRA { get; set; }
        public double SPD_ABONO_MENSUAL { get; set; }
        public decimal SPD_CUOTA_UNIDAD_PAGADORA { get; set; }
        public double SPD_INTERESES_REDUCCION { get; set; }
        public double SPD_CUOTA_REDUCCION { get; set; }
        public double SPD_COBRO_FONVIVIENDA { get; set; }
        public decimal SPD_SALDO_LEASING { get; set; }
        public decimal SPD_INTERES_LEASING { get; set; }
        public decimal SPD_CAPITAL_LEASING { get; set; }
        public decimal SPD_CUOTA_LEASING { get; set; }
        public decimal SPD_CUOTA_LEASING_SUBSIDIO { get; set; }
        public decimal SPD_INTERES_TRADICIONAL_SUBSIDIO { get; set; }
        public double SPD_INTERES_LEASING_SUBSIDIO { get; set; }
        public decimal SPD_SEGURO_VIDA_LEASING { get; set; }
        public decimal SPD_SALDO_REDUCIDO_LEASING { get; set; }
        public decimal SPD_INTERESES_PAGADOS_LEASING { get; set; }
        public decimal SPD_CAPITAL_PAGADO_LEASING { get; set; }
        public decimal SPD_CAPITAL_A_PAGAR_LEASING { get; set; }
        public decimal SPD_SALDO_CAPITAL_LEASING { get; set; }
        public decimal SPD_INTERES_A_PAGAR_LEASING { get; set; }
        public decimal SPD_SALDO_TOTAL_LEASING { get; set; }
        public decimal SPD_AFECTACION_INDIVIDUAL_LEASING { get; set; }
        public double SPD_ABONO_UNICO { get; set; }
        public int AFL_ID { get; set; }
        public string SPD_NUMERO_DOCUMENTO { get; set; }
    }

}
