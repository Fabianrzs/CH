using System.ComponentModel;

namespace GA2.Infraestructure.Data.CierreDiario
{
    public enum EnumCierreDiarioParams
    {
        [Description("@GENERA_ACTUALIZACION")]
        ActualizaDatos,

        [Description("@FECHA_CIERRE")]
        FechaCierreDiario,

        [Description("@CREADO_POR")]
        CreadoPor
    }
}
