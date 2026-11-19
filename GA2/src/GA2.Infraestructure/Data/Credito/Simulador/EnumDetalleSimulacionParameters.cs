using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    public enum EnumDetalleSimulacionParameters
    {

        [Description("@SMD_ID")]
        SMD_ID,

        [Description("@SMC_ID")]
        SMC_ID,

        [Description("@SMD_CUOTA")]
        SMD_CUOTA,

        [Description("@SMD_FECHA_CORTE")]
        SMD_FECHA_CORTE,

        [Description("@SMD_FECHA_PAGO")]
        SMD_FECHA_PAGO,

        [Description("@SMD_SALDO")]
        SMD_SALDO,

        [Description("@SMD_INTERESES")]
        SMD_INTERESES,

        [Description("@SMD_CAPITAL")]
        SMD_CAPITAL,

        [Description("@SMD_SEGURO_VIDA")]
        SMD_SEGURO_VIDA,

        [Description("@SMD_SEGURO_TERREMOTO")]
        SMD_SEGURO_TERREMOTO,

        [Description("@SMD_CUOTA_TOTAL")]
        SMD_CUOTA_TOTAL,

        [Description("@SMD_ABONO_EXTRA")]
        SMD_ABONO_EXTRA,

        [Description("@SMD_ABONO_MENSUAL")]
        SMD_ABONO_MENSUAL,

        [Description("@SMD_CUOTA_UNIDAD_PAGADORA")]
        SMD_CUOTA_UNIDAD_PAGADORA,

        [Description("@SMD_INTERESES_REDUCCION")]
        SMD_INTERESES_REDUCCION,

        [Description("@SMD_CUOTA_REDUCCION")]
        SMD_CUOTA_REDUCCION,

        [Description("@SMD_COBRO_FONVIVIENDA")]
        SMD_COBRO_FONVIVIENDA,

        [Description("@SMD_SALDO_LEASING")]
        SMD_SALDO_LEASING,

        [Description("@SMD_INTERES_LEASING")]
        SMD_INTERES_LEASING,

        [Description("@SMD_CAPITAL_LEASING")]
        SMD_CAPITAL_LEASING,

        [Description("@SMD_CUOTA_LEASING")]
        SMD_CUOTA_LEASING,

        [Description("@SMD_CUOTA_LEASING_SUBSIDIO")]
        SMD_CUOTA_LEASING_SUBSIDIO,

        [Description("@SMD_INTERES_TRADICIONAL_SUBSIDIO")]
        SMD_INTERES_TRADICIONAL_SUBSIDIO,

        [Description("@SMD_INTERES_LEASING_SUBSIDIO")]
        SMD_INTERES_LEASING_SUBSIDIO,

        [Description("@SMD_SEGURO_VIDA_LEASING")]
        SMD_SEGURO_VIDA_LEASING,

        [Description("@SMD_SALDO_REDUCIDO_LEASING")]
        SMD_SALDO_REDUCIDO_LEASING,

        [Description("@SMD_INTERESES_PAGADOS_LEASING")]
        SMD_INTERESES_PAGADOS_LEASING,

        [Description("@SMD_CAPITAL_PAGADO_LEASING")]
        SMD_CAPITAL_PAGADO_LEASING,

        [Description("@SMD_CAPITAL_A_PAGAR_LEASING")]
        SMD_CAPITAL_A_PAGAR_LEASING,

        [Description("@SMD_SALDO_CAPITAL_LEASING")]
        SMD_SALDO_CAPITAL_LEASING,

        [Description("@SMD_INTERES_A_PAGAR_LEASING")]
        SMD_INTERES_A_PAGAR_LEASING,

        [Description("@SMD_SALDO_TOTAL_LEASING")]
        SMD_SALDO_TOTAL_LEASING,

        [Description("@SMD_AFECTACION_INDIVIDUAL_LEASING")]
        SMD_AFECTACION_INDIVIDUAL_LEASING,

        [Description("@SMD_ABONO_UNICO")]
        SMD_ABONO_UNICO,

        [Description("@NUMERO_DOCUMENTO")]
        NUMERO_DOCUMENTO,

        [Description("@AFL_ID")]
        AFL_ID,

        [Description("@SMD_VALOR_FINANCIAR")]
        SMD_VALOR_FINANCIAR,

        [Description("@SMD_SEGUROS_TOTALES")]
        SMD_SEGUROS_TOTALES,
        
        [Description("@SMD_SCORE")]
        SMD_SCORE
    }
}
