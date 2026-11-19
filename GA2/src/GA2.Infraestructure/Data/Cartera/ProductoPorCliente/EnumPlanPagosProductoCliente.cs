using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Cartera.ProductoPorCliente
{
    public enum EnumPlanPagosProductoCliente
    {
        [Description("@PLN_NUMERO_CREDITO")]
        PLN_NUMERO_CREDITO,
        [Description("@PLN_NUMERO_CUOTA")]
        PLN_NUMERO_CUOTA,
        [Description("@PLN_FECHA_PAGO")]
        PLN_FECHA_PAGO,
        [Description("@PLN_SALDO_INICIAL")]
        PLN_SALDO_INICIAL,
        [Description("@PLN_CAPITAL")]
        PLN_CAPITAL,
        [Description("@PLN_CAPITAL_POR_PAGAR")]
        PLN_CAPITAL_POR_PAGAR,
        [Description("@PLN_INTERESES")]
        PLN_INTERESES,
        [Description("@PLN_INTERES_POR_PAGAR")]
        PLN_INTERES_POR_PAGAR,
        [Description("@PLN_SEGURO_VIDA")]
        PLN_SEGURO_VIDA,
        [Description("@PLN_SEGURO_HOGAR")]
        PLN_SEGURO_HOGAR,
        [Description("@PLN_A_PAGAR")]
        PLN_A_PAGAR,
        [Description("@PLN_PLAZO")]
        PLN_PLAZO,
        [Description("@PLN_VALOR_DEUDA_REMANENTE")]
        PLN_VALOR_DEUDA_REMANENTE,
        [Description("@PLN_F14")]
        PLN_F14,
        [Description("@TCR_ID")]
        TCR_ID
    }
}
