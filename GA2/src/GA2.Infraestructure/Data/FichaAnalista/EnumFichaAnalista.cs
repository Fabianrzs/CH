using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.FichaAnalista
{
    public enum EnumFichaAnalista
    {
        [Description("@FA_ID")]
        FA_ID,

        [Description("@SOC_ID")]
        SOC_ID,

        [Description("@FA_NOMBRE")]
        FA_NOMBRE,

        [Description("@FA_CARGO")]
        FA_CARGO
    }
}
