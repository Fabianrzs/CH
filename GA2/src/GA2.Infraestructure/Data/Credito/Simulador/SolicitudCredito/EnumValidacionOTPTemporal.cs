using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito.Simulador.SolicitudCredito
{
    public enum EnumValidacionOTPTemporal
    {

        [Description("@VOT_CELULAR")]
        Celular,

        [Description("@VOT_IDENTIFICACION")]
        Identificacion,

        [Description("@FECHA")]
        FechaRegistro,

    }
}
