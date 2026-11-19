using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumExcepcionTipoAfiliadoParams
    {
        [Description("@PAR_ID")]
        IdParametro,

        [Description("@SIM_ID")]
        IdSimulador,

        [Description("@TPA_ID")]
        IdTipoActor,

        [Description("@PAR_TASA_EA")]
        TasaEA

}
}
