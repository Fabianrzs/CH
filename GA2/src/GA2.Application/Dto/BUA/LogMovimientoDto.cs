using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class LogMovimientoDto
    {
        public int ConceptoId { get; set; }
        public int CuentaId { get; set; }
        public double ValorBloque { get; set; }
        public double ValorSaldo { get; set; }
        public int IdTipoReintegro { get; set; }
    }
}
