namespace GA2.Domain.Entities.VerificacionDerechos
{
    public class DatosCedulasCesantias
    {
        public string Identificacion { get; set; }
        public int IdCliente { get; set; }
        public int CuentaNumero { get; set; }
        public int ConceptoId { get; set; }
        public int CuentaId { get; set; }
    }
    public class DatosInfoCensatiaByCuentaPerioso
    {
        public double ValorIngresos { get; set; }
        public double ValorEgresos { get; set; }
        public double SaldoCuenta { get; set; }
        public double SaldoDisponible { get; set; }

        public double SaldoAnual { get; set; }
    }

}