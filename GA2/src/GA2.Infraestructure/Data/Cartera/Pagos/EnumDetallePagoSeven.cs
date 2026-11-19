using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cartera.Pagos
{
    public enum EnumDetallePagoSeven
    {
        [Description("@CPG_ID")]
        CPG_ID,

        [Description("@EMP_CODI")]
        EMP_CODI,

        [Description("@TOP_CODI")]
        TOP_CODI,

        [Description("@MCO_FECH")]
        MCO_FECH,

        [Description("@MOD_CODI")]
        MOD_CODI,

        [Description("@MCO_DESC")]
        MCO_DESC,

        [Description("@CUE_CODI")]
        CUE_CODI,

        [Description("@DMC_DESC")]
        DMC_DESC,

        [Description("@DMC_CANT")]
        DMC_CANT,

        [Description("@DMC_REFE")]
        DMC_REFE,

        [Description("@DMC_VADB")]
        DMC_VADB,

        [Description("@DMC_VACR")]
        DMC_VACR,

        [Description("@TER_CODA")]
        TER_CODA,

        [Description("@ARB_CODC")]
        ARB_CODC,

        [Description("@ARB_CODA")]
        ARB_CODA,

        [Description("@ARB_CODP")]
        ARB_CODP,

        [Description("@ARB_CODS")]
        ARB_CODS,

        [Description("@MOV_CONTABLE_SEVEN")]
        MOV_CONTABLE_SEVEN,

        [Description("@RESPUESTA_SEVEN")]
        RESPUESTA_SEVEN,

        [Description("@FECHA_EJECUCION")]
        FECHA_EJECUCION,

        [Description("@CC_ID")]
        CC_ID
    }

    public enum EnumComprobantePagoSeven
    {
        [Description("@EMP_CODI")]
        EMP_CODI,

        [Description("@TOP_CODI")]
        TOP_CODI,

        [Description("@MCO_FECH")]
        MCO_FECH,

        [Description("@MOD_CODI")]
        MOD_CODI,

        [Description("@MCO_DESC")]
        MCO_DESC,

        [Description("@MOV_CONTABLE_SEVEN")]
        MOV_CONTABLE_SEVEN,

        [Description("@RESPUESTA_SEVEN")]
        RESPUESTA_SEVEN,

        [Description("@FECHA_EJECUCION")]
        FECHA_EJECUCION
    }


    public enum EnumMovimientosCarteraComprobante
    {
        [Description("@CC_ID")]
        CC_ID,

        [Description("@MCC_CUENTA_CONTABLE")]
        MCC_CUENTA_CONTABLE,

        [Description("@MCC_DESCRIPCION_CUENTA")]
        MCC_DESCRIPCION_CUENTA,

        [Description("@MCC_CENTRO_COSTO")]
        MCC_CENTRO_COSTO,

        [Description("@MCC_DETALLE")]
        MCC_DETALLE,

        [Description("@MCC_TIPO")]
        MCC_TIPO,

        [Description("@MCC_MONTO")]
        MCC_MONTO
    }

}
