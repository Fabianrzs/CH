using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Embargos
{
    public enum EnumPagosEmbargosSevenRespuesta
    {
        [Description("@EBA_ID")]
        EmbargoID,

        [Description("@PEM_CODIGO_PROVEEDOR")]
        CodigoProveedor,

        [Description("@PEM_CDP")]
        CDP,

        [Description("@PEM_NUMERO_FACTURA")]
        NumeroFactura,

        [Description("@PEM_FLUJO")]
        Flujo,
    }
}
