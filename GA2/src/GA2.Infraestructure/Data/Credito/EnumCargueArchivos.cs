using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumCargueArchivos
    {
        [Description("@DCD_ID")]
        DCD_ID,

        [Description("@DCD_DESCRIPCION")]
        DCD_DESCRIPCION,

        [Description("@DCD_FECHA_CARGUE")]
        DCD_FECHA_CARGUE,

        [Description("@DCD_CARGADO")]
        DCD_CARGADO,

        [Description("@SLS_ID")]
        SLS_ID,

        [Description("@RUTA")]
        RUTA,

        [Description("@TDC_ID")]
        TDC_ID,

        [Description("@DCD_CARGUE_361")]
        DCD_CARGUE_361,

        [Description("@DCD_CARGUE_83")]
        DCD_CARGUE_83,

        [Description("@DCD_ESTADO")]
        DCD_ESTADO,

        [Description("@DCD_CREADO_POR")]
        DCD_CREADO_POR,

        [Description("@DCD_FECHA_CREACION")]
        DCD_FECHA_CREACION,

        [Description("@DCD_MODIFICADO_POR")]
        DCD_MODIFICADO_POR,

        [Description("@DCD_FECHA_MODIFICACION")]
        DCD_FECHA_MODIFICACION
    }
}
