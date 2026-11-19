namespace GA2.Domain.Entities.Reportes
{
    public class ReporteRCreditoDatosGenerales
    {
        public int ID_MOD { get; set; }
        public int? ID_MOD_PRINCIPAL { get; set; }
        public string MODALIDAD { get; set; }
        public decimal? SALDO_CAPITAL_MES_ANTERIOR { get; set; }
        public decimal? SALDO_DESEMBOLSOS { get; set; }
        public int? CANTIDAD_DESEMBOLSOS { get; set; }
        public decimal? SALDO_RECAUDOS { get; set; }
        public int? CANTIDAD_RECAUDOS { get; set; }
        public decimal? SALDO_CANCELACIONES { get; set; }
        public int? CANTIDAD_CANCELACIONES { get; set; }
        public decimal? SALDO_CASTIGOS { get; set; }
        public int? CANTIDAD_CASTIGOS { get; set; }
        public decimal? SALDO_RESTRUCTURACIONES { get; set; }
        public int? CANTIDAD_RESTRUCTURACIONES { get; set; }
    }
}
