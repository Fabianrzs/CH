using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumSolicitudConsecutivo
    {

        [Description("@CNS_ID")]
        CNS_ID,

        [Description("@CNS_DESCRIPCION")]
        CNS_DESCRIPCION,

        [Description("@CNS_ULTIMO_NUMERO")]
        CNS_ULTIMO_NUMERO,

        [Description("@CNS_ESTADO")]
        CNS_ESTADO,        

        [Description("@CNS_CREADO_POR")]
        CNS_CREADO_POR,

        [Description("@CNS_FECHA_CREACION")]
        CNS_FECHA_CREACION,

        [Description("@CNS_ACTUALIZADO_POR")]
        CNS_ACTUALIZADO_POR,

        [Description("@CNS_FECHA_ACTUALIZA")]
        CNS_FECHA_ACTUALIZA,


    }
}
