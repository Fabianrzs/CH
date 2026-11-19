using System;

namespace GA2.Domain.Entities.Credito.Simulador.SolicitudCredito
{
    /// <summary>
    /// DTO para HISTÓRICO de causación diaria
    /// Representa cada causación diaria históricamente registrada
    /// </summary>
    public class CausacionHistoricoDto
    {
        public string TipoDato { get; set; } = "HISTORICO";
        public long NumeroCredito { get; set; }
        public int IdTipoCredito { get; set; }
        public DateTime FechaCausacion { get; set; }

        // Causación DIARIA (interés + seguros del día)
        public decimal CausacionInteresesCorrientes { get; set; }
        public decimal? CausacionInteresesRemanente { get; set; }
        public decimal CausacionSeguroVida { get; set; }
        public decimal CausacionSeguroHogar { get; set; }

        // Totales ACUMULADOS hasta esta fecha
        public decimal AcumuladoInteresesCorrientes { get; set; }
        public decimal? AcumuladoInteresesRemanente { get; set; }
        public decimal AcumuladoSeguroVida { get; set; }
        public decimal AcumuladoSeguroHogar { get; set; }

        // Info de saldo y días
        public decimal? ValorDeudaRemanente { get; set; }
        public decimal SaldoCapital { get; set; }
    }
}
