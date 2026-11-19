using System.ComponentModel;

namespace GA2.Infraestructure.Data.Cartera.Cierres
{
    public enum EnumCierresCausacionesRealizadas
    {
        [Description("@TIPOS_CIERRE")]
        TiposCierre,

        [Description("@FECHA_CIERRE_INICIAL")]
        FechaCierreInicial,

        [Description("@FECHA_CIERRE_FINAL")]
        FechaCierreFinal
    }
}
