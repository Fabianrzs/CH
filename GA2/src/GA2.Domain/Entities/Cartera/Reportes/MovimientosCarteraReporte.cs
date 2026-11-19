using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class MovimientosCarteraReporte
    {

        public string CUENTAS_NIF { get; set; }
        public string DESCRIPCION_CUENTA { get; set; }
        public decimal COSTO { get; set; }
        public string DESCRIPCION_OPERACION { get; set; }
        public string TIPO_CUENTA { get; set; }
        public decimal MONTO_CUENTA { get; set; }
        
    }
}
