namespace GA2.Application.Dto
{
    public class ValidarAplicacionPagoDto
    {
        public bool Mora { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorPagoCuota { get; set; }
        public decimal AcumuladosAlDia { get; set; } 
    }
}
