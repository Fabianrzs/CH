using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumGenerarListaParams
    {
        [Description("@UEJ_ID")]
        IdUnidadEjecutora,

        [Description("@TCR_ID")]
        TipoCredito,

        [Description("@FECHA_APLICACION")]
        FechaAplicacion
    }
}
