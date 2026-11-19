using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cartera.Pagos
{
    public enum EnumActualizarCreCreditoEliminarPlanPagos
    {
        [Description("@NUEVOSALDOCAPITAL")]
        NUEVOSALDOCAPITAL,
        [Description("@NUEVOVALORCUOTAREDUCIDO")]
        NUEVOVALORCUOTAREDUCIDO,
        [Description("@NUEVOSALDOTRADICIONAL")]
        NUEVOSALDOTRADICIONAL,
        [Description("@NUEVOVALORCUOTATRADICIONAL")]
        NUEVOVALORCUOTATRADICIONAL,
        [Description("@CREID")]
        CREID,
        [Description("@TCRID")]
        TCRID,
        [Description("@NROCREDITO")]
        NROCREDITO,
        [Description("@MODIFICADOPOR")]
        MODIFICADOPOR,
        [Description("@AAP_CODIGO")]
        AAP_CODIGO,
        [Description("@CRE_PLAZO_TOTAL")]
        CRE_PLAZO_TOTAL,
        [Description("@CRE_PLAZO_ACTUAL")]
        CRE_PLAZO_ACTUAL,
        [Description("@CRE_CANON_EXTRAORDINARIO")]
        CRE_CANON_EXTRAORDINARIO,
        [Description("@CRE_FECHA_FINALIZA_PAGOS")]
        CRE_FECHA_FINALIZA_PAGOS,
    }
}
