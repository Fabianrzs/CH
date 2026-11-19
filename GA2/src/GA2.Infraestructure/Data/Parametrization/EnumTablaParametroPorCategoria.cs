using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Parametrization
{
    public enum EnumTablaParametroPorCategoria
    {
        [Description("@CTG_ID")]
        CTG_ID, 
        [Description("@Anio")]
        ANIO, 
        [Description("@CategoriaId")]
        ID_CATEGORIA,
    }
}
