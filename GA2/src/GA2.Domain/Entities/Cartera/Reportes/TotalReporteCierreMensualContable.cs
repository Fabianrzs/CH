namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class TotalReporteCierreMensualContable
    {
        public int TIPO_OPERACION { get; set; }
        public string DESCRIPCION_OPERACION { get; set; }
        public decimal DEBITO { get; set; }
        public decimal CREDITO { get; set; }
        public int TOTAL_CREDITOS { get; set; }
    }

}
