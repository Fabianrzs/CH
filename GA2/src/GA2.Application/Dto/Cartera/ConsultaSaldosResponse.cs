using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ConsultaSaldosResponse
    {
        public long NumeroObligacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public decimal SaldoLeasing { get; set; }
        public decimal SaldoHipotecario { get; set; }
        public decimal Provision { get; set; }
        public decimal totalCreditoLeasing { get; set; }
        public decimal totalCreditoHipotecario { get; set; }
        public string TipoCredito { get; set; }

    }
}
