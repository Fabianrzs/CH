using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsReporteInformacionContable
    {
        [Description("@FECHA_INICIAL")]
        FechaInicial,

        [Description("@FECHA_FINAL")]
        FechaFinal,

        [Description("@CUENTA_CONTABLE")]
        CuentaContable
    }
}