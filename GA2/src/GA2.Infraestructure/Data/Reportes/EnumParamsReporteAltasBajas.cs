using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsReporteAltasBajas
    {
        [Description("@UEJ_ID")]
        IdUnidadEjecutora,

        [Description("@TIPO_REPORTE")]
        TipoReporte,
        [Description("@PERIODO_GENERACION")]
        Periodo
    }
}
