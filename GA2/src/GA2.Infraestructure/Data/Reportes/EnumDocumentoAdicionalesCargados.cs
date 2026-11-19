using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Reportes
{
    public enum EnumDocumentoAdicionalesCargados
    {
        [Description("@DSA_ID")]
        Id,

        [Description("@DSA_DOCUMENTO")]
        Documento,

        [Description("@DSA_RUTA")]
        Ruta,

        [Description("@DSA_CARPETA")]
        Carpeta,

        [Description("@DSA_FECHA_CARGUE")]
        FechaCargue,

        [Description("@DSA_CARGADO_DODO")]
        CargadoDodo,

        [Description("@DSA_MODULO")]
        Modulo,

        [Description("@DSA_NUMERO_DOCUMENTO")]
        NumeroDocumento
    }
}
