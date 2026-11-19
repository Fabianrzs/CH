namespace GA2.Application.Dto.Cartera.ParametersRequest.Extractos
{
    public class ReporteExtractoParametersDto
    {
        public ReporteExtractoParametersDto()
        {
        }

        public ReporteExtractoParametersDto(int tipoCredito, long numeroCredito, int anio, int? mes)
        {
            TipoCredito = tipoCredito;
            NumeroCredito = numeroCredito;
            Anio = anio;
            Mes = mes;
        }

        public int TipoCredito { get; set; }
        public long NumeroCredito { get; set; }
        public int Anio { get; set; }
        public int? Mes { get; set; }

        public TipoExtracto TipoExtracto => Mes is null ? TipoExtracto.Anual : TipoExtracto.Mensual;
    }


}
