namespace GA2.Application.Dto.Cartera.ParametersRequest.Reportes
{
    public class ReporteGeneralCarteraParametersDto
    {
        public bool PeriodoActual { get; set; }
        public int? Anio { get; set; }
        public int? Mes { get; set; }
        public int? TipoCredito { get; set; }
        public long? NumeroCredito { get; set; }
        public int? Fuerza { get; set; }
        public int? Categoria { get; set; }
    }
}