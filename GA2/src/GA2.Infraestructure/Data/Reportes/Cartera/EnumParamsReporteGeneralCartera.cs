using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes.Cartera
{
    public enum EnumParamsReporteGeneralCartera
    {
        [Description("@PERIODO_ACTUAL")]
        PeriodoActual,

        [Description("@ANIO")]
        Anio,

        [Description("@MES")]
        Mes,

        [Description("@TIPO_CREDITO")]
        TipoCredito,

        [Description("@NUMERO_CREDITO")]
        NumeroCredito,
    }
}