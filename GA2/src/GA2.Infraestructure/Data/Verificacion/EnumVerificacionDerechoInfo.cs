using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionDerechoInfo
    {

        [Description("@VDD_ID")]
        VDD_ID,

        [Description("@PARAM_VD_ID")]
        PARAM_VD_ID,

        [Description("@ESTADO_INICIAL")]
        ESTADO_INICIAL,

        [Description("@ESTADO_FINAL")]
        ESTADO_FINAL,

        [Description("@MOTIVO_CAMBIO")]
        MOTIVO_CAMBIO,

        [Description("@CUENTAS_AFECTADAS")]
        CUENTAS_AFECTADAS,
              

    }
}
