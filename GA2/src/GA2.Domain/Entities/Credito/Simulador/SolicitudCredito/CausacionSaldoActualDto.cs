using System;

namespace GA2.Domain.Entities.Credito.Simulador.SolicitudCredito
{
    /// <summary>
    /// DTO para SALDO ACTUAL de causación
    /// Representa el último estado Causacion del crédito
    /// </summary>
    public class CausacionSaldoActualDto
    {
        public string TipoDato { get; set; } = "SALDO ACTUAL";
        public long NumeroCredito { get; set; }
        public DateTime FechaCausacion { get; set; }
        public decimal SaldoInicial { get; set; }

        // Causaciones diarias
        public decimal CausacionInteresCorriente { get; set; }
        public decimal? CausacionInteresRemanente { get; set; }
        public decimal CausacionSeguroVida { get; set; }
        public decimal CausacionSeguroHogar { get; set; }
        public decimal TotalCausacionDiaria { get; set; }
        public decimal SaldoFinal { get; set; }
        public decimal AbonosPagosRealizados { get; set; }

        // Acumulados
        //public decimal AcumuladoInteresCorriente { get; set; }
        //public decimal? AcumuladoInteresRemanente { get; set; }
        //public decimal AcumuladoSeguroVida { get; set; }
        //public decimal AcumuladoSeguroHogar { get; set; }
        //public decimal? ValorDeudaRemanente { get; set; }
    }
}
