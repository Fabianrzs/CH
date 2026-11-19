using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.ProductoPorCliente
{
    public class InsertPlanPagosDto
    {
        public long PlnNroCredito { get; set; }
        public int PlnCuota { get; set; }
        public DateTime PlnFechaPago { get; set; }
        public decimal PlnSaldoIncial { get; set; }
        public decimal PlnCapital { get; set; }
        public decimal PlnCapitalPorPagar { get; set; }
        public decimal PlnInteres { get; set; }
        public decimal PlnInteresPorPagar { get; set; }
        public decimal PlnSeguroVida { get; set; }
        public decimal PlnSeguroHogar { get; set; }
        public decimal PlnAPagar { get; set; }
        public int PlnPlazo { get; set; }
        public decimal PlnValorDeudaRemanente { get; set; }
        public string PlnF14 { get; set; }
    }
}
