using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumDocumentoActividadCreditoParameters
    {
        [Description("@TAC_ID")]
        TAC_ID,

        [Description("@TDC_ID")]
        TDC_ID,

        [Description("@DCA_OBLIGATORIO")]
        DCA_OBLIGATORIO,

        [Description("@DCA_CARGA")]
        DCA_CARGA,

        [Description("@DCA_VISUALIZA")]
        DCA_VISUALIZA
    }
}
