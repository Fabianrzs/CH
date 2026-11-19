using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumSMCParameters
    {
        [Description("@TCR_ID")]
        IdTipoCredito,

        [Description("@SMC_ID")]
        IdSimulacionCliente,

        [Description("@SLS_ID")]
        IdSimulacion,

        [Description("@SDP_ID")]
        IdDatosPersonales,

        [Description("@SMC_TIPO_VIVIENDA")]
        TipoVivienda,

        [Description("@SMC_VALOR_VIVIENDA")]
        ValorVivienda,

        [Description("@SMC_VALOR_VIVIENDA_SEGURO")]
        ValorViviendaSeguro,

        [Description("@SMC_TIPO_ALIVIO")]
        TipoAlivio,

        [Description("@SMC_TIPO_ABONO")]
        TipoAbono,

        [Description("@SMC_NUMERO_DOCUMENTO")]
        Documento,

        [Description("@SMC_VALOR_PRESTAMO")]
        ValorPrestamo,

        [Description("@SMC_VALOR_TASA_EA")]
        TasaEa,

        [Description("@SMC_VALOR_TASA_MV")]
        TasaMv,

        [Description("@SMC_VALOR_TASA_MV_FRESH")]
        TasaFrech,

        [Description("@SMC_VIVIENDA_VIS")]
        ViviendaVis,

        [Description("@SMC_FECHA_SIMULACION")]
        FechaSimulacion,

        [Description("@SMC_USA_REC_ACUMULADO")]
        UsaRecursosAcumulados,

        [Description("@SMC_TOMA_SEGURO")]
        TomaSeguro,

        [Description("@SMC_VALOR_PRESTAMO_LSG")]
        ValorPrestamosLeasing,

        [Description("@SMC_NUMERO_PREAPROBADO")]
        NumeroPreaprobado,

        [Description("@SMC_TOTAL_CUOTAS")]
        TotalCuotas,

        [Description("@SMC_FECHA_APROBACION")]
        FechaAprobacion,

        [Description("@SMC_OPCION_COMPRA")]
        OpcionCompra,

        [Description("@SMC_TASA_SEGURO_VIDA")]
        TasaSeguroVida,

        [Description("@SMC_TASA_SEGURO_HOGAR")]
        TasaSeguroHogar,

        [Description("@SMC_VALOR_PRESTAMO_SOLICITADO")]
        ValorPrestamoSolicitado,

        [Description("@SMC_PORCEN_CANON_INICIAL")]
        PorcentajeCanonInicial,

        [Description("@SMC_PORCEN_OPCION_DE_COMPRA")]
        PorcentajeOpcionCompra,

        [Description("@SMC_TASA_VOLUNTARIA")]
        TasaVoluntaria,

        [Description("@SMC_FACTOR")]
        Factor,

        [Description("@USR_ID")]
        UsuarioId,

        [Description("@AFL_ID")]
        AflId,

        [Description("@CLC_ID")]
        Clc_id

    }
}
