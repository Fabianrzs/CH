using System.ComponentModel;

namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionCuentasAVC
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


        [Description("@VALOR_AVC")]
        VALOR_AVC,
        [Description("@SALDO_ANUAL_AVC")]
        SALDO_ANUAL_AVC,
        [Description("@MOVIMIENTOS_MENSUAL_AVC")]
        MOVIMIENTOS_MENSUAL_AVC,
        [Description("@INTERESES_AVC")]
        INTERESES_AVC,
        [Description("@INTERESES_NO_CAUSADO_AVC")]
        INTERESES_NO_CAUSADO_AVC,
        [Description("@INTERESES_CAUSADO_AVC")]
        INTERESES_CAUSADO_AVC,
        [Description("@SALDO_ANTERIOR_AVC")]
        SALDO_ANTERIOR_AVC,
        [Description("@SALDO_FINAL_AVC")]
        SALDO_FINAL_AVC,


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

        [Description("@INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC")]
        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC,

    }
}

