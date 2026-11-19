using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ConsultaSaldosRequest
    {
        public int TipoCredito { get; set; }
        public string NumeroObligacion { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public bool PeriodoActual{ get; set; }
    }
}
