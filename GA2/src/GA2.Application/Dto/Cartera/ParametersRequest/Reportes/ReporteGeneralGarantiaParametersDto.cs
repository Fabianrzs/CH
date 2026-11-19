namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReporteGeneralGarantiaParametersDto
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int? TipoCredito { get; set; }
        public string? Identificacion { get; set; }
        public string? NumeroCredito { get; set; }
    }
}