using System;

namespace GA2.Application.Dto.Cartera.Novedades
{
    public class SaldoObligacionDto
    {
        public Guid IdCredito { get; set; }
        public long NumeroCredito { get; set; }
        public decimal SeguroHogar { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal InteresCorriente { get; set; }
        public decimal InteresMora { get; set; }
        public decimal InteresRemanente { get; set; }
        public decimal CapitalRemanente { get; set; }
        public decimal Capital { get; set; }
        public decimal TotalDeuda { get; set; }
    }
}
