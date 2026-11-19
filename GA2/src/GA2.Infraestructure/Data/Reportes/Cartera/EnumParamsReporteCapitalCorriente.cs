using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes.Cartera
{
    public enum EnumParamsReporteCapitalCorriente
    {
        [Description("@ANIO")]
        Anio,

        [Description("@MES")]
        Mes,

        [Description("@TIPO_CREDITO")]
        TipoCredito,
    }
}