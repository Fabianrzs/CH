using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// DTO para SALDO ACTUAL de causación
    /// Representa el último estado acumulado del crédito
    /// </summary>
    /// <summary>
    /// Mapea resultado dinámico a CausacionSaldoActualDto
    /// </summary>
    public class CausacionSaldoActual
    {
        public string TipoDato { get; set; } = "SALDO_ACTUAL";
        public long NumeroCredito { get; set; }
        public int IdTipoCredito { get; set; }
        public DateTime FechaCausacion { get; set; }
        public decimal SaldoCapital { get; set; }

        // Causaciones diarias (lo que se causó ese día)
        public decimal CausacionInteresCorriente { get; set; }
        public decimal? CausacionInteresRemanente { get; set; }
        public decimal CausacionSeguroVida { get; set; }
        public decimal CausacionSeguroHogar { get; set; }
        public decimal TotalCausacionDiaria { get; set; }

        // Acumulados (suma histórica)
        public decimal AcumuladoInteresCorriente { get; set; }
        public decimal? AcumuladoInteresRemanente { get; set; }
        public decimal AcumuladoSeguroVida { get; set; }
        public decimal AcumuladoSeguroHogar { get; set; }
        public decimal? ValorDeudaRemanente { get; set; }
        public decimal SaldoSinPagoCuota { get; set; }
        public decimal SaldoConPagoCuota { get; set; }

    }
}











