using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cartera.Cierres
{
    public enum EnumCausacionDiariaInteresesCorrientes
    {
        [Description("@GENERA_ACTUALIZACION")]
        ActualizaDatos,

        [Description("@FECHA_CIERRE")]
        FechaCierreDiario,

        [Description("@CREADO_POR")]
        CreadoPor
    }
}
