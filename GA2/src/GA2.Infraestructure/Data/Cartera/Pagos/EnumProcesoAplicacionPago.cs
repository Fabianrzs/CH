using System.ComponentModel;

namespace GA2.Infraestructure.Data.Cartera.Pagos
{
    public enum EnumProcesoAplicacionPago
    {
        [Description("@CPG_ID")]
        CpgID,        
        
        [Description("@PAP_ID")]
        IdProceso,

        [Description("@APD_ID")]
        IdDetalle,

        [Description("@PAP_FECHA_APLICACION")]
        FechaAplicacion,

        [Description("@PAP_FECHA_PAGO")]
        PAP_FECHA_PAGO,

        [Description("@PAP_PERIODO")]
        Periodo,

        [Description("@CRE_ID")]
        IdCredito,

        [Description("@UEJ_ID")]
        UnidadEjecutoraId,

        [Description("@TCR_ID")]
        TipoCreditoId,

        [Description("@PAP_FECHA_CREACION")]
        FechaCreacion,

        [Description("@PAP_CREADO_POR")]
        CreadoPor,

        [Description("@PAP_MODIFICADO_POR")]
        ModificadoPor,

        [Description("@PAP_ESTADO")]
        Estado,

        [Description("@PAP_DESCRIPCION")]
        Descripcion,

        [Description("@PAP_JSON_PAGOS")]
        JsonPagos,

        [Description("@VALOR_SEGURO_HOGAR")]
        ValorSeguroHogar,

        [Description("@VALOR_SEGURO_VIDA")]
        ValorSeguroVida,

        [Description("@VALOR_INTERES_CORRIENTE")]
        ValorInteresCorriente,

        [Description("@VALOR_CAPITAL")]
        ValorCapital,

        [Description("@VALOR_CUOTA")]
        ValorCuota,

    }
}
