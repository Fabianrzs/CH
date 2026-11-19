using System.ComponentModel;

namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionDerechoResult
    {

        [Description("@ID")]
        ID,

        [Description("@CTA_ID")]
        CTA_ID,

        [Description("@CTA_NUMERO")]
        CTA_NUMERO,

        [Description("@CLI_ID")]
        CLI_ID,

        [Description("@FECHA_ULTIMO_MOVIMIENTOS_CREDITO")]
        FECHA_ULTIMO_MOVIMIENTOS_CREDITO,

        [Description("@CTA_FECHA_PRIMER_APORTE")]
        CTA_FECHA_PRIMER_APORTE,

        [Description("@CTA_CUOTAS")]
        CTA_CUOTAS,

        [Description("@CUOTAS_PENDIENTES")]
        CUOTAS_PENDIENTES,

        [Description("@ECT_INICIAL")]
        ECT_INICIAL,

        [Description("@ECT_FINAL")]
        ECT_FINAL,

        [Description("CAMBIO_ESTADO")]
        CAMBIO_ESTADO,

        [Description("@PERIODO_ANIO")]
        PERIODO_ANIO,

        [Description("@PERIODO_MES")]
        PERIODO_MES,

        [Description("@CTA_ID_INTEGRACION")]
        CTA_ID_INTEGRACION,

        [Description("@TCT_ID")]
        TCT_ID,

        [Description("@CTA_FECHA_CREACION")]
        CTA_FECHA_CREACION,

        [Description("@CANTIDAD_MOVIMIENTOS_CREDITO")]
        CANTIDAD_MOVIMIENTOS_CREDITO,

        [Description("@ULTIMO_ANIO_PERIODO")]
        ULTIMO_ANIO_PERIODO,

        [Description("@ULTIMO_MES_PERIODO")]
        ULTIMO_MES_PERIODO,

        [Description("@AHORROS_OBLIGATORIOS")]
        AHORROS_OBLIGATORIOS,

        [Description("@FLAG")]
        FLAG,

        [Description("@IDENTIFICACION")]
        IDENTIFICACION,

        [Description("@CTA_SALDO")]
        CTA_SALDO,



        [Description("@SALDO_ANUAL_APORTES")]
        SALDO_ANUAL_APORTES,

        [Description("@SALDO_ANUAL_CESANTIAS")]
        SALDO_ANUAL_CESANTIAS,

        [Description("@MOVIMIENTOS_MENSUAL_APORTES")]
        MOVIMIENTOS_MENSUAL_APORTES,

        [Description("@MOVIMIENTOS_MENSUAL_CESANTIAS")]
        MOVIMIENTOS_MENSUAL_CESANTIAS,



        [Description("@SALDO_ANTERIOR_APORTES")]
        SALDO_ANTERIOR_APORTES,

        [Description("@SALDO_ANTERIOR_CESANTIAS")]
        SALDO_ANTERIOR_CESANTIAS,

        [Description("@SALDO_FINAL_APORTES")]
        SALDO_FINAL_APORTES,

        [Description("@SALDO_FINAL_CESANTIAS")]
        SALDO_FINAL_CESANTIAS,



        [Description("@INTERESES_CAUSADO_APORTES")]
        INTERESES_CAUSADO_APORTES,

        [Description("@INTERESES_CAUSADO_CESANTIAS")]
        INTERESES_CAUSADO_CESANTIAS,

        [Description("@INTERESES_NO_CAUSADO_APORTES")]
        INTERESES_NO_CAUSADO_APORTES,

        [Description("@INTERESES_NO_CAUSADO_CESANTIAS")]
        INTERESES_NO_CAUSADO_CESANTIAS,

        [Description("@UNIDAD_EJECUTORA")]
        UNIDAD_EJECUTORA,

        [Description("@NUMERO_SOLICITUD")]
        NUMERO_SOLICITUD,
        [Description("@TIPO_SOLICITUD")]
        TIPO_SOLICITUD,


        [Description("@DATA")]
        DATA,

        [Description("@INTERES_APORTES")]
        INTERES_APORTES,

        [Description("@INTERES_CESANTIAS")]
        INTERES_CESANTIAS,


        [Description("@INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES")]
        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES,

        [Description("@INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS")]
        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS,
    }
}
