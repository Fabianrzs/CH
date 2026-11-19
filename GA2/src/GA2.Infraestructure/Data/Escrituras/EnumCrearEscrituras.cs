using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Escrituras
{
    public enum EnumCrearEscrituras
    {
        [Description("@SOC_ID")]
        SOC_NRO_SOLICITUD,

        [Description("@CLI_IDENTIFICACION")]
        CLI_IDENTIFICACION,

        [Description("@ESC_ESCRITURA")]
        ESC_ESCRITURA,

        [Description("@ESC_FECHA")]
        ESC_FECHA,

        [Description("@ESC_NOTARIA")]
        ESC_NOTARIA,
    }
}
