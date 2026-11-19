using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class InclusionSaldoDto
    {
        public int IdInclusionSaldoOpciones { get; set; }
        public DateTime PeriodoDesde { get; set; }
        public DateTime PeriodoHasta { get; set; }
        public int IdConcepto { get; set; }
        public int Valor { get; set; }
        public int Cuotas { get; set; }
        public int CuotasAntiguas { get; set; }
    }
}
