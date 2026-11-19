using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsReporteContable
    {
        [Description("@FECHA_INICIAL")]
        FechaInicial,

        [Description("@FECHA_FINAL")]
        FechaFinal,

        [Description("@TIPO_CREDITO")]
        TipoCredito,

        [Description("@CANTIDAD_REGISTROS")]
        CantidadRegistros,

        [Description("@NUMERO_PAGINA")]
        NumeroPagina,

        [Description("@ANIO")]
        Anio,

        [Description("@MES")]
        Mes,
    }
}