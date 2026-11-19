using System;

namespace GA2.Application.Dto.Cartera.Comprobantes.BalanceCartera
{
    public class RecalcularPlanPagoRequest
    {
        public string NumeroCredito { get; set; }
        public DateTime FechaCorte { get; set; }
        public decimal PorcentajeSeguroVida { get; set; }
        public decimal ValorSeguroHogar { get; set; }
    }

}

