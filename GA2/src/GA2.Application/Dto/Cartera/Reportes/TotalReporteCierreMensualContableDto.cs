namespace GA2.Application.Dto.Cartera.Reportes
{
    public class TotalReporteCierreMensualContableDto
    {
        public int TipoOperacion { get; set; }
        public string DescripcionOperacion { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public int TotalCreditos { get; set; }
    }
}
