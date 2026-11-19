using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cartera.ProductoPorCliente
{
    public enum EnumPagosProductoCliente
    {
        [Description("@CPG_ID")]
        CPG_ID,
        [Description("@CRE_ID")]
        CRE_ID,
        [Description("@CPG_FECHA_PAGO")]
        CPG_FECHA_PAGO,
        [Description("@CPG_FECHA_APLICACION")]
        CPG_FECHA_APLICACION,
        [Description("@CPG_ABONO_SEGURO_HOGAR")]
        CPG_ABONO_SEGURO_HOGAR,
        [Description("@CPG_ABONO_SEGURO_VIDA")]
        CPG_ABONO_SEGURO_VIDA,
        [Description("@CPG_ABONO_INTERES_CORRIENTE")]
        CPG_ABONO_INTERES_CORRIENTE,
        [Description("@CPG_ABONO_INTERES_MORA")]
        CPG_ABONO_INTERES_MORA,
        [Description("@CPG_ABONO_CAPITAL")]
        CPG_ABONO_CAPITAL,
        [Description("@CPG_VALOR_PAGO")]
        CPG_VALOR_PAGO,
        [Description("@DCT_ID")]
        DCT_ID,
        [Description("@CPG_CREADO_POR")]
        CPG_CREADO_POR,
        [Description("@CPG_FECHA_CREACION")]
        CPG_FECHA_CREACION,
        [Description("@CPG_MODIFICADO_POR")]
        CPG_MODIFICADO_POR,
        [Description("@CPG_FECHA_MODIFICACION")]
        CPG_FECHA_MODIFICACION,
        [Description("@TPP_ID")]
        TPP_ID,
        [Description("@CPG_ABONO_CAPITAL_REMANENTE")]
        CPG_ABONO_CAPITAL_REMANENTE,
        [Description("@CPG_ABONO_INTERES_REMANENTE")]
        CPG_ABONO_INTERES_REMANENTE,
        [Description("@CPG_ABONO_CAPITAL_MORA")]
        CPG_ABONO_CAPITAL_MORA
    }
}
