using System.ComponentModel;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumParamsReporteEstadoCuenta
    {
        [Description("@CRE_ID")]
        IdCredito,

        [Description("@TIPO_CREDITO")]
        TipoCredito,

        [Description("@CRE_NRO_CREDITO")]
        NumeroCredito,

        [Description("@CLI_IDENTIFICACION")]
        NumeroIdentificacion,
    }
}