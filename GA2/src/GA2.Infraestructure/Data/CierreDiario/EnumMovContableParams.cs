using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumMovContableParams
    {
        [Description("@CRE_MOV_ID")]
        CRE_MOV_ID,

        [Description("@NUM_COMPROBANTE")]
        NUM_COMPROBANTE,

        [Description("@FECHA_MOVIMIENTO")]
        FECHA_MOVIMIENTO,

        [Description("@CUENTA_CONTABLE_SEVEN")]
        CUENTA_CONTABLE_SEVEN,
        
        [Description("@NUM_CREDITO")]
        NUM_CREDITO,

        [Description("@TIPO_IDENTIFICACION")]
        TIPO_IDENTIFICACION,

        [Description("@CLI_IDENTIFICACION")]
        CLI_IDENTIFICACION,

        [Description("@CONCEPTO_APLICADO")]
        CONCEPTO_APLICADO,

        [Description("@VALOR_MOVIMIENTO")]
        VALOR_MOVIMIENTO,

        [Description("@TIPO_MOVIMIENTO")]
        TIPO_MOVIMIENTO

    }
}
