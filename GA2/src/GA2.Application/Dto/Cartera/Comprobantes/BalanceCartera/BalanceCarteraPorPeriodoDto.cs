using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Comprobantes.BalanceCarteraPorPeriodoDto
{

    public class BalanceCarteraPorPeriodoDto
    {
        public Guid Id { get; set; }
        public DateTime Periodo { get; set; }
        public string CuentasNif { get; set; }
        public string CuentaContable { get; set; }
        public string DescripcionCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoFinal { get; set; }
        public decimal Debitos { get; set; }
        public decimal Creditos { get; set; }
    }
}
