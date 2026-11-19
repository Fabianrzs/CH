using System;
using System.ComponentModel;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumObtenerComprobantesCredito
    {
        [Description("@FECHA_INICIO")]
        FECHA_INICIO,
        [Description("@FECHA_FIN")]
        FECHA_FIN,
        [Description("@TCC_CODIGO")]
        TCC_CODIGO
    }
}
