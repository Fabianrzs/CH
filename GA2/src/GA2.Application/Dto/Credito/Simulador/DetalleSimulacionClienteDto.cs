using System;

namespace GA2.Application.Dto
{
    public class DetalleSimulacionClienteDto
    {
        public Guid SimulacionDetalleId { get; set; }
        public Guid SimulacionId { get; set; }
        public int Cuota { get; set; }
        public DateTime FechaCorte { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Saldo { get; set; }
        public decimal Intereses { get; set; }
        public decimal Capital { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal SeguroTerremoto { get; set; }
        public decimal CuotaTotal { get; set; }
        public decimal AbonoExtra { get; set; }
        public decimal AbonoMensual { get; set; }
        public decimal CuotaUnidadPagadora { get; set; }
        public decimal InteresesReduccion { get; set; }
        public decimal CuotaReduccion { get; set; }
        public decimal CobroFonvivienda { get; set; }
        public decimal SaldoLeasing { get; set; }
        public decimal InteresLeasing { get; set; }
        public decimal CapitalLeasing { get; set; }
        public decimal CuotaLeasing { get; set; }
        public decimal CuotaLeasingSubsidio { get; set; }
        public decimal InteresTradicionalSubsidio { get; set; }
        public decimal InteresLeasingSubsidio { get; set; }
        public decimal SeguroVidaLeasing { get; set; }
        public decimal AbonoUnico { get; set; }
        public decimal SaldoReducidoLeasing { get; set; }
        public decimal InteresesPagadosLeasing { get; set; }
        public decimal CapitalPagadoLeasing { get; set; }
        public decimal CapitalAPagarLeasing { get; set; }
        public decimal SaldoCapitalLeasing { get; set; }
        public decimal InteresAPagarLeasing { get; set; }
        public decimal SalDoTotalLeasing { get; set; }
        public decimal AfectacionIndividualLeasing { get; set; }
        public decimal ValorFinanciar { get; set; }
        public decimal SegurosTotal { get; set; }
        public int plazoAnos { get; set; }
        public string plnNroCredito { get; set; }
        public int Score { get; set; }
        public DateTime FechaDesembolso { get; set; }

    }
}
