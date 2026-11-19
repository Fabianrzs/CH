using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.DatosAliadoEstrategico
{
    public enum EnumDatosAliadoEstrategicoParams
    {
        [Description("@DAT_ID ")]
        DAT_ID,

        [Description("@SOC_ID")]
        SOC_ID,

        [Description("@DAT_NOMBRE_ALIADO")]
        DAT_NOMBRE_ALIADO,

        [Description("@DAT_FECHA_PAGO_ESTUDIO_TITULO ")]
        DAT_FECHA_PAGO_ESTUDIO_TITULO,

        [Description("@DAT_FECHA_RECEPCION_ESTUDIO_TITULO")]
        DAT_FECHA_RECEPCION_ESTUDIO_TITULO,

        [Description("@DAT_CONCEPTO ")]
        DAT_CONCEPTO,

        [Description("@DAT_TIPO_PARQUEADERO")]
        DAT_TIPO_PARQUEADERO,

        [Description("@DAT_EXTRAPRIMA")]
        DAT_EXTRAPRIMA,

        [Description("@DAT_DEAL_INSURANCE")]
        DAT_DEAL_INSURANCE,

        [Description("@DAT_DEAL_INSURANCE_HOME")]
        DAT_DEAL_INSURANCE_HOME,

        [Description("@DAT_IMC")]
        DAT_IMC,

        [Description("@DAT_INSURANCE_ID")]
        DAT_INSURANCE_ID,

        [Description("@DAT_HOME_INSURANCE_ID")]
        DAT_HOME_INSURANCE_ID,
    }
}