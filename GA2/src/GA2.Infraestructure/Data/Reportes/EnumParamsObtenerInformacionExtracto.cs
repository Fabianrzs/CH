using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsObtenerInformacionExtracto
    {
        [Description("@CRE_NRO_CREDITO")]
        NumeroCredito,

        [Description("@TIPO_CREDITO")]
        TipoCredito,

        [Description("@UNIDAD_EJECUTORA")]
        UnidadEjecutora,

        [Description("@ANIO")]
        Anio,

        [Description("@MES")]
        Mes,
    }
}