using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Reportes
{
    public class DatosFacturaSevenParam
    {
        public string TipoCredito { get; set; }
        public string TipoDocumentoAfiliado { get; set; }
        public string TipoDocumentoVendedor { get; set; }
        public string TipoCuentaVendedor { get; set; }
        public string BancoVendedor { get; set; }
    }
}
