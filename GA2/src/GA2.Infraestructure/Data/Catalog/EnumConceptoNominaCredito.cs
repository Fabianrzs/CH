using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Catalog
{
    public enum EnumConceptoNominaCredito
    {
        [Description("@CNN_DESCRIPCION")]
        CNN_DESCRIPCION,

        [Description("@CNN_SIGLA")]
        CNN_SIGLA,

        [Description("@CNN_ESTADO")]
        CNN_ESTADO,

        [Description("@CNN_CREADO_POR")]
        CNN_CREADO_POR,

        [Description("@CMM_ID")]
        CMM_ID,

        [Description("@CNN_SALARIO")]
        CNN_SALARIO,

        [Description("@CTG_ID")]
        CTG_ID,

        [Description("@CNN_MODIFICADO_POR")]
        CNN_MODIFICADO_POR,

        [Description("@CNN_ID")]
        CNN_ID
    }
}
