using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    public enum EnumConceptosSaldosCuentas
    {
        [Description("@CCT_ID")]
        CCT_ID,
        [Description("@CTA_ID")]
        CTA_ID,
        [Description("@CNC_ID")]
        CNC_ID,
        [Description("@CCT_SALDO")]
        CCT_SALDO,
        [Description("@CCT_FECHA_ULTIMO_APORTE")]
        CCT_FECHA_ULTIMO_APORTE,
        [Description("@CCT_INGRESOS")]
        CCT_INGRESOS,
        [Description("@CCT_SVA")]
        CCT_SVA,
        [Description("@BCC_ID")]
        BCC_ID,
        [Description("@CCT_BLOQUEO")]
        CCT_BLOQUEO,
    }
}
