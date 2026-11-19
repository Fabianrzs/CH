using System.ComponentModel;

namespace GA2.Infraestructure.Data.Credito.Simulador
{
    public enum EnumValoresProyectados
    {
        [Description("@SVP_ID")]
        Sim_Valores_Proyectado,

        [Description("@SLS_ID")]
        Simulacion_ID,

        [Description("@SVP_SUELDO")]
        Sim_Valores_Sueldo,

        [Description("@SVP_AHORROS")]
        Sim_Valores_Ahorros,

        [Description("@SVP_CESANTIAS")]
        Sim_Valores_Cesantias,

        [Description("@SVP_SUBSIDIO")]
        Sim_Valores_Subsidio,

        [Description("@SVP_EA")]
        Sim_Valores_EA,
    }
}
