namespace GA2.Application.Dto
{
    public class PlanPagoGeneradoDto
    {
        public string NombreArchivo { get; set; }
        public byte[] Bytes { get; set; }
    }
    public class RequestPlanPagoGeneradoDto
    {
        public string Periodo { get; set; }
        public int TipoCredito { get; set; }
    }
}
