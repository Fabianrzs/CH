using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumProgramaEducativoParametros
    {
        [Description("@PGE_ID")]
        PGE_ID,

        [Description("@ENE_ID_FK")]
        ENE_ID,

        [Description("@PGE_NVE_ID")]
        PGE_NVE_ID,

        [Description("@PGE_CTL_ID")]
        PGE_CTL_ID,

        [Description("@PGE_DESCRIPCION")]
        PGE_DESCRIPCION,

        [Description("@PGE_ESTADO")]
        PGE_ESTADO,

        [Description("@PGE_CREATEDOPOR")]
        PGE_CREATEDOPOR,

        [Description("@PGE_FECHACREACION")]
        PGE_FECHACREACION,

        [Description("@PGE_MODIFICADOPOR")]
        PGE_MODIFICADOPOR,

        [Description("@PGE_FECHAMODIFICACION")]
        PGE_FECHAMODIFICACION,

        [Description("@TID_ID_FK")]
        TipoIdentificacion,

        [Description("@ESE_NUMERO_IDENTIFICACION")]
        NumeroIdentificacion,

        [Description("@ESE_NIT")]
        Nit,

        [Description("@ESE_NOMBRE_RAZON_SOCIAL")]
        Razon,

        [Description("@ESE_CREATEDOPOR")]
        CreadoPor
    }
}