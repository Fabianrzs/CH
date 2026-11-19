using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.ValoresCategoria
{
    public enum EnumValoresCategoria
    {
        [Description("@CRG_ID")]
        CRG_ID,

        [Description("@CRG_DESCRIPCION")]
        CRG_DESCRIPCION,

        [Description("@CTG_ID")]
        CTG_ID,

        [Description("@CRG_FACTOR_SUBS")]
        CRG_FACTOR_SUBS,

        [Description("@CRG_ANIOS_ASCENSO")]
        CRG_ANIOS_ASCENSO,

        [Description("@CRA_SALARIO_BASICO")]
        CRA_SALARIO_BASICO,

        [Description("@CRA_SALARIO_TOTAL")]
        CRA_SALARIO_TOTAL,

        [Description("@CRA_ANIO")]
        CRA_ANIO,

        [Description("@CRA_PORC_AUM")]
        CRA_PORC_AUM,

        [Description("@CRG_PROYECTA_SM")]
        CRG_PROYECTA_SM,

        [Description("@CRG_PROYECTA_SB")]
        CRG_PROYECTA_SB
    }
}
