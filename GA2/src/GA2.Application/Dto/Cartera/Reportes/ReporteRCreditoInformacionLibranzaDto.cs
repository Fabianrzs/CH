using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteRCreditoInformacionLibranzaDto
    {
        public DateTime FechaEvaluacion { get; set; }
        public string NitEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string CaracterEmpresa { get; set; }
        public string SectorEmpresa { get; set; }
        public string EstadoPagaduria { get; set; }
        public int PlazoMaximo { get; set; }
        public int NumeroObligaciones { get; set; }
        public int NumeroDeudores { get; set; }
        public decimal SaldoCapital { get; set; }
        public int NumeroObligacionesMora { get; set; }
        public int NumeroDeudoresMora { get; set; }
        public decimal SaldoCapitalMora { get; set; }
    }
}