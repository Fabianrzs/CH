using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EnumInsertarDocumentoCargadoDodo
{
    [Description("@DCD_DESCRIPCION")]
    DCD_DESCRIPCION,

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
    DCD_CARGUE_83


}

public enum EnumConsultaDocumentoCargadoDodo
{
    [Description("@SLS_ID")]
    SLS_ID
}
