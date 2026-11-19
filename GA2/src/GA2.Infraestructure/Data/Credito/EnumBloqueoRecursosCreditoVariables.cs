using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumBloqueoRecursosCredito
    {
        [Description("@BRC_TIPO_IDENTIFICACION")]
        TipoDocumento,
        [Description("@BRC_NUMERO_IDENTIFICACION")]
        NumeroDocumento,
        [Description("@BRC_NUMERO_CUENTA")]
        NumeroCuenta,
        [Description("@BRC_ESTADO")]
        EstadoCuenta
    }
}
