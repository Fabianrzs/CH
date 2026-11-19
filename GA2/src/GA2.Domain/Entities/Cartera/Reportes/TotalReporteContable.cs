namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class TotalReporteContable
    {
        public int TCR_ID { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public decimal DEBITO { get; set; }
        public decimal CREDITO { get; set; }
        public int TOTAL_CREDITOS { get; set; }
    }

}
