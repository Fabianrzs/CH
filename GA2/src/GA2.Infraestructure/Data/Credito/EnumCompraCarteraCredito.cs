using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumCompraCarteraCredito
    {
        [Description("@CCC_ID")]
        CCC_ID,
        [Description("@CCC_VALOR_COMPRA_CARTERA")]
        CCC_VALOR_COMPRA_CARTERA,
        [Description("@CCC_CUOTA")]
        CCC_CUOTA,
        [Description("@CCC_PLAZO")]
        CCC_PLAZO,
        [Description("@CCC_VALOR_FINAL_PRESTAMO_CARTERA")]
        CCC_VALOR_FINAL_PRESTAMO_CARTERA,
        [Description("@SLS_ID")]
        SLS_ID,
        [Description("@CCC_NUMERO_OBLIGACION_CARTERA")]
        CCC_NUMERO_OBLIGACION_CARTERA

    }
}
