using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{

    public enum EnumValoresSimulacionCreditoProspeccion
    {
        [Description("@NUMERO_DOCUMENTO")]
        NUMERO_DOCUMENTO,

        [Description("@TASA_CREDITO")]
        TASA_CREDITO,

        [Description("@TASA_NOMINAL")]
        TASA_NOMINAL,

        [Description("@SEGURO_VIDA")]
        SEGURO_VIDA,

        [Description("@SEGURO_HOGAR")]
        SEGURO_HOGAR,

        [Description("@SALARIO_MINIMO")]
        SALARIO_MINIMO,

        [Description("@SEGURO_VIDA_EXTRAPRIMA")]
        SEGURO_VIDA_EXTRAPRIMA,

        [Description("@TASA_MIXTA")]
        TASA_MIXTA,

        [Description("@TASA_MIXTA_EXTRAPRIMA")]
        TASA_MIXTA_EXTRAPRIMA,

        [Description("@FACTOR")]
        FACTOR,

        [Description("@FACTOR_EXTRA_PRIMA")]
        FACTOR_EXTRA_PRIMA,

        [Description("@PLAZO")]
        PLAZO,

        [Description("@VALOR_PRESTAMO")]
        VALOR_PRESTAMO,

        [Description("@CUOTA_SIN_SEGURO_HOGAR")]
        CUOTA_SIN_SEGURO_HOGAR,

        [Description("@CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA")]
        CUOTA_SIN_SEGURO_HOGAR_EXTRAPRIMA,

        [Description("@CUOTA_SEGURO_HOGAR")]
        CUOTA_SEGURO_HOGAR,

        [Description("@CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA")]
        CUOTA_CON_SEGURO_HOGAR_EXTRAPRIMA,

        [Description("@VALOR_INMUEBLE")]
        VALOR_INMUEBLE,

        [Description("@VALOR_AVALUO")]
        VALOR_AVALUO,

        [Description("@CAPACIDAD_PAGO")]
        CAPACIDAD_PAGO,

        [Description("@VALOR_PRESTAMO_SEGUN_CUOTA")]
        VALOR_PRESTAMO_SEGUN_CUOTA
    }
}
