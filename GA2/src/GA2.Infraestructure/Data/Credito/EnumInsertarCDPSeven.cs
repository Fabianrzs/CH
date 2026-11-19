using System;
using System.ComponentModel;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumInsertarCDPSeven
    {
       
        [Description("@SOC_ID")]
        SOC_ID,
        [Description("@NUMERO_DOC")]
        NUMERO_DOC,
        [Description("@NOMBRE_AFILIADO")]
        NOMBRE_AFILIADO,
        [Description("@VALOR_PRESTAMO")]
        VALOR_PRESTAMO,
        [Description("@SOC_DESEMBOLSO_APLICADO")]
        SOC_DESEMBOLSO_APLICADO,
        [Description("@MPR_CONT")]
        MPR_CONT,
        [Description("@TCR_ID")]
        TCR_ID,
        [Description("@TID_ID")]
        TID_ID,
        [Description("@Emp_codi")]
        Emp_codi,
        [Description("@Top_codi")]
        Top_codi,
        [Description("@Arb_csuc")]
        Arb_csuc,
        [Description("@DPD_ID")]
        DPD_ID,
        [Description("@DPC_ID")]
        DPC_ID,
        [Description("@DMP_VALO")]
        Dmp_valo,
        [Description("@ARB_CODC")]
        Arb_codc,
        [Description("@PRO_CODI")]
        Pro_codi,
        [Description("@FECHA_CREACION_CDP")]
        FECHA_CREACION_CDP,
    }
}