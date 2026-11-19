using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Cierres.CierresCausacionesRealizadas
{
    public class RequestCierresCausacionesRealizadas
    {
        public DateTime? FechaCierreInicial { get; set; }
        public DateTime? FechaCierreFinal { get; set; }
        public IEnumerable<int> TiposCierre { get; set; }
    }
}
