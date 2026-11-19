using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cartera.Comprobantes
{
    public enum EnumMovimientoComprobanteCartera
    {
        [Description("@MCC_ID")]
        MCC_ID,
        [Description("@CC_ID")]
        CC_ID,
        [Description("@MCC_CUENTA_CONTABLE")]
        MCC_CUENTA_CONTABLE,
        [Description("@MCC_DESCRIPCION_CUENTA")]
        MCC_DESCRIPCION_CUENTA,
        [Description("@MCC_CENTRO_COSTO")]
        MCC_CENTRO_COSTO,
        [Description("@MCC_DETALLE")]
        MCC_DETALLE,
        [Description("@MCC_TIPO")]
        MCC_TIPO,
        [Description("@MCC_MONTO")]
        MCC_MONTO,
        [Description("@TRANID")]
        TRANID
    }
}
