using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumOpcionesTemporales
    {
        [Description("@UOT_ID")]
        UOT_ID,

        [Description("@USR_ID")]
        USR_ID,

        [Description("@OPC_ID")]
        OPC_ID,

        [Description("@OPC_NOMBRE")]
        OPC_NOMBRE,

        [Description("@OPC_DESCRIPCION")]
        OPC_DESCRIPCION,

        [Description("@OPC_CODIGO")]
        OPC_CODIGO,

        [Description("@UOT_FECHA_DESDE")]
        UOT_FECHA_DESDE,

        [Description("@UOT_FECHA_HASTA")]
        UOT_FECHA_HASTA,
    }
}
