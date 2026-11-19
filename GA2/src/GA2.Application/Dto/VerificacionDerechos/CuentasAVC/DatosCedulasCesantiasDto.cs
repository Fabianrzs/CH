namespace GA2.Application.Dto.VerificacionDerechos.CuentasAVC
{
    public class DatosCedulasCesantiasDto
    {
        public string Identificacion { get; set; }
        public int IdCliente { get; set; }
        public int CuentaNumero { get; set; }
        public int ConceptoId { get; set; }
        public int CuentaId { get; set; }
    }
    public class DatosInfoCensatiaByCuentaPeriodoDto
    {
        public double ValorIngresos { get; set; }
        public double ValorEgresos { get; set; }
        public double SaldoCuenta { get; set; }
        public double SaldoDisponible { get; set; }
        public double SaldoAnual { get; set; }
    }
    public class ObtenerInfoCedulaRequestDto
    {

        public int CuentaId { get; set; }
        public int Periodo { get; set; }

    }
}
