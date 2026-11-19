using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.InfoVendedor
{
    public enum EnumInformacionVendedor
    {
        [Description("@IVEN_ID")]
        IVEN_ID,

        [Description("@SOC_ID")]
        SOC_ID,

        [Description("@SLS_ID")]
        SLS_ID,

        [Description("@IVEN_TIPO_DOC")]
        IVEN_TIPO_DOC,

        [Description("@IVEN_NRO_DOC")]
        IVEN_NRO_DOC,

        [Description("@IVEN_NOMBRE")]
        IVEN_NOMBRE,

        [Description("@IVEN_DIRECCION")]
        IVEN_DIRECCION,

        [Description("@IVEN_DEPARTAMANTO")]
        IVEN_DEPARTAMANTO,

        [Description("@IVEN_CIUDAD")]
        IVEN_CIUDAD,

        [Description("@IVEN_CORREO")]
        IVEN_CORREO,

        [Description("@IVEN_TELEFONO")]
        IVEN_TELEFONO,

        [Description("@IVEN_FECHA_ENTREGA")]
        IVEN_FECHA_ENTREGA,

        [Description("@IVEN_TIPO_CUENTA")]
        IVEN_TIPO_CUENTA,

        [Description("@IVEN_BANCO")]
        IVEN_BANCO,

        [Description("@IVEN_NRO_CUENTA")]
        IVEN_NRO_CUENTA,

        [Description("@PVD_CODD")]
        PVD_CODD,

    }
}