using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumExcepcioTasaUEJParam
    {
        [Description("@PAR_ID")]
        IdParametro, 

        [Description("@SIM_ID")]
        IdSimulador,

        [Description("@UEJ_ID")]
        UEJ,

        [Description("@PAR_TASA_EA")]
        TasaEA,

    }
}
