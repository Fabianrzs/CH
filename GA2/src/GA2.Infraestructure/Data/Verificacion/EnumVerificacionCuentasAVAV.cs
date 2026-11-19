using System.ComponentModel;

namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionCuentasAVAV
    {

        [Description("@ID")]
        ID,
        [Description("@CTA_ID")]
        CTA_ID,
        [Description("@CTA_NUMERO")]
        CTA_NUMERO,
        [Description("@ECT_ID")]
        ECT_ID,
        [Description("@PERIODO_ANIO")]
        PERIODO_ANIO,
        [Description("@PERIODO_MES")]
        PERIODO_MES,
        [Description("@CTA_SALDO")]
        CTA_SALDO,


        [Description("@VALOR_AVAV")]
        VALOR_AVAV,
        [Description("@SALDO_ANUAL_AVAV")]
        SALDO_ANUAL_AVAV,
        [Description("@MOVIMIENTOS_MENSUAL_AVAV")]
        MOVIMIENTOS_MENSUAL_AVAV,
        [Description("@INTERESES_AVAV")]
        INTERESES_AVAV,
        [Description("@INTERESES_NO_CAUSADO_AVAV")]
        INTERESES_NO_CAUSADO_AVAV,
        [Description("@INTERESES_CAUSADO_AVAV")]
        INTERESES_CAUSADO_AVAV,
        [Description("@SALDO_ANTERIOR_AVAV")]
        SALDO_ANTERIOR_AVAV,
        [Description("@SALDO_FINAL_AVAV")]
        SALDO_FINAL_AVAV,


        [Description("@CLI_ID")]
        CLI_ID,
        [Description("@IDENTIFICACION")]
        IDENTIFICACION,
        [Description("@UNIDAD_EJECUTORA")]
        UNIDAD_EJECUTORA,


        [Description("@NUMERO_SOLICITUD")]
        NUMERO_SOLICITUD,
        [Description("@TIPO_SOLICITUD")]
        TIPO_SOLICITUD,

        [Description("@INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAV")]
        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAV,

    }
}

