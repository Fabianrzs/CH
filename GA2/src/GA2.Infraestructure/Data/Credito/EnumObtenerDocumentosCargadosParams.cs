using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumObtenerDocumentosCargadosParams
    {
        [Description("@TDC_ID")]
        TipoDocumento,

        [Description("@SOC_ID")]
        IdSolicitud,
    }
}
