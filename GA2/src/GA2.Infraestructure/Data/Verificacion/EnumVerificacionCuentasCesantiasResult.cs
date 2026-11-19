using System.ComponentModel;

namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionCuentasCesantiasResult
    {

        [Description("@ID")]
        ID,

        [Description("@CTA_ID")]
        CTA_ID,

        [Description("@CTA_NUMERO")]
        CTA_NUMERO,

        [Description("@CLI_ID")]
        CLI_ID,

        [Description("@TCT_ID")]
        TCT_ID,

        [Description("@CTA_SALDO")]
        CTA_SALDO,

        [Description("@FECHA_ULTIMO_MOVIMIENTOS_CREDITO")]
        FECHA_ULTIMO_MOVIMIENTOS_CREDITO,

        [Description("@IDENTIFICACION")]
        IDENTIFICACION,

        [Description("@PERIODO_ANIO")]
        PERIODO_ANIO,

        [Description("@PERIODO_MES")]
        PERIODO_MES,

        [Description("SALDO_ANUAL_CESANTIAS")]
        SALDO_ANUAL_CESANTIAS,

        [Description("@MOVIMIENTOS_MENSUAL_CESANTIAS")]
        MOVIMIENTOS_MENSUAL_CESANTIAS,

        [Description("@INTERESES_NO_CAUSADO_CESANTIAS")]
        INTERESES_NO_CAUSADO_CESANTIAS,

        [Description("@INTERESES_CAUSADO_CESANTIAS")]
        INTERESES_CAUSADO_CESANTIAS,

        [Description("@SALDO_ANTERIOR_CESANTIAS")]
        SALDO_ANTERIOR_CESANTIAS,

        [Description("@SALDO_FINAL_CESANTIAS")]
        SALDO_FINAL_CESANTIAS,


        [Description("@UNIDAD_EJECUTORA")]
        UNIDAD_EJECUTORA,


        [Description("@NUMERO_SOLICITUD")]
        NUMERO_SOLICITUD,
        [Description("@TIPO_SOLICITUD")]
        TIPO_SOLICITUD,

        [Description("@INTERES_CESANTIAS")]
        INTERES_CESANTIAS,

        [Description("@INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS")]
        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS,

    }
}
