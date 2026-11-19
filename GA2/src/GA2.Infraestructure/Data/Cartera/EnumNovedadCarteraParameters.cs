using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumNovedadCarteraParameters
    {

        [Description("@ID_NOVEDAD")]
        ID_NOVEDAD,

        [Description("@CRE_ID")]
        CRE_ID,

        [Description("@TNV_ID")]
        TNV_ID,

        [Description("@FECHA_NOVEDAD")]
        FECHA_NOVEDAD,

        [Description("@CAUSAL")]
        CAUSAL,

        [Description("@FECHA_FINALIZACION")]
        FECHA_FINALIZACION,

        [Description("@ENC_ID")]
        ENC_ID,

        [Description("@CAN_ID")]
        CAN_ID,

        [Description("@VALOR_PAGO")]
        VALOR_PAGO,

        [Description("@FECHA_APLICACION")]
        FECHA_APLICACION,

        [Description("@CREADO_POR")]
        CREADO_POR,

        [Description("@APLICAR_PAGO")]
        APLICAR_PAGO,

        [Description("@SEGURO_HOGAR")]
        SEGURO_HOGAR,

        [Description("@SEGURO_VIDA")]
        SEGURO_VIDA,

        [Description("@CAPITAL")]
        CAPITAL,

        [Description("@INTERESES_CORRIENTES")]
        INTERESES_CORRIENTES,

        [Description("@INTERESES_MORA")]
        INTERESES_MORA,

        [Description("@CAPITAL_REMANENTE")]
        CAPITAL_REMANENTE,

        [Description("@INTERES_REMANENTE")]
        INTERES_REMANENTE,

        [Description("@TOTAL_DEUDA")]
        TOTAL_DEUDA,

    }
}
