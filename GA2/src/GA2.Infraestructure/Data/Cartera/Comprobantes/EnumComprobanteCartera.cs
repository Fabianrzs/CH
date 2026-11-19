using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cartera.Comprobantes
{
    public enum EnumComprobanteCartera
    {
        [Description("@CC_FECHA_COMPROBANTE")]
        CC_FECHA_COMPROBANTE,
        [Description("@TCC_TIPO_COMPROBANTE_ID")]
        TCC_TIPO_COMPROBANTE_ID,
        [Description("@DPD_DEPARTAMENTO_ID")]
        DPD_DEPARTAMENTO_ID,
        [Description("@DPC_CIUDAD_ID")]
        DPC_CIUDAD_ID,
        [Description("@TCC_TIPO_CONCEPTO_ID")]
        TCC_TIPO_CONCEPTO_ID,
        [Description("@TBC_TIPO_BENEFICIARIO_ID")]
        TBC_TIPO_BENEFICIARIO_ID,
        [Description("@TIPO_IDENTIFICACION")]
        TIPO_IDENTIFICACION,
        [Description("@CLI_IDENTIFICACION")]
        CLI_IDENTIFICACION,
        [Description("@TOTAL_COMPROMBANTE_CREDITO")]
        TOTAL_COMPROMBANTE_CREDITO,
        [Description("@TOTAL_COMPROMBANTE_DEBITO")]
        TOTAL_COMPROMBANTE_DEBITO,        
        [Description("@CC_APROBADO")]
        CC_APROBADO
    }
}
