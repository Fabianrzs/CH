using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.ValidacionTelAfiliado
{
    public enum EnumValidacionTelAfiliado
    {
        [Description("@VAL_ID")]
        VAL_ID,

        [Description("@SOC_ID")]
        SOC_ID,

        [Description("@VAL_CONF_VAL_INM")]
        VAL_CONF_VAL_INM,

        [Description("@VAL_ANIOS_FUERZA")]
        VAL_ANIOS_FUERZA,

        [Description("@VAL_LUGAR_TRABAJO")]
        VAL_LUGAR_TRABAJO,

        [Description("@VAL_CARGO_ACTUAL")]
        VAL_CARGO_ACTUAL,

        [Description("@VAL_VALOR_ARRIENDO")]
        VAL_VALOR_ARRIENDO,

        [Description("@VAL_VALOR_ACTIVOS")]
        VAL_VALOR_ACTIVOS,

        [Description("@VAL_VALOR_PASIVOS")]
        VAL_VALOR_PASIVOS,

        [Description("@VAL_MONTOS_RECURSOS_PROPIOS")]
        VAL_MONTOS_RECURSOS_PROPIOS,

        [Description("@VAL_OBSERVACIONES")]
        VAL_OBSERVACIONES
    }
}