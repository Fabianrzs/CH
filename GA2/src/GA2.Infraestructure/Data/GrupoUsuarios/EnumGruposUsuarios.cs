using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data.GrupoUsuarios
{
    public enum EnumGruposUsuarios
    {
        [Description("@ID")]
        ID,

        [Description("@CODIGO")]
        CODIGO,

        [Description("@NOMBRE")]
        NOMBRE,

        [Description("@DESCRIPCION")]
        DESCRIPCION,

        [Description("@ESTADO")]
        ESTADO,

        [Description("@USR_CREADOR")]
        USR_CREADOR,

        [Description("@USR_MODIFICADOR")]
        USR_MODIFICADOR,

    }


    public enum EnumGruposPorUsuarios
    {
        [Description("@USR_ID")]
        USR_ID,

        [Description("@GRP_ID")]
        GRP_ID,

    }

  

    public enum EnumGruposPorOpciones
    {
        [Description("@OPC_ID")]
        OPC_ID,

        [Description("@GRP_ID")]
        GRP_ID,
   
    }

    public enum EnumSubOpcionesPorOpciones
    {
        [Description("@IdOpcion")]
        IdOpcion
    }
}
