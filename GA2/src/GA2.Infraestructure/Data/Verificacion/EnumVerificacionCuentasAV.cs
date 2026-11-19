using System.ComponentModel;

namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionCuentasAV
    {

        [Description("@ID")]
        ID,
        [Description("@CTA_ID")]
        CTA_ID,
        [Description("@CTA_NUMERO")]
        CTA_NUMERO, 
        [Description("@PERIODO_ANIO")]
        PERIODO_ANIO,
        [Description("@PERIODO_MES")]
        PERIODO_MES,


        [Description("@VALOR_AVAC")]
        VALOR_AVAC,
        [Description("@SALDO_ANUAL_AVAC")]
        SALDO_ANUAL_AVAC,
        [Description("@MOVIMIENTOS_MENSUAL_AVAC")]
        MOVIMIENTOS_MENSUAL_AVAC,
        [Description("@INTERESES_NO_CAUSADO_AVAC")]
        INTERESES_NO_CAUSADO_AVAC,
        [Description("@INTERESES_CAUSADO_AVAC")]
        INTERESES_CAUSADO_AVAC,
        [Description("@SALDO_ANTERIOR_AVAC")]
        SALDO_ANTERIOR_AVAC,
        [Description("@SALDO_FINAL_AVAC")]
        SALDO_FINAL_AVAC,


        [Description("@VALOR_AVAV")]
        VALOR_AVAV,
        [Description("@SALDO_ANUAL_AVAV")]
        SALDO_ANUAL_AVAV,
        [Description("@MOVIMIENTOS_MENSUAL_AVAV")]
        MOVIMIENTOS_MENSUAL_AVAV,
        [Description("@INTERESES_NO_CAUSADO_AVAV")]
        INTERESES_NO_CAUSADO_AVAV,
        [Description("@INTERESES_CAUSADO_AVAV")]
        INTERESES_CAUSADO_AVAV,
        [Description("@SALDO_ANTERIOR_AVAV")]
        SALDO_ANTERIOR_AVAV,
        [Description("@SALDO_FINAL_AVAV")]
        SALDO_FINAL_AVAV,


      
        [Description("@VALOR_AVC")]
        VALOR_AVC,
        [Description("@SALDO_ANUAL_AVC")]
        SALDO_ANUAL_AVC,
        [Description("@MOVIMIENTOS_MENSUAL_AVC")]
        MOVIMIENTOS_MENSUAL_AVC,
        [Description("@INTERESES_NO_CAUSADO_AVC")]
        INTERESES_NO_CAUSADO_AVC,
        [Description("@INTERESES_CAUSADO_AVC")]
        INTERESES_CAUSADO_AVC,
        [Description("@SALDO_ANTERIOR_AVC")]
        SALDO_ANTERIOR_AVC,
        [Description("@SALDO_FINAL_AVC")]
        SALDO_FINAL_AVC,


        [Description("@IDENTIFICACION")]
        IDENTIFICACION,
        [Description("@UNIDAD_EJECUTORA")]
        UNIDAD_EJECUTORA,


        [Description("@NUMERO_SOLICITUD")]
        NUMERO_SOLICITUD,
        [Description("@TIPO_SOLICITUD")]
        TIPO_SOLICITUD,

    }
}

