using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumValidacionCreditoProspeccionParams
    {
        [Description("@VCP_ID")]
        VCP_ID,

        [Description("@VCP_NUMERO_DOCUMENTO")]
        VCP_NUMERO_DOCUMENTO,

        [Description("@VCP_CARGAR_DOCUMENTO")]
        VCP_CARGAR_DOCUMENTO,

        [Description("@VCP_ESTADO")]
        VCP_ESTADO,

        [Description("@VCP_FECHA")]
        VCP_FECHA
    }
}
