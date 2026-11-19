using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsReportePago
    {
        [Description("@TIPO_CREDITO")]
        TipoCredito,

        [Description("@UNIDAD_EJECUTORA")]
        UnidadEjecutora,

        [Description("@FECHA_APLICACION_INICIAL")]
        FechaAplicacionInicial,

        [Description("@FECHA_APLICACION_FINAL")]
        FechaAplicacionFinal,

        [Description("@FECHA_PAGO_INICIAL")]
        FechaPagoInicial,

        [Description("@FECHA_PAGO_FINAL")]
        FechaPagoFinal
    }
}