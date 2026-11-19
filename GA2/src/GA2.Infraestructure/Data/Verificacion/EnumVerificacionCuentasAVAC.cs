using System.ComponentModel;

namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionCuentasAVAC
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


        [Description("@VALOR_AVAC")]
        VALOR_AVAC,
        [Description("@SALDO_ANUAL_AVAC")]
        SALDO_ANUAL_AVAC,
        [Description("@MOVIMIENTOS_MENSUAL_AVAC")]
        MOVIMIENTOS_MENSUAL_AVAC,
        [Description("@INTERESES_AVAC")]
        INTERESES_AVAC,
        [Description("@INTERESES_NO_CAUSADO_AVAC")]
        INTERESES_NO_CAUSADO_AVAC,
        [Description("@INTERESES_CAUSADO_AVAC")]
        INTERESES_CAUSADO_AVAC,
        [Description("@SALDO_ANTERIOR_AVAC")]
        SALDO_ANTERIOR_AVAC,
        [Description("@SALDO_FINAL_AVAC")]
        SALDO_FINAL_AVAC,


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

        [Description("@INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC")]
        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC,

    }
}

