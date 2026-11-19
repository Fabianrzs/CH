using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Catalog
{
    public enum EnumLineaCreditoParametros
    {
        [Description("@CLC_ID")]
        CLC_ID,
        [Description("@CLC_DESCRIPCION")]
        CLC_DESCRIPCION,
        [Description("@TCR_ID")]
        TCR_ID,
        [Description("@TPA_ID")]
        TPA_ID,
        [Description("@CLC_CUOTA_MINIMA")]
        CLC_CUOTA_MINIMA,
        [Description("@CLC_CUOTA_MAXIMA")]
        CLC_CUOTA_MAXIMA,
        [Description("@CLC_FECHA_CREACION")]
        CLC_FECHA_CREACION
    }
}
