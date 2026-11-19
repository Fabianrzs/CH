using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumObtenerUsuarioVerificacionDerechos
    {

        [Description("@CodigoPerfil")]
        CodigoPerfil,

        [Description("@Usuario")]
        Usuario,

        [Description("@Notificacion")]
        Notificacion,

        [Description("@NumRadicado")]
        NumRadicado,        

        [Description("@Modulo")]
        Modulo,

        [Description("@NUM_CEDULA")]
        NUM_CEDULA,

        [Description("@TareaSiguiente")]
        TareaSiguiente,

        [Description("@UsuarioEnvia")]
        UsuarioEnvia,
        [Description("@FechaRegistro")]
        FechaRegistro,

        [Description("@SLT_ID")]
        SLT_ID,

        [Description("@TRA_ID")]
        TRA_ID,

        [Description("@SOL_ID")]
        SOL_ID,

        [Description("@USR_ID")]
        USR_ID,

        [Description("@OBSERVACION")]
        OBSERVACION,

        [Description("@SLT_ESTADO_SOLICITUD")]
        SLT_ESTADO_SOLICITUD,

        [Description("@Link")]
        Link,

    }
}
