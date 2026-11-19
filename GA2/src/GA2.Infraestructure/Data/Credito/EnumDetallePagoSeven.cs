using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumDetallePagoSeven
    {
    
        [Description("@cuenta")]
        CUENTA,

        [Description("@identidad")]
        ACT_ID,

        [Description("@no_comprobante")]
        NO_COMPROBANTE,

        [Description("@fecha_ini")]
        FECHA_INICIAL,

        [Description("@fecha_fin")]
        FECHA_FIN,

        [Description("@TCR_ID")]
        TCR_ID
    }


}

