using System;

namespace GA2.Domain.Core
{
    public class CausacionProcesadaDto
    {
        public Guid CreId { get; set; }
        public int TcrId { get; set; }
        public DateTime FechaCausacion { get; set; }

        // Saldos del crédito
        public decimal SaldoCapital { get; set; }
        public decimal InteresCorrente { get; set; }
        public decimal InteresRemanente { get; set; }
        public decimal CapitalRemanente { get; set; }
        public decimal SaldoCapitalTradicional { get; set; }

        // Seguros
        public decimal SeguroVida { get; set; }
        public decimal SeguroHogar { get; set; }
        public int DiasEnMora { get; set; }

        // Saldos acumulados
        public decimal SaldoInteresCorrientes { get; set; }
        public decimal SaldoInteresesRemanentes { get; set; }
        public decimal SaldoCapitalRemanente { get; set; }
        public decimal SaldoSeguroVida { get; set; }
        public decimal SaldoSeguroHogar { get; set; }
        public decimal SaldoMora { get; set; }
        public decimal SaldoTotalCausacionDiaria { get; set; }

        // Causaciones diarias
        public decimal SaldoInteresCorrienteCausado { get; set; }
        public decimal SaldoInteresesRemanenteCausado { get; set; }
        public decimal SaldoSeguroVidaCausado { get; set; }
        public decimal SaldoSeguroHogarCausado { get; set; }

        // Datos de afiliado
        public string Identificacion { get; set; }
        public decimal TotalCausacionDiaria =>
            InteresCorrente + InteresRemanente + SeguroVida + SeguroHogar;
    }



}

