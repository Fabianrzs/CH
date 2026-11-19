namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReporteContableCierreMensualParametersDto
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int? TipoCredito { get; set; }

        public int? RegistrosPorPagina { get; set; }
        public int? NumeroPagina { get; set; }
    }
}