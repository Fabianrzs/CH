using System.ComponentModel;

namespace GA2.Infraestructure.Data.Parametrization
{ /// <summary>
  /// Data de parametrizacion legal tasa
  /// <author>Cesar Hernandez</author>
  /// <date>21/03/2023</date>
  /// </summary>
    public enum EnumIPCParameters
    {

        [Description("@ID")]
        ID,

        [Description("@ANIO")]
        ANIO,

        [Description("@MES")]
        MES,

        [Description("@FECHA_ACTUALIZACION")]
        FECHA_ACTUALIZACION,

        [Description("@VALOR_IPC")]
        VALOR_IPC,

        [Description("@PORCENTAJE_IPC")]
        PORCENTAJE_IPC,

        [Description("@Desde_Anio")]
        Desde_Anio,

        [Description("@Desde_Mes")]
        Desde_Mes, 

        [Description("@Hasta_Anio")]
        Hasta_Anio,

        [Description("@Hasta_Mes")]
        Hasta_Mes,

    }
}
