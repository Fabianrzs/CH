using System;
using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumMatriculaCreditoParametros
    {
        [Description("@IdMatricula")]
        IdMatricula,

        [Description("@NumeroMatricula")]
        NumeroMatricula,

        [Description("@FechaMatricula")]
        FechaMatricula,

        [Description("@Chip")]
        Chip,

        [Description("@CedulaCatastral")]
        CedulaCatastral,

        [Description("@AreaPrivadaConstruida")]
        AreaPrivadaConstruida,

        [Description("@AreaAvaluoComercial")]
        AreaAvaluoComercial,

        [Description("@AreaAvaluoCatastral")]
        AreaAvaluoCatastral,

        [Description("@SocId")]
        SocId,

        [Description("@Tipo_Matricula")]
        Tipo_Matricula,

        [Description("@AreaConstruida")]
        AreaConstruidaDescripcion
    }
}
