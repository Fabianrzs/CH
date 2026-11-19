using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsReporteEstadoMovimientos
    {
        [Description("@CRE_ID")]
        IdCredito,

        [Description("@FECHA_INICIAL")]
        FechaInicial,

        [Description("@FECHA_FINAL")]
        FechaFinal,
    }
}