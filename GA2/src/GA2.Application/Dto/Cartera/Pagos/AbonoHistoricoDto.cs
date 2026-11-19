using System;

namespace GA2.Application.Dto
{
    public class AbonoHistoricoDto
    {
        public int Id { get; set; }
        public string NumeroCredito { get; set; }
        public int TipoCredito { get; set; }
        public decimal Capital { get; set; }
        public decimal Interes { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal SeguroHogar { get; set; }
        public int NumeroCuota { get; set; }
        public decimal ValorPago { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string EstadoSeguroHogar { get; set; }   // P / NC
        public string EstadoSeguroVida { get; set; }
        public string EstadoInteres { get; set; }
        public string EstadoCapital { get; set; }
        public decimal PendienteTotal { get; set; }
        public decimal AbonadoTotal { get; set; }

        public decimal PendienteSeguroHogar { get; set; }
        public decimal PendienteSeguroVida { get; set; }
        public decimal PendienteInteres { get; set; }
        public decimal PendienteCapital { get; set; }
    }

}
