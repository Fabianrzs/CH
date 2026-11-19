using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class PlanPagoDto
    {
        public DateTime Fecha { get; set; }
        public int Cuota { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal SeguroHogar { get; set; }
        public decimal CuotaTradicional { get; set; }
        public decimal CuotaUnidadPagadora { get; set; }
        public decimal CapitalTradicional { get; set; }
        public decimal InteresTradicional { get; set; }
        public decimal SaldoTradicional { get; set; }
        public decimal CapitalReducido { get; set; }
        public decimal InteresReducido { get; set; }
        public decimal SaldoReducido { get; set; }
        public decimal CapitalPorPagar { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal InteresPorPagar { get; set; }
        public decimal SaldoTotal { get; set; }
        public decimal Saldo { get; set; }
        public decimal Interes { get; set; }
        public decimal Capital { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal DeudaRemanente { get; set; }
        public string EstadoCuota { get; set; }

        public decimal AbonosCuota { get; set; }

    }
    public class ResponsePlanPagoDto { 
    
       public string TipoCredito{ get; set; }  
       public List<PlanPagoDto> PlanPago { get; set; }
    
    }
}
