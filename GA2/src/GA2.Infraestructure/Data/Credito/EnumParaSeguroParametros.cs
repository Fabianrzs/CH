using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumParaSeguroParametros
    {
        [Description("@SEU_ID")]
        SEU_ID,

        [Description("@SEU_TODO_RIESGO")]
        SEU_TODO_RIESGO,

        [Description("@DPD_ID")]
        DPD_ID,

        [Description("@DPC_ID")]
        DPC_ID,

        [Description("@SEU_LIMITE_VIS")]
        SEU_LIMITE_VIS,

        [Description("@SEU_LIMITE_VIP")]
        SEU_LIMITE_VIP,

    }
}