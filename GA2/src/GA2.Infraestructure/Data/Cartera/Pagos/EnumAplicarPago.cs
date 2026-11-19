using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumAplicarPago
    {
        [Description("@CPG_ID")]
        IdAplicacionPago,

        [Description("@CRE_ID")]
        IdCredito,

        [Description("@CPG_FECHA_PAGO")]
        FechaPago,

        [Description("@CPG_FECHA_APLICACION")]
        FechaAplicacion,

        [Description("@CPG_ABONO_SEGURO_HOGAR")]
        AbonoSeguroHogar,

        [Description("@CPG_ABONO_SEGURO_VIDA")]
        AbonoSeguroVida,

        [Description("@CPG_ABONO_INTERES_CORRIENTE")]
        AbonoInteresCorriente,

        [Description("@CPG_ABONO_INTERES_MORA")]
        AbonoInteresMora,

        [Description("@CPG_ABONO_CAPITAL")]
        AbonoCapital,

        [Description("@CPG_VALOR_PAGO")]
        ValorPago,

        [Description("@DCT_ID")]
        IdDct,

        [Description("@CPG_CREADO_POR")]
        CreadoPor,

        [Description("@CPG_FECHA_CREACION")]
        FechaCreacion,

        [Description("@CPG_MODIFICADO_POR")]
        ModificadoPor,

        [Description("@CPG_FECHA_MODIFICACION")]
        FechaModificacion,

        [Description("@TPP_CODIGO")]
        CodigotipoPago,

        [Description("@FUERZA")]
        FUERZA,

        [Description("@TIPO_NOMINA")]
        TIPO_NOMINA,

        [Description("@CONCEPTO_COD")]
        CONCEPTO_COD,

        [Description("@CONCEPTO")]
        CONCEPTO,

        [Description("@FUNCIONARIO")]
        FUNCIONARIO,

        [Description("@CEDULA")]
        CEDULA,

        [Description("@CODIGO_MILITAR")]
        CODIGO_MILITAR,
        
        [Description("@VALOR")]
        VALOR,
        
        [Description("@FECHA_DESDE")]
        FECHA_DESDE,
        
        [Description("@FECHA_HASTA")]
        FECHA_HASTA,
          
        [Description("@APLICA_PAGOS")]
        APLICA_PAGOS,

        [Description("@CRE_NRO_CREDITO")]
        CRE_NRO_CREDITO,

        [Description("@VALOR_PAGO")]
        VALOR_PAGO,

        [Description("@FECHA_APLICACION")]
        FECHA_APLICACION,

        [Description("@AAP_CODIGO")]
        AAP_CODIGO,

        [Description("@AAP_ID")]
        AAP_ID,

        [Description("@CREADO_POR")]
        CREADO_POR,

        [Description("@FECHA_APLICACION_PAGO")]
        FECHA_APLICACION_PAGO,

        [Description("@FECHA_CUOTA_PAGADA")]
        FECHA_CUOTA_PAGADA,

        [Description("@TPA_ID")]
        TPA_ID,

        [Description("@CUOTAS_ACUMULADAS")]
        CUOTAS_ACUMULADAS,
        
        [Description("@nCuota")]
        NUMERO_CUOTAS,

        [Description("@APLICA_SIMULACION")]
        APLICA_SIMULACION,

        [Description("@TCR_ID")]
        TCR_ID,

        [Description("@TIPORECURSO")]
        TIPO_RECURSO

    }
}
